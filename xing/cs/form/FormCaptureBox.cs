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
    public partial class FormCaptureBox : Form
    {
        public FormCaptureBox()
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
    }
}
