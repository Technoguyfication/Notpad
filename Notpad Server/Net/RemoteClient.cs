using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Threading;
using Notpad.Server;

namespace Notpad.Server.Net
{
	public class RemoteClient : IStreamable, IDisposable
	{
		public delegate void ClientPacketReceivedEventHandler(object sender, ClientPacketReceivedEventArgs e);
		public event ClientPacketReceivedEventHandler PacketReceived;
		public delegate void ClientDisconnectedEventHandler(object sender, ClientDisconnectedEventArgs e);
		public event ClientDisconnectedEventHandler Disconnected;
		public event EventHandler ClientReady;

		public readonly object StreamReadLock = new object();
		public readonly object StreamWriteLock = new object();

		private string _username = null;
		public string Username
		{
			get
			{
				if (_username != null)
					return _username;
				else
					return ToString();
			}
			set
			{
				_username = value;
			}
		}
		public const string SPECIAL_CHAR_REGEX = @"[^\w]";

		public ClientConnectionState CurrentState = ClientConnectionState.DISCONNECTED;

		public TcpClient Client { get; set; }
		public Thread ListenThread { get; set; }
		public IPEndPoint Endpoint { get; set; }
		public NetworkStream Stream
		{
			get
			{
				return Client.GetStream();
			}
		}
		public string UniqueID
		{
			get
			{
				SHA256 sha = SHA256.Create();
				sha.Initialize();
				byte[] hash = sha.ComputeHash(Encoding.Unicode.GetBytes(Endpoint.ToString()));
				return BitConverter.ToString(hash).Replace("-", "");
			}
		}

		public RemoteClient(TcpClient client)
		{
			Client = client;
			Endpoint = (IPEndPoint)Client.Client.RemoteEndPoint;
			Console.WriteLine($"Connecting client {Endpoint.ToString()}");
			ChangeClientState((client.Connected) ? ClientConnectionState.CONNECTED : ClientConnectionState.DISCONNECTED);
		}

		~RemoteClient()
		{
			Dispose();
		}

		public void Dispose()
		{
			if ((int)CurrentState % 5 != 0)
				Disconnect(false);

			if (Client != null)
				Client.Close();
			Client = null;
		}

		public override string ToString()
		{
			return Endpoint.ToString();
		}

		public void Disconnect(bool sendToClient, string reason = "")
		{
			if ((int)CurrentState % 5 != 0)
				ChangeClientState(ClientConnectionState.DISCONNECTED);
			else
				return;

			try
			{
				if (sendToClient)
				{
					lock (StreamWriteLock)
					{
						Write(GetDisconnectPacket(reason));
					}
				}
			}
			catch (Exception) { }

			try
			{
				if (Client.Connected)
					Client.Close();
			}
			catch (Exception) { }

			Disconnected?.Invoke(this, new ClientDisconnectedEventArgs()
			{
				Reason = reason,
				Client = this,
			});
		}

		public void ChangeClientState(ClientConnectionState state)
		{
			if (CurrentState != state && state == ClientConnectionState.READY)
				ClientReady?.Invoke(this, EventArgs.Empty);

			CurrentState = state;
			Console.WriteLine($"Client {ToString()} state changed to ({state.ToString()})");
		}

		public void Write(Packet packet)
		{
			Write(packet.Raw);
		}

		public void Write(byte[] buffer)
		{
			lock (StreamWriteLock)
			{
				Write(buffer, 0, buffer.Length);
			}
		}

		public void Write(byte[] buffer, int offset, int size)
		{
			Stream.Write(buffer, offset, size);
		}

		public void Read(byte[] buffer, int offset, int size)
		{
			int bytesRead = 0;
			lock (StreamReadLock)
			{
				while (bytesRead < size)
				{
					int newBytesRead = Stream.Read(buffer, offset + bytesRead, size - bytesRead);
					if (newBytesRead <= 0 && size != 0)
					{
						throw new Exception("Failed to read from the network stream.");
					}
					bytesRead += newBytesRead;
				}
			}
		}

		public void ListenLoop()
		{
			while ((int)CurrentState % 8 == 0)
			{
				lock (StreamReadLock)
				{
					Packet packet;
					try
					{
						packet = this.GetNextPacket();
					}
					catch (Exception)
					{
						new Thread(() =>
						{
							Disconnect(false, "Connection closed by remote host");
						})
						{
							Name = "Client Disconnect Thread",
							IsBackground = true,
						}.Start();
						return;
					}
					new Thread(() =>
					{
						PacketReceived?.Invoke(this, new ClientPacketReceivedEventArgs()
						{
							Client = this,
							Packet = packet,
						});
					})
					{
						IsBackground = true,
						Name = "Packet Handler",
					}.Start();
				}
			}
		}

		#region Packet Factory

		public static Packet GetDisconnectPacket(string reason)
		{
			List<byte> builder = new List<byte>();
			byte[] reasonRaw = Encoding.Unicode.GetBytes(reason);
			builder.AddRange(BitConverter.GetBytes(reasonRaw.Length).CheckEndianness());
			builder.AddRange(reasonRaw);
			return new Packet((byte)SCPackets.DISCONNECT, builder.ToArray());
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

		public static Packet GetReadyPacket()
		{
			return new Packet((byte)SCPackets.READY);
		}

		public static Packet GetNotificationPacket(NotificationLevel level, string message)
		{
			List<byte> builder = new List<byte>();
			builder.AddRange(BitConverter.GetBytes((int)level).CheckEndianness());
			byte[] messageRaw = Encoding.Unicode.GetBytes(message);
			builder.AddRange(BitConverter.GetBytes(messageRaw.Length).CheckEndianness());
			builder.AddRange(messageRaw);
			return new Packet((byte)SCPackets.NOTIFICATION, builder.ToArray());
		}

		#endregion
	}

	public class ClientPacketReceivedEventArgs : EventArgs
	{
		public RemoteClient Client { get; set; }
		public Packet Packet { get; set; }
	}

	public class ClientDisconnectedEventArgs : EventArgs
	{
		public RemoteClient Client { get; set; }
		public string Reason { get; set; }
	}

	/// <summary>
	/// Disconnected % 5, Connected % 8
	/// </summary>
	public enum ClientConnectionState : int
	{
		DISCONNECTED = 5,
		CONNECTED = 8,
		READY = 16
	}

	public enum CSPackets : byte
	{
		QUERY = 0xFF,

		IDENTIFY = 0x01,
		MESSAGE = 0x02,
		DISCONNECT = 0xF0,
	}

	public enum SCPackets : byte
	{
		QUERY = 0xFF,

		READY = 0x01,
		MESSAGE = 0x02,
		NOTIFICATION = 0x03,
		DISCONNECT = 0xF0,
	}

	public enum NotificationLevel : int
	{
		NONE = 0,
		INFO = 1,
		WARN = 2,
		ERROR = 3,
	}
}
