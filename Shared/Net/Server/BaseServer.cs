using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Packets;
using Technoguyfication.Notpad.Shared.Net.Structs;

namespace Technoguyfication.Notpad.Shared.Net.Server
{
	public class BaseServer
	{
		public string ServerName { get; protected set; }
		public string MOTD { get; protected set; }
		public int MaxUsers { get; protected set; }
		public int UsersOnline => _users.Where(c => c.Status == ClientStatus.Ready || c.Status == ClientStatus.Login).Count();

		public ServerInfo ServerInfo
		{
			get
			{
				return new ServerInfo()
				{
					Name = ServerName,
					MOTD = MOTD,
					MaxUsers = MaxUsers,
					UsersOnline = UsersOnline
				};
			}
		}

		// basic events
		public event EventHandler OnStarted;
		public event EventHandler OnStopped;

		// log events
		public event EventHandler<Exception> OnException;
		public event EventHandler<string> OnDebugMessage;

		// user-based events
		public event EventHandler<ServerUser> OnUserJoin;
		public event EventHandler<ServerUser> OnUserLogin;
		public event EventHandler<ServerUser> OnUserDisconnect;

		private TcpListener _listener;
		private UdpClient _udpClient;
		private readonly List<ServerUser> _users;

		private bool _stopping = false;
		private readonly object _stoppingLock = new();
		private bool _started = false;

		protected BaseServer()
		{
			_users = new List<ServerUser>();
		}

		public void Start(int port, IPAddress bindAddress)
		{
			// Server can only ever be started once
			if (_started) throw new InvalidOperationException("Server has already been started");

			// start tcp listener
			_listener = new TcpListener(bindAddress, port);
			_listener.Start();

			// start udp client to listen for broadcast messages
			_udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));

			// start udp discovery listener
			Task.Run(async () =>
			{
				try
				{
					await DiscoveryListener();
				}
				catch (Exception ex)
				{
					OnException?.Invoke(this, ex);
					Stop(true);
				}
			});

			// start new client listener
			Task.Run(async () =>
			{
				try
				{
					await NewClientListener();
				}
				catch (Exception ex)
				{
					OnException?.Invoke(this, ex);
					Stop(true);
				}
			});

