using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notpad.Server
{
	public class ClientCollection : List<NetClient>
	{
		public int Online
		{
			get
			{
				return Count;
			}
		}

		public void BroadcastToClients(Packet packet)
		{
			BroadcastToClients(packet.Raw);
		}

		public void BroadcastToClients(byte[] buffer)
		{
			lock (this)
			{
				foreach (NetClient client in this)
				{
					client.Write(buffer);
				}
			}
		}

		public void AddClient(NetClient client)
		{
			if (Exists(client))
				throw new Exception("Client already added!");

			lock (this)
			{
				Add(client);
			}

			client.Disconnected += ClientDisconnected;
			client.PacketReceived += ClientPacketReceived;

			if (client.ListenThread != null && client.ListenThread.ThreadState != ThreadState.Stopped)
				client.ListenThread.Abort();

			client.ListenThread = new Thread(client.ListenLoop)
			{
				IsBackground = true,
				Name = "Client listen thread",
			};
			client.ListenThread.Start();

			Console.WriteLine($"Successfully connected client {client.ToString()}");
		}

		public bool Exists(NetClient client)
		{
			return Exists((c) => { return c.UniqueID == client.UniqueID; });
		}

		public void RemoveClient(NetClient client)
		{
			if (!Exists(client))
				return;

			Find((c) =>
			{
				return c.UniqueID == client.UniqueID;
			});
		}

		public void RemoveClientAt(int index)
		{
			if (Count >= index)
			{
				lock (this)
				{
					RemoveAt(index);
				}
			}
		}

		private void ClientPacketReceived(object sender, ClientPacketReceivedEventArgs e)
		{
			HandlePacket(e.Packet, e.Client);
		}

		private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
		{
			string clientName = e.Client.ToString();
			RemoveClient(e.Client);
			e.Client.Dispose();
			Console.WriteLine($"Client {clientName} disconnected.");
		}

		private void HandlePacket(Packet packet, NetClient client)
		{
			List<byte> payload = new List<byte>(packet.Payload);

			switch (packet.PacketID)
			{
				case (byte)CSPackets.QUERY:
					if ((int)client.CurrentState % 8 != 0)
						break;

					Console.WriteLine($"Query from {client}");

					client.Write(GetQueryPacket(
						Program.Settings.Name,
						Online,
						Program.Settings.MaxUsers));

					break;
			}
		}

		#region Packet Factory

		private static Packet GetQueryPacket(string name, int online, int maxOnline)
		{
			List<byte> builder = new List<byte>();
			byte[] serverName = Encoding.Unicode.GetBytes(name);
			builder.AddRange(BitConverter.GetBytes(serverName.Length).CheckEndianness());
			builder.AddRange(serverName);
			builder.AddRange(BitConverter.GetBytes(online).CheckEndianness());
			builder.AddRange(BitConverter.GetBytes(maxOnline));
			return new Packet((byte)CSPackets.QUERY, builder.ToArray());
		}

		#endregion
	}
}
