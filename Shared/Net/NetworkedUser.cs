using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Net
{
	public abstract class NetworkedUser : User
	{
		public abstract void SetUsername(string username);

		public event EventHandler OnDisconnect;

		protected void Disconnect()
		{
			OnDisconnect?.Invoke(this, null);
		}
	}
}
