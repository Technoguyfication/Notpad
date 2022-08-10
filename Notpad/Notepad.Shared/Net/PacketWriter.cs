using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Net
{
    /// <summary>
    /// Easily write packet data using Linq-style chaining
    /// </summary>
    internal class PacketWriter : IDisposable
    {
        private MemoryStream _stream;
        private BinaryWriter _writer;

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
    }
}
