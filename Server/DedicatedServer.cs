using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net.Server;

namespace Technoguyfication.Notpad.Dedicated
{
    class DedicatedServer : BaseServerImplementation
    {
        public DedicatedServer(string name, string motd, int maxUsers)
        {
            ServerName = name;
            MOTD = motd;
            MaxUsers = maxUsers;
        }
    }
}
