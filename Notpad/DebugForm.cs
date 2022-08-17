using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technoguyfication.Notpad
{
	public partial class DebugForm : Form
	{
		private MainForm _mainForm;

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
		internal static extern IntPtr GetFocus();

		public DebugForm(MainForm mainForm)
		{
			_mainForm = mainForm;

			InitializeComponent();

			// set up logger
			Program.DebugMessage += (s, message) =>
			{
				var oldScrollPos = debugConsoleTextBox.VerticalScrollPosition;
				var oldSelectStart = debugConsoleTextBox.SelectionStart;
				var oldSelectLength = debugConsoleTextBox.SelectionLength;
				var wasFocused = debugConsoleTextBox.Focused;
				
				// append text to textbox
				debugConsoleTextBox.AppendText(message + Environment.NewLine);

				// return scroll if auto scroll is off
				if (!debugConsoleAutoScrollCheckBox.Checked)
				{
					debugConsoleTextBox.VerticalScrollPosition = oldScrollPos;					
				}

				// return focus and selection
				if (wasFocused)
				{
					debugConsoleTextBox.Focus();
					debugConsoleTextBox.Select(oldSelectStart, oldSelectLength);
				}
			};
		}

		private void DebugScrollToEnd()
		{
			debugConsoleTextBox.Select(debugConsoleTextBox.Text.Length, 0);
			debugConsoleTextBox.ScrollToCaret();
		}

		private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
			}
		}

		private void connectButton_Click(object sender, EventArgs e)
		{

		}

		private void debugConsoleAutoScrollCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			// scroll to end when auto scroll enabled
			if (debugConsoleAutoScrollCheckBox.Checked)
			{
				DebugScrollToEnd();
			}
		}

		private void debugConsoleTextBox_ScrollChanged(object sender, ScrollEventArgs e)
		{
			// stop auto scroll on scroll event
			debugConsoleAutoScrollCheckBox.Checked = false;
		}
	}
}
