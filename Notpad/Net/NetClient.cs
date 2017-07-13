﻿using Notpad.Client.Net;
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
	public class NetClient : IStreamable, IDisposable
	{
		public object StreamReadLock { get; } = new object();
		public object StreamWriteLock { get; } = new object();
		public TcpClient Client { get; private set; }
		public NetworkStream Stream
		{
			get
			{
				return Client.GetStream();
			}
		}

		public Server CurrentServer { get; private set; }

		public string Username { get; set; }
		public ClientConnectionState CurrentState { get; private set; } = ClientConnectionState.DISCONNECTED;

		public delegate void MessageEventHandler(object sender, MessageEventArgs e);
		public delegate void ServerQueryReceivedEventHandler(object sender, ServerQueryReceivedEventArgs e);
		public delegate void ConnectionDisconnectedEventHandler(object sender, ConnectionDisconnectedEventArgs e);

		public event MessageEventHandler Message;
		public event ServerQueryReceivedEventHandler ServerQueryReceived;
		public event EventHandler ClientReady;
		public event EventHandler Connected;
		public event ConnectionDisconnectedEventHandler ConnectionDisconnected;

		private Thread ListenThread;

		public NetClient(string username)
		{
			Username = username;
			CurrentState = ClientConnectionState.DISCONNECTED;
		}

		~NetClient()
		{
			Dispose();
		}

		public void Dispose()
		{
			if ((int)CurrentState % 5 != 0)
				Disconnect("Client being disposed.");

			ListenThread.Abort();

			Client.Close();
			Client = null;
		}

		/// <summary>
		/// Connect to the specified <see cref="IPEndPoint"/>
		/// </summary>
		/// <param name="endpoint"></param>
		public void Connect(Server server)
		{
			if ((int)CurrentState % 5 != 0)
				Disconnect("Connecting to new server");

			Client = new TcpClient();

			CurrentServer = server;

			CurrentState = ClientConnectionState.UNVERIFIED;
			try
			{
				Client.Connect(server.Address, server.Port);
			}
			catch (Exception)
			{
				Disconnect("Failed to connect to server");
				throw;
			}

			if (ListenThread != null)
				ListenThread.Abort();

			ListenThread = new Thread(ReadLoop)
			{
				IsBackground = true,
				Name = "Network Listener",
			};

			ListenThread.Start();

			Connected?.Invoke(this, EventArgs.Empty);
		}

		public void Disconnect(string reason)
		{
			try
			{
				Write(GetDisconnectPacket());
			}
			catch (Exception)
			{ }

			try
			{
				Client.Close();

				if (ListenThread != null && ((int)ListenThread.ThreadState % 16) == 0)
					ListenThread.Abort();
			}
			catch (Exception e)
			{
				SendMessage(MessageType.CHAT, $"Error closing connection: {e.Message}");
			}
			if (CurrentState != ClientConnectionState.DISCONNECTED)
			{
				CurrentState = ClientConnectionState.DISCONNECTED;
				ConnectionDisconnected?.Invoke(this, new ConnectionDisconnectedEventArgs()
				{
					Reason = reason,
					Client = this,
				});
			}
		}

		#region Packet Factory

		public static Packet GetDisconnectPacket()
		{
			return new Packet((byte)CSPackets.DISCONNECT);
		}

		public static Packet GetQueryPacket()
		{
			return new Packet((byte)CSPackets.QUERY);
		}

		public static Packet GetHandshakePacket()
		{
			return new Packet((byte)CSPackets.HANDSHAKE);
		}

		public static Packet GetIdentifyPacket(string username)
		{
			byte[] usernameBytes = Encoding.Unicode.GetBytes(username);
			List<byte> builder = new List<byte>();
			builder.AddRange(BitConverter.GetBytes(usernameBytes.Length).CheckEndianness());
			builder.AddRange(usernameBytes);
			return new Packet((byte)CSPackets.IDENTIFY, builder.ToArray());
		}

		public static Packet GetMessagePacket(string message)
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

					CurrentServer.Name = name;
					CurrentServer.MaxOnline = maxOnline;
					CurrentServer.Online = online;
					CurrentServer.Status = ServerStatus.ONLINE;

					ServerQueryReceived?.Invoke(this,
						new ServerQueryReceivedEventArgs()
						{
							Server = CurrentServer,
							Client = this
						});
					break;
				case (byte)SCPackets.HANDSHAKE:
					if (CurrentState != ClientConnectionState.UNVERIFIED)
						break;

					Write(GetIdentifyPacket(Username));
					break;
				case (byte)SCPackets.MESSAGE:
					if (CurrentState != ClientConnectionState.READY)
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
					if (CurrentState != ClientConnectionState.UNVERIFIED)
						break;

					bool ready = BitConverter.ToBoolean(payloadList.GetByteInByteCollection(), 0);

					if (!ready)
					{
						int reasonLength = payloadList.GetNextInt();
						string reason = Encoding.Unicode.GetString(payloadList.GetBytes(reasonLength));
						Disconnect($"Server refused connection: {reason}");
						break;
					}
					CurrentState = ClientConnectionState.READY;
					ClientReady?.Invoke(this, EventArgs.Empty);
					break;
				case (byte)SCPackets.NOTIFICATION:
					if (CurrentState != ClientConnectionState.READY)
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
						Client = this,
					});
					break;
				case (byte)SCPackets.DISCONNECT:
					Disconnect("Server closed connection.");
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
			while (CurrentState != ClientConnectionState.DISCONNECTED)
			{
				lock (StreamReadLock)
				{
					try
					{
						Packet packet = this.GetNextPacket();
						new Thread(() =>
						{
							HandlePacket(packet);
						})
						{
							IsBackground = true,
							Name = "Packet Handler Thread"
						}.Start();
					}
					catch (Exception e)
					{
						Disconnect(e.Message);
						break;
					}
				}
			}
		}

		public void Read(byte[] buffer, int offset, int size)
		{
			int bytesRead = 0;
			lock (StreamReadLock)
			{
				while (bytesRead < size)
				{
					int newBytesRead = Stream.Read(buffer, offset + bytesRead, size - bytesRead);
					if (newBytesRead == 0 && size != 0)
					{
						throw new Exception("Failed to read from network stream");
					}
					bytesRead += newBytesRead;
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
				Author = author,
				Client = this,
			});
		}

		private void SendMessage(MessageEventArgs e)
		{
			Message?.Invoke(this, e);
		}
	}

	public class MessageEventArgs : EventArgs
	{
		public MessageType Type { get; set; }
		public string Author { get; set; }
		public string Content { get; set; }
		public MessageBoxIcon NotificationIcon { get; set; }
		public NetClient Client;
	}

	public class ServerQueryReceivedEventArgs : EventArgs
	{
		public Server Server;
		public NetClient Client;
	}

	public class ConnectionDisconnectedEventArgs : EventArgs
	{
		public NetClient Client;
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
	public enum ClientConnectionState : int
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
