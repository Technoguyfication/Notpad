using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Net.Packets
{
	[NetworkPacket(PacketId.SQuery)]
	class SQueryPacket : Packet
	{
		public override byte[] Bytes => new byte[0];

		public override void Deserialize(byte[] bytes) => Expression.Empty();
	}
}
