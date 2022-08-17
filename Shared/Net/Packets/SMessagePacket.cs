using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.SMessage)]
	public class SMessagePacket : Packet
	{
		public string Content { get => _content; set => _content = value; }
		private string _content;

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);

			reader
				.ReadString(out _content);
		}

		public override byte[] Serialize()
		{
			using var writer = new PacketWriter();

			return writer
				.WriteString(_content)
				.ToArray();
		}
	}
}
