namespace Notpad.Client
{
	partial class ConnectionWindow
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
			System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Online", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Offline", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Querying...", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Offline Server",
            "154.894.648.265:8888",
            "0/0"}, -1);
			System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Unknown Server",
            "8.8.8.8:8888",
            "0/0"}, -1);
			System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Online Server",
            "127.0.0.1:2424",
            "5/25"}, -1);
			this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.addressColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.onlineColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.serverListView = new System.Windows.Forms.ListView();
			this.queryButton = new System.Windows.Forms.Button();
			this.connectButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.newButton = new System.Windows.Forms.Button();
			this.usernameTextbox = new System.Windows.Forms.TextBox();
			this.usernameLabel = new System.Windows.Forms.Label();
			this.directConnectButton = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// nameColumn
			// 
			this.nameColumn.Text = "Name";
			this.nameColumn.Width = 120;
			// 
			// addressColumn
			// 
			this.addressColumn.Text = "Address";
			this.addressColumn.Width = 125;
			// 
			// onlineColumn
			// 
			this.onlineColumn.Text = "Online";
			this.onlineColumn.Width = 55;
			// 
			// serverListView
			// 
			this.serverListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.serverListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.addressColumn,
            this.onlineColumn});
			this.serverListView.FullRowSelect = true;
			listViewGroup4.Header = "Online";
			listViewGroup4.Name = "onlineGroup";
			listViewGroup5.Header = "Offline";
			listViewGroup5.Name = "offlineGroup";
			listViewGroup6.Header = "Querying...";
			listViewGroup6.Name = "queryingGroup";
			this.serverListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
			listViewItem4.Group = listViewGroup5;
			listViewItem4.StateImageIndex = 0;
			listViewItem5.Group = listViewGroup6;
			listViewItem6.Group = listViewGroup4;
			this.serverListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
			this.serverListView.Location = new System.Drawing.Point(12, 12);
			this.serverListView.MultiSelect = false;
			this.serverListView.Name = "serverListView";
			this.serverListView.Size = new System.Drawing.Size(356, 163);
			this.serverListView.TabIndex = 0;
			this.serverListView.UseCompatibleStateImageBehavior = false;
			this.serverListView.View = System.Windows.Forms.View.Details;
			this.serverListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ServerListViewSelectionChanged);
			// 
			// queryButton
			// 
			this.queryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.queryButton.Location = new System.Drawing.Point(103, 207);
			this.queryButton.Name = "queryButton";
			this.queryButton.Size = new System.Drawing.Size(64, 23);
			this.queryButton.TabIndex = 4;
			this.queryButton.Text = "Refresh";
			this.queryButton.UseVisualStyleBackColor = true;
			this.queryButton.Click += new System.EventHandler(this.QueryButtonClick);
			// 
			// connectButton
			// 
			this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.connectButton.Location = new System.Drawing.Point(12, 207);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(85, 23);
			this.connectButton.TabIndex = 2;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.ConnectButtonClick);
			// 
			// deleteButton
			// 
			this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.deleteButton.Location = new System.Drawing.Point(240, 207);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(65, 23);
			this.deleteButton.TabIndex = 6;
			this.deleteButton.Text = "Delete";
			this.deleteButton.UseVisualStyleBackColor = true;
			// 
			// newButton
			// 
			this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.newButton.Location = new System.Drawing.Point(173, 207);
			this.newButton.Name = "newButton";
			this.newButton.Size = new System.Drawing.Size(61, 23);
			this.newButton.TabIndex = 5;
			this.newButton.Text = "New";
			this.newButton.UseVisualStyleBackColor = true;
			// 
			// usernameTextbox
			// 
			this.usernameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.usernameTextbox.Location = new System.Drawing.Point(76, 181);
			this.usernameTextbox.Name = "usernameTextbox";
			this.usernameTextbox.Size = new System.Drawing.Size(158, 20);
			this.usernameTextbox.TabIndex = 1;
			this.usernameTextbox.TextChanged += new System.EventHandler(this.UsernameTextBoxTextChanged);
			// 
			// usernameLabel
			// 
			this.usernameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.usernameLabel.AutoSize = true;
			this.usernameLabel.Location = new System.Drawing.Point(12, 184);
			this.usernameLabel.Name = "usernameLabel";
			this.usernameLabel.Size = new System.Drawing.Size(58, 13);
			this.usernameLabel.TabIndex = 6;
			this.usernameLabel.Text = "Username:";
			// 
			// directConnectButton
			// 
			this.directConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.directConnectButton.Location = new System.Drawing.Point(266, 179);
			this.directConnectButton.Name = "directConnectButton";
			this.directConnectButton.Size = new System.Drawing.Size(102, 23);
			this.directConnectButton.TabIndex = 3;
			this.directConnectButton.Text = "Direct Connect";
			this.directConnectButton.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(311, 207);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(57, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Update";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// ConnectionWindow
			// 
			this.AcceptButton = this.connectButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(380, 242);
			this.Controls.Add(this.directConnectButton);
			this.Controls.Add(this.usernameLabel);
			this.Controls.Add(this.usernameTextbox);
			this.Controls.Add(this.newButton);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.connectButton);
			this.Controls.Add(this.queryButton);
			this.Controls.Add(this.serverListView);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(396, 281);
			this.Name = "ConnectionWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Connect - Notpad";
			this.Load += new System.EventHandler(this.WindowLoaded);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WindowKeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ColumnHeader nameColumn;
		private System.Windows.Forms.ColumnHeader addressColumn;
		private System.Windows.Forms.ColumnHeader onlineColumn;
		private System.Windows.Forms.ListView serverListView;
		private System.Windows.Forms.Button queryButton;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button newButton;
		private System.Windows.Forms.TextBox usernameTextbox;
		private System.Windows.Forms.Label usernameLabel;
		private System.Windows.Forms.Button directConnectButton;
		private System.Windows.Forms.Button button1;
	}
}