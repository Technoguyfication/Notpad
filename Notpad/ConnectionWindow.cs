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
		List<Thread> queryThreads = new List<Thread>();
		DirectConnect directConnect;

		public ConnectionWindow(Notpad mainForm)
		{
			MainForm = mainForm;
			InitializeComponent();
			directConnect = new DirectConnect(this);
		}

		private void WindowLoaded(object sender, EventArgs e)
		{
			serverListView.Items.Clear();

			PopulateServers();
			QueryServers();
			usernameTextbox.Text = RegSettings.Username;
			CheckServerSelected();
		}

		private void PopulateServers()
		{
			Server[] Servers = RegSettings.Servers;
			foreach (Server server in Servers)
			{
				UpdateServerListing(server);
			}
		}

		private void QueryServers()
		{
			foreach (Thread thread in queryThreads)
			{
				thread.Abort();
			}

			queryThreads.Clear();

			foreach (ListViewItem item in serverListView.Items)
			{
				Thread thread = new Thread(() =>
				{
					NetClient client = new NetClient(null);
					Server server = GetServerFromItem(item);
					client.ServerQueryReceived += (object sender, ServerQueryReceivedEventArgs e) =>
					{
						updateServer(e.Server);
						e.Client.Dispose();
					};
					client.ConnectionDisconnected += (object sender, ConnectionDisconnectedEventArgs e) =>
					{
						server.Status = ServerStatus.OFFLINE;
						updateServer(server);
					};
					client.Connected += (object sender, EventArgs e) =>
					{
						client.Write(NetClient.GetQueryPacket());
					};

					server.Status = ServerStatus.UNAVAILABLE;
					updateServer(server);

					try
					{
						client.Connect(server);
					}
					catch (Exception)
					{ }

					void updateServer(Server s)
					{
						this.InvokeIfRequired(() => { UpdateServerListing(s); CheckServerSelected(); });
					}
				})
				{
					IsBackground = true,
					Name = "Server Query Thread",
					Priority = ThreadPriority.BelowNormal,
				};
				queryThreads.Add(thread);
			}

			try
			{
				foreach (Thread thread in queryThreads)
				{
					if (thread.ThreadState != ThreadState.Running)
						thread.Start();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Could not poll servers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateServerListing(Server server)
		{
			ListViewItem item = serverListView.Items.Find((ListViewItem _item) =>
			{
				return ((Server)_item.Tag).Equals(server);
			});

			if (item == null)
			{
				serverListView.Items.Add(CreateItemForServer(server, false));
				return;
			}

			int itemIndex = serverListView.Items.IndexOf(item);
			serverListView.Items[itemIndex] = CreateItemForServer(server, item.Selected);
		}

		private ListViewItem CreateItemForServer(Server server, bool selected)
		{
			ListViewItem item = new ListViewItem(new string[] { server.Name, server.ToString(), $"{server.Online}/{server.MaxOnline}" })
			{
				Tag = server,
				Selected = selected,
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

		private Server GetServerFromItem(ListViewItem item)
		{
			return ((Server)item.Tag);
		}

		private void QueryButtonClick(object sender, EventArgs e)
		{
			QueryServers();
		}

		private void ConnectButtonClick(object sender, EventArgs e)
		{
			Server server = GetSelectedServer();
			if (server == null)
			{
				MessageBox.Show("No server selected");
				return;
			}
			Connect(server);
		}

		public void Connect(Server server)
		{
			string username = usernameTextbox.Text.Trim();
			if (string.IsNullOrEmpty(username))
			{
				MessageBox.Show("Please enter a username.", "Error", MessageBoxButtons.OK);
				return;
			}

			MainForm.InvokeIfRequired(() =>
			{
				MainForm.ConnectToServer(server, username);
			});

			DialogResult = DialogResult.OK;
			return;
		}

		private Server GetSelectedServer()
		{
			if (serverListView.SelectedItems.Count <= 0)
				return null;
			return GetServerFromItem(serverListView.SelectedItems[0]);
		}

		private void WindowKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				QueryServers();
				e.SuppressKeyPress = true;
			}
			else if (e.KeyCode == Keys.Escape)
			{
				Hide();
				e.SuppressKeyPress = true;
			}
		}

		private void UsernameTextBoxTextChanged(object sender, EventArgs e)
		{
			RegSettings.Username = usernameTextbox.Text.Trim();
		}

		private void CheckServerSelected()
		{
			if (GetSelectedServer() == null)
			{
				connectButton.Enabled = false;
			}
			else
			{
				connectButton.Enabled = true;
			}
		}

		private void ServerListViewSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			CheckServerSelected();
		}

		private void DirectConnectButtonClick(object sender, EventArgs e)
		{
			directConnect.ShowDialog();
		}
	}
}
