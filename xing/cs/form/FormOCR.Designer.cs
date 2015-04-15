namespace xing.cs.form
{
    partial class FormOCR
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbxTimerSetting = new System.Windows.Forms.GroupBox();
            this.rb10second = new System.Windows.Forms.RadioButton();
            this.rb5second = new System.Windows.Forms.RadioButton();
            this.rb3second = new System.Windows.Forms.RadioButton();
            this.rb2second = new System.Windows.Forms.RadioButton();
            this.rb1second = new System.Windows.Forms.RadioButton();
            this.btnOcrStart = new System.Windows.Forms.Button();
            this.btnOcrEnd = new System.Windows.Forms.Button();
            this.pbCurImage = new System.Windows.Forms.PictureBox();
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbxTimerSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurImage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOcrEnd);
            this.groupBox1.Controls.Add(this.btnOcrStart);
            this.groupBox1.Controls.Add(this.gbxTimerSetting);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 191);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "설정";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pbCurImage);
            this.groupBox2.Location = new System.Drawing.Point(219, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 191);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "현재 인식 이미지";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tbxLog);
            this.groupBox3.Location = new System.Drawing.Point(13, 210);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1059, 240);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "로그";
            // 
            // gbxTimerSetting
            // 
            this.gbxTimerSetting.Controls.Add(this.rb10second);
            this.gbxTimerSetting.Controls.Add(this.rb5second);
            this.gbxTimerSetting.Controls.Add(this.rb3second);
            this.gbxTimerSetting.Controls.Add(this.rb2second);
            this.gbxTimerSetting.Controls.Add(this.rb1second);
            this.gbxTimerSetting.Location = new System.Drawing.Point(7, 50);
            this.gbxTimerSetting.Name = "gbxTimerSetting";
            this.gbxTimerSetting.Size = new System.Drawing.Size(187, 46);
            this.gbxTimerSetting.TabIndex = 4;
            this.gbxTimerSetting.TabStop = false;
            this.gbxTimerSetting.Text = "감시간격설정(단위:초)";
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
            // 
            // btnOcrStart
            // 
            this.btnOcrStart.Location = new System.Drawing.Point(7, 21);
            this.btnOcrStart.Name = "btnOcrStart";
            this.btnOcrStart.Size = new System.Drawing.Size(75, 23);
            this.btnOcrStart.TabIndex = 5;
            this.btnOcrStart.Text = "시작";
            this.btnOcrStart.UseVisualStyleBackColor = true;
            this.btnOcrStart.Click += new System.EventHandler(this.btnOcrStart_Click);
            // 
            // btnOcrEnd
            // 
            this.btnOcrEnd.Location = new System.Drawing.Point(119, 21);
            this.btnOcrEnd.Name = "btnOcrEnd";
            this.btnOcrEnd.Size = new System.Drawing.Size(75, 23);
            this.btnOcrEnd.TabIndex = 6;
            this.btnOcrEnd.Text = "중지";
            this.btnOcrEnd.UseVisualStyleBackColor = true;
            this.btnOcrEnd.Click += new System.EventHandler(this.btnOcrEnd_Click);
            // 
            // pbCurImage
            // 
            this.pbCurImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCurImage.Location = new System.Drawing.Point(6, 20);
            this.pbCurImage.Name = "pbCurImage";
            this.pbCurImage.Size = new System.Drawing.Size(180, 165);
            this.pbCurImage.TabIndex = 5;
            this.pbCurImage.TabStop = false;
            // 
            // tbxLog
            // 
            this.tbxLog.BackColor = System.Drawing.Color.Black;
            this.tbxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxLog.ForeColor = System.Drawing.Color.White;
            this.tbxLog.Location = new System.Drawing.Point(3, 17);
            this.tbxLog.Multiline = true;
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxLog.Size = new System.Drawing.Size(1053, 220);
            this.tbxLog.TabIndex = 0;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // FormOCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 462);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormOCR";
            this.Text = "FormOCR";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbxTimerSetting.ResumeLayout(false);
            this.gbxTimerSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOcrEnd;
        private System.Windows.Forms.Button btnOcrStart;
        private System.Windows.Forms.GroupBox gbxTimerSetting;
        private System.Windows.Forms.RadioButton rb10second;
        private System.Windows.Forms.RadioButton rb5second;
        private System.Windows.Forms.RadioButton rb3second;
        private System.Windows.Forms.RadioButton rb2second;
        private System.Windows.Forms.RadioButton rb1second;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pbCurImage;
        private System.Windows.Forms.TextBox tbxLog;
        private System.Windows.Forms.Timer timer;
    }
}