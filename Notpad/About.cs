using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notpad.Client
{
	public partial class About : Form
	{
		public About()
		{
			InitializeComponent();
		}

		private void OKButtonClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void WindowLoaded(object sender, EventArgs e)
		{
			versionLabel.Text = string.Format(versionLabel.Text, Application.ProductVersion, Environment.OSVersion.VersionString);
		}
	}
}
