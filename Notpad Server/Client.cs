using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Server
{
	class Client : TcpClient
	{
		public Client(TcpClient client)
		{
			Client = client.Client;
		}
	}
}
