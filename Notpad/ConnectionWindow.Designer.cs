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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Online", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Querying...", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Offline", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "topkekm8",
            "test",
            "awdawdaw"}, -1);
			this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.addressColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.onlineColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.serverListView = new System.Windows.Forms.ListView();
			this.queryButton = new System.Windows.Forms.Button();
			this.connectButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.newButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// nameColumn
			// 
			this.nameColumn.Text = "Name";
			this.nameColumn.Width = 150;
			// 
			// addressColumn
			// 
			this.addressColumn.Text = "Address";
			this.addressColumn.Width = 105;
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
			listViewGroup1.Header = "Online";
			listViewGroup1.Name = "onlineGroup";
			listViewGroup2.Header = "Querying...";
			listViewGroup2.Name = "queryingGroup";
			listViewGroup3.Header = "Offline";
			listViewGroup3.Name = "offlineGroup";
			this.serverListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
			listViewItem1.StateImageIndex = 0;
			this.serverListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.serverListView.Location = new System.Drawing.Point(12, 12);
			this.serverListView.MultiSelect = false;
			this.serverListView.Name = "serverListView";
			this.serverListView.Size = new System.Drawing.Size(318, 158);
			this.serverListView.TabIndex = 0;
			this.serverListView.UseCompatibleStateImageBehavior = false;
			this.serverListView.View = System.Windows.Forms.View.Details;
			// 
			// queryButton
			// 
			this.queryButton.Location = new System.Drawing.Point(12, 176);
			this.queryButton.Name = "queryButton";
			this.queryButton.Size = new System.Drawing.Size(75, 23);
			this.queryButton.TabIndex = 1;
			this.queryButton.Text = "Refresh";
			this.queryButton.UseVisualStyleBackColor = true;
			this.queryButton.Click += new System.EventHandler(this.QueryButtonClick);
			// 
			// connectButton
			// 
			this.connectButton.Location = new System.Drawing.Point(245, 176);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(85, 23);
			this.connectButton.TabIndex = 2;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.ConnectButtonClick);
			// 
			// deleteButton
			// 
			this.deleteButton.Location = new System.Drawing.Point(93, 176);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(67, 23);
			this.deleteButton.TabIndex = 3;
			this.deleteButton.Text = "Delete";
			this.deleteButton.UseVisualStyleBackColor = true;
			// 
			// newButton
			// 
			this.newButton.Location = new System.Drawing.Point(166, 176);
			this.newButton.Name = "newButton";
			this.newButton.Size = new System.Drawing.Size(57, 23);
			this.newButton.TabIndex = 4;
			this.newButton.Text = "New";
			this.newButton.UseVisualStyleBackColor = true;
			// 
			// ConnectionWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(342, 211);
			this.Controls.Add(this.newButton);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.connectButton);
			this.Controls.Add(this.queryButton);
			this.Controls.Add(this.serverListView);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Connect - Notpad";
			this.Load += new System.EventHandler(this.WindowLoad);
			this.ResumeLayout(false);

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
	}
}