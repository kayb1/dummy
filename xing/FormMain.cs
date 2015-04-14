using System;
using System.IO;
using System.Windows.Forms;
using System.Net.Json;
using System.Data;
using System.Collections;

using xing.cs.form;

namespace xing
{
	/// <summary>
	/// 메인 폼
	/// </summary>
    public partial class FormMain : Form {

		/// <summary>로그인 시도 횟수</summary>
		public int mLoginCount = 0;	

		/// <summary>로그 폼</summary>
		public FormLog mfLog;

		/// <summary>로그인 폼</summary>
		public FormLogin mfLogin;

		/// <summary>환경설정 폼</summary>
		public FormSetting mfSetting;

		/// <summary>Trading 폼</summary>
		public FormTrading mfTrading;

        /// <summary>ocr 폼</summary>
        public FormOcrCaptureBox mfOcrCaptureBox;

        /// <summary>캡쳐 폼</summary>
        public FormOcrMain mfOcrMain;

		/// <summary>
		/// 메인 폼 생성자
		/// </summary>
        public FormMain() {
            InitializeComponent();

			#region 폼 위치/사이즈 복원

			if (Properties.Settings.Default.FORM_MAIN_LEFT >= 0)
			{
				this.Left = Properties.Settings.Default.FORM_MAIN_LEFT;
			}
			else
			{
				this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
			}


			if (Properties.Settings.Default.FORM_MAIN_TOP >= 0)
			{
				this.Top = Properties.Settings.Default.FORM_MAIN_TOP;
			}
			else
			{
				this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height - 29;
			}
			

			#endregion
        }	


		/// <summary>
		/// 메인 폼이 로딩될 때.. 이벤트 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormMain_Load(object sender, EventArgs e)
		{
			try
			{
				#region 로그 출력을 위한 윈폼이 제일 먼저 실행됨

				// 로그 폼 - 제일 먼저 나와야 함
				mfLog = new FormLog();
				mfLog.Owner = this;
				mfLog.mFormMain = this;
				mfLog.Show();
				CheckShowFormLog.Checked = true;

				#endregion

				#region 현재 프로그램 정보 확인 및 세팅

				// 프로그램 경로
				setting.program_execute_dir = Util.GetCurrentDirectoryWithPath();

				setting.program_full_name = Environment.GetCommandLineArgs()[0];

				// 프로그램 실행시 받은 파라미터 확인
				if (Environment.GetCommandLineArgs().Length == 1)
				{
					Log.WriteLine("##### 프로그램 시작 #####");
				}
				else
				{
					Log.WriteLine("##### 프로그램 재시작 #####");
				}

				#endregion

				#region 서브 윈도우 폼 로딩

				// 환경설정 폼
				mfSetting = new FormSetting();
				mfSetting.Owner = this;
				mfSetting.Show();
				CheckShowFormSetting.Checked = true;

				// Trading 폼
				mfTrading = new FormTrading();
				mfTrading.Owner = this;
				mfTrading.mfMain = this;
				mfTrading.Show();
				CheckShowFormTrading.Checked = true;

				// 로그인 폼
				mfLogin = new FormLogin();
				mfLogin.Owner = this;
				mfLogin.mFormMain = this;
				mfLogin.Show();
				CheckShowFormLogin.Checked = true;

                // OCR 캡쳐 폼
                mfOcrCaptureBox = new FormOcrCaptureBox();
                mfOcrCaptureBox.Owner = this;
                mfOcrCaptureBox.mFormMain = this;
                mfOcrCaptureBox.Show();                

                // OCR 메인 폼
                mfOcrMain = new FormOcrMain();
                mfOcrMain.Owner = this;
                mfOcrMain.mFormMain = this;
                mfOcrMain.Show();

				#endregion

				
				// xing component 로드
				fnLoadXing();

				// 자동 로그인 설정이 되어 있으면...
				if (setting.login_auto_yn)
				{
					Log.WriteLine("자동 로그인 시작");
					TimerLogin.Start();
				}

			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}	// end function


		/// <summary>폼이 닫힐때 이벤트 처리</summary>
		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			fnExitProgram();
		}	// end function

