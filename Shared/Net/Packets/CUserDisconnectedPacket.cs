using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;
using Technoguyfication.Notpad.Shared.Types;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
    [NetworkPacket(PacketId.CUserDisconnected)]
	public class CUserDisconnectedPacket : Packet
	{
		public Guid UserID { get => _userId; set => _userId = value; }
		private Guid _userId;

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);

			reader
				.ReadGuid(out _userId);
		}

		public override byte[] Serialize()
		{
			using var writer = new PacketWriter();

			return writer
				.WriteGuid(_userId)
				.ToArray();
		}
	}
}
