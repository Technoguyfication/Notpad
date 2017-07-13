using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Notpad.Client.Util;
using Notpad.Client.Net;
using System.Net;
using System.Threading;

namespace Notpad.Client
{
	public partial class Notpad : Form
	{
		public int MaxLines = 50;
		public NetClient Client;
		public ConnectionWindow connectionWindow;

		private List<string> MessageHistory = new List<string>();
		private int HistoryIndex = -1;

		public Notpad()
		{
			InitializeComponent();
			connectionWindow = new ConnectionWindow(this);
		}

		#region Network Interaction

		public void ConnectToServer(Server server, string username)
		{
			PrintString($"Connecting to {server.ToString()}");
			new Thread(() =>
			{
				try
				{
					if (Client == null)
					{
						Client = new NetClient(username);
						Client.Message += ClientMessage;
						Client.ClientReady += ClientReady;
						Client.Connected += ClientConnected;
						Client.ConnectionDisconnected += ClientConnectionDisconnect;
					}
					else
						Client.Username = username;

					Client.Connect(server);
				}
				catch (Exception e)
				{
					PrintString($"Error connecting to server: {e.Message}");
				}
			})
			{
				Name = "Server Connect",
				IsBackground = true,
			}.Start(); ;
		}

		private void ClientConnected(object sender, EventArgs e)
		{
			Client.Write(NetClient.GetIdentifyPacket(Client.Username));
		}

		private void ClientMessage(object sender, MessageEventArgs e)
		{
			switch (e.Type)
			{
				case MessageType.CHAT:
					PrintString($"{e.Author}: {e.Content}");
					break;
				case MessageType.RAW:
					PrintString(e.Content);
					break;
				case MessageType.NOTIFICATION:
					MessageBox.Show(e.Content, "Server Notification", MessageBoxButtons.OK, e.NotificationIcon);
					break;
				case MessageType.BROADCAST:
					PrintString($"(Broadcast): {e.Content}");
					break;
			}
		}

		private void ClientReady(object sender, EventArgs e)
		{
			NetClient client = (NetClient)sender;
			PrintString($"Connected to {client.CurrentServer.ToString()} as {client.Username}");
		}

		private void ClientConnectionDisconnect(object sender, ConnectionDisconnectedEventArgs e)
		{
			if (e.Reason != null)
				PrintString($"Disconnected: {e.Reason}");
			else
				PrintString("Disconnected.");
		}

		#endregion

		public void SetTitle(string title)
		{
			Text = $"{title} - Notpad";
		}

		private string RandomFileName()
		{
			string[] filenames =
			{
				"tumblr rant",
				"google is satan",
				"come hither",
				"crawling in my skin",
				"life hacks",
				"letter to papa franku",
				"how to eat a bigmac",
				"how can this happen to me",
				"who can say where the road goes",
				"hate letter to alcoholics anonymous",
				"hl3 release date",
				"RIP AND TEAR",
				"satanic ritual instructions",
				"another settlement needs your help",
				"patrolling the mojave almost makes you wish for a nuclear winter",
				"diy chemo at home instructions",
				"best porn sites 2002",
				"not furry porn",
				"hank hill slash fiction",
				"trigger_hurt",
				"moms spaghetti recipe",
				"not a virus.exe",
				"gta v hacks",
				"mineraft servers",
				"how to aimbot in csgo",
				"love me like you do lyrics",
				"top 10 best buzzfeed articles",
				"north korea launch codes",
				"united airlines fanfiction",
				"elon's musk",
				"falcon heavy blueprints",
				"space shuttle porn",
			};
			return $"{filenames[new Random().Next(filenames.Length)]}.txt";
		}

		private void WindowResized(object sender, EventArgs e)
		{
			RegSettings.WindowSizeX = Size.Width;
			RegSettings.WindowSizeY = Size.Height;
		}

		private void WindowLoaded(object sender, EventArgs e)
		{
			Size = new Size(RegSettings.WindowSizeX, RegSettings.WindowSizeY);

			mainTextBox.WordWrap = RegSettings.WordWrap;
			wordWrapMenuItem.Checked = mainTextBox.WordWrap;

			SetFont(RegSettings.SelectedFont);

			SetTitle(RandomFileName());
		}

		private void FontMenuItemClick(object sender, EventArgs e)
		{
			FontDialog dialog = new FontDialog()
			{
				Font = RegSettings.SelectedFont
			};
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				RegSettings.SelectedFont = dialog.Font;
				SetFont(dialog.Font);
			}
		}

		private void SetFont(Font font)
		{
			mainTextBox.Font = font;
			inputTextBox.Font = font;
		}

		private void WordWrapMenuItemClick(object sender, EventArgs e)
		{
			wordWrapMenuItem.Checked ^= true;
			SetWordWrap(wordWrapMenuItem.Checked);
		}

