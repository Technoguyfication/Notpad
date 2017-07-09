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
	public partial class ConfirmClose : Form
	{
		public ConfirmClose()
		{
			InitializeComponent();
		}

		private void CancelButtonClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void CloseButtonClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
