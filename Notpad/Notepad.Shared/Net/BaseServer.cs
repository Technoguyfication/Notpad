using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Technoguyfication.Notpad.Shared
{
	public class BaseServer
	{
		public readonly byte[] BroadcastMessage = new byte[] { (byte)'N', (byte)'o', (byte)'t', (byte)'p', (byte)'a', (byte)'d', (byte)'D', (byte)'i', (byte)'s', (byte)'c', (byte)'o', (byte)'v', (byte)'e', (byte)'r' };

		public event DiscoveryEventHandler DiscoveryEvent;
		public delegate void DiscoveryEventHandler(object sender, DiscoveryEventArgs e);

		private TcpListener _listener;
		private UdpClient _udpClient;

		private Task _discoveryListener;

		public void Start(int port, IPAddress bindAddress)
		{
			// start tcp listener
			_listener = new TcpListener(bindAddress, port);

			// start udp client to listen for broadcast messages
			_udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));

			_discoveryListener = DiscoveryListener();
		}

		public void Stop()
		{
			_udpClient.Close();
			_udpClient = null;
		}

		private async Task DiscoveryListener()
		{
			while (_udpClient != null)
			{
				var data = await _udpClient.ReceiveAsync();
				DiscoveryEvent?.Invoke(this, new DiscoveryEventArgs() { RemoteEndPoint = data.RemoteEndPoint });

				// check if the client is a Notpad client
				if (data.Buffer.SequenceEqual(BroadcastMessage))
				{
					// respond with the multicast message
					await _udpClient.SendAsync(BroadcastMessage, BroadcastMessage.Length, data.RemoteEndPoint);
				}
			}
		}

		public class DiscoveryEventArgs : EventArgs
		{
			public IPEndPoint RemoteEndPoint { get; set; }
		}
	}
}
