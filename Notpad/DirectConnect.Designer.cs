namespace Notpad.Client
{
	partial class DirectConnect
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
			this.addressTextBox = new System.Windows.Forms.TextBox();
			this.connectButton = new System.Windows.Forms.Button();
			this.portTextBox = new System.Windows.Forms.TextBox();
			this.portLabel = new System.Windows.Forms.Label();
			this.addressLabel = new System.Windows.Forms.Label();
			this.validationToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// addressTextBox
			// 
			this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.addressTextBox.Location = new System.Drawing.Point(66, 12);
			this.addressTextBox.Name = "addressTextBox";
			this.addressTextBox.Size = new System.Drawing.Size(144, 20);
			this.addressTextBox.TabIndex = 0;
			// 
			// connectButton
			// 
			this.connectButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.connectButton.Location = new System.Drawing.Point(311, 10);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(75, 23);
			this.connectButton.TabIndex = 1;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.ConnectButtonClick);
			// 
			// portTextBox
			// 
			this.portTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.portTextBox.Location = new System.Drawing.Point(251, 12);
			this.portTextBox.MaxLength = 10;
			this.portTextBox.Name = "portTextBox";
			this.portTextBox.Size = new System.Drawing.Size(54, 20);
			this.portTextBox.TabIndex = 2;
			this.validationToolTip.SetToolTip(this.portTextBox, "invalid stuff");
			// 
			// portLabel
			// 
			this.portLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.portLabel.AutoSize = true;
			this.portLabel.Location = new System.Drawing.Point(216, 15);
			this.portLabel.Name = "portLabel";
			this.portLabel.Size = new System.Drawing.Size(29, 13);
			this.portLabel.TabIndex = 3;
			this.portLabel.Text = "Port:";
			// 
			// addressLabel
			// 
			this.addressLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.addressLabel.AutoSize = true;
			this.addressLabel.Location = new System.Drawing.Point(12, 15);
			this.addressLabel.Name = "addressLabel";
			this.addressLabel.Size = new System.Drawing.Size(48, 13);
			this.addressLabel.TabIndex = 3;
			this.addressLabel.Text = "Address:";
			// 
			// validationToolTip
			// 
			this.validationToolTip.IsBalloon = true;
			// 
			// DirectConnect
			// 
			this.AcceptButton = this.connectButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(398, 44);
			this.Controls.Add(this.addressLabel);
			this.Controls.Add(this.portLabel);
			this.Controls.Add(this.portTextBox);
			this.Controls.Add(this.connectButton);
			this.Controls.Add(this.addressTextBox);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1000, 83);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(358, 83);
			this.Name = "DirectConnect";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Direct Connect";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WindowKeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox addressTextBox;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.TextBox portTextBox;
		private System.Windows.Forms.Label portLabel;
		private System.Windows.Forms.Label addressLabel;
		private System.Windows.Forms.ToolTip validationToolTip;
	}
}