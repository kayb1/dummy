﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xing.cs.form
{
    public partial class FormOCR : Form
    {
        /// <summary>메인 폼 참조</summary>
        public FormMain mFormMain;

        private static readonly int NUM_OF_FILE = 2;
        private static readonly string SPLIT = "/";
        private static readonly float IMAGE_SCALE_FACTOR = 2f;        
        private static readonly float DEFAULT_DPI = 4800f;

        private FormCaptureBox mFrmCaptureBox;
        private int mNumOfFile;
        private Boolean mIsProcessing;
        private MODI.MiLANGUAGES mLangType;
        private string mOcrCharList;

        public FormOCR()
        {
            InitializeComponent();

            mFrmCaptureBox = new FormCaptureBox();
            mFrmCaptureBox.Show();
            mNumOfFile = 0;
            mIsProcessing = false;
            mLangType = MODI.MiLANGUAGES.miLANG_ENGLISH;
            mOcrCharList = "0123456789";
            //mOcrCharList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,$-/#&=()\"':?";

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
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CopyFromScreen(mFrmCaptureBox.PointToScreen(new Point(mFrmCaptureBox.PnCaptureBox.Location.X, mFrmCaptureBox.PnCaptureBox.Location.Y)), new Point(0, 0), mFrmCaptureBox.PnCaptureBox.Size);

            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            pbCurImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbCurImage.Image = (Image)new Bitmap(bitmap);

            bitmap.Save("c:\\test" + mNumOfFile + ".png", System.Drawing.Imaging.ImageFormat.Png);
            bitmap.Dispose();
            #endregion

            string strText = "";
            //string strText2 = "";

            #region tessnet OCR
            // Code usage sample           
            using (Bitmap bmpOriginal = new Bitmap("c:\\test" + mNumOfFile + ".png", true)) {                
                bmpOriginal.SetResolution(300f, 300f);                
                using (Bitmap bmpFinal = new Bitmap(bmpOriginal, (int)(bmpOriginal.Width * IMAGE_SCALE_FACTOR), (int)(bmpOriginal.Height * IMAGE_SCALE_FACTOR)))
                {                    
                    bmpFinal.SetResolution(DEFAULT_DPI, DEFAULT_DPI);               
                    tessnet2.Tesseract tessocr = new tessnet2.Tesseract();
                    tessocr.SetVariable("tessedit_char_whitelist", mOcrCharList);
                    tessocr.Init(@"C:\Users\WMPRO1402\Documents\Visual Studio 2013\Projects\dummy\xing\bin\x86Release\tessdata", "eng", false);
                    List<tessnet2.Word> words = tessocr.DoOCR(bmpFinal, Rectangle.Empty);
                    foreach (tessnet2.Word word in words)
                    {
                        strText += word.Text;
                    }
                }                
            }

            //using (Bitmap bmpOriginal = new Bitmap("c:\\test" + mNumOfFile + ".png", true))
            //{
            //    bmpOriginal.SetResolution(300f, 300f);
            //    using (Bitmap bmpFinal = new Bitmap(bmpOriginal, bmpOriginal.Width * 3, bmpOriginal.Height * 3))
            //    {
            //        bmpFinal.SetResolution(DEFAULT_DPI, DEFAULT_DPI);
            //        tessnet2.Tesseract tessocr = new tessnet2.Tesseract();
            //        tessocr.SetVariable("tessedit_char_whitelist", mOcrCharList);
            //        tessocr.Init(@"C:\Users\WMPRO1402\Documents\Visual Studio 2013\Projects\dummy\xing\bin\x86Release\tessdata", "eng", false);
            //        List<tessnet2.Word> words = tessocr.DoOCR(bmpFinal, Rectangle.Empty);
            //        foreach (tessnet2.Word word in words)
            //        {
            //            strText2 += word.Text;
            //        }
            //    }
            //}
            #endregion            

            //if (strText.Equals(strText2))
            //{
            //    HHLog("같음");
            //}
            //else
            //{
            //    HHLog("다름:" + strText2);
            //}

            #region MODI OCR
            //MODI.Document md = new MODI.Document();            
            //try
            //{
            //    md.Create("c:\\test" + mNumOfFile + ".jpg");
            //    md.OCR(mLangType, false, true);

            //    MODI.Image mdImage = (MODI.Image)md.Images[0];
            //    MODI.Layout mdLayout = mdImage.Layout;
            //    //단어별로 구분할때는 아래코드 사용
            //    /*for (int i = 0; i < mdLayout.Words.Count; i++)
            //    {
            //        MODI.Word mdWord = (MODI.Word)mdLayout.Words[i];
            //        if (strText.Length > 0)
            //        {
            //            strText += " ";
            //        }
            //        strText += mdWord.Text;
            //    }*/
            //    strText = mdLayout.Text;
            //    HHLog("추출결과 : " + strText);
            //}
            //catch (Exception exception)
            //{
            //    HHLog("문자추출실패 : " + exception.Message);
            //    tbxLog.Invalidate();
            //    md.Close(false);
            //}
            //finally
            //{
            //    md.Close(false);
            //}
            #endregion

            #region 텍스트 보정
            if (strText.Length > 0)
            {
                //(MODI 일 경우에만 사용)
                //strText = strText.Replace("o", "0");
                //strText = strText.Replace("O", "0");

                //strText = strText.Replace("l", "1");
                //strText = strText.Replace("i", "1");
                //strText = strText.Replace("I", "1");

                //strText = strText.Replace("s", "5");
                //strText = strText.Replace("S", "5");

                //숫자가 아니면 제거하자 
                //strText = removeExceptNumberAndNewLine(strText);
                //strText = strText.Replace("\n", SPLIT);

                HHLog("변환결과 : " + strText);

                //string[] arr8407 = strText.Split('/');
                //strText = strText.Replace("/", "");
                //int count = arr8407.Length;
                int count = strText.Length / 6;
                if (count > 0)
                {
                    count -= 1;
                }

                if (count > 0)
                {
                    Log.WriteLine("ocr " + count + " " + strText);
                    setting.mxTr8407_kiwum.call_request(count.ToString(), strText);
                }
            }
            #endregion

            mNumOfFile++;
            if (mNumOfFile >= NUM_OF_FILE)
            {
                mNumOfFile = 0;
            }

            mIsProcessing = false;
        }

        private void btnOcrStart_Click(object sender, EventArgs e)
        {
            if (!timer.Enabled)
            {
                timer.Enabled = true;

                //mFrmCaptureBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                btnOcrStart.Enabled = false;
                gbxTimerSetting.Enabled = false;
                btnOcrEnd.Enabled = true;
            }
        }

        private void btnOcrEnd_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;

                //mFrmCaptureBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                btnOcrStart.Enabled = true;
                gbxTimerSetting.Enabled = true;
                btnOcrEnd.Enabled = false;
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
