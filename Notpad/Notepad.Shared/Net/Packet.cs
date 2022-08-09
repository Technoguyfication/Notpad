using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Net
{
	public abstract class Packet
	{
		public abstract byte[] Bytes { get; }
	}

	public class PacketType : Attribute
	{
		public PacketTypes Type { get; private set; }
		public PacketType(PacketTypes type)
		{
			Type = type;
		}
	}

	public enum PacketTypes : byte
	{
		// Serverbound packets
		SQuery = 0x00,

		// Clientbound packets
		CQueryResponse = 0xF0
	}
}
