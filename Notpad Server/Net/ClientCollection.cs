using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Notpad.Server.Net
{
	public class ClientCollection : List<RemoteClient>
	{
		public int UsersOnline
		{
			get
			{
				return Count;
			}
		}

		public int BroadcastToClients(Packet packet)
		{
			return BroadcastToClients(packet.Raw);
		}

		public int SendMessage(bool server, string message, string author = "")
		{
			if (server)
				Console.WriteLine($"(GLOBAL): {message}");
			else
				Console.WriteLine($"Message: ({author}) {message}");

			return BroadcastToClients(RemoteClient.GetMessagePacket(server, message, author));
		}

		public int BroadcastToClients(byte[] buffer)
		{
			List<RemoteClient> brokenClients = new List<RemoteClient>();
			int affected = 0;
			lock (this)
			{
				foreach (RemoteClient client in this)
				{
					try
					{
						client.Write(buffer);
						affected++;
					}
					catch (Exception)
					{
						brokenClients.Add(client);
					}
				}
			}
			foreach (RemoteClient client in brokenClients)
				client.Disconnect(false, "Client connection closed unexpectedly.");

			return affected;
		}

		public void AddClient(RemoteClient client)
		{
			if (Exists(client))
				throw new Exception("Client already added!");

			lock (this)
			{
				Add(client);
			}

			client.Disconnected += ClientDisconnected;
			client.PacketReceived += ClientPacketReceived;
			client.ClientReady += ClientReady;

			if (client.ListenThread != null)
				client.ListenThread.Abort();

			client.ListenThread = new Thread(client.ListenLoop)
			{
				IsBackground = true,
				Name = "Client listen thread",
			};
			client.ListenThread.Start();
		}

		public bool Exists(RemoteClient client)
		{
			lock (this)
			{
				return Exists((c) => { return c.UniqueID == client.UniqueID; });
			}
		}

		public int GetClientIndex(RemoteClient client)
		{
			return FindIndex((c) =>
			{
				return c.UniqueID == client.UniqueID;
			});
		}

		public void RemoveClient(RemoteClient client, string reason = null)
		{
			lock (this)
			{
				if (!Exists(client))
					return;

				RemoveClientAt(GetClientIndex(client), reason);
			}
		}

		public void RemoveClientAt(int index, string reason = null)
		{
			if (Count > index)
			{
				lock (this)
				{
					RemoveAt(index);
				}
			}
		}

		private void ClientReady(object sender, EventArgs e)
		{
			SendMessage(true, $"{((RemoteClient)sender).Username} has connected.");
		}

		private void ClientPacketReceived(object sender, ClientPacketReceivedEventArgs e)
		{
			HandlePacket(e.Packet, e.Client);
		}

		private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
		{
			string clientName = e.Client.Username;
			RemoveClient(e.Client);
			e.Client.Dispose();
			SendMessage(true, $"\"{clientName}\" disconnected: {e.Reason ?? "Unknown"}");
		}

		private void HandlePacket(Packet packet, RemoteClient client)
		{
			List<byte> payload = new List<byte>(packet.Payload);

			switch (packet.PacketID)
			{
				case (byte)CSPackets.QUERY:
					if ((int)client.CurrentState % 8 != 0)
						break;

					Console.WriteLine($"Query from {client}");

					client.Write(RemoteClient.GetQueryPacket(
						Program.Settings.Name,
						UsersOnline,
						Program.Settings.MaxUsers));

					break;
				case (byte)CSPackets.MESSAGE:
					if (client.CurrentState != ClientConnectionState.READY)
						break;

					int messageLength = payload.GetNextInt();
					string message = Encoding.Unicode.GetString(payload.GetBytes(messageLength));

					SendMessage(false, message, client.Username);
					break;
				case (byte)CSPackets.IDENTIFY:
					if (client.CurrentState != ClientConnectionState.CONNECTED)
						break;

					int usernameLength = payload.GetNextInt();
					string username = Encoding.Unicode.GetString(payload.GetBytes(usernameLength));

					Regex specialChars = new Regex(RemoteClient.SPECIAL_CHAR_REGEX);

					if (username.Length > 20 || specialChars.IsMatch(username))
					{
						client.Username = $"({specialChars.Replace(username.Truncate(20), string.Empty)})";
						client.Disconnect(true, "Invalid username.");
					}

					lock (this)
					{
						string[] usernames = new string[Count];
						for (int i = 0; i < Count; i++)
						{
							usernames[i] = this[i].Username;
						}

						if (usernames.Contains(username))
						{
							client.Username = $"({username})";
							client.Disconnect(true, "Username already in use.");
							break;
						}
					}

					client.Username = username;
					client.Write(RemoteClient.GetReadyPacket());
					client.ChangeClientState(ClientConnectionState.READY);
					Console.WriteLine($"Client {client.ToString()} identified as \"{client.Username}\"");
					break;
				case (byte)CSPackets.DISCONNECT:
					int reasonLength = payload.GetNextInt();
					string reason = Encoding.Unicode.GetString(payload.GetBytes(reasonLength));
					client.Disconnect(false, reason);
					break;
			}
		}
	}
}
