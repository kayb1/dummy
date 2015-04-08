using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace xing.cs.form
{
	public partial class FormLog : Form
	{
		/// <summary>메인 폼 참조시 사용</summary>
		public FormMain mFormMain;

		public FormLog()
		{
			InitializeComponent();

			// 로그 그리드 세팅
			LogGrid.SetGridDebug(GridLog);

			#region 폼 위치/사이즈 복원

			if (Properties.Settings.Default.FORM_LOG_LEFT >= 0)
			{
				this.Left = Properties.Settings.Default.FORM_LOG_LEFT;
			}
			else
			{
				this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
			}

			if (Properties.Settings.Default.FORM_LOG_TOP >= 0)
			{
				this.Top = Properties.Settings.Default.FORM_LOG_TOP;
			}
			else
			{
				this.Top = Screen.PrimaryScreen.Bounds.Height - 300 - 29 - this.Height;
			}

			if (Properties.Settings.Default.FORM_LOG_WIDTH >= 0 && Properties.Settings.Default.FORM_LOG_HEIGHT >= 0)
			{
				this.Size = new System.Drawing.Size(
						Properties.Settings.Default.FORM_LOG_WIDTH,
						Properties.Settings.Default.FORM_LOG_HEIGHT
					);
			}

			#endregion


		}

		/// <summary>
		/// 로그 내용을 파일로 보기
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonOpenLogFile_Click(object sender, EventArgs e)
		{
			try
			{
				bool flag = false;

				// 현재 실행중인 wintail 프로그램 목록 가져옴
				System.Diagnostics.Process[] aProcess = System.Diagnostics.Process.GetProcessesByName("WinTail");

				foreach (System.Diagnostics.Process process in aProcess)
				{
					// xing 폴더 안의 내용을 보고 있는 녀석
					if (process.MainWindowTitle.IndexOf(setting.program_execute_dir) >= 0)
					{
						// 현재 실행중인 로그 파일을 가진 녀석
						if (process.MainWindowTitle.IndexOf(Log.mLogFile) >= 0)
						{
							flag = true;
						}
						// 이전 로그를 보기 위한 프로그램은 종료 처리
						else
						{
							Log.WriteLine("과거 로그 파일을 가진 wintail 종료 :: " + process.MainWindowTitle);

							process.Kill();
						}
					}
				}

				// wintail 프로그램으로 로그 확인
				if (flag == false)
				{
					string execute_file = setting.program_execute_dir + @"\utils\wintail\WinTail.exe";
					System.Diagnostics.Process.Start(execute_file, Log.mLogFile);
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		
		

		
	}
}
