using Notpad.Client.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notpad.Client.Net
{
	public class NetClient : TcpClient, IStreamable
	{
		public readonly object StreamReadLock = new object();
		public readonly object StreamWriteLock = new object();
		public NetworkStream Stream
		{
			get
			{
				return GetStream();
			}
		}

		public string Username { get; private set; }

		public ConnectionState CurrentState;

		public delegate void MessageEventHandler(object sender, MessageEventArgs e);
		public event MessageEventHandler Message;

		public event EventHandler ConnectionEstablished;
		public event EventHandler ConnectionDisconnected;

		public NetClient(string username) : base()
		{
			Username = username;
			CurrentState = ConnectionState.DISCONNECTED;
		}

		public void Disconnect()
		{
			try
			{
				if (Client.Connected)
					Client.Disconnect(true);
			}
			catch (Exception e)
			{
				SendMessage(MessageType.CHAT, $"Error closing connection: {e.Message}");
			}
			CurrentState = ConnectionState.DISCONNECTED;
			ConnectionDisconnected?.Invoke(this, EventArgs.Empty);
		}

		public Server Query()
		{
			Write(GetQueryPacket());
			Packet response = this.GetNextPacket();
			List<byte> buffer = new List<byte>(response.Payload);
			int serverNameLength = buffer.GetNextInt();
			string serverName = Encoding.Unicode.GetString(buffer.GetBytes(serverNameLength));
			int online = buffer.GetNextInt();
			int maxOnline = buffer.GetNextInt();
			return new Server()
			{
				Name = serverName,
				Online = online,
				MaxOnline = online,
				Endpoint = (IPEndPoint)Client.RemoteEndPoint,
				Status = ServerStatus.ONLINE,
			};
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
				case (byte)SCPackets.HANDSHAKE:
					if (CurrentState != ConnectionState.CONNECTING)
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
					if (CurrentState != ConnectionState.CONNECTING)
						break;

					bool ready = BitConverter.ToBoolean(payloadList.GetByteInByteCollection(), 0);

					if (!ready)
					{
						int reasonLength = payloadList.GetNextInt();
						string reason = Encoding.Unicode.GetString(payloadList.GetBytes(reasonLength));
						SendMessage(MessageType.RAW, $"Server refused connection: {reason}");
						Disconnect();
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

		public void Read(byte[] buffer, int offset, int size)
		{
			lock (StreamReadLock)
			{
				int bytesRead = Stream.Read(buffer, offset, size);
				if (bytesRead == 0 && size != 0)
				{
					throw new DisconnectedException("Failed to read from network stream");
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

public enum MessageType
{
	BROADCAST,
	RAW,
	CHAT,
	NOTIFICATION,
}

public enum ConnectionState
{
	CONNECTING,		// attempting to connect
	DISCONNECTED,  // no handshake received
	READY,          // client ready
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
