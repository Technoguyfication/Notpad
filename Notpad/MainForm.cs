using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technoguyfication.Notpad
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void connectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ConnectForm().ShowDialog();
		}
	}
}
