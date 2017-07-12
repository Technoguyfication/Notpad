using Notpad.Client.Net;
using Notpad.Client.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notpad.Client
{
	public partial class DirectConnect : Form
	{
		private ConnectionWindow _connectionWindow;
		public DirectConnect(ConnectionWindow connectionWindow)
		{
			InitializeComponent();
			_connectionWindow = connectionWindow;

			addressTextBox.Text = RegSettings.DirectConnectAddress;
			portTextBox.Text = RegSettings.DirectConnectPort.ToString();
		}

		private void WindowKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				DialogResult = DialogResult.Cancel;
			}
		}

		private void ConnectButtonClick(object sender, EventArgs e)
		{
			if (!ValidateForm())
				return;

			RegSettings.DirectConnectAddress = addressTextBox.Text;
			RegSettings.DirectConnectPort = int.Parse(portTextBox.Text);

			_connectionWindow.Connect(new Server()
			{
				Address = addressTextBox.Text,
				Port = int.Parse(portTextBox.Text),
			});
			DialogResult = DialogResult.OK;
		}

		private bool ValidateForm()
		{
			if (!ushort.TryParse(portTextBox.Text, out var _)) // var needed b/c vs bug
			{
				MessageBox.Show("Invalid Port");
				return false;
			}

			if (string.IsNullOrEmpty(addressTextBox.Text))
			{
				MessageBox.Show("Invalid Address");
				return false;
			}

			return true;
		}
	}
}
