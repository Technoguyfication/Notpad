using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;
using Technoguyfication.Notpad.Shared.Types;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.CUserList)]
	public class CUserListPacket : Packet
	{
		public User[] Users { get => _users; set => _users = value; }
		private User[] _users;

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);

			reader
				.ReadArray(out _users, reader.ReadUser);
		}

		public override byte[] Serialize()
		{
			using var writer = new PacketWriter();

			return writer
				.WriteArray(_users, writer.WriteUser)
				.ToArray();
		}
	}
}
