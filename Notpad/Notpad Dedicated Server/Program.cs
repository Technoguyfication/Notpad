using System;
using System.Net;
using Technoguyfication.Notpad.Server.Net;
using Technoguyfication.Notpad.Shared;

namespace Technoguyfication.Notpad.Server
{
	static class Program
	{
		static DedicatedServer _server;

		static void Main(string[] args)
		{
			_server = new DedicatedServer();

			_server.DiscoveryEvent += _server_DiscoveryEvent;

			_server.Start(42069, IPAddress.Any);

			Console.WriteLine("Server running on port 42069");

			Console.ReadLine();

			_server.Stop();
		}

		private static void _server_DiscoveryEvent(object sender, BaseServer.DiscoveryEventArgs e)
		{
			Console.WriteLine($"Discovery from {e.RemoteEndPoint.ToString()}");
		}
	}
}
