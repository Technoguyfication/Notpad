using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Net.Structs
{
	public struct ServerInfo
	{
		public string Name { get; set; }
		public string MOTD { get; set; }
		public int UsersOnline { get; set; }
		public int MaxUsers { get; set; }
	}
}
