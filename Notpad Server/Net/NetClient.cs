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
	public class NetClient : IStreamable, IDisposable
	{
		public delegate void ClientPacketReceivedEventHandler(object sender, ClientPacketReceivedEventArgs e);
		public event ClientPacketReceivedEventHandler PacketReceived;
		public delegate void ClientDisconnectedEventHandler(object sender, ClientDisconnectedEventArgs e);
		public event ClientDisconnectedEventHandler Disconnected;

		public readonly object StreamReadLock = new object();
		public readonly object StreamWriteLock = new object();

		public string Username { get; set; }

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

		public NetClient(TcpClient client)
		{
			Client = client;
			Endpoint = (IPEndPoint)Client.Client.RemoteEndPoint;
			Console.WriteLine($"Connecting client {Endpoint.ToString()}");
			ChangeClientState((client.Connected) ? ClientConnectionState.CONNECTED : ClientConnectionState.DISCONNECTED);
		}

		~NetClient()
		{
			Dispose();
		}

		public override string ToString()
		{
			return Endpoint.ToString();
		}

		public void Dispose()
		{
			if ((int)CurrentState % 5 != 0)
				Disconnect();

			Client.Close();
			Client = null;
		}

		public void Disconnect(string reason = null)
		{
			try
			{
				Write(GetDisconnectPacket());
			}
			catch (Exception) { }

			try
			{
				if (Client.Connected)
					Client.Close();
			}
			catch (Exception) { }

			if (ListenThread != null)
				ListenThread.Abort();

			if ((int)CurrentState % 5 != 0)
			{
				ChangeClientState(ClientConnectionState.DISCONNECTED);
				Disconnected?.Invoke(this, new ClientDisconnectedEventArgs()
				{
					Reason = reason,
					Client = this,
				});
			}
		}

		public void ChangeClientState(ClientConnectionState state)
		{
			CurrentState = state;
			Console.WriteLine($"Client {ToString()} state changed to ({state.ToString()})");
		}

		public void Write(Packet packet)
		{
			Write(packet.Raw);
		}

		public void Write(byte[] buffer)
		{
			Write(buffer, 0, buffer.Length);
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
					catch (Exception e)
					{
						Disconnect(e.Message);
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

	public class ClientPacketReceivedEventArgs : EventArgs
	{
		public NetClient Client { get; set; }
		public Packet Packet { get; set; }
	}

	public class ClientDisconnectedEventArgs : EventArgs
	{
		public NetClient Client { get; set; }
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

		HANDSHAKE = 0x00,
		IDENTIFY = 0x01,
		MESSAGE = 0x02,
		DISCONNECT = 0xF0,
	}

	public enum SCPackets : byte
	{
		QUERY = 0xFF,

		HANDSHAKE = 0x00,
		READY = 0x01,
		MESSAGE = 0x02,
		NOTIFICATION = 0x03,
		DISCONNECT = 0xF0,
	}
}
