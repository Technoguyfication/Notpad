using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Server
{
	public class ServerManager
	{
		public TcpListener Listener { get; private set; }
		public Thread ListenThread { get; private set; }
		public ClientCollection Clients { get; set; } = new ClientCollection();

		public ServerManager()
		{

		}

		public void Start()
		{
			Listener = new TcpListener(Program.Settings.IPAddress, Program.Settings.Port);
			ListenThread = new Thread(ClientListenLoop)
			{
				IsBackground = false,
				Name = "Client Listen Loop",
			};

			ListenThread.Start();
			Console.WriteLine($"Now listening on {Program.Settings.EndPoint.ToString()}");
		}

		private void ClientListenLoop()
		{
			Listener.Start();
			while (true)
			{
				NetClient client = new NetClient(Listener.AcceptTcpClient());
				Console.WriteLine($"Client connect request from {client.Endpoint}");
				Clients.AddClient(client);
			}
		}
	}
}