		private void SetWordWrap(bool value)
		{
			mainTextBox.WordWrap = value;
			RegSettings.WordWrap = value;
		}

		private void ChangeTitleMenuItemClick(object sender, EventArgs e)
		{
			SetTitle(RandomFileName());
		}

		private void WindowClosing(object sender, FormClosingEventArgs e)
		{
			if ((e.CloseReason == CloseReason.UserClosing) && Client != null && (int)Client.CurrentState % 8 == 0)
			{
				ConfirmClose unsavedChanges = new ConfirmClose();
				DialogResult result = unsavedChanges.ShowDialog(this);
				if (result == DialogResult.Cancel)
					e.Cancel = true;
			}
		}

		private void MainTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			e.SuppressKeyPress = true;  // suppress by default so we can't type into it
			if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control) // ctrl+a
			{
				mainTextBox.SelectAll();
				e.SuppressKeyPress = true;
			}
			else if ((e.KeyData & Keys.Modifiers) != Keys.None) // allow modifier keys through
				e.SuppressKeyPress = false;
		}

		private void InputTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control) // ctrl+a
			{
				inputTextBox.SelectAll();
				e.SuppressKeyPress = true;
			}
			else if (e.KeyCode == Keys.Enter && e.Modifiers != (Keys.Shift | Keys.Control))
			{
				MessageHistory.AddFirst(inputTextBox.Text);
				HistoryIndex = -1;

				if (Client == null || Client.CurrentState != ClientConnectionState.READY)
					PrintString("Can't send: Client not connected.");
				else
					Client.Write(NetClient.GetMessagePacket(inputTextBox.Text));

				e.SuppressKeyPress = true;
				inputTextBox.Text = string.Empty;
			}
			else if (e.KeyCode == Keys.Up && e.Alt && MessageHistory.Count > 0)
			{
				if (HistoryIndex < MessageHistory.Count - 1)
					HistoryIndex++;

				inputTextBox.Text = MessageHistory[HistoryIndex];
			}
			else if (e.KeyCode == Keys.Down && e.Alt && MessageHistory.Count > 0)
			{
				if (HistoryIndex > 0)
					HistoryIndex--;
				else
				{
					inputTextBox.Text = string.Empty;
					return;
				}

				inputTextBox.Text = MessageHistory[HistoryIndex];
			}
		}

		private void AboutMenuItemClick(object sender, EventArgs e)
		{
			new AboutBox().ShowDialog();
		}

		public void PrintString(string text, bool newline = true)
		{
			if (InvokeRequired)
			{
				this.InvokeIfRequired(() =>
				{
					PrintString(text, newline);
				});
				return;
			}

			List<string> updatedLines = mainTextBox.Lines.ToList();

			if (updatedLines.Count >= MaxLines)
				updatedLines = (List<string>)updatedLines.Take(MaxLines - 1);

			updatedLines.Add(text);

			mainTextBox.Lines = updatedLines.ToArray();
		}

		private void ExitMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		private void TimeDateMenuItemClick(object sender, EventArgs e)
		{
			inputTextBox.Text += DateTime.Now.ToString();
		}

		private void ClearTextMenuItemClick(object sender, EventArgs e)
		{
			mainTextBox.Text = string.Empty;
		}

		private void ConnectMenuItemClick(object sender, EventArgs e)
		{
			connectionWindow.ShowDialog();
		}

		private void DisconnectMenuItemClick(object sender, EventArgs e)
		{
			if (Client != null && Client.CurrentState != ClientConnectionState.DISCONNECTED)
				Client.Disconnect(true);
		}

		private void FileMenuItemPopup(object sender, EventArgs e)
		{
			CheckDisconnectButton();
			CheckReconnectButton();
		}

		private void CheckDisconnectButton()
		{
			if (Client == null)
			{
				disconnectMenuItem.Enabled = false;
				return;
			}

			if ((int)Client.CurrentState % 5 == 0)
			{
				disconnectMenuItem.Enabled = false;
				return;
			}

			disconnectMenuItem.Enabled = true;
		}

		private void CheckReconnectButton()
		{
			if (Client == null)
			{
				reconnectMenuItem.Enabled = false;
				return;
			}

			if ((int)Client.CurrentState % 5 != 0)
			{
				reconnectMenuItem.Enabled = false;
				return;
			}

			if (Client.CurrentServer == null)
			{
				reconnectMenuItem.Enabled = false;
				return;
			}

			reconnectMenuItem.Enabled = true;
		}

		private void ReconnectMenuItemClick(object sender, EventArgs e)
		{
			ConnectToServer(Client.CurrentServer, Client.Username);
		}

		private void WindowClosed(object sender, FormClosedEventArgs e)
		{
			if (Client != null)
				Client.Dispose();
		}
	}
}
