using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Net;
using Technoguyfication.Notpad.Net.Packets;
using Technoguyfication.Notpad.Shared.Net;

namespace Technoguyfication.Notpad.Shared
{
    public class BaseServer
    {
        public event EventHandler<string> OnDebugMessage;

        public event EventHandler<DiscoveryEventArgs> OnDiscoveryRequest;
        public event EventHandler OnQuery;
        public event EventHandler<User> OnClientJoin;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly List<User> _clients;

        private TcpListener _listener;
        private UdpClient _udpClient;

        private Task _discoveryListenerTask;
        private Task _clientListenerTask;

        public BaseServer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _clients = new List<User>();
        }

        public void Start(int port, IPAddress bindAddress)
        {
            // start tcp listener
            _listener = new TcpListener(bindAddress, port);
            _listener.Start();

            // start udp client to listen for broadcast messages
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));

            // create and start tasks to handle listener events
            _discoveryListenerTask = new Task(async () => await DiscoveryListener(), _cancellationTokenSource.Token);
            _clientListenerTask = new Task(async () => await NewClientListener(), _cancellationTokenSource.Token);

            _discoveryListenerTask.Start();
            _clientListenerTask.Start();
        }

        public void Stop()
        {
            // stop listeners if they exist
            _udpClient?.Close();
            _listener?.Stop();

            // cancel the cancellation token for this server
            // this will also stop listener tasks
            _cancellationTokenSource.Cancel();
        }

        private async Task ClientListener()
        {
            // todo: this
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
                    OnDebugMessage?.Invoke(this, $"Socket at {socket.RemoteEndPoint} failed to become a client: {ex.Message}");
                }

                // handle packet based on packet type
                switch (packet)
                {
                    case SQueryPacket:
                        OnQuery?.Invoke(this, null);
                        break;
                    default:
                        continue;
                }

                //_clients.Add(client);

                //OnClientJoin?.Invoke(this, client);
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
