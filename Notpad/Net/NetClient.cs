using Notpad.Client.Net;
using Notpad.Client.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notpad.Client.Net
{
	public class NetClient : TcpClient, IStreamable
	{
		public object StreamReadLock { get; } = new object();
		public object StreamWriteLock { get; } = new object();
		public NetworkStream Stream
		{
			get
			{
				return GetStream();
			}
		}

		public string Username { get; private set; }
		public ConnectionState CurrentState { get; private set; } = ConnectionState.DISCONNECTED;

		public delegate void MessageEventHandler(object sender, MessageEventArgs e);
		public delegate void ServerQueryReceivedEventHandler(object sender, ServerQueryReceivedEventArgs e);
		public delegate void ConnectionDisconnectedEventHandler(object sender, ConnectionDisconnectedEventArgs e);

		public event MessageEventHandler Message;
		public event ServerQueryReceivedEventHandler ServerQueryReceived;
		public event EventHandler ConnectionEstablished;
		public event ConnectionDisconnectedEventHandler ConnectionDisconnected;

		private Thread ReadLoopThread;

		public NetClient(string username) : base()
		{
			Username = username;
			CurrentState = ConnectionState.DISCONNECTED;
		}

		~NetClient()
		{
			Disconnect("Client being disposed");
		}

		/// <summary>
		/// Connect to the specified <see cref="IPEndPoint"/>
		/// </summary>
		/// <param name="endpoint"></param>
		new public void Connect(IPEndPoint endpoint)
		{
			if (CurrentState != ConnectionState.DISCONNECTED)
				Disconnect("Connection already disconnected");

			CurrentState = ConnectionState.UNVERIFIED;
			try
			{
				base.Connect(endpoint);
			}
			catch (Exception)
			{
				Disconnect("Failed to connect to server");
				throw;
			}

			if (ReadLoopThread != null && ((int)ReadLoopThread.ThreadState % 16) == 0)
				ReadLoopThread.Abort();

			ReadLoopThread = new Thread(ReadLoop)
			{
				IsBackground = true,
				Name = "Network Listener",
			};

			ReadLoopThread.Start();
		}

		public void Disconnect()
		{
			Disconnect(null);
		}

		public void Disconnect(string reason)
		{
			try
			{
				if (Client.Connected)
					Client.Disconnect(true);

				if (ReadLoopThread != null && ((int)ReadLoopThread.ThreadState % 16) == 0)
					ReadLoopThread.Abort();
			}
			catch (Exception e)
			{
				SendMessage(MessageType.CHAT, $"Error closing connection: {e.Message}");
			}
			if (CurrentState != ConnectionState.DISCONNECTED)
			{
				CurrentState = ConnectionState.DISCONNECTED;
				ConnectionDisconnected?.Invoke(this, new ConnectionDisconnectedEventArgs()
				{
					Reason = reason
				});
			}
		}

		#region Packet Factory

		public Packet GetQueryPacket()
		{
			return new Packet((byte)CSPackets.QUERY);
		}

		public Packet GetHandshakePacket()
		{
			return new Packet((byte)CSPackets.HANDSHAKE);
		}

		public Packet GetIdentifyPacket(string username)
		{
			byte[] usernameBytes = Encoding.Unicode.GetBytes(username);
			List<byte> builder = new List<byte>();
			builder.AddRange(BitConverter.GetBytes(usernameBytes.Length).CheckEndianness());
			builder.AddRange(usernameBytes);
			return new Packet((byte)CSPackets.IDENTIFY, builder.ToArray());
		}

		public Packet GetMessagePacket(string message)
		{
			byte[] messageBytes = Encoding.Unicode.GetBytes(message);
			List<byte> builder = new List<byte>();
			builder.AddRange(BitConverter.GetBytes(messageBytes.Length).CheckEndianness());
			builder.AddRange(messageBytes);
			return new Packet((byte)CSPackets.MESSAGE, builder.ToArray());
		}

		#endregion

		private void HandlePacket(byte type, byte[] payload)
		{
			List<byte> payloadList = new List<byte>(payload);
			switch (type)
			{
				case (byte)SCPackets.QUERY:
					if ((int)CurrentState % 8 == 0)
						break;

					int nameLength = payloadList.GetNextInt();
					string name = Encoding.Unicode.GetString(payloadList.GetBytes(nameLength));
					int maxOnline = payloadList.GetNextInt();
					int online = payloadList.GetNextInt();

					ServerQueryReceived?.Invoke(this,
						new ServerQueryReceivedEventArgs()
						{
							Server = new Server()
							{
								Name = name,
								MaxOnline = maxOnline,
								Online = online,
								Status = ServerStatus.ONLINE,
								Endpoint = (IPEndPoint)Client.RemoteEndPoint
							}
						});
					break;
				case (byte)SCPackets.HANDSHAKE:
					if (CurrentState != ConnectionState.UNVERIFIED)
						break;

					Write(GetIdentifyPacket(Username));
					break;
				case (byte)SCPackets.MESSAGE:
					if (CurrentState != ConnectionState.READY)
						break;

					bool broadcast = BitConverter.ToBoolean(payloadList.GetByteInByteCollection().CheckEndianness(), 0);
					int authorLength = payloadList.GetNextInt();
					string author = Encoding.Unicode.GetString(payloadList.GetBytes(authorLength));
					int messageLength = payloadList.GetNextInt();
					string message = Encoding.Unicode.GetString(payloadList.GetBytes(messageLength));
					SendMessage(
						(broadcast) ? MessageType.BROADCAST : MessageType.CHAT,
						message,
						author);
					break;
				case (byte)SCPackets.READY:
					if (CurrentState != ConnectionState.UNVERIFIED)
						break;

					bool ready = BitConverter.ToBoolean(payloadList.GetByteInByteCollection(), 0);

					if (!ready)
					{
						int reasonLength = payloadList.GetNextInt();
						string reason = Encoding.Unicode.GetString(payloadList.GetBytes(reasonLength));
						Disconnect($"Server refused connection: {reason}");
						break;
					}
					CurrentState = ConnectionState.READY;
					ConnectionEstablished?.Invoke(this, EventArgs.Empty);
					break;
				case (byte)SCPackets.NOTIFICATION:
					if (CurrentState != ConnectionState.READY)
						break;

					MessageBoxIcon[] iconMap = new MessageBoxIcon[]
					{
						MessageBoxIcon.None,
						MessageBoxIcon.Information,
						MessageBoxIcon.Warning,
						MessageBoxIcon.Error,
					};
					int level = payloadList.GetNextInt();
					int contentLength = payloadList.GetNextInt();
					string content = Encoding.Unicode.GetString(payloadList.GetBytes(contentLength));
					SendMessage(new MessageEventArgs()
					{
						Content = content,
						Type = MessageType.NOTIFICATION,
						NotificationIcon = iconMap[level],
					});
					break;
				default:
					break;
			}
		}

		private void HandlePacket(Packet packet)
		{
			HandlePacket(packet.PacketID, packet.Payload);
		}

		private void ReadLoop()
		{
			while (CurrentState != ConnectionState.DISCONNECTED)
			{
				lock (StreamReadLock)
				{
					try
					{
						Packet packet = this.GetNextPacket();
						HandlePacket(packet);
					}
					catch (DisconnectedException)
					{
						Disconnect("Connection closed by remote host.");
						break;
					}
					catch (Exception e)
					{
						Disconnect($"Error reading server stream: {e.Message}");
						break;
					}
				}
			}
		}

		public void Read(byte[] buffer, int offset, int size)
		{
			lock (StreamReadLock)
			{
				int bytesRead = 0;
				while (bytesRead < size)
				{
					int currentBytesRead = Stream.Read(buffer, offset + bytesRead, size - bytesRead);
					if (currentBytesRead == 0 && size != 0)
					{
						throw new DisconnectedException("Failed to read from network stream");
					}
					bytesRead += currentBytesRead;
				}
			}
		}

		public void Write(byte[] buffer, int offset, int size)
		{
			lock (StreamWriteLock)
			{
				Stream.Write(buffer, offset, size);
			}
		}

		public void Write(byte[] buffer)
		{
			Write(buffer, 0, buffer.Length);
		}

		public void Write(Packet packet)
		{
			Write(packet.Raw);
		}

		private void SendMessage(MessageType type, string content, string author = null)
		{
			SendMessage(new MessageEventArgs()
			{
				Type = type,
				Content = content,
				Author = author
			});
		}

		private void SendMessage(MessageEventArgs e)
		{
			Message?.Invoke(this, e);
		}
	}
}

