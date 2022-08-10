using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Net
{
    public class User
    {
        public string Username { get; set; }
        public IPEndPoint RemoteEndpoint { get; set; }
        public ClientStatus Status { get; set; }

        private readonly NetworkClient _tcpClient;
        private readonly BaseServer _server;

        /// <summary>
        /// Instantiate an instance of the TCP client
        /// </summary>
        /// <param name="client"></param>
        public User(NetworkClient client, BaseServer server)
        {
            _tcpClient = client;
            _server = server;
        }

        /// <summary>
        /// Disconnects the client from the server
        /// </summary>
        public void Disconnect()
        {
            _tcpClient?.Close(); 
        }

        public enum ClientStatus : int
        {
            Login = 1,
            Ready = 2,
            Disconnected = 3,
        }
    }
}
