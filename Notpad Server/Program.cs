using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading;

namespace Notpad.Server
{
	class Program
	{
		public static readonly string ConfigPath = Path.GetFullPath("config.json");
		public static ServerSettings Settings;

		public TcpListener Listener { get; private set; }
		public Thread ListenThread { get; private set; }

		static void Main(string[] args)
		{
			string configJson;
			try
			{
				configJson = File.ReadAllText(ConfigPath);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine($"Config file could not be found at {ConfigPath}");
				return;
			}
			catch (IOException e)
			{
				Console.WriteLine($"Failed to open config file: {e.Message}");
				return;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Unhandled exception loading config: {e.Message}");
				return;
			}

			Settings = JsonConvert.DeserializeObject<ServerSettings>(configJson);

			new Program().Start();
		}

		public void Start()
		{
			Listener = new TcpListener(Settings.IPAddress, Settings.Port);
			ListenThread = new Thread(ClientListenLoop)
			{
				IsBackground = false,
				Name = "Client Listen Loop",
			};

			ListenThread.Start();
			Console.WriteLine($"Now listening on {Settings.EndPoint.ToString()}");
		}

		private void ClientListenLoop()
		{
			Listener.Start();
			while (true)
			{
				Client client = new Client(Listener.AcceptTcpClient());
			}
		}
	}
}
