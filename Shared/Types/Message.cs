using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.Notpad.Shared.Types
{
	public struct Message
	{
		public User Author { get; }
		public string Content { get; }

		public Message(User author, string content)
		{
			Author = author;
			Content = content;
		}

		public enum MessageAuthorType : byte
		{
			Server = 0,
			User = 1
		}
	}
}
