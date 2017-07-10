using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Notpad.Client.Net
{
	[Serializable]
	public struct Server
	{
		public IPAddress Address { get; set; }
		public ushort Port { get; set; }
		public string Name { get; set; }

		public IPEndPoint Endpoint
		{
			get
			{
				return new IPEndPoint(Address, Port);
			}
			set
			{
				Address = value.Address;
				Port = (ushort)value.Port;
			}
		}

		public int Online { get; set; }
		public int MaxOnline { get; set; }

		public string UniqueID
		{
			get
			{
				SHA256 sha = SHA256.Create();
				sha.Initialize();
				byte[] hash = sha.ComputeHash(Encoding.Unicode.GetBytes(Address.ToString()));
				return BitConverter.ToString(hash).Replace("-", "");
			}
		}
	}
}
