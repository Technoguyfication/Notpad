using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Client.Net
{
	class NetClient : TcpClient
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

		public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
		public event MessageReceivedEventHandler MessageReceived;

		public NetClient(string username) : base()
		{
			Username = username;
			CurrentState = ConnectionState.UNINITIALIZED;
		}

		private void HandlePacket(byte type, byte[] payload)
		{
			List<byte> payloadList = new List<byte>(payload);
			switch (type)
			{
				case (byte)SCPackets.HANDSHAKE:
					if (CurrentState == ConnectionState.UNINITIALIZED)
						Write(GetIdentifyPacket(Username));
					else
					{

					}
					break;
				case (byte)SCPackets.MESSAGE:
					bool broadcast = BitConverter.ToBoolean(payloadList.GetByteInByteCollection().CheckEndianness(), 0);
					int authorLength = payloadList.GetNextInt();
					string author = Encoding.Unicode.GetString(payloadList.GetBytes(authorLength));
					int messageLength = payloadList.GetNextInt();
					string message = Encoding.Unicode.GetString(payloadList.GetBytes(messageLength));
					MessageReceived?.Invoke(this, new MessageReceivedEventArgs(broadcast, author, message));
					break;
			}
		}

		private void HandlePacket(byte[] buffer)
		{
			HandlePacket(buffer[0], buffer.ToList().Skip(1).ToArray());
		}

		#region Packets

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

		public void Read(byte[] buffer, int offset, int size)
		{
			lock (StreamReadLock)
			{
				Stream.Read(buffer, offset, size);
			}
		}

		public void Write(byte[] buffer, int offset, int size)
		{
			lock (StreamWriteLock)
			{
				Stream.Write(buffer, offset, size);
			}
		}

		public void Write(Packet packet)
		{
			Write(packet.Raw, 0, packet.Length);
		}
	}
}

public class MessageReceivedEventArgs : EventArgs
{
	public bool Broadcast { get; set; }
	public string Author { get; set; }
	public string Message { get; set; }

	public MessageReceivedEventArgs()
	{

	}

	public MessageReceivedEventArgs(bool Broadcast, string Author, string Message)
	{

	}
}

public enum ConnectionState
{
	UNINITIALIZED,	// no handshake received
	READY,			// client ready
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
