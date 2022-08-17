using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

		public override bool Equals([NotNullWhen(true)] object obj)
		{
			return obj is Message msg && msg.Author == Author && msg.Content == Content;
		}

		public override int GetHashCode()
		{
			return Author.GetHashCode() ^ Content.GetHashCode();
		}

		public static bool operator ==(Message left, Message right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Message left, Message right)
		{
			return !(left == right);
		}
	}
}
