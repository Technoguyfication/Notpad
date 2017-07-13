using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Server
{
	public interface IStreamable
	{
		void Write(byte[] buffer, int offset, int count);

		void Read(byte[] buffer, int offset, int count);
	}
}
