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
using Technoguyfication.Notpad.Net;

namespace Technoguyfication.Notpad
{
	public partial class ConnectForm : Form
	{
		public ConnectForm()
		{
			InitializeComponent();
		}

		private void queryButton_Click(object sender, EventArgs e)
		{
			// parse address
			if (!IPEndPoint.TryParse(queryAddressTextBox.Text, out var ep))
			{
				queryResponseTextBox.Text = "Invalid IP Address";
				return;
			}

			var context = WindowsFormsSynchronizationContext.Current;

			Task.Run(async () =>
			{
				// run query
				try
				{
					var result = await ClientUser.QueryServer(ep, TimeSpan.FromSeconds(10000));

					context.Send((obj) =>
					{
						queryResponseTextBox.Text = $"Results for {ep}:\n\n{result.Name}\n{result.MOTD}\nUsers: {result.UsersOnline} / {result.MaxUsers}";
					}, null);
				}
				catch (Exception ex)
				{
					context.Send((obj) =>
					{
						queryResponseTextBox.Text = $"Received {ex.GetType().Name}: {ex.Message}\n\n{ex?.InnerException?.Message}";
					}, null);
				}
			});
		}
	}
}
