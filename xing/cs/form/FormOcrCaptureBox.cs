using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xing.cs.form
{
    public partial class frmCaptureBox : Form
    {
        public frmCaptureBox()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        public Panel PnCaptureBox
        {
            get
            {
                return pnCaptureBox;
            }
        }

        private void frmCaptureBox_LocationChanged(object sender, EventArgs e)
        {
            //Size si = SystemInformation.PrimaryMonitorSize;

            //int nWidth = si.Width;
            //int nHeight = si.Height;

            //this.Location = new Point(nWidth - this.Width, nHeight - this.Height);
        }
    }
}
