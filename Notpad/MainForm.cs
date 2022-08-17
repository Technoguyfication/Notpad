using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technoguyfication.Notpad.Net;

namespace Technoguyfication.Notpad
{
	public partial class MainForm : Form
	{
		internal ClientUser Client { get; }

#if DEBUG
		private DebugForm _debugForm;
#endif

		public MainForm()
		{
			Client = new ClientUser(Guid.NewGuid());

			InitializeComponent();

#if DEBUG
			_debugForm = new DebugForm(this);

			var debugMenuItem = new ToolStripMenuItem()
			{
				Text = "Open Debug Menu"
			};

			debugMenuItem.Click += (s, a) =>
			{
				_debugForm.Show();
			};

			fileToolStripMenuItem.DropDownItems.Add(debugMenuItem);
#endif
		}

		private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ConnectForm(this).ShowDialog();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			Program.Log("Main form shown");

#if DEBUG
			_debugForm.Show();
#endif
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
#if DEBUG
			_debugForm?.Close();
#endif
		}
	}
}
