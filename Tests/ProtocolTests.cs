using Technoguyfication.Notpad.Shared.Net;
using Technoguyfication.Notpad.Shared.Net.Packets;

namespace Technoguyfication.Notpad.Tests
{
	[TestClass]
	public class ProtocolTests
	{
		[TestMethod]
		public void CheckPacketClassInitialization()
		{
			Assert.AreEqual(Packet.GetPacketType(PacketId.SHandshake), typeof(SHandshakePacket));
		}
	}
}