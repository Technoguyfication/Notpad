using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Client.Net
{
	public class Packet
	{
		public int Length
		{
			get
			{
				return Raw.Length;
			}
		}
		public int DataLength
		{
			get
			{
				return Payload.Length + 1;	// length of id
			}
		}
		public byte[] LengthBytes
		{
			get
			{
				return BitConverter.GetBytes(DataLength).CheckEndianness();
			}
		}
		public byte PacketID { get; set; }
		public byte[] Payload { get; set; }
		public byte[] Body
		{
			get
			{
				List<byte> builder = new List<byte>(Payload);
				builder.Insert(0, PacketID);
				return builder.ToArray();
			}
		}
		public byte[] Raw
		{
			get
			{
				List<byte> builder = new List<byte>(Body);
				builder.InsertRange(0, LengthBytes);
				return builder.ToArray();
			}
		}

		public Packet(byte[] buffer)
		{
			List<byte> bytes = new List<byte>(buffer);
			PacketID = bytes[0];
			Payload = bytes.Skip(1).ToArray();
		}

		public Packet(byte id, byte[] payload = null)
		{
			PacketID = id;
			if (payload == null)
				payload = new byte[0];

			Payload = payload;
		}

		public static implicit operator byte[] (Packet p)
		{
			return p.Raw;
		}
	}
}
