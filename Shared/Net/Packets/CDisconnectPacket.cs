using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
	[NetworkPacket(PacketId.CDisconnect)]
	public class CDisconnectPacket : Packet
	{
		public string Reason { get => _reason; set => _reason = value; }
		private string _reason;

		public override byte[] Bytes
		{
			get
			{
				using var writer = new PacketWriter();
				
				return writer.WriteString(Reason)

					.ToArray();
			}
		}

		public override void Deserialize(byte[] bytes)
		{
			using var reader = new PacketReader(bytes);
			
			reader.ReadString(out _reason);
		}
	}
}