		/// <summary>트레이 아이콘 더블클릭</summary>
		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			fnTrayFrom();
		}	// end function


		/// <summary>
		/// 트레이에서 해당 메뉴 클릭시 이벤트 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Name == "Open")
			{
				fnTrayFrom();
			}
			else if (e.ClickedItem.Name == "Exit")
			{
				fnExitProgram();
			}
		}

		/// <summary>
		/// 트레이 아이콘에서 화면으로 복귀
		/// </summary>
		private void fnTrayFrom()
		{
			this.notifyIcon1.Visible = false;				// 트레이 아이콘 비활성
			this.Show();									// 폼을 열고			

			#region 서브창들 보이기

			if (CheckShowFormLog.Checked)
			{
				mfLog.Show();
			}

			if (CheckShowFormLogin.Checked)
			{
				mfLogin.Show();
			}
			
			if (CheckShowFormSetting.Checked)
			{
				mfSetting.Show();
			}
			
			if (CheckShowFormTrading.Checked)
			{
				mfTrading.Show();
			}

			#endregion
		}

		/// <summary>
		/// 트레이 아이콘으로 이동시키기 
		/// </summary>
		public void fnTrayTo()
		{
			this.notifyIcon1.Visible = true;
			this.Hide();
			notifyIcon1.Text = setting.program_full_name;

			#region 서브창들 숨기기

			mfLog.Hide();
			mfLogin.Hide();
			mfSetting.Hide();
			mfTrading.Hide();

			#endregion

		}	// end function



		/// <summary>
		/// 시간조회 타이머 구동시 이벤트 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Timer0167_Tick(object sender, EventArgs e)
		{
			// 서버의 시간을 검색 호출
			setting.mxTr0167.call_request();
		}


		/// <summary>
		/// 자동 로그인 설정이 호출되는 타이머 이벤트
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerLogin_Tick(object sender, EventArgs e)
		{
			try
			{
				mLoginCount++;

				// 로그인 시도 횟수가 10번이 넘어가면 프로그램 재시작
				if (mLoginCount > 10)
				{
					fnRestartProgram();
				}
				// 로그인 시도
				else
				{
					setting.mxSession.fnLogin();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}	// end function


		/// <summary>
		/// 자동거래 타이머 구동 이벤트 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerBuyRun_Tick(object sender, EventArgs e)
		{
			try
			{
				// 잔고조회 및 매도 주문
				setting.mxTr0424.call_request();

				// 미체결 항목 - 정정/취소 주문
				setting.mxTr0425.call_request();

				// 증거금별 매수가능 금액 조회
				setting.mxTrCSPAQ02200.call_request();

			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}	// end function

        /// <summary>
        /// 자동거래 실시간 종목 검색 이벤트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerRealSh_Tick(object sender, EventArgs e)
        {
            try
            {
                // 종목 검색 주문
                setting.mxTr1833.call_request();
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
        }	// end function

		/// <summary>
		/// xing component 로드
		/// </summary>
		private void fnLoadXing()
		{
			try
			{
				// session
				setting.mxSession = new xing_session();
				setting.mxSession.mfMain = this;
				setting.mxSession.mfLogin = mfLogin;

				// 장정보
				setting.mxRealJif = new xing_real_jif();				
				setting.mxRealJif.mfTrading = mfTrading;

				// 시간
				setting.mxTr0167 = new xing_tr_0167();					
				setting.mxTr0167.mfMain = this;
				setting.mxTr0167.mfTrading = mfTrading;

				// 체결/미체결 조회
				setting.mxTr0425 = new xing_tr_0425();					
				setting.mxTr0425.mfTrading = mfTrading;

				// 주식잔고2
				setting.mxTr0424 = new xing_tr_0424();
				setting.mxTr0424.mfTrading = mfTrading;

				// 현물취소주문
				setting.mxTrCSPAT00800 = new xing_tr_CSPAT00800();
				setting.mxTrCSPAT00800.mfTrading = mfTrading;

				// 현물정정주문
				setting.mxTrCSPAT00700 = new xing_tr_CSPAT00700();		
				setting.mxTrCSPAT00700.mFormMain = this;
				setting.mxTrCSPAT00700.mFormTrading = mfTrading;

				// 현물정상주문
				setting.mxTrCSPAT00600 = new xing_tr_CSPAT00600();		


				// 현물계좌 예수금/주문가능금액/총평가 조회
				setting.mxTrCSPAQ02200 = new xing_tr_CSPAQ02200();		
				setting.mxTrCSPAQ02200.mfTrading = mfTrading;
				setting.mxTrCSPAQ02200.mfMain = this;

				// 멀티현재가 
				setting.mxTr8407 = new xing_tr_8407();					
				setting.mxTr8407.mFormTrading = mfTrading;

				// 종목검색
				setting.mxTr1833 = new xing_tr_1833();		            
				setting.mxTr1833.mfTrading = mfTrading;
				setting.mxTr1833.mfSetting = mfSetting;

                // 주식차트(일주월)
                setting.mxTr8413 = new xing_tr_8413();

				// 주식종목검색
				setting.mxTr8430 = new xing_tr_8430();		            
				setting.mxTr8430.mfMain = this;

				// 현물계좌 증거금별 주문가능 수량 조회
				setting.mxTrCSPBQ00200 = new xing_tr_CSPBQ00200();
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);

				MessageBox.Show(ex.Message);

				fnExitProgram();
			}
		}	// end function


		

		/// <summary>
		/// 현재 프로그램 종료
		/// </summary>
		public void fnExitProgram()
		{
			#region 설정파일에 필요한 정보 저장

			// 메인 윈폼 위치 저장
			Properties.Settings.Default.FORM_MAIN_LEFT = this.Left;
			Properties.Settings.Default.FORM_MAIN_TOP = this.Top;

			// 환경설정 윈폼 위치/사이즈 저장
			Properties.Settings.Default.FORM_SETTING_LEFT = mfSetting.Left;
			Properties.Settings.Default.FORM_SETTING_TOP = mfSetting.Top;

			// 로그 윈폼 위치/사이즈 저장
			Properties.Settings.Default.FORM_LOG_LEFT = mfLog.Left;
			Properties.Settings.Default.FORM_LOG_TOP = mfLog.Top;

			Properties.Settings.Default.FORM_LOG_WIDTH = mfLog.Size.Width;
			Properties.Settings.Default.FORM_LOG_HEIGHT = mfLog.Size.Height;

			// Trading 윈폼 위치/사이즈 저장
			Properties.Settings.Default.FORM_TRADING_LEFT = mfTrading.Left;
			Properties.Settings.Default.FORM_TRADING_TOP = mfTrading.Top;

			Properties.Settings.Default.FORM_TRADING_WIDTH = mfTrading.Size.Width;
			Properties.Settings.Default.FORM_TRADING_HEIGHT = mfTrading.Size.Height;

			// 잔고정보 유지할 수 있도록 값을 임시로 저장해 둠
			Properties.Settings.Default.T0424_JSON = setting.mxTr0424.mJson.ToString();

            // 당일 같은 종목 재매수 세팅시 같은 종목을 무시하기 위해 값을 임시로 저장해 둠
            Properties.Settings.Default.REBUY_JSON = setting.mxTr0424.mRebuyJson.ToString();

			Properties.Settings.Default.Save();

			#endregion

			Log.WriteLine("##### 프로그램 종료 #####");

			// Log 파일 스트림 종료
			Log.Close();

			this.Dispose();
			Application.Exit();
		}	// end function


		/// <summary>
		/// 현재 프로그램 재시작
		/// </summary>
		public void fnRestartProgram()
		{
			#region 설정파일에 필요한 정보 저장

			// 메인 윈폼 위치 저장
			Properties.Settings.Default.FORM_MAIN_LEFT = this.Left;
			Properties.Settings.Default.FORM_MAIN_TOP = this.Top;

			// 환경설정 윈폼 위치/사이즈 저장
			Properties.Settings.Default.FORM_SETTING_LEFT = mfSetting.Left;
			Properties.Settings.Default.FORM_SETTING_TOP = mfSetting.Top;

			// 로그 윈폼 위치/사이즈 저장
			Properties.Settings.Default.FORM_LOG_LEFT = mfLog.Left;
			Properties.Settings.Default.FORM_LOG_TOP = mfLog.Top;

			Properties.Settings.Default.FORM_LOG_WIDTH = mfLog.Size.Width;
			Properties.Settings.Default.FORM_LOG_HEIGHT = mfLog.Size.Height;

			// Trading 윈폼 위치/사이즈 저장
			Properties.Settings.Default.FORM_TRADING_LEFT = mfTrading.Left;
			Properties.Settings.Default.FORM_TRADING_TOP = mfTrading.Top;

			Properties.Settings.Default.FORM_TRADING_WIDTH = mfTrading.Size.Width;
			Properties.Settings.Default.FORM_TRADING_HEIGHT = mfTrading.Size.Height;

			// 잔고정보 유지할 수 있도록 값을 임시로 저장해 둠
			Properties.Settings.Default.T0424_JSON = setting.mxTr0424.mJson.ToString();

            // 당일 같은 종목 재매수 세팅시 같은 종목을 무시하기 위해 값을 임시로 저장해 둠
            Properties.Settings.Default.REBUY_JSON = setting.mxTr0424.mRebuyJson.ToString();

			Properties.Settings.Default.Save();

			#endregion

			Log.WriteLine("##### 프로그램 재시작 #####");

			// Log 파일 스트림 종료
			Log.Close();

			// 프로그램 다시 띄움
			System.Diagnostics.Process.Start(setting.program_full_name, "restart");

			this.Dispose();
			Application.Exit();
		}


		

		/// <summary>
		/// 로그 창 보기 체크박스
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckShowFormLog_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (CheckShowFormLog.Checked)
				{
					mfLog.Show();
				}
				else
				{
					mfLog.Hide();
				}

			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 로그인 창 보기 체크박스
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckShowFormLogin_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (CheckShowFormLogin.Checked)
				{
					mfLogin.Show();
				}
				else
				{
					mfLogin.Hide();
				}

			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// Trading 창 보기 체크박스
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckShowFormTrading_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (CheckShowFormTrading.Checked)
				{
					mfTrading.Show();
				}
				else
				{
					mfTrading.Hide();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 설정 창 보기 체크박스
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckShowFormSetting_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (CheckShowFormSetting.Checked)
				{
					mfSetting.Show();
				}
				else
				{
					mfSetting.Hide();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}


		

		/// <summary>트레이로 이동 버튼 클릭</summary>
		private void ButtonTrayTo_Click(object sender, EventArgs e)
		{
			fnTrayTo();
		}

		/// <summary>종료 버튼 클릭</summary>
		private void ButtonProgramExit_Click(object sender, EventArgs e)
		{
			fnExitProgram();
		}

		/// <summary>0424 json 내용 확인</summary>
		private void ButtonShowJson0424_Click(object sender, EventArgs e)
		{
			MessageBox.Show(setting.mxTr0424.mJson.ToString());
		}

		/// <summary>0424 json 초기화</summary>
		private void ButtonInit0424_Click(object sender, EventArgs e)
		{
			setting.mxTr0424.mJson = new JsonObjectCollection();
		}




	}	// end class
}	// end namespace
