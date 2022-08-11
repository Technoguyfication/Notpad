using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Net;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
    [NetworkPacket(PacketId.CQueryResponse)]
    public class CQueryResponsePacket : Packet
    {
        public string ServerName { get => _serverName; set => _serverName = value; }
        private string _serverName;

        public int MaxUsers { get => _maxUsers; set => _maxUsers = value; }
        private int _maxUsers;

        public int UsersOnline { get => _usersOnline; set => _usersOnline = value; }
        private int _usersOnline;

        public string MOTD { get => _motd; set => _motd = value; }
        private string _motd;

        public override byte[] Bytes
        {
            get
            {
                // todo use "using" here for better memory management
                
                return new PacketWriter()
                    .WriteString(ServerName)
                    .WriteInt32(MaxUsers)
                    .WriteInt32(UsersOnline)
                    .WriteString(MOTD)

                    .ToArray();
            }
        }

        public override void Deserialize(byte[] bytes)
        {
            // todo use "using" here for better memory management

            new PacketReader(bytes)
                .ReadString(out _serverName)
                .ReadInt32(out _maxUsers)
                .ReadInt32(out _usersOnline)
                .ReadString(out _motd);
        }
    }
}
