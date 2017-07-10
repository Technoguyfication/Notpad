using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Client.Util
{
	public interface IStreamable
	{
		void Write(byte[] buffer, int offset, int count);

		void Read(byte[] buffer, int offset, int count);
	}
}
