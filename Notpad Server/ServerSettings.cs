using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Server
{
	[Serializable]
	class ServerSettings
	{
		public string Name { get; set; } = "Notpad Server";
		public IPAddress Bind { get; set; } = IPAddress.Any;
		public ushort Port { get; set; } = 10334;
		public int MaxUsers { get; set; } = 25;
	}
}
