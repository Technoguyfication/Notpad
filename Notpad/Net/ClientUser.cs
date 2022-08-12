using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net;
using Technoguyfication.Notpad.Shared.Net.Structs;

namespace Technoguyfication.Notpad.Net
{
	class ClientUser : NetworkedUser
	{
		private NetworkClient _client;

		public ClientUser()
		{
			
		}

		public override void SetUsername(string username)
		{
			throw new NotImplementedException();
		}

		public ServerInfo QueryServer(IPEndPoint endpoint)
		{
			// todo add client query support
			throw new NotImplementedException();
		}
	}
}
