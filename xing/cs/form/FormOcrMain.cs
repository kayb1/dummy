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
    public partial class frmMain : Form
    {
        private static readonly int NUM_OF_FILE = 2;
        private static readonly string SPLIT = "/";
        private frmCaptureBox mFrmCaptureBox;
        private int mNumOfFile;
        private Boolean mIsProcessing;
        private MODI.MiLANGUAGES mLangType;

        public frmMain()
        {
            InitializeComponent();

            mFrmCaptureBox = new frmCaptureBox();
            mFrmCaptureBox.Show();
            mNumOfFile = 0;
            mIsProcessing = false;
            mLangType = MODI.MiLANGUAGES.miLANG_ENGLISH;

            #region 폼 초기위치 설정
            //현재는 화면 왼쪽 하단에 위치하도록
            Size si = SystemInformation.PrimaryMonitorSize;
            //this.Location = new Point(si.Width - this.Width, si.Height - this.Height);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, si.Height - this.Height);
            //this.Width = si.Width;
            #endregion
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mIsProcessing)
            {
                HHLog("이미지 프로세싱 중.. 작업 한번 건너뜀");
                return;
            }

            mIsProcessing = true;

            #region 캡쳐
            Bitmap bitmap = new Bitmap(mFrmCaptureBox.PnCaptureBox.Width, mFrmCaptureBox.PnCaptureBox.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(mFrmCaptureBox.PointToScreen(new Point(mFrmCaptureBox.PnCaptureBox.Location.X, mFrmCaptureBox.PnCaptureBox.Location.Y)), new Point(0, 0), mFrmCaptureBox.PnCaptureBox.Size);

            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            pbCurImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbCurImage.Image = (Image)new Bitmap(bitmap);

            bitmap.Save("c:\\test" + mNumOfFile + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            bitmap.Dispose();
            #endregion

            #region OCR 인식
            MODI.Document md = new MODI.Document();
            md.Create("c:\\test" + mNumOfFile + ".jpg");
            string strText = "";
            try
            {
                md.OCR(mLangType, false, true);

                MODI.Image mdImage = (MODI.Image)md.Images[0];
                MODI.Layout mdLayout = mdImage.Layout;
                //단어별로 구분할때는 아래코드 사용
                /*for (int i = 0; i < mdLayout.Words.Count; i++)
                {
                    MODI.Word mdWord = (MODI.Word)mdLayout.Words[i];
                    if (strText.Length > 0)
                    {
                        strText += " ";
                    }
                    strText += mdWord.Text;
                }*/
                strText = mdLayout.Text;
            }
            catch
            {
                HHLog("문자추출실패");
                tbxLog.Invalidate();
                md.Close(false);
            }
            finally
            {
                HHLog("추출결과 : " + strText);
                md.Close(false);
            }
            #endregion

            #region 텍스트 보정
            strText = strText.Replace("o", "0");
            strText = strText.Replace("O", "0");

            strText = strText.Replace("l", "1");
            strText = strText.Replace("i", "1");
            strText = strText.Replace("I", "1");

            strText = strText.Replace("s", "5");
            strText = strText.Replace("S", "5");

            //숫자가 아니면 제거하자
            strText = removeExceptNumberAndNewLine(strText);

            strText = strText.Replace("\n", SPLIT);
            HHLog("변환결과 : " + strText);
            #endregion

            mNumOfFile++;
            if (mNumOfFile >= NUM_OF_FILE)
            {
                mNumOfFile = 0;
            }

            mIsProcessing = false;
        }

        private void btnCaptureStart_Click(object sender, EventArgs e)
        {
            if (!timer.Enabled)
            {
                timer.Enabled = true;

                mFrmCaptureBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                btnCaptureStart.Enabled = false;
                gbxTimerSetting.Enabled = false;
                btnCaptureEnd.Enabled = true;                
            }
        }

        private void btnCaptureEnd_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;

                mFrmCaptureBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                btnCaptureStart.Enabled = true;
                gbxTimerSetting.Enabled = true;
                btnCaptureEnd.Enabled = false;
            }
        }

        private void HHLog(string log)
        {
            tbxLog.AppendText(System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss ") + log + "\n");
            tbxLog.Invalidate();
        }

        private string removeExceptNumberAndNewLine(string text)
        {
            String returnValue = "";
            char[] convertedText = text.ToCharArray();
            for (int i = 0; i < convertedText.Length; i++)
            {
                if (convertedText[i] >= 48 && convertedText[i] <= 57)
                {
                    returnValue += convertedText[i];
                }
                else if (convertedText[i] == '\n')
                {
                    returnValue += convertedText[i];
                }
            }
            return returnValue;
        }

        private void rb1second_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1second.Checked)
                timer.Interval = 1000;
        }

        private void rb2Second_CheckedChanged(object sender, EventArgs e)
        {
            if (rb2second.Checked)
                timer.Interval = 2000;
        }

        private void rb3second_CheckedChanged(object sender, EventArgs e)
        {
            if (rb3second.Checked)
                timer.Interval = 3000;
        }

        private void rb5second_CheckedChanged(object sender, EventArgs e)
        {
            if (rb5second.Checked)
                timer.Interval = 5000;
        }

        private void rb10second_CheckedChanged(object sender, EventArgs e)
        {
            if (rb10second.Checked)
                timer.Interval = 10000;
        }
    }
}
