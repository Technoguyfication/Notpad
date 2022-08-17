using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Technoguyfication.Notpad.Shared.Types;

namespace Technoguyfication.Notpad.Shared.Net
{
	public static class Protocol
	{
		public static readonly byte[] BroadcastMessage = new byte[] { (byte)'N', (byte)'o', (byte)'t', (byte)'p', (byte)'a', (byte)'d', (byte)'D', (byte)'i', (byte)'s', (byte)'c', (byte)'o', (byte)'v', (byte)'e', (byte)'r' };

		public static readonly int UsernameMinLength = 3;
		public static readonly int UsernameMaxLength = 20;
		public static readonly Regex UsernameRegex = new(@"^\w+$");

		/// <summary>
		/// Protocol version
		/// All clients and servers communicating with each other must have the same protocol version for compatibility
		/// </summary>
		public const int Version = 1;

		/// <summary>
		/// Endianness-agnostic bytes to int32 converter (uses big endian)
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static int BytesToInt32(byte[] bytes)
		{
			if (bytes.Length != sizeof(int)) throw new ArgumentException($"Invalid array length, expected {sizeof(int)}");

			var buffer = new byte[bytes.Length];
			Buffer.BlockCopy(bytes, 0, buffer, 0, bytes.Length);

			// input is big endian so convert if needed
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(buffer);
			}

			return BitConverter.ToInt32(buffer);
		}

		public static byte[] Int32ToBytes(int value)
		{
			var buffer = BitConverter.GetBytes(value);

			// make output big endian
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(buffer);
			}

			return buffer;
		}

		public static string BytesToString(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

		public static byte[] StringToBytes(string value)
		{
			return Encoding.UTF8.GetBytes(value);
		}

		public static Guid BytesToGuid(byte[] bytes)
		{
			if (bytes.Length != 16) throw new ArgumentException("Invalid array length, expected 16");

			return new Guid(bytes);
		}

		public static byte[] GuidToBytes(Guid value)
		{
			return value.ToByteArray();
		}
	}


	[Serializable]
	public class IncompatibleProtocolVersionException : Exception
	{
		public int RequiredVersion { get; set; }

		public override string Message
		{
			get
			{
				return $"Incompatible protocol version. We have {Protocol.Version}, remote client requires {RequiredVersion}";
			}
		}

		public IncompatibleProtocolVersionException(int version)
		{
			RequiredVersion = version;
		}

		protected IncompatibleProtocolVersionException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}


	[Serializable]
	public class PacketFormatException : Exception
	{
		public PacketFormatException() { }
		public PacketFormatException(string message) : base(message) { }
		public PacketFormatException(string message, Exception inner) : base(message, inner) { }
		protected PacketFormatException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}