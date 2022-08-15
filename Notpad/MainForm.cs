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

		public MainForm()
		{
			Client = new ClientUser();

			InitializeComponent();
		}

		private void connectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ConnectForm().ShowDialog();
		}
	}
}
