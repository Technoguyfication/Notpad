using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Server.Net
{
	public class Packet
	{
		public byte PacketID { get; set; }
		public byte[] Payload { get; set; }
		public int DataLength
		{
			get
			{
				return Payload.Length + 1;	// add id byte
			}
		}
		public byte[] Raw
		{
			get
			{
				List<byte> builder = new List<byte>();
				builder.AddRange(BitConverter.GetBytes(DataLength).CheckEndianness());
				builder.Add(PacketID);
				builder.AddRange(Payload);
				return builder.ToArray();
			}
		}

		public Packet(byte id, byte[] payload = null)
		{
			PacketID = id;
			if (payload == null)
				payload = new byte[0];

			Payload = payload;
		}

		public Packet(byte[] buffer)
		{
			List<byte> bytes = new List<byte>(buffer);
			PacketID = bytes[0];
			Payload = bytes.Skip(1).ToArray();
		}
	}
}
