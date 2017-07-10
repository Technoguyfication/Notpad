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
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "topkekm8",
            "test",
            "awdawdaw"}, -1);
			this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.addressColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.onlineColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.serverListView = new System.Windows.Forms.ListView();
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
			listViewItem2.StateImageIndex = 0;
			this.serverListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
			this.serverListView.Location = new System.Drawing.Point(12, 12);
			this.serverListView.MultiSelect = false;
			this.serverListView.Name = "serverListView";
			this.serverListView.Size = new System.Drawing.Size(318, 158);
			this.serverListView.TabIndex = 0;
			this.serverListView.UseCompatibleStateImageBehavior = false;
			this.serverListView.View = System.Windows.Forms.View.Details;
			// 
			// ConnectionWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(342, 290);
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
	}
}