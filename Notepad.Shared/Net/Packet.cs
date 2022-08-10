using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Net
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

		private static Dictionary<PacketId, Type> _packetTypes;

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

			// Check that all packet IDs have an associated type
			foreach (var packetId in Enum.GetValues(typeof(PacketId)).Cast<PacketId>())
			{
				if (!_packetTypes.ContainsKey(packetId)) throw new NotImplementedException($"Packet ID {Enum.GetName(typeof(PacketId), packetId)} does not have a Type associated with it");
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
		SQuery = 0x00,

		// Clientbound packets
		CQueryResponse = 0xF0
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
