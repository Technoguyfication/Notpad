namespace Notpad.Client
{
	partial class Notpad
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notpad));
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.fileMenuItem = new System.Windows.Forms.MenuItem();
			this.connectMenuItem = new System.Windows.Forms.MenuItem();
			this.disconnectMenuItem = new System.Windows.Forms.MenuItem();
			this.fileMenuItemDivider1 = new System.Windows.Forms.MenuItem();
			this.reconnectMenuItem = new System.Windows.Forms.MenuItem();
			this.fileMenuItemDivider2 = new System.Windows.Forms.MenuItem();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.editMenuItem = new System.Windows.Forms.MenuItem();
			this.clearTextMenuItem = new System.Windows.Forms.MenuItem();
			this.timeDateMenuItem = new System.Windows.Forms.MenuItem();
			this.formatMenuItem = new System.Windows.Forms.MenuItem();
			this.wordWrapMenuItem = new System.Windows.Forms.MenuItem();
			this.fontMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuItem = new System.Windows.Forms.MenuItem();
			this.changeTitleMenuItem = new System.Windows.Forms.MenuItem();
			this.helpMenuItem = new System.Windows.Forms.MenuItem();
			this.viewHelpMenuItem = new System.Windows.Forms.MenuItem();
			this.helpMenuItemDivider = new System.Windows.Forms.MenuItem();
			this.aboutMenuItem = new System.Windows.Forms.MenuItem();
			this.mainTextBox = new System.Windows.Forms.TextBox();
			this.inputTextBox = new System.Windows.Forms.TextBox();
			this.basicToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.formatMenuItem,
            this.viewMenuItem,
            this.helpMenuItem});
			// 
			// fileMenuItem
			// 
			this.fileMenuItem.Index = 0;
			this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.connectMenuItem,
            this.disconnectMenuItem,
            this.fileMenuItemDivider1,
            this.reconnectMenuItem,
            this.fileMenuItemDivider2,
            this.exitMenuItem});
			this.fileMenuItem.Text = "&File";
			this.fileMenuItem.Popup += new System.EventHandler(this.FileMenuItemPopup);
			// 
			// connectMenuItem
			// 
			this.connectMenuItem.Index = 0;
			this.connectMenuItem.Text = "&Connect...";
			this.connectMenuItem.Click += new System.EventHandler(this.ConnectMenuItemClick);
			// 
			// disconnectMenuItem
			// 
			this.disconnectMenuItem.Index = 1;
			this.disconnectMenuItem.Text = "&Disconnect";
			this.disconnectMenuItem.Click += new System.EventHandler(this.DisconnectMenuItemClick);
			// 
			// fileMenuItemDivider1
			// 
			this.fileMenuItemDivider1.Index = 2;
			this.fileMenuItemDivider1.Text = "-";
			// 
			// reconnectMenuItem
			// 
			this.reconnectMenuItem.Index = 3;
			this.reconnectMenuItem.Text = "&Reconnect";
			this.reconnectMenuItem.Click += new System.EventHandler(this.ReconnectMenuItemClick);
			// 
			// fileMenuItemDivider2
			// 
			this.fileMenuItemDivider2.Index = 4;
			this.fileMenuItemDivider2.Text = "-";
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 5;
			this.exitMenuItem.Text = "Exit";
			this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItemClick);
			// 
			// editMenuItem
			// 
			this.editMenuItem.Index = 1;
			this.editMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.clearTextMenuItem,
            this.timeDateMenuItem});
			this.editMenuItem.Text = "&Edit";
			// 
			// clearTextMenuItem
			// 
			this.clearTextMenuItem.Index = 0;
			this.clearTextMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlDel;
			this.clearTextMenuItem.Text = "Clear Text";
			this.clearTextMenuItem.Click += new System.EventHandler(this.ClearTextMenuItemClick);
			// 
			// timeDateMenuItem
			// 
			this.timeDateMenuItem.Index = 1;
			this.timeDateMenuItem.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.timeDateMenuItem.Text = "Time/Date";
			this.timeDateMenuItem.Click += new System.EventHandler(this.TimeDateMenuItemClick);
			// 
			// formatMenuItem
			// 
			this.formatMenuItem.Index = 2;
			this.formatMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.wordWrapMenuItem,
            this.fontMenuItem});
			this.formatMenuItem.Text = "F&ormat";
			// 
			// wordWrapMenuItem
			// 
			this.wordWrapMenuItem.Checked = true;
			this.wordWrapMenuItem.Index = 0;
			this.wordWrapMenuItem.Text = "Word Wrap";
			this.wordWrapMenuItem.Click += new System.EventHandler(this.WordWrapMenuItemClick);
			// 
			// fontMenuItem
			// 
			this.fontMenuItem.Index = 1;
			this.fontMenuItem.Text = "Font...";
			this.fontMenuItem.Click += new System.EventHandler(this.FontMenuItemClick);
			// 
			// viewMenuItem
			// 
			this.viewMenuItem.Index = 3;
			this.viewMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.changeTitleMenuItem});
			this.viewMenuItem.Text = "&View";
			// 
			// changeTitleMenuItem
			// 
			this.changeTitleMenuItem.Index = 0;
			this.changeTitleMenuItem.Text = "Change &Title";
			this.changeTitleMenuItem.Click += new System.EventHandler(this.ChangeTitleMenuItemClick);
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.Index = 4;
			this.helpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.viewHelpMenuItem,
            this.helpMenuItemDivider,
            this.aboutMenuItem});
			this.helpMenuItem.Text = "&Help";
			// 
			// viewHelpMenuItem
			// 
			this.viewHelpMenuItem.Index = 0;
			this.viewHelpMenuItem.Text = "View Help";
			// 
			// helpMenuItemDivider
			// 
			this.helpMenuItemDivider.Index = 1;
			this.helpMenuItemDivider.Text = "-";
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Index = 2;
			this.aboutMenuItem.Text = "About Notpad";
			this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItemClick);
			// 
			// mainTextBox
			// 
			this.mainTextBox.AcceptsReturn = true;
			this.mainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mainTextBox.Font = new System.Drawing.Font("Consolas", 11.25F);
			this.mainTextBox.Location = new System.Drawing.Point(0, 0);
			this.mainTextBox.MaxLength = 2147483646;
			this.mainTextBox.Multiline = true;
			this.mainTextBox.Name = "mainTextBox";
			this.mainTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.mainTextBox.Size = new System.Drawing.Size(636, 231);
			this.mainTextBox.TabIndex = 0;
			this.mainTextBox.TabStop = false;
			this.mainTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainTextBoxKeyDown);
			// 
			// inputTextBox
			// 
			this.inputTextBox.AcceptsReturn = true;
			this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inputTextBox.Font = new System.Drawing.Font("Consolas", 11.25F);
			this.inputTextBox.HideSelection = false;
			this.inputTextBox.Location = new System.Drawing.Point(0, 231);
			this.inputTextBox.Multiline = true;
			this.inputTextBox.Name = "inputTextBox";
			this.inputTextBox.Size = new System.Drawing.Size(636, 65);
			this.inputTextBox.TabIndex = 1;
			this.inputTextBox.Text = "Type your message here\r\n\r\nThis is a multiline text box";
			this.inputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputTextBoxKeyDown);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// Notpad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(636, 296);
			this.Controls.Add(this.inputTextBox);
			this.Controls.Add(this.mainTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "Notpad";
			this.Text = "Notpad";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WindowClosed);
			this.Load += new System.EventHandler(this.WindowLoaded);
			this.ResizeEnd += new System.EventHandler(this.WindowResized);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem fileMenuItem;
		private System.Windows.Forms.MenuItem editMenuItem;
		private System.Windows.Forms.MenuItem formatMenuItem;
		private System.Windows.Forms.MenuItem viewMenuItem;
		private System.Windows.Forms.MenuItem helpMenuItem;
		public System.Windows.Forms.TextBox mainTextBox;
		private System.Windows.Forms.MenuItem wordWrapMenuItem;
		private System.Windows.Forms.MenuItem fontMenuItem;
		private System.Windows.Forms.MenuItem changeTitleMenuItem;
		private System.Windows.Forms.MenuItem viewHelpMenuItem;
		private System.Windows.Forms.MenuItem helpMenuItemDivider;
		private System.Windows.Forms.MenuItem aboutMenuItem;
		private System.Windows.Forms.ToolTip basicToolTip;
		public System.Windows.Forms.TextBox inputTextBox;
		private System.Windows.Forms.MenuItem fileMenuItemDivider2;
		private System.Windows.Forms.MenuItem exitMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.MenuItem timeDateMenuItem;
		private System.Windows.Forms.MenuItem clearTextMenuItem;
		private System.Windows.Forms.MenuItem connectMenuItem;
		private System.Windows.Forms.MenuItem disconnectMenuItem;
		private System.Windows.Forms.MenuItem fileMenuItemDivider1;
		private System.Windows.Forms.MenuItem reconnectMenuItem;
	}
}

