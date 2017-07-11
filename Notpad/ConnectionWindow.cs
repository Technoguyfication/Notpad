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

		public ConnectionWindow(Notpad mainForm)
		{
			MainForm = mainForm;
			InitializeComponent();
		}

		private void WindowLoaded (object sender, EventArgs e)
		{
			PopulateServers();
			QueryServers();
			usernameTextbox.Text = RegSettings.Username;
			CheckServerSelected();
		}

		private void PopulateServers()
		{
			Server[] Servers = RegSettings.Servers;
			serverListView.Items.Clear();
			foreach (Server server in Servers)
			{
				AddServerListing(server);
			}
		}

		private void UpdateServer(Server server)
		{
			List<Server> servers = RegSettings.Servers.ToList();
			int serverIndex = servers.FindIndex((s) => { return s.UniqueID == server.UniqueID; });

			if (serverIndex == -1)
				servers.Add(server);
			else
				servers[serverIndex] = server;

			RegSettings.Servers = servers.ToArray();
			CheckServerSelected();
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
					server.Status = ServerStatus.UNAVAILABLE;

					this.InvokeIfRequired(() => { AddServerListing(server); CheckServerSelected();  });

					try
					{
						client.Connect(server.Endpoint);
						server = client.Query();
					}
					catch (Exception)   // server is most likely offline or otherwise unreachable
					{
						server.Status = ServerStatus.OFFLINE;
					}

					this.InvokeIfRequired(() => { AddServerListing(server); CheckServerSelected(); });
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

		private void AddServerListing(Server server)
		{
			ListViewItem item = serverListView.Items.Find((ListViewItem _item) =>
			{
				return ((Server)_item.Tag).Equals(server);
			});

			if (item == null)
			{
				serverListView.Items.Add(CreateItemForServer(server));
				return;
			}

			serverListView.Items[serverListView.Items.IndexOf(item)] = CreateItemForServer(server);
		}

		private ListViewItem CreateItemForServer(Server server)
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
			MainForm.InvokeIfRequired(() =>
			{
				string username = usernameTextbox.Text.Trim();
				if (string.IsNullOrEmpty(username))
				{
					MessageBox.Show("Please enter a username.", "Error", MessageBoxButtons.OK);
					return;
				}
				Server server = GetSelectedServer();
				if (server == null)
				{
					MessageBox.Show("No server selected");
					return;
				}
				DialogResult = DialogResult.OK;
				MainForm.InvokeIfRequired(() =>
				{
					MainForm.ConnectToServer(GetSelectedServer().Endpoint, username);
				});
			});
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
	}
}
