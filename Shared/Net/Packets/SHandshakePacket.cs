using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.SHandshake)]
	public class SHandshakePacket : Packet
	{
		public int ProtocolVersion { get => _protocolVersion; set => _protocolVersion = value; }
		private int _protocolVersion;

		public HandshakeIntent Intent { get => _intent; set => _intent = value; }
		private HandshakeIntent _intent;

		public override byte[] Serialize()
		{
			using var writer = new PacketWriter();

			return writer
				.WriteInt32(_protocolVersion)
				.WriteByte((byte)_intent)
				.ToArray();
		}

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);

			reader.ReadInt32(out _protocolVersion);

			reader.ReadByte(out var _intentByte);
			_intent = (HandshakeIntent)_intentByte;
		}

		public enum HandshakeIntent : byte
		{
			Query = 0x00,
			Login = 0x01
		}
	}
}
