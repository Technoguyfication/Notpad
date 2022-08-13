using Technoguyfication.Notpad.Shared.Net;
using Technoguyfication.Notpad.Shared.Net.Packets;

namespace Technoguyfication.Notpad.Tests
{
	[TestClass]
	public class ProtocolTests
	{
		[TestMethod]
		public void AllPacketTypesDefined()
		{
			// Check that all packet IDs have an associated type
			foreach (var packetId in Enum.GetValues(typeof(PacketId)).Cast<PacketId>())
			{
				try
				{
					Packet.GetPacketType(packetId);
				}
				catch (KeyNotFoundException)
				{
					throw new NotImplementedException($"Packet ID {Enum.GetName(typeof(PacketId), packetId)} does not have a Type associated with it");
				}
			}
		}
	}
}