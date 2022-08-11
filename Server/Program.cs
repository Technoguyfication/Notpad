using System;
using System.Net;
using Technoguyfication.Notpad.Shared.Net.Server;

namespace Technoguyfication.Notpad.Dedicated
{
    static class Program
	{
		static DedicatedServer _server;

#pragma warning disable IDE0060
		static void Main(string[] args)
#pragma warning restore IDE0060
		{
			_server = new DedicatedServer();

			_server.OnDiscoveryRequest += Server_OnDiscoveryRequest;

			_server.OnQuery += (sender, args) =>
			{
				Console.WriteLine("Server query received");
			};

			_server.Start(42069, IPAddress.Any);

			Console.WriteLine("Server running on port 42069");

			Console.ReadLine();

			_server.Stop(false);
		}

		private static void Server_OnDiscoveryRequest(object sender, BaseServer.DiscoveryEventArgs e)
		{
			Console.WriteLine($"Discovery from {e.RemoteEndPoint}");
		}
	}
}
