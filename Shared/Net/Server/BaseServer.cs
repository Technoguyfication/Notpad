using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Net;
using Technoguyfication.Notpad.Net.Packets;
using Technoguyfication.Notpad.Shared.Net.Packets;

namespace Technoguyfication.Notpad.Shared.Net.Server
{
	public class BaseServer
	{
		public string ServerName { get; private set; }
		public string MOTD { get; private set; }
		public int MaxUsers { get; private set; }
		public int UsersOnline => _users.Where(c => c.Status == ClientStatus.Ready || c.Status == ClientStatus.Login).Count();

		// basic events
		public event EventHandler OnStarted;
		public event EventHandler OnStopped;

		// log events
		public event EventHandler<Exception> OnException;
		public event EventHandler<string> OnDebugMessage;

		// network query events
		public event EventHandler<DiscoveryEventArgs> OnDiscoveryRequest;
		public event EventHandler<IPEndPoint> OnQuery;

		// user-based events
		public event EventHandler<ServerUser> OnUserJoin;
		public event EventHandler<ServerUser> OnUserLogin;
		public event EventHandler<ServerUser> OnUserDisconnect;

		private TcpListener _listener;
		private UdpClient _udpClient;
		private readonly List<ServerUser> _users;

		private bool _stopping = false;
		private readonly object _stoppingLock = new();

		public BaseServer()
		{
			_users = new List<ServerUser>();
		}

		public void Start(int port, IPAddress bindAddress)
		{
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

		private void User_OnDisconnect(object sender, EventArgs e)
		{
			var user = sender as ServerUser;

			// todo: send all users disconnect message

			// fire user disconnect event
			OnUserDisconnect?.Invoke(this, user);

			// remove user from users list
			_users.Remove(user);
		}

		/// <summary>
		/// Adds a new user from a network client.
		/// To be used after the client has sent a handshake packet
		/// with the "login" intent and BEFORE reading a login packet
		/// </summary>
		/// <param name="client"></param>
		private void AddUser(NetworkClient client)
		{
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

				Packet packet = null;
				try
				{
					packet = networkClient.ReceivePacket();
				}
				catch (IOException ex)
				{
					OnDebugMessage?.Invoke(this, $"Socket at {networkClient.Client.RemoteEndPoint} failed to become a client: {ex.Message}");

					networkClient.Close();
					continue;
				}

				// initial packet must be a handshake packet
				if (packet is not SHandshakePacket)
				{
					OnDebugMessage?.Invoke(this, $"Client at {networkClient.Client.RemoteEndPoint} did not send a handshake packet as initial packet, sent {packet.GetType().Name} instead");

					networkClient.Close();
					continue;
				}

				var hsPacket = packet as SHandshakePacket;

				// check for compatible protocol version
				if (hsPacket.ProtocolVersion != Protocol.Version)
				{
					networkClient.SendPacket(new CInvalidVersionPacket()
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
						OnQuery?.Invoke(this, (IPEndPoint)networkClient.Client.RemoteEndPoint);

						networkClient.SendPacket(new CQueryResponsePacket()
						{
							MaxUsers = MaxUsers,
							UsersOnline = UsersOnline,
							ServerName = ServerName,
							MOTD = MOTD
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
				OnDiscoveryRequest?.Invoke(this, new DiscoveryEventArgs() { RemoteEndPoint = data.RemoteEndPoint });

				// check if the client is a Notpad client
				if (data.Buffer.SequenceEqual(Protocol.BroadcastMessage))
				{
					// respond with the multicast message
					await _udpClient.SendAsync(Protocol.BroadcastMessage, Protocol.BroadcastMessage.Length, data.RemoteEndPoint);
				}
			}
		}

		public class DiscoveryEventArgs : EventArgs
		{
			public IPEndPoint RemoteEndPoint { get; set; }
		}
	}
}
