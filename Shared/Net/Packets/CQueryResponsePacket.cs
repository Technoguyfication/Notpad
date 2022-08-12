using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Net;
using Technoguyfication.Notpad.Shared.Net.Structs;
using Technoguyfication.Notpad.Shared.Net.Utility;

namespace Technoguyfication.Notpad.Shared.Net.Packets
{
    [NetworkPacket(PacketId.CQueryResponse)]
    public class CQueryResponsePacket : Packet
    {
        public ServerInfo ServerInfo
        {
            get
            {
                return new ServerInfo()
                {
                    Name = _serverName,
                    MOTD = _motd,
                    MaxUsers = _maxUsers,
                    UsersOnline = _usersOnline
                };
            }

            set
            {
                _serverName = value.Name;
                _maxUsers = value.MaxUsers;
                _usersOnline = value.UsersOnline;
                _motd = value.MOTD;
            }
        }
        
        private string _serverName;
		private string _motd;
		private int _maxUsers;
        private int _usersOnline;

        public override byte[] Bytes
        {
            get
            {
                using var writer = new PacketWriter();
                
                return writer
                    .WriteString(_serverName)
                    .WriteString(_motd)
					.WriteInt32(_maxUsers)
                    .WriteInt32(_usersOnline)

                    .ToArray();
            }
        }

        public override void Deserialize(byte[] bytes)
        {
            using var reader = new PacketReader(bytes);

            reader
                .ReadString(out _serverName)
                .ReadString(out _motd)
                .ReadInt32(out _maxUsers)
                .ReadInt32(out _usersOnline);
        }
    }
}
