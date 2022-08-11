﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Net.Packets
{
	[NetworkPacket(PacketId.SHandshake)]
	class SHandshakePacket : Packet
	{
		public int ProtocolVersion { get => _protocolVersion; set => _protocolVersion = value; }
		private int _protocolVersion;

		public HandshakeIntent Intent { get => _intent; set => _intent = value; }
		private HandshakeIntent _intent;

		public override byte[] Bytes => Array.Empty<byte>();

		public override void Deserialize(byte[] bytes) => Expression.Empty();

		public enum HandshakeIntent : byte
		{
			Query = 0x00,
			Login = 0x01
		}
	}
}
