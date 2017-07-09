namespace Notpad.Client
{
	partial class CloseConfirm
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
			this.highlightPanel = new System.Windows.Forms.Panel();
			this.alsoCloseButton = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.infoLabel = new System.Windows.Forms.Label();
			this.infoLabel2 = new System.Windows.Forms.Label();
			this.basicToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.highlightPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// highlightPanel
			// 
			this.highlightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.highlightPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.highlightPanel.Controls.Add(this.alsoCloseButton);
			this.highlightPanel.Controls.Add(this.closeButton);
			this.highlightPanel.Controls.Add(this.cancelButton);
			this.highlightPanel.Location = new System.Drawing.Point(0, 82);
			this.highlightPanel.Name = "highlightPanel";
			this.highlightPanel.Size = new System.Drawing.Size(336, 41);
			this.highlightPanel.TabIndex = 0;
			// 
			// alsoCloseButton
			// 
			this.alsoCloseButton.Font = new System.Drawing.Font("Calibri", 9.5F);
			this.alsoCloseButton.Location = new System.Drawing.Point(154, 9);
			this.alsoCloseButton.Name = "alsoCloseButton";
			this.alsoCloseButton.Size = new System.Drawing.Size(92, 23);
			this.alsoCloseButton.TabIndex = 1;
			this.alsoCloseButton.Text = "Also Close";
			this.basicToolTip.SetToolTip(this.alsoCloseButton, "The same as \"Close\"\r\n\r\nThis only exists to keep UI consistency");
			this.alsoCloseButton.UseVisualStyleBackColor = true;
			this.alsoCloseButton.Click += new System.EventHandler(this.CloseButtonClick);
			// 
			// closeButton
			// 
			this.closeButton.Font = new System.Drawing.Font("Calibri", 9.5F);
			this.closeButton.Location = new System.Drawing.Point(76, 9);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(72, 23);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "Close";
			this.basicToolTip.SetToolTip(this.closeButton, "Close Notpad, terminating any open connection");
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Font = new System.Drawing.Font("Calibri", 9.5F);
			this.cancelButton.Location = new System.Drawing.Point(252, 9);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(72, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.basicToolTip.SetToolTip(this.cancelButton, "Don\'t close Notpad");
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
			// 
			// infoLabel
			// 
			this.infoLabel.AutoSize = true;
			this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.infoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(82)))), ((int)(((byte)(155)))));
			this.infoLabel.Location = new System.Drawing.Point(12, 9);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(192, 20);
			this.infoLabel.TabIndex = 1;
			this.infoLabel.Text = "You are currently connected";
			// 
			// infoLabel2
			// 
			this.infoLabel2.AutoSize = true;
			this.infoLabel2.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.infoLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(82)))), ((int)(((byte)(155)))));
			this.infoLabel2.Location = new System.Drawing.Point(12, 29);
			this.infoLabel2.Name = "infoLabel2";
			this.infoLabel2.Size = new System.Drawing.Size(316, 20);
			this.infoLabel2.TabIndex = 2;
			this.infoLabel2.Text = "...\\closing will disconnect you from any active...";
			// 
			// CloseConfirm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(336, 123);
			this.Controls.Add(this.infoLabel2);
			this.Controls.Add(this.infoLabel);
			this.Controls.Add(this.highlightPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CloseConfirm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Notpad";
			this.highlightPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel highlightPanel;
		private System.Windows.Forms.Button alsoCloseButton;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.Label infoLabel2;
		private System.Windows.Forms.ToolTip basicToolTip;
	}
}