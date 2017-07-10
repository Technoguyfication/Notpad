using Notpad.Client.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
		public event MessageEventHandler Notification;

		public event EventHandler ConnectionEstablished;
		public event EventHandler ConnectionDisconnected;

		public NetClient(string username) : base()
		{
			Username = username;
			CurrentState = ConnectionState.UNINITIALIZED;
		}

		new public void Connect(IPEndPoint endpoint)
		{
			try
			{
				base.Connect(endpoint);
			}
			catch (Exception e)
			{
				SendMessage($"Failed to connect to {endpoint.ToString()}: {e.Message}");
				return;
			}
			SendMessage($"Connected to {endpoint.Address.ToString()}");
		}

		public void Disconnect()
		{
			try
			{
				Stream.Close();
				Disconnect();
			}
			catch (Exception e)
			{
				SendMessage($"Error closing connection: {e.Message}");
			}
			ConnectionDisconnected?.Invoke(this, EventArgs.Empty);
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
					if (CurrentState == ConnectionState.UNINITIALIZED)
						Write(GetIdentifyPacket(Username));
					else
						SendMessage("Warning: Handshake packet received when client was not in ready state!");
					break;
				case (byte)SCPackets.MESSAGE:
					if (CurrentState != ConnectionState.READY)
						break;

					bool broadcast = BitConverter.ToBoolean(payloadList.GetByteInByteCollection().CheckEndianness(), 0);
					int authorLength = payloadList.GetNextInt();
					string author = Encoding.Unicode.GetString(payloadList.GetBytes(authorLength));
					int messageLength = payloadList.GetNextInt();
					string message = Encoding.Unicode.GetString(payloadList.GetBytes(messageLength));
					SendMessage((broadcast) ? $"[SERVER] {message}" : $"{author}: {message}");
					break;
				case (byte)SCPackets.READY:
					if (CurrentState == ConnectionState.READY)
						SendMessage("Warning: Server tried changing connection state to an invalid state.");
					CurrentState = ConnectionState.READY;
					ConnectionEstablished?.Invoke(this, EventArgs.Empty);
					break;
				case (byte)SCPackets.NOTIFICATION:

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

		private void SendMessage(string message)
		{
			Message?.Invoke(this, new MessageEventArgs(message));
		}

		private void SendNotification(string message)
		{
			Notification?.Invoke(this, new MessageEventArgs(message));
		}
	}
}

public class MessageEventArgs : EventArgs
{
	public string Content { get; set; }
	public MessageEventArgs()
	{

	}

	public MessageEventArgs(string content)
	{
		Content = content;
	}
}

public enum ConnectionState
{
	UNINITIALIZED,  // no handshake received
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
