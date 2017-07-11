using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Notpad.Client.Net;

namespace Notpad.Client.Util
{
	public static class RegSettings
	{
		public static RegistryKey BaseKey = Registry.CurrentUser
			.CreateSubKey("Software")
			.CreateSubKey("Technoguyfication")
			.CreateSubKey("Notpad");
		public static RegistryKey ServerKey = BaseKey.CreateSubKey("Servers");
		public static int WindowSizeX
		{
			get
			{
				return (int)BaseKey.GetValue("WindowSizeX", 1025);
			}
			set
			{
				BaseKey.SetValue("WindowSizeX", value, RegistryValueKind.DWord);
			}
		}
		public static int WindowSizeY
		{
			get
			{
				return (int)BaseKey.GetValue("WindowSizeY", 525);
			}
			set
			{
				BaseKey.SetValue("WindowSizeY", value, RegistryValueKind.DWord);
			}
		}
		public static bool WordWrap
		{
			get
			{
				return BitConverter.ToBoolean((byte[])BaseKey.GetValue("WordWrap", new byte[1] { 0x00 }), 0);
			}
			set
			{
				BaseKey.SetValue("WordWrap", BitConverter.GetBytes(value), RegistryValueKind.Binary);
			}
		}
		public static Font SelectedFont
		{
			get
			{
				string fontString = (string)BaseKey.GetValue("Font");
				if (fontString == null)
					fontString = "Consolas, 11.25pt";

				return (Font)new FontConverter().ConvertFrom(fontString);
			}
			set
			{
				BaseKey.SetValue("Font", new FontConverter().ConvertToString(value));
			}
		}
		public static Server[] Servers
		{
			get
			{
				string[] serverkeys = ServerKey.GetSubKeyNames();
				List<Server> servers = new List<Server>();
				for (int i = 0; i < serverkeys.Length; i++)
				{
					RegistryKey key = ServerKey.CreateSubKey(serverkeys[i]);
					string _address = (string)key.GetValue("Address");
					string _port = (string)key.GetValue("Port");
					string name = (string)key.GetValue("Name");

					if (_address == null || _port == null)
						continue;

					if (!IPAddress.TryParse(_address, out IPAddress address))
						continue;

					if (!ushort.TryParse(_port, out ushort port))
						continue;

					servers.Add(new Server { Address = address, Port = port, Name = name });
				}

				return servers.ToArray();
			}
			set
			{
				foreach (string subkey in ServerKey.GetSubKeyNames())
					ServerKey.DeleteSubKeyTree(subkey, false);

				foreach (Server server in value)
				{
					RegistryKey key = ServerKey.CreateSubKey(server.UniqueID);
					key.SetValue("Address", server.Address.ToString());
					key.SetValue("Port", server.Port.ToString());
					if (!string.IsNullOrEmpty(server.Name))
						key.SetValue("Name", server.Name);
				}
			}
		}
		public static string Username
		{
			get
			{
				return (string)BaseKey.GetValue("Username");
			}
			set
			{
				BaseKey.SetValue("Username", value);
			}
		}
	}
}
