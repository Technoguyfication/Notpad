using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.CUserJoined)]
	public class CUserJoinedPacket : Packet
	{
		public User NewUser { get => _newUser; set => _newUser = value; }
		private User _newUser;

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);

			reader
				.ReadUser(out _newUser);
		}

		public override byte[] Serialize()
		{
			using var writer = new PacketWriter();

			return writer
				.WriteUser(_newUser)
				.ToArray();
		}
	}
}
