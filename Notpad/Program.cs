using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technoguyfication.Notpad
{
	static class Program
	{
		public static event EventHandler<string> DebugMessage;

		public const int WM_HSCROLL = 0x114;
		public const int WM_VSCROLL = 0x115;
		public const int WM_MOUSEWHEEL = 0x20A;

		private static MainForm _mainForm;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			_mainForm = new MainForm();

			Application.Run(_mainForm);
		}

		public static void Log(string message) => Log(message, null);

		public static void Log(string message, object context)
		{
			var sb = new StringBuilder(message);

			// stringify context object and attach
			if (context != null)
			{
				sb.AppendLine("Context:");
				sb.AppendLine(context.ToString());
			}

			DebugMessage?.Invoke(null, sb.ToString());
		}
	}
}
