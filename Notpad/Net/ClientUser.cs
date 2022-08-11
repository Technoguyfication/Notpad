using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net;

namespace Technoguyfication.Notpad.Net
{
	class ClientUser : NetworkedUser
	{
		public override void SetUsername(string username)
		{
			throw new NotImplementedException();
		}
	}
}
