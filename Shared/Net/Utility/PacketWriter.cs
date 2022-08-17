using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Types;

namespace Technoguyfication.Notpad.Shared.Net.Utility
{
    /// <summary>
    /// Easily write packet data using Linq-style chaining
    /// </summary>
    internal class PacketWriter : IDisposable
    {
        private readonly MemoryStream _stream;
        private readonly BinaryWriter _writer;

        public PacketWriter()
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
        }

        /// <summary>
        /// Gets this packet's serialized data as a byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return _stream.ToArray();
        }

        public void Dispose()
        {
            _writer.Dispose();
            _stream.Dispose();
        }

        public delegate PacketWriter WriteFunc<T>(T value);

        /// <summary>
        /// Writes an array of any value using it's write function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PacketWriter WriteArray<T>(T[] value, WriteFunc<T> writer)
        {
            // write length
            WriteInt32(value.Length);

            // write length amount of values
            for (int i = 0; i < value.Length; i++)
            {
                writer(value[i]);
            }

            return this;
        }

        public PacketWriter WriteString(string value)
        {
            // get string as bytes
            var bytes = Protocol.StringToBytes(value);

            // prefix with length
            _writer.Write(Protocol.Int32ToBytes(bytes.Length));

            // write string
            _writer.Write(bytes);

            return this;
        }

        public PacketWriter WriteInt32(int value)
        {
            _writer.Write(Protocol.Int32ToBytes(value));

            return this;
        }

        public PacketWriter WriteByte(byte value)
        {
            _writer.Write(value);

            return this;
        }

        public PacketWriter WriteGuid(Guid value)
        {
            _writer.Write(value.ToByteArray());

            return this;
        }

        public PacketWriter WriteUser(User value)
        {
            WriteGuid(value.ID);
            WriteString(value.Username);

            return this;
        }
    }
}
