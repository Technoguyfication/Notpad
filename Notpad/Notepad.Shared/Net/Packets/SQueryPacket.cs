using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Net.Packets
{
	[PacketType(PacketTypes.SQuery)]
	class SQueryPacket : Packet
	{
		public override byte[] Bytes => throw new NotImplementedException();
	}
}
