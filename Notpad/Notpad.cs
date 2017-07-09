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

namespace Notpad.Client
{
	public partial class Notpad : Form
	{
		public Notpad()
		{
			InitializeComponent();
		}

		private void SetTitle(string title)
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
				mainTextBox.Font = dialog.Font;
			}
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

		private void StatusBarMenuItemClick(object sender, EventArgs e)
		{
			SetTitle(RandomFileName());
		}

		private void WindowClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				CloseConfirm unsavedChanges = new CloseConfirm();
				DialogResult result = unsavedChanges.ShowDialog(this);
				if (result == DialogResult.Cancel)
					e.Cancel = true;
			}
		}
	}
}