public class MessageEventArgs : EventArgs
{
	public MessageType Type { get; set; }
	public string Author { get; set; }
	public string Content { get; set; }
	public MessageBoxIcon NotificationIcon { get; set; }
}

public class ServerQueryReceivedEventArgs : EventArgs
{
	public Server Server;
}

public class ConnectionDisconnectedEventArgs : EventArgs
{
	public string Reason;
}

public enum MessageType
{
	BROADCAST,
	RAW,
	CHAT,
	NOTIFICATION,
}

/// <summary>
/// Statuses divisible by 5 are not open, divisible by 8 are open.
/// </summary>
public enum ConnectionState : int
{
	DISCONNECTED = 5,
	UNVERIFIED = 8,     // attempting to start full connection
	READY = 16,         // client ready
}

public enum CSPackets : byte
{
	QUERY = 0xFF,

	HANDSHAKE = 0x00,
	IDENTIFY = 0x01,
	MESSAGE = 0x02,
}

public enum SCPackets : byte
{
	QUERY = 0xFF,

	HANDSHAKE = 0x00,
	READY = 0x01,
	MESSAGE = 0x02,
	NOTIFICATION = 0x03,
}

[Serializable]
public class DisconnectedException : Exception
{
	public DisconnectedException() { }
	public DisconnectedException(string message) : base(message) { }
	public DisconnectedException(string message, Exception inner) : base(message, inner) { }
	protected DisconnectedException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
