using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Types
{
    public abstract class NetworkedUser : User
    {
        public abstract void SetUsername(string username);

        public NetworkedUser()
        {

        }

        public NetworkedUser(Guid id) : base(id)
        {

        }
    }
}
