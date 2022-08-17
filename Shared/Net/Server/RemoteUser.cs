using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Packets;
using Technoguyfication.Notpad.Shared.Types;

namespace Technoguyfication.Notpad.Shared.Net.Server
{
    public class RemoteUser : NetworkedUser
    {
        public IPEndPoint RemoteEndPoint => _networkClient?.Client?.RemoteEndPoint as IPEndPoint;
        public ClientStatus Status { get; private set; }

        public event EventHandler<string> OnDebugMessage;
        
        public event EventHandler OnLogin;
		public event EventHandler OnDisconnect;

        public event EventHandler<Message> OnMessage;

        private bool _disconnecting = false;
        private readonly object _disconnectLock = new();

        private readonly NetworkClient _networkClient;
		private readonly BaseServerImplementation _server;

		private readonly ConcurrentQueue<Packet> _packetQueue;

        /// <summary>
        /// Instantiate an instance of the TCP client
        /// </summary>
        /// <param name="client"></param>
        public RemoteUser(NetworkClient client, BaseServerImplementation server)
        {
            Status = ClientStatus.Uninitialized;
            
            _networkClient = client;
            _server = server;

            _packetQueue = new ConcurrentQueue<Packet>();
        }

        /// <summary>
        /// Initializes the User
        /// </summary>
        public void Initialize()
        {
            // start the network read task
            Task.Run(async () =>
            {
                try
                {
                    await Worker_NetworkRead();
                }
                catch (IOException ioex)
                {
                    // forcefully disconnect the user since the connection has failed
                    Disconnect($"Network IO Error: {ioex.Message}", true);
                }
            });

			// login status means we are waiting for client to log in
			Status = ClientStatus.Login;
        }

        public void Tick()
        {
            if (Status == ClientStatus.Uninitialized) throw new InvalidOperationException("User not initialized");

            // handle packets
            while (!_packetQueue.IsEmpty)
            {
                if (_packetQueue.TryDequeue(out var packet))
                {
                    HandlePacket(packet);
                }
            }

            // todo: heartbeats

            // todo: login timeout - user must send a login packet n seconds after a handshake
        }

        /// <summary>
        /// Sets the username of the client, not implemented
        /// </summary>
        /// <param name="username"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void SetUsername(string username)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a message to the client
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Message message)
        {
            _networkClient.WritePacket(new CMessagePacket()
            {
                AuthorID = message.Author.ID,
                Content = message.Content
            });
        }

        /// <summary>
        /// Used for sending packets that are not handled by another method
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(Packet packet)
        {
            _networkClient.WritePacket(packet);
        }

        /// <summary>
        /// Disconnects the user from the server
        /// </summary>
        /// <param name="force">Whether to forcefully disconnect the user and skip sending disconnect packet</param>
        public void Disconnect(string reason, bool force)
        {
            // only disconnect the user once
            lock (_disconnectLock)
            {
                if (_disconnecting) return;
                _disconnecting = true;
            }

            Status = ClientStatus.Disconnected;

            // send the client a disconnect message
            if (!force && Status == ClientStatus.Ready)
            {
                try
                {
                    _networkClient.WritePacket(new CDisconnectPacket() { Reason = reason });
                }
                catch (IOException)
                {
                    // eat io exceptions because they may happen during disconnections
                }
            }

            _networkClient?.Close();

            // raise disconnect event
            OnDisconnect?.Invoke(this, null);
        }

        private void HandlePacket(Packet packet)
        {
            switch (packet)
            {
                case SLoginPacket loginPacket:
                    Packet_Login(loginPacket);
                    break;

                case SMessagePacket messagePacket:
                    Packet_Message(messagePacket);
                    break;

                default:
                    throw new NotImplementedException($"Unable to handle packet of type {packet.GetType().Name}");
            }
        }

        private void Packet_Message(SMessagePacket messagePacket)
        {
            // turn packet data into a message
            var message = new Message(this, messagePacket.Content);

            OnMessage?.Invoke(this, message);
        }

        private void Packet_Login(SLoginPacket packet)
        {
            // check for valid user login
            (var validLogin, string loginFailReason) = _server.UserLoginRequest(packet.UserID, packet.Username);

			if (!validLogin)
            {
                Disconnect($"Failed to login: ${loginFailReason}", false);
                return;
            }

            // set user id
            ID = packet.UserID;

            // user is logged in
            Status = ClientStatus.Ready;

            /**
             * Some explanation is needed here:
             * When we fire the OnLogin event, the server will broadcast a CUserJoined packet
             * to _all_ clients, including this one since the Status is Ready. The client will
             * use this broadcasted packet as confirmation that it has joined successfully.
             * */
            OnLogin?.Invoke(this, null);

            // send the user list to this user
            var onlineUsers = _server.Users.Where(u => u.Status == ClientStatus.Ready).ToArray();

            Debug($"Sending user list of {onlineUsers.Length} users to {this}");

            _networkClient.WritePacket(new CUserListPacket()
            {
                Users = onlineUsers
            });
        }

        private async Task Worker_NetworkRead()
        {
            // we could change the method return type to void
            // but this makes it easier to await an async task
            // inside this method later +
            // it maintains the same pattern used elsewhere in
            // the code where tasks are awaited inside workers

            // wrap this in a task wrapper
            await Task.Run(() =>
            {
                while (Status == ClientStatus.Ready)
                {
                    // read next packet from network
                    var packet = _networkClient.ReadPacket();

                    // add packet to packet queue
                    _packetQueue.Enqueue(packet);
                }
            });
        }

        private void Debug(string message)
        {
            OnDebugMessage?.Invoke(this, message); 
        }
    }

    public enum ClientStatus : int
    {
        Uninitialized = -1,
        Login = 1,
        Ready = 2,
        Disconnected = 3,
    }
}
