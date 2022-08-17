namespace Technoguyfication.Notpad
{
	partial class DebugForm
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
			this.connectButton = new System.Windows.Forms.Button();
			this.addressTextBox = new System.Windows.Forms.TextBox();
			this.chatTextBox = new System.Windows.Forms.TextBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.sendServerButton = new System.Windows.Forms.Button();
			this.disconnectButton = new System.Windows.Forms.Button();
			this.debugConsoleAutoScrollCheckBox = new System.Windows.Forms.CheckBox();
			this.debugConsoleTextBox = new Technoguyfication.Notpad.Components.ScrollTextBox();
			this.SuspendLayout();
			// 
			// connectButton
			// 
			this.connectButton.Location = new System.Drawing.Point(236, 12);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(75, 23);
			this.connectButton.TabIndex = 0;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			// 
			// addressTextBox
			// 
			this.addressTextBox.Location = new System.Drawing.Point(12, 12);
			this.addressTextBox.Name = "addressTextBox";
			this.addressTextBox.Size = new System.Drawing.Size(218, 23);
			this.addressTextBox.TabIndex = 1;
			// 
			// chatTextBox
			// 
			this.chatTextBox.Location = new System.Drawing.Point(12, 95);
			this.chatTextBox.Multiline = true;
			this.chatTextBox.Name = "chatTextBox";
			this.chatTextBox.Size = new System.Drawing.Size(402, 83);
			this.chatTextBox.TabIndex = 2;
			// 
			// sendButton
			// 
			this.sendButton.Location = new System.Drawing.Point(339, 184);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(75, 23);
			this.sendButton.TabIndex = 4;
			this.sendButton.Text = "Send";
			this.sendButton.UseVisualStyleBackColor = true;
			// 
			// sendServerButton
			// 
			this.sendServerButton.Location = new System.Drawing.Point(227, 184);
			this.sendServerButton.Name = "sendServerButton";
			this.sendServerButton.Size = new System.Drawing.Size(106, 23);
			this.sendServerButton.TabIndex = 4;
			this.sendServerButton.Text = "Send as Server";
			this.sendServerButton.UseVisualStyleBackColor = true;
			// 
			// disconnectButton
			// 
			this.disconnectButton.Location = new System.Drawing.Point(317, 12);
			this.disconnectButton.Name = "disconnectButton";
			this.disconnectButton.Size = new System.Drawing.Size(97, 23);
			this.disconnectButton.TabIndex = 0;
			this.disconnectButton.Text = "Disconnect";
			this.disconnectButton.UseVisualStyleBackColor = true;
			// 
			// debugConsoleAutoScrollCheckBox
			// 
			this.debugConsoleAutoScrollCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.debugConsoleAutoScrollCheckBox.AutoSize = true;
			this.debugConsoleAutoScrollCheckBox.Checked = true;
			this.debugConsoleAutoScrollCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.debugConsoleAutoScrollCheckBox.Location = new System.Drawing.Point(1058, 458);
			this.debugConsoleAutoScrollCheckBox.Name = "debugConsoleAutoScrollCheckBox";
			this.debugConsoleAutoScrollCheckBox.Size = new System.Drawing.Size(84, 19);
			this.debugConsoleAutoScrollCheckBox.TabIndex = 5;
			this.debugConsoleAutoScrollCheckBox.Text = "Auto Scroll";
			this.debugConsoleAutoScrollCheckBox.UseVisualStyleBackColor = true;
			this.debugConsoleAutoScrollCheckBox.CheckedChanged += new System.EventHandler(this.debugConsoleAutoScrollCheckBox_CheckedChanged);
			// 
			// debugConsoleTextBox
			// 
			this.debugConsoleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.debugConsoleTextBox.Location = new System.Drawing.Point(420, 12);
			this.debugConsoleTextBox.Multiline = true;
			this.debugConsoleTextBox.Name = "debugConsoleTextBox";
			this.debugConsoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.debugConsoleTextBox.Size = new System.Drawing.Size(722, 440);
			this.debugConsoleTextBox.TabIndex = 6;
			this.debugConsoleTextBox.ScrollChanged += new System.Windows.Forms.ScrollEventHandler(this.debugConsoleTextBox_ScrollChanged);
			// 
			// DebugForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1154, 489);
			this.Controls.Add(this.debugConsoleTextBox);
			this.Controls.Add(this.debugConsoleAutoScrollCheckBox);
			this.Controls.Add(this.sendServerButton);
			this.Controls.Add(this.sendButton);
			this.Controls.Add(this.chatTextBox);
			this.Controls.Add(this.addressTextBox);
			this.Controls.Add(this.disconnectButton);
			this.Controls.Add(this.connectButton);
			this.Name = "DebugForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DebugForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.TextBox addressTextBox;
		private System.Windows.Forms.TextBox chatTextBox;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.Button sendServerButton;
		private System.Windows.Forms.Button disconnectButton;
		private System.Windows.Forms.CheckBox debugConsoleAutoScrollCheckBox;
		private Components.ScrollTextBox debugConsoleTextBox;
	}
}