using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Types
{
	public class ServerUser : User
	{
		public ServerUser(Guid id) : base(id)
		{
			Username = "SERVER";
		}
	}
}
