namespace xing
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.CheckShowFormTrading = new System.Windows.Forms.CheckBox();
			this.CheckShowFormSetting = new System.Windows.Forms.CheckBox();
			this.CheckShowFormLogin = new System.Windows.Forms.CheckBox();
			this.CheckShowFormLog = new System.Windows.Forms.CheckBox();
			this.TimerLogin = new System.Windows.Forms.Timer(this.components);
			this.Timer0167 = new System.Windows.Forms.Timer(this.components);
			this.TimerBuyRun = new System.Windows.Forms.Timer(this.components);
            this.TimerRealSh = new System.Windows.Forms.Timer(this.components);
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Open = new System.Windows.Forms.ToolStripMenuItem();
			this.Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.ButtonTrayTo = new System.Windows.Forms.Button();
			this.ButtonProgramExit = new System.Windows.Forms.Button();
			this.ButtonShowJson0424 = new System.Windows.Forms.Button();
			this.ButtonInit0424 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.contextMenuStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// CheckShowFormTrading
			// 
			this.CheckShowFormTrading.AutoSize = true;
			this.CheckShowFormTrading.Location = new System.Drawing.Point(3, 51);
			this.CheckShowFormTrading.Name = "CheckShowFormTrading";
			this.CheckShowFormTrading.Size = new System.Drawing.Size(67, 16);
			this.CheckShowFormTrading.TabIndex = 154;
			this.CheckShowFormTrading.Text = "Trading";
			this.CheckShowFormTrading.UseVisualStyleBackColor = true;
			this.CheckShowFormTrading.CheckedChanged += new System.EventHandler(this.CheckShowFormTrading_CheckedChanged);
			// 
			// CheckShowFormSetting
			// 
			this.CheckShowFormSetting.AutoSize = true;
			this.CheckShowFormSetting.Location = new System.Drawing.Point(3, 27);
			this.CheckShowFormSetting.Name = "CheckShowFormSetting";
			this.CheckShowFormSetting.Size = new System.Drawing.Size(72, 16);
			this.CheckShowFormSetting.TabIndex = 153;
			this.CheckShowFormSetting.Text = "환경설정";
			this.CheckShowFormSetting.UseVisualStyleBackColor = true;
			this.CheckShowFormSetting.CheckedChanged += new System.EventHandler(this.CheckShowFormSetting_CheckedChanged);
			// 
			// CheckShowFormLogin
			// 
			this.CheckShowFormLogin.AutoSize = true;
			this.CheckShowFormLogin.Location = new System.Drawing.Point(3, 3);
			this.CheckShowFormLogin.Name = "CheckShowFormLogin";
			this.CheckShowFormLogin.Size = new System.Drawing.Size(60, 16);
			this.CheckShowFormLogin.TabIndex = 152;
			this.CheckShowFormLogin.Text = "로그인";
			this.CheckShowFormLogin.UseVisualStyleBackColor = true;
			this.CheckShowFormLogin.CheckedChanged += new System.EventHandler(this.CheckShowFormLogin_CheckedChanged);
			// 
			// CheckShowFormLog
			// 
			this.CheckShowFormLog.AutoSize = true;
			this.CheckShowFormLog.Location = new System.Drawing.Point(3, 75);
			this.CheckShowFormLog.Name = "CheckShowFormLog";
			this.CheckShowFormLog.Size = new System.Drawing.Size(48, 16);
			this.CheckShowFormLog.TabIndex = 150;
			this.CheckShowFormLog.Text = "로그";
			this.CheckShowFormLog.UseVisualStyleBackColor = true;
			this.CheckShowFormLog.CheckedChanged += new System.EventHandler(this.CheckShowFormLog_CheckedChanged);
			// 
			// TimerLogin
			// 
			this.TimerLogin.Interval = 5000;
			this.TimerLogin.Tick += new System.EventHandler(this.TimerLogin_Tick);
			// 
			// Timer0167
			// 
			this.Timer0167.Interval = 500;
			this.Timer0167.Tick += new System.EventHandler(this.Timer0167_Tick);
			// 
			// TimerBuyRun
			// 
			this.TimerBuyRun.Interval = 1100;
			this.TimerBuyRun.Tick += new System.EventHandler(this.TimerBuyRun_Tick);
			// 
            // TimerRealSh
            // 
            this.TimerRealSh.Interval = 1100;
            this.TimerRealSh.Tick += new System.EventHandler(this.TimerRealSh_Tick);
            // 
			// ToolTip
			// 
			this.ToolTip.AutoPopDelay = 20000;
			this.ToolTip.InitialDelay = 500;
			this.ToolTip.IsBalloon = true;
			this.ToolTip.ReshowDelay = 100;
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.BalloonTipTitle = "xing";
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "xing";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open,
            this.Exit});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
			this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
			// 
			// Open
			// 
			this.Open.Name = "Open";
			this.Open.Size = new System.Drawing.Size(100, 22);
			this.Open.Text = "열기";
			// 
			// Exit
			// 
			this.Exit.Name = "Exit";
			this.Exit.Size = new System.Drawing.Size(100, 22);
			this.Exit.Text = "종료";
			// 
			// ButtonTrayTo
			// 
			this.ButtonTrayTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonTrayTo.Location = new System.Drawing.Point(5, 168);
			this.ButtonTrayTo.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
			this.ButtonTrayTo.Name = "ButtonTrayTo";
			this.ButtonTrayTo.Size = new System.Drawing.Size(69, 20);
			this.ButtonTrayTo.TabIndex = 156;
			this.ButtonTrayTo.Text = "트레이로..";
			this.ButtonTrayTo.UseVisualStyleBackColor = true;
			this.ButtonTrayTo.Click += new System.EventHandler(this.ButtonTrayTo_Click);
			// 
			// ButtonProgramExit
			// 
			this.ButtonProgramExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonProgramExit.Location = new System.Drawing.Point(76, 168);
			this.ButtonProgramExit.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
			this.ButtonProgramExit.Name = "ButtonProgramExit";
			this.ButtonProgramExit.Size = new System.Drawing.Size(47, 20);
			this.ButtonProgramExit.TabIndex = 157;
			this.ButtonProgramExit.Text = "종료";
			this.ButtonProgramExit.UseVisualStyleBackColor = true;
			this.ButtonProgramExit.Click += new System.EventHandler(this.ButtonProgramExit_Click);
			// 
			// ButtonShowJson0424
			// 
			this.ButtonShowJson0424.Location = new System.Drawing.Point(6, 143);
			this.ButtonShowJson0424.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
			this.ButtonShowJson0424.Name = "ButtonShowJson0424";
			this.ButtonShowJson0424.Size = new System.Drawing.Size(78, 20);
			this.ButtonShowJson0424.TabIndex = 158;
			this.ButtonShowJson0424.Text = "0424 확인";
			this.ButtonShowJson0424.UseVisualStyleBackColor = true;
			this.ButtonShowJson0424.Click += new System.EventHandler(this.ButtonShowJson0424_Click);
			// 
			// ButtonInit0424
			// 
			this.ButtonInit0424.Location = new System.Drawing.Point(6, 119);
			this.ButtonInit0424.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
			this.ButtonInit0424.Name = "ButtonInit0424";
			this.ButtonInit0424.Size = new System.Drawing.Size(78, 20);
			this.ButtonInit0424.TabIndex = 159;
			this.ButtonInit0424.Text = "0424 초기화";
			this.ButtonInit0424.UseVisualStyleBackColor = true;
			this.ButtonInit0424.Click += new System.EventHandler(this.ButtonInit0424_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tableLayoutPanel1);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(121, 117);
			this.groupBox1.TabIndex = 160;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "서브창 컬트롤";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.CheckShowFormLogin, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.CheckShowFormSetting, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.CheckShowFormTrading, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.CheckShowFormLog, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(115, 97);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(124, 189);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.ButtonInit0424);
			this.Controls.Add(this.ButtonShowJson0424);
			this.Controls.Add(this.ButtonProgramExit);
			this.Controls.Add(this.ButtonTrayTo);
			this.MaximizeBox = false;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "xing";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
			this.contextMenuStrip1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		public System.Windows.Forms.Timer TimerLogin;
		public System.Windows.Forms.Timer Timer0167;
		public System.Windows.Forms.Timer TimerBuyRun;
        public System.Windows.Forms.Timer TimerRealSh;
		public System.Windows.Forms.ToolTip ToolTip;
		public System.Windows.Forms.NotifyIcon notifyIcon1;
		public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		public System.Windows.Forms.ToolStripMenuItem Exit;
		private System.Windows.Forms.ToolStripMenuItem Open;
		public System.Windows.Forms.CheckBox CheckShowFormLog;
		public System.Windows.Forms.CheckBox CheckShowFormTrading;
		public System.Windows.Forms.CheckBox CheckShowFormSetting;
		public System.Windows.Forms.CheckBox CheckShowFormLogin;
		public System.Windows.Forms.Button ButtonTrayTo;
		public System.Windows.Forms.Button ButtonProgramExit;
		public System.Windows.Forms.Button ButtonShowJson0424;
		public System.Windows.Forms.Button ButtonInit0424;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

