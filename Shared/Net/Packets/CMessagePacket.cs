using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;
using Technoguyfication.Notpad.Shared.Types;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.CMessage)]
	public class CMessagePacket : Packet
	{
		public User Author { get => _author; set => _author = value; }
		private User _author;

		public string Content { get => _content; set => _content = value; }
		private string _content;

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);

			reader
				.ReadUser(out _author)
				.ReadString(out _content);
		}

		public override byte[] Serialize()
		{
			using var writer = new PacketWriter();

			return writer
				.WriteUser(_author)
				.WriteString(_content)
				.ToArray();
		}
	}
}
