namespace xing.cs.form
{
    partial class FormCaptureBox
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
            this.pnCaptureBox = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnCaptureBox
            // 
            this.pnCaptureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnCaptureBox.BackColor = System.Drawing.Color.LightCoral;
            this.pnCaptureBox.Location = new System.Drawing.Point(13, 13);
            this.pnCaptureBox.Name = "pnCaptureBox";
            this.pnCaptureBox.Size = new System.Drawing.Size(259, 237);
            this.pnCaptureBox.TabIndex = 1;
            // 
            // FormCaptureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Firebrick;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.pnCaptureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormCaptureBox";
            this.Text = "캡쳐영역설정";
            this.TransparencyKey = System.Drawing.Color.LightCoral;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnCaptureBox;
    }
}