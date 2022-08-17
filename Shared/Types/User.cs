using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Types
{
    public class User
    {
        public string Username { get; protected set; }

        public Guid ID { get; }

        public User(Guid id)
        {
            ID = id;
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
