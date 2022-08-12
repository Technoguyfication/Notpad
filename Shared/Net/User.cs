using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Net
{
	public class User
	{
		public string Username { get; private set; }

		public Guid ID { get; private set; }

		public User()
		{
			// empty user constructor
		}

		public User(Guid id, string username)
		{
			ID = id;
			Username = username;
		}

		public override bool Equals(object obj)
		{
			if (obj is not User u) return false;

			return ID.Equals(u.ID);
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public override string ToString()
		{
			return $"{Username} / {ID}";
		}
	}
}
