namespace xing.cs.form
{
    partial class frmCaptureBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCaptureBox));
            this.pnCaptureBox = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnCaptureBox
            // 
            resources.ApplyResources(this.pnCaptureBox, "pnCaptureBox");
            this.pnCaptureBox.BackColor = System.Drawing.Color.LightCoral;
            this.pnCaptureBox.Name = "pnCaptureBox";
            // 
            // frmCaptureBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Firebrick;
            this.Controls.Add(this.pnCaptureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmCaptureBox";
            this.TransparencyKey = System.Drawing.Color.LightCoral;
            this.LocationChanged += new System.EventHandler(this.frmCaptureBox_LocationChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnCaptureBox;


    }
}