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
			_server = new DedicatedServer("Testing Server", "I have no idea what to write as a MOTD. Why did I even write this feature?", 69);
			_server.Start(42069, IPAddress.Any);

			// set up event handlers
			_server.OnDebugMessage += (s, e) => Console.WriteLine(e.ToString());

			Console.WriteLine("Server running on port 42069");
			Console.ReadLine();	// pause here

			_server.Stop(false);
		}
	}
}
