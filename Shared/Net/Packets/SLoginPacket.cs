using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.SLogin)]
	public class SLoginPacket : Packet
	{
		public Guid UserID { get => _userId; set => _userId = value; }
		private Guid _userId;

		public string Username { get => _username; set => _username = value; }
		private string _username;

		public override byte[] Serialize()
		{
			using var writer = new PacketWriter();

			return writer
				.WriteGuid(_userId)
				.WriteString(_username)

				.ToArray();
		}

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);
			
			reader
				.ReadGuid(out _userId)
				.ReadString(out _username);
		}
	}
}
