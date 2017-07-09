using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Client
{
	public static class RegSettings
	{
		public static RegistryKey BaseKey = Registry.CurrentUser
			.CreateSubKey("Software")
			.CreateSubKey("Technoguyfication")
			.CreateSubKey("Notpad");
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
	}
}
