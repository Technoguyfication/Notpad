using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Net
{
	public class NetworkClient : TcpClient
	{
		// Expose TcpClient's "Active" property
		new public bool Active => base.Active;

		private readonly object _writeLock = new();
		private readonly object _readLock = new();

		/// <summary>
		/// Whether this client has data available to be read
		/// </summary>
		public bool DataAvailable => Client.Available > 0;

		/// <summary>
		/// Starts a new network client with a disconnected socket
		/// </summary>
		public NetworkClient()
		{

		}

		/// <summary>
		/// Create a network client out of an existing socket connection
		/// </summary>
		/// <param name="socket"></param>
		public NetworkClient(Socket socket)
		{
			// set the underlying socket to the provided one
			Client = socket;
		}

		/// <summary>
		/// Reads the next packet available from the network stream. Blocks until an entire packet is available to be read
		/// </summary>
		/// <returns></returns>
		/// <exception cref="IOException">Thrown when the peer sends invalid data or closes the connection unexpectedly</exception>
		public Packet ReadPacket()
		{
			lock (_readLock)
			{
				// read packet ID header
				byte headerRaw = ReadBytes(1)[0];

				// check if a valid packet ID was provided
				if (!Enum.IsDefined(typeof(PacketId), headerRaw))
				{
					throw new IOException($"Invalid Packet ID provided: {headerRaw}");
				}

				// get packet id
				//var packetId = Enum.Parse<PacketId>(Enum.GetName(typeof(PacketId), headerRaw));
				var packetId = (PacketId)headerRaw;

				// match packet ID with it's packet type
				Type type = Packet.GetPacketType(packetId);

				// create the packet object
				Packet packet = (Packet)Activator.CreateInstance(type);

				// read packet length
				byte[] lengthRaw = ReadBytes(sizeof(int));
				int packetLength = Protocol.BytesToInt32(lengthRaw);

				// read packet body
				var body = ReadBytes(packetLength);

				// deserialize packet
				try
				{
					packet.Deserialize(body);
				}
				catch (Exception ex)
				{
					throw new PacketFormatException("Failed to deserialize packet, see inner exception", ex);
				}

				return packet;
			}
		}

		public void WritePacket(Packet[] packets)
		{
			// lock so these packets are sent in order with nothing in between
			lock (_writeLock)
			{
				foreach (var packet in packets)
				{
					WritePacket(packet);
				}
			}
		}

		public void WritePacket(Packet packet)
		{
			lock (_writeLock)
			{
				// write header
				Client.Send(new byte[] { (byte)packet.ID });

				// packet data (evalating this property serializes the packet so it is a heavy operation)
				var bytes = packet.Serialize();

				// write packet length
				byte[] lengthRaw = Protocol.Int32ToBytes(bytes.Length);
				Client.Send(lengthRaw);

				// write packet data
				Client.Send(bytes);
			}
		}

		/// <summary>
		/// Reads a specified amount of bytes from the network connection
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		/// <exception cref="IOException">Thrown when the connection is closed during reading</exception>
		private byte[] ReadBytes(int length)
		{
			lock (_readLock)
			{
				var buffer = new byte[length];

				// how many bytes we've received total so far
				int total = 0;

				do
				{
					// read data into buffer
					var received = Client.Receive(buffer, total, length - total, SocketFlags.None);

					// if read returns 0 bytes the connection has closed
					if (received == 0) throw new IOException("Connection closed");

					total += received;
				} while (total < length);    // repeat until buffer is filled

				return buffer;
			}
		}
	}
}
