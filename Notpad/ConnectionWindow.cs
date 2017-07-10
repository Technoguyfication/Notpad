using Notpad.Client.Net;
using Notpad.Client.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notpad.Client
{
	public partial class ConnectionWindow : Form
	{
		Notpad MainForm;

		public ConnectionWindow(Notpad mainForm)
		{
			MainForm = mainForm;
			InitializeComponent();
		}

		private void WindowLoad(object sender, EventArgs e)
		{
			Server[] Servers = RegSettings.Servers;
			serverListView.Items.Clear();
			foreach (Server server in Servers)
			{
				ListViewItem item = GetServerListViewItem(server);
				serverListView.Items.Add(item);
			}

			for (int i = 0; i < serverListView.Items.Count; i++)
			{
				new Thread(() =>
				{
					NetClient client = new NetClient(null);
					try
					{
						client.Connect(((Server)serverListView.Items[i].Tag).Endpoint);
					}
					catch (Exception) { return; }

					Server updatedServer = client.Query();

					this.InvokeIfRequired(() =>
					{
						serverListView.Items[i] = GetServerListViewItem(updatedServer);
					});
				})
				{
					IsBackground = true,
					Name = "Server Query Thread",
					Priority = ThreadPriority.BelowNormal,
				}.Start();
			}
		}

		private ListViewItem GetServerListViewItem(Server server)
		{
			return new ListViewItem(new string[] { server.Name, server.Endpoint.ToString(), $"{server.Online}/{server.MaxOnline}" })
			{
				Tag = server
			};
		}
	}
}
