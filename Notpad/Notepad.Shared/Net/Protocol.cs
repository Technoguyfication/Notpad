using System;
using System.Text;

namespace Technoguyfication.Notpad.Shared.Net
{
    public static class Protocol
    {
        public static readonly byte[] BroadcastMessage = new byte[] { (byte)'N', (byte)'o', (byte)'t', (byte)'p', (byte)'a', (byte)'d', (byte)'D', (byte)'i', (byte)'s', (byte)'c', (byte)'o', (byte)'v', (byte)'e', (byte)'r' };

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

        public static byte[] Int32ToBytes(int input)
        {
            var buffer = BitConverter.GetBytes(input);

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

        public static byte[] StringToBytes(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }
    }
}