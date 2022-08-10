using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Net;

namespace Technoguyfication.Notpad.Shared.Net
{
    public class NetworkClient : TcpClient
    {
        // Expose TcpClient's "Active" property
        new public bool Active => base.Active;

        /// <summary>
        /// Whether this client has data available to be read
        /// </summary>
        public bool DataAvailable => Client.Available > 0;

        public NetworkClient(Socket socket)
        {
            // set the underlying socket to the provided one
            Client = socket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="IOException">Thrown when the client sends invalid data or closes the connection unexpectedly</exception>
        public Packet ReceivePacket()
        {
            // read packet ID header
            byte headerRaw = ReadBytes(1)[0];
            
            // check if a valid packet ID was provided
            if (!Enum.IsDefined(typeof(PacketId), headerRaw))
            {
                throw new IOException($"Invalid Packet ID provided: {headerRaw}");
            }

            // get packet id
            var packetId = Enum.Parse<PacketId>(Enum.GetName(typeof(PacketId), headerRaw));

            // match packet ID with it's packet type
            Type type = Packet.GetPacketType(packetId);

            // create the packet object
            Packet packet = (Packet)Activator.CreateInstance(type);
            
            // read packet length
            byte[] lengthRaw = ReadBytes(sizeof(int));
            int packetLength = Protocol.BytesToInt32(lengthRaw);

            // read packet body
            var body = ReadBytes(packetLength);

            // deserialize packet
            packet.Deserialize(body);

            return packet;
        }

        /// <summary>
        /// Reads a specified amount of bytes from the network connection
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="IOException">Thrown when the connection is closed during reading</exception>
        private byte[] ReadBytes(int length)
        {
            var buffer = new byte[length];

            // how many bytes we've received total so far
            int total = 0;

            do
            {
                // read data into buffer
                var received = Client.Receive(buffer, total, length - total, SocketFlags.None);

                // if read returns 0 bytes the connection has closed
                if (received == 0) throw new IOException("Connection closed");

                total += received;
            } while (total < length);    // repeat until buffer is filled

            return buffer;
        }

        public void SendPacket(Packet packet)
        {
            var bytes = packet.Bytes;

            // todo implement header

            Client.Send(bytes);
        }
    }
}
