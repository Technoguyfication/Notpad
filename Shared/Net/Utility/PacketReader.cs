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
    /// Easily read packet data using Linq-style chaining
    /// </summary>
    internal class PacketReader : IDisposable
    {
        private readonly MemoryStream _stream;
        private readonly BinaryReader _reader;

        public PacketReader(byte[] buffer)
        {
            _stream = new MemoryStream(buffer);
            _reader = new BinaryReader(_stream);
        }

        public void Dispose()
        {
            _reader.Dispose();
            _stream.Dispose();
        }

        public PacketReader ReadString(out string value)
        {
            // read string length and string data
            var lengthRaw = _reader.ReadBytes(sizeof(int));
            var bytes = _reader.ReadBytes(Protocol.BytesToInt32(lengthRaw));

            // decode string
            value = Protocol.BytesToString(bytes);

            return this;
        }

        public PacketReader ReadInt32(out int value)
        {
            value = Protocol.BytesToInt32(_reader.ReadBytes(sizeof(int)));

            return this;
        }

        public PacketReader ReadByte(out byte value)
        {
            value = _reader.ReadByte();

            return this;
        }

        public PacketReader ReadGuid(out Guid value)
        {
            value = new Guid(_reader.ReadBytes(16));

            return this;
        }

        public PacketReader ReadUser(out User value)
        {
            ReadGuid(out var guid);
            ReadString(out var username);

            value = new User(guid, username);

            return this;
        }
    }
}
