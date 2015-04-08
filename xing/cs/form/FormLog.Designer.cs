namespace xing.cs.form
{
	partial class FormLog
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			this.GridLog = new System.Windows.Forms.DataGridView();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ButtonOpenLogFile = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.GridLog)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// GridLog
			// 
			this.GridLog.AllowUserToAddRows = false;
			this.GridLog.AllowUserToDeleteRows = false;
			this.GridLog.AllowUserToResizeColumns = false;
			this.GridLog.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.NullValue = null;
			this.GridLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.GridLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			this.GridLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.GridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.NullValue = null;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.GridLog.DefaultCellStyle = dataGridViewCellStyle3;
			this.GridLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GridLog.Location = new System.Drawing.Point(0, 0);
			this.GridLog.Margin = new System.Windows.Forms.Padding(0);
			this.GridLog.MultiSelect = false;
			this.GridLog.Name = "GridLog";
			this.GridLog.ReadOnly = true;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			this.GridLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			dataGridViewCellStyle5.NullValue = null;
			this.GridLog.RowsDefaultCellStyle = dataGridViewCellStyle5;
			this.GridLog.RowTemplate.Height = 23;
			this.GridLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.GridLog.Size = new System.Drawing.Size(343, 200);
			this.GridLog.TabIndex = 33;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.GridLog, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.ButtonOpenLogFile, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(343, 227);
			this.tableLayoutPanel1.TabIndex = 34;
			// 
			// ButtonOpenLogFile
			// 
			this.ButtonOpenLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOpenLogFile.Location = new System.Drawing.Point(279, 203);
			this.ButtonOpenLogFile.Name = "ButtonOpenLogFile";
			this.ButtonOpenLogFile.Size = new System.Drawing.Size(61, 21);
			this.ButtonOpenLogFile.TabIndex = 34;
			this.ButtonOpenLogFile.Text = "파일보기";
			this.ButtonOpenLogFile.UseVisualStyleBackColor = true;
			this.ButtonOpenLogFile.Click += new System.EventHandler(this.ButtonOpenLogFile_Click);
			// 
			// FormLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(343, 227);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "FormLog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "로그";
			((System.ComponentModel.ISupportInitialize)(this.GridLog)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.DataGridView GridLog;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button ButtonOpenLogFile;
	}
}