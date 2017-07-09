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
	public partial class AboutBox : Form
	{
		public static string githubLink = "https://github.com/Technoguyfication/Notpad";

		public AboutBox()
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
			aboutLabel.Text = string.Format(aboutLabel.Text, githubLink);
		}

		private void GithubButtonClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(githubLink);
		}
	}
}