			_started = true;
			OnStarted?.Invoke(this, null);
		}

		public void Stop(bool force)
		{
			// only allow server to be stopped once
			lock (_stoppingLock)
			{
				if (_stopping) return;
				_stopping = true;
			}

			// gracefully disconnect all users if this is not a forceful shutdown
			if (!force)
			{
				OnDebugMessage?.Invoke(this, $"Disconnecting {_users.Count} users...");

				// disconnect all users
				while (_users.Count > 0)
				{
					var user = _users.First();

					// disconnecting the user removes them from the user list
					user.Disconnect("Server Closing", false);
				}
			}

			// stop listeners if they exist
			_udpClient?.Close();
			_listener?.Stop();

			OnStopped?.Invoke(this, null);
		}

		/// <summary>
		/// Validates user login requests.
		/// Checks uniqueness of user id and username requirements
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public (bool, string) UserLoginRequest(Guid userId, string username)
		{
			Debug($"Checking username for {userId}: {username}");

			// check for duplicate guid
			if (_users.Where(u => u.ID.Equals(userId)).Any())
			{
				return (false, "User ID already logged in. If you have recently disconnected, wait a minute and try again");
			}

			// check for username requirements
			if (username.Length > Protocol.UsernameMaxLength || username.Length < Protocol.UsernameMinLength)
			{
				return (false, $"Username must be between {Protocol.UsernameMinLength} and {Protocol.UsernameMaxLength} characters");
			}
			else if (Protocol.UsernameRegex.IsMatch(username))
			{
				return (false, "Username must only contain letters, numbers, and underscores");
			}

			// check duplicate username
			if (_users.Where(u => u.Username.Equals(username)).Any())
			{
				return (false, "This username is already taken on this server");
			}

			// all checks passed
			return (true, null);
		}

		private void User_OnDisconnect(object sender, EventArgs e)
		{
			var user = sender as ServerUser;

			Debug($"{user} is disconnecting");

			// todo: send all users disconnect message

			// fire user disconnect event
			OnUserDisconnect?.Invoke(this, user);

			// remove user from users list
			_users.Remove(user);

			Debug($"{user} fully disconnected");
		}

		/// <summary>
		/// Adds a new user from a network client.
		/// To be used after the client has sent a handshake packet
		/// with the "login" intent and BEFORE reading a login packet
		/// </summary>
		/// <param name="client"></param>
		private void AddUser(NetworkClient client)
		{
			Debug($"Adding new user from {client.Client.RemoteEndPoint}");

			// the client should have sent a login packet immediately after the login intent packet
			// so we upgrade this client into a User and the User's internal packet handler will
			// handle the login process

			var user = new ServerUser(client, this);

			// add user to the server's collection of users
			_users.Add(user);

			// subscribe to events
			user.OnDisconnect += User_OnDisconnect;

			// fire join event
			OnUserJoin?.Invoke(this, user);

			// initialize the user
			// this starts it's own listener task and it will begin reading packets from the
			// network asynchronously
			user.Initialize();
		}

		private async Task NewClientListener()
		{
			while (_listener?.Server?.Blocking ?? false)
			{
				var socket = await _listener.AcceptSocketAsync();
				var networkClient = new NetworkClient(socket);

				Debug($"Accepting connection from {networkClient.Client.RemoteEndPoint}");

				Packet packet;
				try
				{
					packet = networkClient.ReadPacket();
				}
				catch (IOException ex)
				{
					Debug($"Socket at {networkClient.Client.RemoteEndPoint} failed to become a client: {ex.Message}");

					networkClient.Close();
					continue;
				}

				// initial packet must be a handshake packet
				if (packet is not SHandshakePacket)
				{
					Debug($"Client at {networkClient.Client.RemoteEndPoint} did not send a handshake packet as initial packet, sent {packet.GetType().Name} instead");

					networkClient.Close();
					continue;
				}

				var hsPacket = packet as SHandshakePacket;

				// check for compatible protocol version
				if (hsPacket.ProtocolVersion != Protocol.Version)
				{
					networkClient.WritePacket(new CInvalidVersionPacket()
					{
						ServerVersion = Protocol.Version
					});

					networkClient.Close();
					continue;
				}

				// client intent
				switch (hsPacket.Intent)
				{
					// client wants servery query
					case SHandshakePacket.HandshakeIntent.Query:
						Debug($"Query request from {networkClient.Client.RemoteEndPoint}");

						networkClient.WritePacket(new CQueryResponsePacket()
						{
							ServerInfo = new ServerInfo()
							{
								Name = ServerName,
								MOTD = MOTD,
								MaxUsers = MaxUsers,
								UsersOnline = UsersOnline
							}
						});

						networkClient.Close();
						break;

					// new user login
					case SHandshakePacket.HandshakeIntent.Login:
						AddUser(networkClient);
						break;
				}
			}
		}

		private async Task DiscoveryListener()
		{
			// todo: send server info instead of discovery handshake
			while (_udpClient?.Client?.Blocking ?? false)
			{
				var data = await _udpClient.ReceiveAsync();

				Debug($"Discovery request from {data.RemoteEndPoint}");

				// check if the client is a Notpad client
				if (data.Buffer.SequenceEqual(Protocol.BroadcastMessage))
				{
					// respond with the multicast message
					await _udpClient.SendAsync(Protocol.BroadcastMessage, Protocol.BroadcastMessage.Length, data.RemoteEndPoint);
				}
			}
		}

		private void Debug(string message)
		{
			OnDebugMessage?.Invoke(this, message);
		}

		public class DiscoveryEventArgs : EventArgs
		{
			public IPEndPoint RemoteEndPoint { get; set; }
		}
	}
}
