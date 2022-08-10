
namespace Technoguyfication.Notpad
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
			this.mainStatusStripFloatRightLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainStatusStripConnectionLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainStatusStripZoomLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainStatusStripExtraLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainStatusStripExtra2Label = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainMenuStrip.SuspendLayout();
			this.mainStatusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.BackColor = System.Drawing.SystemColors.Window;
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.MaximumSize = new System.Drawing.Size(0, 20);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(6, 0, 0, 2);
			this.mainMenuStrip.Size = new System.Drawing.Size(800, 20);
			this.mainMenuStrip.TabIndex = 0;
			this.mainMenuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 18);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 18);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// formatToolStripMenuItem
			// 
			this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
			this.formatToolStripMenuItem.Size = new System.Drawing.Size(57, 18);
			this.formatToolStripMenuItem.Text = "F&ormat";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 18);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 18);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.textBox1.Location = new System.Drawing.Point(0, 22);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(800, 406);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// mainStatusStrip
			// 
			this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainStatusStripFloatRightLabel,
            this.mainStatusStripConnectionLabel,
            this.mainStatusStripZoomLabel,
            this.mainStatusStripExtraLabel,
            this.mainStatusStripExtra2Label});
			this.mainStatusStrip.Location = new System.Drawing.Point(0, 428);
			this.mainStatusStrip.Name = "mainStatusStrip";
			this.mainStatusStrip.Size = new System.Drawing.Size(800, 22);
			this.mainStatusStrip.TabIndex = 2;
			this.mainStatusStrip.Text = "statusStrip1";
			// 
			// mainStatusStripFloatRightLabel
			// 
			this.mainStatusStripFloatRightLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.mainStatusStripFloatRightLabel.Name = "mainStatusStripFloatRightLabel";
			this.mainStatusStripFloatRightLabel.Size = new System.Drawing.Size(375, 17);
			this.mainStatusStripFloatRightLabel.Spring = true;
			// 
			// mainStatusStripConnectionLabel
			// 
			this.mainStatusStripConnectionLabel.AutoSize = false;
			this.mainStatusStripConnectionLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.mainStatusStripConnectionLabel.Name = "mainStatusStripConnectionLabel";
			this.mainStatusStripConnectionLabel.Size = new System.Drawing.Size(140, 17);
			this.mainStatusStripConnectionLabel.Text = "Disconnected";
			this.mainStatusStripConnectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mainStatusStripZoomLabel
			// 
			this.mainStatusStripZoomLabel.AutoSize = false;
			this.mainStatusStripZoomLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.mainStatusStripZoomLabel.Name = "mainStatusStripZoomLabel";
			this.mainStatusStripZoomLabel.Size = new System.Drawing.Size(50, 17);
			this.mainStatusStripZoomLabel.Text = "100%";
			this.mainStatusStripZoomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mainStatusStripExtraLabel
			// 
			this.mainStatusStripExtraLabel.AutoSize = false;
			this.mainStatusStripExtraLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.mainStatusStripExtraLabel.Name = "mainStatusStripExtraLabel";
			this.mainStatusStripExtraLabel.Size = new System.Drawing.Size(120, 17);
			this.mainStatusStripExtraLabel.Text = "Windows (CRLF)";
			this.mainStatusStripExtraLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mainStatusStripExtra2Label
			// 
			this.mainStatusStripExtra2Label.AutoSize = false;
			this.mainStatusStripExtra2Label.Name = "mainStatusStripExtra2Label";
			this.mainStatusStripExtra2Label.Size = new System.Drawing.Size(100, 17);
			this.mainStatusStripExtra2Label.Text = "UTF-8 with BOM";
			this.mainStatusStripExtra2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.mainStatusStrip);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.mainMenuStrip);
			this.MainMenuStrip = this.mainMenuStrip;
			this.Name = "Form1";
			this.Text = "Notpad";
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.mainStatusStrip.ResumeLayout(false);
			this.mainStatusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.StatusStrip mainStatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel mainStatusStripFloatRightLabel;
		private System.Windows.Forms.ToolStripStatusLabel mainStatusStripConnectionLabel;
		private System.Windows.Forms.ToolStripStatusLabel mainStatusStripZoomLabel;
		private System.Windows.Forms.ToolStripStatusLabel mainStatusStripExtraLabel;
		private System.Windows.Forms.ToolStripStatusLabel mainStatusStripExtra2Label;
	}
}

