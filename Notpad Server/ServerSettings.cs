using Newtonsoft.Json;
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
		public string Address { get; set; } = IPAddress.Any.ToString();
		public ushort Port { get; set; } = 10334;
		public int MaxUsers { get; set; } = 25;

		[JsonIgnore]
		public IPAddress IPAddress
		{
			get
			{
				return IPAddress.Parse(Address);
			}
			set
			{
				Address = value.ToString();
			}
		}
		[JsonIgnore]
		public IPEndPoint EndPoint
		{
			get
			{
				return new IPEndPoint(IPAddress, Port);
			}
			set
			{
				Port = (ushort)value.Port;
				IPAddress = value.Address;
			}
		}
	}
}
