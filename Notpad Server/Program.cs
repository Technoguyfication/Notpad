using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading;
using Notpad.Server.Net;

namespace Notpad.Server
{
	public static class Program
	{
		public static readonly string ConfigPath = Path.GetFullPath("config.json");
		public static ServerSettings Settings;

		static void Main(string[] args)
		{
			string configJson;
			try
			{
				configJson = File.ReadAllText(ConfigPath);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine($"Config file could not be found at {ConfigPath}");
				return;
			}
			catch (IOException e)
			{
				Console.WriteLine($"Failed to open config file: {e.Message}");
				return;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Unhandled exception loading config: {e.Message}");
				return;
			}

			Settings = JsonConvert.DeserializeObject<ServerSettings>(configJson);

			new ServerManager().Start();
		}


		public static byte[] CheckEndianness(this byte[] buffer)
		{
			if (BitConverter.IsLittleEndian)
				return buffer.Reverse().ToArray();
			else
				return buffer;
		}

		public static byte GetByte(this List<byte> bytes, int index = 0)
		{
			byte value = bytes.GetBytes(1, index)[0];
			return value;
		}

		public static byte[] GetByteInByteCollection(this List<byte> bytes, int index = 0)
		{
			return new byte[1] { bytes.GetByte(index) };
		}

		public static byte[] GetBytes(this List<byte> bytes, int count, int index = 0)
		{
			byte[] value = bytes.Skip(index).Take(count).ToArray();
			bytes.RemoveRange(index, count);
			return value;
		}

		public static int GetNextInt(this List<byte> bytes)
		{
			return BitConverter.ToInt32(bytes.GetBytes(4).CheckEndianness(), 0);
		}

		public static int GetNextInt(this IStreamable stream)
		{
			byte[] bytes = new byte[4];
			stream.Read(bytes, 0, 4);
			return bytes.ToList().GetNextInt();
		}

		public static Packet GetNextPacket(this IStreamable stream)
		{
			int length = stream.GetNextInt();
			byte[] buffer = new byte[length];
			stream.Read(buffer, 0, length);
			return new Packet(buffer);
		}
	}
}
