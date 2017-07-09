namespace Notpad.Client
{
	partial class About
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
			this.notpadLabel = new System.Windows.Forms.Label();
			this.horizontalDivider = new System.Windows.Forms.Label();
			this.authorLabel = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			this.copyrightLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.logoBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
			this.SuspendLayout();
			// 
			// notpadLabel
			// 
			this.notpadLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.notpadLabel.Font = new System.Drawing.Font("Segoe UI", 40F);
			this.notpadLabel.Location = new System.Drawing.Point(12, 9);
			this.notpadLabel.Name = "notpadLabel";
			this.notpadLabel.Size = new System.Drawing.Size(420, 78);
			this.notpadLabel.TabIndex = 0;
			this.notpadLabel.Text = "Notpad";
			this.notpadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// horizontalDivider
			// 
			this.horizontalDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.horizontalDivider.Location = new System.Drawing.Point(12, 97);
			this.horizontalDivider.Name = "horizontalDivider";
			this.horizontalDivider.Size = new System.Drawing.Size(420, 1);
			this.horizontalDivider.TabIndex = 1;
			// 
			// authorLabel
			// 
			this.authorLabel.AutoSize = true;
			this.authorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.authorLabel.Location = new System.Drawing.Point(54, 114);
			this.authorLabel.Name = "authorLabel";
			this.authorLabel.Size = new System.Drawing.Size(95, 13);
			this.authorLabel.TabIndex = 2;
			this.authorLabel.Text = "Technoguyfication";
			// 
			// versionLabel
			// 
			this.versionLabel.AutoSize = true;
			this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.versionLabel.Location = new System.Drawing.Point(54, 132);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size(100, 13);
			this.versionLabel.TabIndex = 2;
			this.versionLabel.Text = "Version {0} (OS {1})\r\n";
			// 
			// copyrightLabel
			// 
			this.copyrightLabel.AutoSize = true;
			this.copyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.copyrightLabel.Location = new System.Drawing.Point(54, 150);
			this.copyrightLabel.Name = "copyrightLabel";
			this.copyrightLabel.Size = new System.Drawing.Size(190, 13);
			this.copyrightLabel.TabIndex = 2;
			this.copyrightLabel.Text = "© 2017 Technoguyfication. Unlicense.";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(54, 168);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(349, 91);
			this.label1.TabIndex = 2;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(357, 332);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.OKButtonClick);
			// 
			// logoBox
			// 
			this.logoBox.Image = global::Notpad.Client.Properties.Resources.notepad33;
			this.logoBox.Location = new System.Drawing.Point(12, 113);
			this.logoBox.Name = "logoBox";
			this.logoBox.Size = new System.Drawing.Size(32, 33);
			this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.logoBox.TabIndex = 4;
			this.logoBox.TabStop = false;
			// 
			// About
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(444, 367);
			this.Controls.Add(this.logoBox);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.copyrightLabel);
			this.Controls.Add(this.versionLabel);
			this.Controls.Add(this.authorLabel);
			this.Controls.Add(this.horizontalDivider);
			this.Controls.Add(this.notpadLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "About";
			this.Load += new System.EventHandler(this.WindowLoaded);
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label notpadLabel;
		private System.Windows.Forms.Label horizontalDivider;
		private System.Windows.Forms.Label authorLabel;
		private System.Windows.Forms.Label versionLabel;
		private System.Windows.Forms.Label copyrightLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.PictureBox logoBox;
	}
}