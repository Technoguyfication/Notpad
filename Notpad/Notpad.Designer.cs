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
			this.editMenuItem = new System.Windows.Forms.MenuItem();
			this.formatMenuItem = new System.Windows.Forms.MenuItem();
			this.wordWrapMenuItem = new System.Windows.Forms.MenuItem();
			this.fontMenuItem = new System.Windows.Forms.MenuItem();
			this.viewMenuItem = new System.Windows.Forms.MenuItem();
			this.statusBarMenuItem = new System.Windows.Forms.MenuItem();
			this.helpMenuItem = new System.Windows.Forms.MenuItem();
			this.mainTextBox = new System.Windows.Forms.TextBox();
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
			this.fileMenuItem.Text = "&File";
			// 
			// editMenuItem
			// 
			this.editMenuItem.Index = 1;
			this.editMenuItem.Text = "&Edit";
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
            this.statusBarMenuItem});
			this.viewMenuItem.Text = "&View";
			// 
			// statusBarMenuItem
			// 
			this.statusBarMenuItem.Index = 0;
			this.statusBarMenuItem.Text = "Status Bar";
			this.statusBarMenuItem.Click += new System.EventHandler(this.StatusBarMenuItemClick);
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.Index = 4;
			this.helpMenuItem.Text = "&Help";
			// 
			// mainTextBox
			// 
			this.mainTextBox.AcceptsReturn = true;
			this.mainTextBox.AcceptsTab = true;
			this.mainTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mainTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTextBox.Font = new System.Drawing.Font("Consolas", 11.25F);
			this.mainTextBox.Location = new System.Drawing.Point(0, 0);
			this.mainTextBox.Multiline = true;
			this.mainTextBox.Name = "mainTextBox";
			this.mainTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.mainTextBox.Size = new System.Drawing.Size(636, 296);
			this.mainTextBox.TabIndex = 0;
			this.mainTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainTextBoxKeyDown);
			// 
			// Notpad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(636, 296);
			this.Controls.Add(this.mainTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "Notpad";
			this.Text = "Notpad";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowClosing);
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
		private System.Windows.Forms.MenuItem statusBarMenuItem;
	}
}

