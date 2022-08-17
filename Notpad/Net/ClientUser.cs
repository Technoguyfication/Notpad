using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net;
using Technoguyfication.Notpad.Shared.Net.Packets;
using Technoguyfication.Notpad.Shared.Types;

namespace Technoguyfication.Notpad.Net
{
	class ClientUser : NetworkedUser
	{
		public ClientStatus Status { get; private set; }

		public event EventHandler OnConnected;

		private NetworkClient _client;
		private readonly Dictionary<Guid, User> _users;

		private bool _disconnecting = false;

		public ClientUser(Guid id) : base(id)
		{
			Status = ClientStatus.Disconnected;

			_users = new Dictionary<Guid, User>();
		}

		/// <summary>
		/// Attempts to connect to the server. The return value will indicate whether the connection succeeded under normal conditions
		/// (i.e. everything is functioning correctly but the client is unable to join due to a username conflict or the server is full)
		/// </summary>
		/// <param name="endpoint"></param>
		/// <returns cref="(bool, string)">A tuple containing a bool to indicate success, and optional string as the rejection message on failure</returns>
		/// <exception cref="IncompatibleProtocolVersionException">Thrown if we try to query a server with an incompatible protocol version</exception>
		/// <exception cref="InvalidOperationException">Thrown if the client is not in a state to connect (eg. already connected or attempting to connect)</exception>
		/// <exception cref="IOException">Thrown if we are unable to communicate with the server</exception>
		/// <exception cref="SocketException">Thrown if we are unable to establish a connection to the server</exception>
		/// <exception cref="TimeoutException">Thrown if the operation does not complete within the specified amount of time</exception>
		public async Task<(bool, string)> Connect(IPEndPoint endpoint, TimeSpan timeout)
		{
			// validate client state
			if (Status != ClientStatus.Disconnected) throw new InvalidOperationException("Client is already connected");

			// set client state variables
			_disconnecting = false;
			Status = ClientStatus.Connecting;

			_client = new NetworkClient();

			// drop into another task so we can use a timeout on the entire operation
			var connectTask = Task.Run(async () =>
			{
				// connect to the server 
				await _client.ConnectAsync(endpoint);

				// the client is now in Login phase
				Status = ClientStatus.Login;

				// send a handshake packet with login intent followed by a login packet
				_client.WritePacket(new Packet[] {
					new SHandshakePacket()
					{
						Intent = SHandshakePacket.HandshakeIntent.Login,
						ProtocolVersion = Protocol.Version
					},
					new SLoginPacket()
					{
						UserID = this.ID,
						Username = this.Username
					}
				});

				// read response packet
				// this may throw a IOException if the socket is closed, it will float up to the caller
				var responsePacket = _client.ReadPacket();

				// handle possible response packets
				if (responsePacket is CUserJoinedPacket ujPkt && ujPkt.NewUser.Equals(this))
				{
					// a CUserJoined packet is fired for this user if we successfully join the server

					// client has successfully logged in
					Status = ClientStatus.Connected;
					OnConnected?.Invoke(this, null);

					// we're done for now. add additional post-login logic here if needed
					return (true, null);
				}
				else if (responsePacket is CDisconnectPacket dcPkt)
				{
					// the server has disconnected us for some reason.
					return (false, dcPkt.Reason);
				}
				else if (responsePacket is CInvalidVersionPacket ivPkt)
				{
					// we're using an incompatible protocol version
					throw new IncompatibleProtocolVersionException(ivPkt.ServerVersion);
				}
				else
				{
					throw new IOException($"Unexpected packet {responsePacket.ID} returned by server after login attempt");
				}
			});

			// wait for the above task to complete, or the timeout to expire
			try
			{
				return await connectTask.WaitAsync(timeout);
			}
			catch (TimeoutException)
			{
				// dispose the client on timeout because the task will continue running in the background
				// this should kill any pending operations and allow the task to complete
				_client.Close();
				throw;
			}
		}

		public void Disconnect()
		{
			// only call disconnect once
			if (_disconnecting) return;
			_disconnecting = true;

			// close client if it exists
			_client?.Close();

			Status = ClientStatus.Disconnected;
		}

		/// <summary>
		/// Sends a message to the server
		/// </summary>
		/// <param name="content">Text content to send as message body</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Thrown if the client is not in the proper state to send messages</exception>
		/// <exception cref="IOException">Thrown if the connection is dropped while the operation is completing</exception>
		public async Task SendMessage(string content)
		{
			if (Status != ClientStatus.Connected) throw new InvalidOperationException("Client must be connected to send messages");

			// this method will primarily be called from ui threads so it's easier to make it non-blocking to begin with
			await Task.Run(() =>
			{
				_client.WritePacket(new SMessagePacket()
				{
					Content = content
				});
			});
		}

		public override void SetUsername(string username)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Queries a remote server for information such as server name, motd, and user count
		/// </summary>
		/// <param name="endpoint">The endpoint of the server to query</param>
		/// <param name="timeout">How long to wait for the query before timing out with a <see cref="TimeoutException"/></param>
		/// <returns></returns>
		/// <exception cref="IncompatibleProtocolVersionException">Thrown if we try to query a server with an incompatible protocol version</exception>
		/// <exception cref="IOException">Thrown if we are unable to communicate with the server</exception>
		/// <exception cref="SocketException">Thrown if we are unable to establish a connection to the server</exception>
		/// <exception cref="TimeoutException">Thrown if the operation does not complete within the specified amount of time</exception>
		public static async Task<ServerInfo> QueryServer(IPEndPoint endpoint, TimeSpan timeout)
		{
			using var queryClient = new NetworkClient();

			// create task to run the query since we need to handle a timeout for this entire function
			var queryTask = Task.Run(async () =>
			{
				// try to connect to the server
				await queryClient.ConnectAsync(endpoint);

				// send handshake with query intent
				queryClient.WritePacket(new SHandshakePacket()
				{
					Intent = SHandshakePacket.HandshakeIntent.Query,
					ProtocolVersion = Protocol.Version
				});

				// read reply
				var reply = queryClient.ReadPacket();

				// handle possible responses
				if (reply is CQueryResponsePacket qrpkt)
				{
					return qrpkt.ServerInfo;
				}
				else if (reply is CInvalidVersionPacket ivpkt)
				{
					throw new IncompatibleProtocolVersionException(ivpkt.ServerVersion);
				}
				else
				{
					throw new IOException($"Invalid packet received from server. It may be running a vastly incompatible protocol version that does not recognize handshake packets from version {Protocol.Version}");
				}
			});

			try
			{
				// this throws a TimeoutException if the Task does not complete within the timespan
				// but the Task will continue running in the background so we manually dispose of 
				// the network client
				return await queryTask.WaitAsync(timeout);
			}
			catch (TimeoutException)
			{
				queryClient.Close();
				throw;
			}
		}

		private User GetUser(Guid id)
		{
			if (_users.ContainsKey(id))
			{
				return _users[id];
			}
			else
			{
				return null;
			}
		}

		internal enum ClientStatus
		{
			Disconnected,
			Connecting,
			Login,
			Connected
		}
	}
}
