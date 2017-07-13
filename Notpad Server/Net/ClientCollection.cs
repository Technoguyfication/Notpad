using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notpad.Server.Net
{
	public class ClientCollection : List<NetClient>
	{
		public int UsersOnline
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

			if (client.ListenThread != null)
				client.ListenThread.Abort();

			client.ListenThread = new Thread(client.ListenLoop)
			{
				IsBackground = true,
				Name = "Client listen thread",
			};
			client.ListenThread.Start();
		}

		public bool Exists(NetClient client)
		{
			lock (this)
			{
				return Exists((c) => { return c.UniqueID == client.UniqueID; });
			}
		}

		public int GetClientIndex(NetClient client)
		{
			return FindIndex((c) =>
			{
				return c.UniqueID == client.UniqueID;
			});
		}

		public void RemoveClient(NetClient client)
		{
			if (!Exists(client))
				return;

			RemoveClientAt(GetClientIndex(client));
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
						UsersOnline,
						Program.Settings.MaxUsers));

					break;
				case (byte)CSPackets.MESSAGE:
					if (client.CurrentState != ClientConnectionState.READY)
						break;

					int messageLength = payload.GetNextInt();
					string message = Encoding.Unicode.GetString(payload.GetBytes(messageLength));

					Console.WriteLine($"MESSAGE: ({client.Username}): {message}");
					BroadcastToClients(GetMessagePacket(false, message, client.Username));
					break;
				case (byte)CSPackets.IDENTIFY:
					if (client.CurrentState != ClientConnectionState.CONNECTED)
						break;

					int usernameLength = payload.GetNextInt();
					string username = Encoding.Unicode.GetString(payload.GetBytes(usernameLength));

					lock (this)
					{
						string[] usernames = new string[Count];
						for (int i = 0; i < Count; i++)
						{
							usernames[i] = this[i].Username;
						}

						if (usernames.Contains(username))
						{
							client.Write(GetReadyPacket(false, "Username already being used"));
							break;
						}
					}

					client.Username = username;
					client.Write(GetReadyPacket(true));
					Console.WriteLine($"Client {client.ToString()} identified as \"{client.Username}\"");
					client.ChangeClientState(ClientConnectionState.READY);
					break;
				case (byte)CSPackets.DISCONNECT:
					client.Disconnect("Client disconnected.");
					break;
			}
		}

		#region Packet Factory

		public static Packet GetDisconnectPacket()
		{
			return new Packet((byte)SCPackets.DISCONNECT);
		}

		public static Packet GetQueryPacket(string name, int online, int maxOnline)
		{
			List<byte> builder = new List<byte>();
			byte[] serverName = Encoding.Unicode.GetBytes(name);
			builder.AddRange(BitConverter.GetBytes(serverName.Length).CheckEndianness());
			builder.AddRange(serverName);
			builder.AddRange(BitConverter.GetBytes(online).CheckEndianness());
			builder.AddRange(BitConverter.GetBytes(maxOnline));
			return new Packet((byte)CSPackets.QUERY, builder.ToArray());
		}

		public static Packet GetMessagePacket(bool server, string content, string author = "")
		{
			List<byte> builder = new List<byte>();
			builder.AddRange(BitConverter.GetBytes(server));
			byte[] authorRaw = Encoding.Unicode.GetBytes(author);
			builder.AddRange(BitConverter.GetBytes(authorRaw.Length).CheckEndianness());
			builder.AddRange(authorRaw);
			byte[] contentRaw = Encoding.Unicode.GetBytes(content);
			builder.AddRange(BitConverter.GetBytes(contentRaw.Length).CheckEndianness());
			builder.AddRange(contentRaw);
			return new Packet((byte)SCPackets.MESSAGE, builder.ToArray());
		}

		public static Packet GetReadyPacket(bool success, string message = "")
		{
			List<byte> builder = new List<byte>();
			builder.AddRange(BitConverter.GetBytes(success));
			byte[] messageRaw = Encoding.Unicode.GetBytes(message);
			builder.AddRange(BitConverter.GetBytes(messageRaw.Length).CheckEndianness());
			builder.AddRange(messageRaw);
			return new Packet((int)SCPackets.READY, builder.ToArray());
		}

		#endregion
	}
}
