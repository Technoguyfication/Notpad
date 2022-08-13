
namespace Technoguyfication.Notpad
{
	partial class ConnectForm
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Local Network", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Test Item");
			this.listView1 = new System.Windows.Forms.ListView();
			this.serverListViewAddressColumn = new System.Windows.Forms.ColumnHeader();
			this.serverListViewNameColumn = new System.Windows.Forms.ColumnHeader();
			this.serverListViewOnlineColumn = new System.Windows.Forms.ColumnHeader();
			this.serverListViewMotdColumn = new System.Windows.Forms.ColumnHeader();
			this.queryAddressTextBox = new System.Windows.Forms.TextBox();
			this.queryResponseTextBox = new System.Windows.Forms.TextBox();
			this.queryButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.serverListViewAddressColumn,
            this.serverListViewNameColumn,
            this.serverListViewOnlineColumn,
            this.serverListViewMotdColumn});
			listViewGroup1.Header = "Local Network";
			listViewGroup1.Name = "serverListViewLocalGroup";
			this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
			listViewItem1.Group = listViewGroup1;
			this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(559, 143);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// serverListViewAddressColumn
			// 
			this.serverListViewAddressColumn.Text = "Address";
			this.serverListViewAddressColumn.Width = 100;
			// 
			// serverListViewNameColumn
			// 
			this.serverListViewNameColumn.Text = "Server Name";
			this.serverListViewNameColumn.Width = 150;
			// 
			// serverListViewOnlineColumn
			// 
			this.serverListViewOnlineColumn.Text = "Online";
			// 
			// serverListViewMotdColumn
			// 
			this.serverListViewMotdColumn.Text = "MOTD";
			this.serverListViewMotdColumn.Width = 300;
			// 
			// queryAddressTextBox
			// 
			this.queryAddressTextBox.Location = new System.Drawing.Point(142, 282);
			this.queryAddressTextBox.Name = "queryAddressTextBox";
			this.queryAddressTextBox.Size = new System.Drawing.Size(227, 23);
			this.queryAddressTextBox.TabIndex = 1;
			// 
			// queryResponseTextBox
			// 
			this.queryResponseTextBox.Location = new System.Drawing.Point(142, 322);
			this.queryResponseTextBox.Multiline = true;
			this.queryResponseTextBox.Name = "queryResponseTextBox";
			this.queryResponseTextBox.Size = new System.Drawing.Size(308, 96);
			this.queryResponseTextBox.TabIndex = 2;
			// 
			// queryButton
			// 
			this.queryButton.Location = new System.Drawing.Point(375, 282);
			this.queryButton.Name = "queryButton";
			this.queryButton.Size = new System.Drawing.Size(75, 23);
			this.queryButton.TabIndex = 3;
			this.queryButton.Text = "Query";
			this.queryButton.UseVisualStyleBackColor = true;
			this.queryButton.Click += new System.EventHandler(this.queryButton_Click);
			// 
			// ConnectForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.queryButton);
			this.Controls.Add(this.queryResponseTextBox);
			this.Controls.Add(this.queryAddressTextBox);
			this.Controls.Add(this.listView1);
			this.Name = "ConnectForm";
			this.Text = "ConnectForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader serverListViewAddressColumn;
		private System.Windows.Forms.ColumnHeader serverListViewNameColumn;
		private System.Windows.Forms.ColumnHeader serverListViewOnlineColumn;
		private System.Windows.Forms.ColumnHeader serverListViewMotdColumn;
		private System.Windows.Forms.TextBox queryAddressTextBox;
		private System.Windows.Forms.TextBox queryResponseTextBox;
		private System.Windows.Forms.Button queryButton;
	}
}