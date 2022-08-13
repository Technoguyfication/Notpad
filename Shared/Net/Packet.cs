using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Net
{
	public abstract class Packet
	{
		public abstract byte[] Bytes { get; }

		public abstract void Deserialize(byte[] bytes);

		public PacketId ID
		{
			get
			{
				return _packetTypes.FirstOrDefault(x => x.Value.Equals(GetType())).Key;
			}
		}

		private static readonly Dictionary<PacketId, Type> _packetTypes;

		static Packet()
		{
			// Pre-initialize by making a map of PacketTypes enum to Packet derived class Types
			_packetTypes = new Dictionary<PacketId, Type>();
			foreach (var type in Assembly.GetAssembly(typeof(Packet)).GetTypes())
			{
				// check if type has NetworkPacket attribute
				var customAttribs = type.GetCustomAttributes(typeof(NetworkPacket), true);
                if (customAttribs.Length > 0)
				{
					var packetAttrib = (NetworkPacket)customAttribs[0];

					// make sure this ID doesn't already have a Type
					if (_packetTypes.ContainsKey(packetAttrib.Type)) throw new InvalidOperationException($"Packet ID {Enum.GetName(typeof(PacketId), packetAttrib.Type)} has multiple Types associated with it");


                    _packetTypes.Add(packetAttrib.Type, type);
				}
			}
		}

		/// <summary>
		/// Gets the Type for a packet ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Type GetPacketType(PacketId id)
		{
			return _packetTypes[id];
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class NetworkPacket : Attribute
	{
		public PacketId Type { get; private set; }
		public NetworkPacket(PacketId type)
		{
			Type = type;
		}
	}

	public enum PacketId : byte
	{
		// Serverbound packets
		SHandshake = 0x00,
		SLogin = 0x01,
		SMessage = 0x02,

		// Clientbound packets
		CQueryResponse = 0xF0,
		CDisconnect = 0xF1,
		CMessage = 0xF2,
		CUserJoined = 0xF3,
		CUserDisconnected = 0xF4,
		CUserList = 0xF5,
		CInvalidVersion = 0xF6
	}

    [Serializable]
    public class InvalidPacketException : Exception
    {
        public InvalidPacketException() { }
        public InvalidPacketException(string message) : base(message) { }
        public InvalidPacketException(string message, Exception inner) : base(message, inner) { }
        protected InvalidPacketException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
