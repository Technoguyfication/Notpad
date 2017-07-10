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

namespace Notpad.Client
{
	public partial class Notpad : Form
	{
		public int MaxLines = 50;
		public NetClient Client;
		public ConnectionWindow connectionWindow;

		public Notpad()
		{
			InitializeComponent();
			connectionWindow = new ConnectionWindow(this);
		}

		#region Network Interaction



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
			if (e.CloseReason == CloseReason.UserClosing)
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
				PrintString(inputTextBox.Text.Trim());
				inputTextBox.Text = "";
			}
		}

		private void AboutMenuItemClick(object sender, EventArgs e)
		{
			new AboutBox().ShowDialog();
		}

		private void RTLChanged(RightToLeft rtl)
		{
			mainTextBox.RightToLeft = inputTextBox.RightToLeft = rtl;
		}

		public void PrintString(string text, bool newline = true)
		{
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
	}
}
