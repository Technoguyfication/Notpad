using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.CInvalidVersion)]
	public class CInvalidVersionPacket : Packet
	{
		public int ServerVersion { get => _serverVersion; set => _serverVersion = value; }
		private int _serverVersion;

		public override byte[] Bytes
		{
			get
			{
				using var writer = new PacketWriter();
				
				return writer
					.WriteInt32(_serverVersion)

					.ToArray();
			}
		}

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);
			reader
				.ReadInt32(out _serverVersion);
		}
	}
}
