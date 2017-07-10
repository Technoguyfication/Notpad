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

			QueryServers();
		}

		private void QueryServers()
		{
			foreach (ListViewItem item in serverListView.Items)
			{
				new Thread(() =>
				{
					NetClient client = new NetClient(null);
					Server server = ((Server)item.Tag);
					try
					{
						client.Connect(server.Endpoint);
						server = client.Query();
					}
					catch (Exception)
					{
						server.Status = ServerStatus.OFFLINE;
					}

					this.InvokeIfRequired(() =>
					{
						serverListView.Items[serverListView.Items.IndexOf(item)] = GetServerListViewItem(server);
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
			ListViewItem item = new ListViewItem(new string[] { server.Name, server.Endpoint.ToString(), $"{server.Online}/{server.MaxOnline}" })
			{
				Tag = server
			};
			switch (server.Status)
			{
				case ServerStatus.UNAVAILABLE:
					item.Group = serverListView.Groups["queryingGroup"];
					break;
				case ServerStatus.OFFLINE:
					item.Group = serverListView.Groups["offlineGroup"];
					break;
				case ServerStatus.ONLINE:
					item.Group = serverListView.Groups["onlineGroup"];
					break;
			}
			return item;
		}

		private void QueryButtonClick(object sender, EventArgs e)
		{
			QueryServers();
		}

		private void ConnectButtonClick(object sender, EventArgs e)
		{

		}
	}
}
