namespace xing.cs.form
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnCaptureStart = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.btnCaptureEnd = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.설정SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.폴더설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbCurImage = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxTimerSetting = new System.Windows.Forms.GroupBox();
            this.rb1second = new System.Windows.Forms.RadioButton();
            this.rb2second = new System.Windows.Forms.RadioButton();
            this.rb3second = new System.Windows.Forms.RadioButton();
            this.rb5second = new System.Windows.Forms.RadioButton();
            this.rb10second = new System.Windows.Forms.RadioButton();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbxTimerSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCaptureStart
            // 
            this.btnCaptureStart.Location = new System.Drawing.Point(6, 20);
            this.btnCaptureStart.Name = "btnCaptureStart";
            this.btnCaptureStart.Size = new System.Drawing.Size(75, 23);
            this.btnCaptureStart.TabIndex = 0;
            this.btnCaptureStart.Text = "start";
            this.btnCaptureStart.UseVisualStyleBackColor = true;
            this.btnCaptureStart.Click += new System.EventHandler(this.btnCaptureStart_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // tbxLog
            // 
            this.tbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxLog.BackColor = System.Drawing.Color.Black;
            this.tbxLog.ForeColor = System.Drawing.Color.White;
            this.tbxLog.Location = new System.Drawing.Point(218, 27);
            this.tbxLog.Multiline = true;
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxLog.Size = new System.Drawing.Size(1020, 456);
            this.tbxLog.TabIndex = 1;
            // 
            // btnCaptureEnd
            // 
            this.btnCaptureEnd.Enabled = false;
            this.btnCaptureEnd.Location = new System.Drawing.Point(6, 49);
            this.btnCaptureEnd.Name = "btnCaptureEnd";
            this.btnCaptureEnd.Size = new System.Drawing.Size(75, 23);
            this.btnCaptureEnd.TabIndex = 2;
            this.btnCaptureEnd.Text = "end";
            this.btnCaptureEnd.UseVisualStyleBackColor = true;
            this.btnCaptureEnd.Click += new System.EventHandler(this.btnCaptureEnd_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.설정SToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1250, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 설정SToolStripMenuItem
            // 
            this.설정SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.폴더설정ToolStripMenuItem});
            this.설정SToolStripMenuItem.Name = "설정SToolStripMenuItem";
            this.설정SToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.설정SToolStripMenuItem.Text = "설정(&S)";
            // 
            // 폴더설정ToolStripMenuItem
            // 
            this.폴더설정ToolStripMenuItem.Name = "폴더설정ToolStripMenuItem";
            this.폴더설정ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.폴더설정ToolStripMenuItem.Text = "폴더 설정";
            // 
            // pbCurImage
            // 
            this.pbCurImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCurImage.Location = new System.Drawing.Point(7, 15);
            this.pbCurImage.Name = "pbCurImage";
            this.pbCurImage.Size = new System.Drawing.Size(179, 179);
            this.pbCurImage.TabIndex = 4;
            this.pbCurImage.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbCurImage);
            this.groupBox1.Location = new System.Drawing.Point(12, 283);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 200);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "현재 인식 이미지";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gbxTimerSetting);
            this.groupBox2.Controls.Add(this.btnCaptureStart);
            this.groupBox2.Controls.Add(this.btnCaptureEnd);
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 250);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "제어부";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(219, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "작업로그";
            // 
            // gbxTimerSetting
            // 
            this.gbxTimerSetting.Controls.Add(this.rb10second);
            this.gbxTimerSetting.Controls.Add(this.rb5second);
            this.gbxTimerSetting.Controls.Add(this.rb3second);
            this.gbxTimerSetting.Controls.Add(this.rb2second);
            this.gbxTimerSetting.Controls.Add(this.rb1second);
            this.gbxTimerSetting.Location = new System.Drawing.Point(7, 79);
            this.gbxTimerSetting.Name = "gbxTimerSetting";
            this.gbxTimerSetting.Size = new System.Drawing.Size(187, 46);
            this.gbxTimerSetting.TabIndex = 3;
            this.gbxTimerSetting.TabStop = false;
            this.gbxTimerSetting.Text = "감시간격설정(단위:초)";
            // 
            // rb1second
            // 
            this.rb1second.AutoSize = true;
            this.rb1second.Checked = true;
            this.rb1second.Location = new System.Drawing.Point(7, 21);
            this.rb1second.Name = "rb1second";
            this.rb1second.Size = new System.Drawing.Size(29, 16);
            this.rb1second.TabIndex = 0;
            this.rb1second.TabStop = true;
            this.rb1second.Text = "1";
            this.rb1second.UseVisualStyleBackColor = true;
            this.rb1second.CheckedChanged += new System.EventHandler(this.rb1second_CheckedChanged);
            // 
            // rb2second
            // 
            this.rb2second.AutoSize = true;
            this.rb2second.Location = new System.Drawing.Point(42, 21);
            this.rb2second.Name = "rb2second";
            this.rb2second.Size = new System.Drawing.Size(29, 16);
            this.rb2second.TabIndex = 1;
            this.rb2second.Text = "2";
            this.rb2second.UseVisualStyleBackColor = true;
            this.rb2second.CheckedChanged += new System.EventHandler(this.rb2Second_CheckedChanged);
            // 
            // rb3second
            // 
            this.rb3second.AutoSize = true;
            this.rb3second.Location = new System.Drawing.Point(77, 21);
            this.rb3second.Name = "rb3second";
            this.rb3second.Size = new System.Drawing.Size(29, 16);
            this.rb3second.TabIndex = 2;
            this.rb3second.Text = "3";
            this.rb3second.UseVisualStyleBackColor = true;
            this.rb3second.CheckedChanged += new System.EventHandler(this.rb3second_CheckedChanged);
            // 
            // rb5second
            // 
            this.rb5second.AutoSize = true;
            this.rb5second.Location = new System.Drawing.Point(112, 21);
            this.rb5second.Name = "rb5second";
            this.rb5second.Size = new System.Drawing.Size(29, 16);
            this.rb5second.TabIndex = 3;
            this.rb5second.Text = "5";
            this.rb5second.UseVisualStyleBackColor = true;
            this.rb5second.CheckedChanged += new System.EventHandler(this.rb5second_CheckedChanged);
            // 
            // rb10second
            // 
            this.rb10second.AutoSize = true;
            this.rb10second.Location = new System.Drawing.Point(147, 21);
            this.rb10second.Name = "rb10second";
            this.rb10second.Size = new System.Drawing.Size(35, 16);
            this.rb10second.TabIndex = 4;
            this.rb10second.Text = "10";
            this.rb10second.UseVisualStyleBackColor = true;
            this.rb10second.CheckedChanged += new System.EventHandler(this.rb10second_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 496);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxLog);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximumSize = new System.Drawing.Size(3000, 534);
            this.MinimumSize = new System.Drawing.Size(1266, 534);
            this.Name = "frmMain";
            this.Text = "실시간감시";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.gbxTimerSetting.ResumeLayout(false);
            this.gbxTimerSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCaptureStart;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TextBox tbxLog;
        private System.Windows.Forms.Button btnCaptureEnd;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 설정SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 폴더설정ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbCurImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbxTimerSetting;
        private System.Windows.Forms.RadioButton rb10second;
        private System.Windows.Forms.RadioButton rb5second;
        private System.Windows.Forms.RadioButton rb3second;
        private System.Windows.Forms.RadioButton rb2second;
        private System.Windows.Forms.RadioButton rb1second;
    }
}

