using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notpad.Server.Net;

namespace Notpad.Server
{
	public class ServerManager
	{
		public TcpListener Listener { get; private set; }
		public Thread ListenThread { get; private set; }
		public ClientCollection Clients { get; set; } = new ClientCollection();

		public ServerManager()
		{
			// was i supposed to put stuff here???
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
			while (true)
			{
				try
				{
					ParseCommand(Console.ReadLine());
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error handling command: {e.Message}");
				}
			}
		}

		private void ParseCommand(string command)
		{
			if (string.IsNullOrEmpty(command))
				return;

			string[] split = command.Split(' ');
			string cmd = split[0];
			string args = string.Join(" ", split.Skip(1).ToArray());
			new Thread(() =>
			{
				switch (cmd)
				{
					case "send":
						Clients.BroadcastToClients(RemoteClient.GetMessagePacket(true, args));
						break;
					case "send!":
						Clients.BroadcastToClients(RemoteClient.GetNotificationPacket(NotificationLevel.NONE, args));
						break;
					default:
						Console.WriteLine($"Command not found: {cmd}");
						break;
				}
			})
			{
				IsBackground = true,
				Name = "Command executer",
			}.Start();
		}

		private void ClientListenLoop()
		{
			Listener.Start();
			while (true)
			{
				RemoteClient client = new RemoteClient(Listener.AcceptTcpClient());
				Clients.AddClient(client);
			}
		}
	}
}
