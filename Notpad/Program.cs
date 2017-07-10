using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notpad.Client
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Notpad());
		}

		public static void InvokeIfRequired(this Control control, MethodInvoker action)
		{
			if (control.InvokeRequired)
				control.Invoke(action);
			else
				action();
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
	}
}
