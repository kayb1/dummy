using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel;
using System.Windows.Forms;

using XA_SESSIONLib;
using xing.cs.form;

namespace xing
{
	public class xing_session : _IXASessionEvents
	{
		/// <summary>xing component</summary>
		private IXASession mSession;

		/// <summary>메인 폼 참조</summary>
		public FormMain mfMain;

		/// <summary>로그인 폼 참조</summary>
		public FormLogin mfLogin;

		// 생성자
		public xing_session()
		{
            IConnectionPoint m_icp;
            IConnectionPointContainer m_icpc;
            int mCookie = 0;

            mSession = new XASession();
            m_icpc = (IConnectionPointContainer)mSession;
            Guid IID_SessionEvents = typeof(_IXASessionEvents).GUID;
            m_icpc.FindConnectionPoint(ref IID_SessionEvents, out m_icp);
            m_icp.Advise(this, out mCookie);
		}	// end function

		// 소멸자
		~xing_session()
		{
			mSession.Logout();
			mSession.DisconnectServer();
		}

		
		/// <summary>
		/// 로그인
		/// </summary>
		public void fnLogin()
		{
			Log.WriteLine("로그인 시도..!!");

			mfLogin.Show();

			// 서버에 접속
			if (mSession.IsConnected() == false)
			{
				mSession.ConnectServer(setting.login_server, 20001);
			}

			if (setting.login_id == "")
			{
				mfMain.TimerLogin.Stop();

				MessageBox.Show("로그인 아이디가 없습니다..!!\n설정 저장 후 다시 로그인 해 주세요..!!");
			}
			else
			{
				// 로그인
				mSession.Login(setting.login_id, setting.login_pw, setting.login_public_pw, 0, false);
			}
		}	// end function


		/// <summary>
		/// 로그인 이벤트 처리 
		/// </summary>
		/// <param name="szCode"></param>
		/// <param name="szMsg"></param>
		void _IXASessionEvents.Login(string szCode, string szMsg)
		{
            try
            {
				Log.WriteLine(szCode + " :: " + szMsg);

				// 정상적으로 로그인 되었으면...
				if (szCode == "0000")
				{
					// 자동로그인 타이머 멈춤
					mfMain.TimerLogin.Stop();

					// 로그인 버튼 비활성
					mfLogin.ButtonLogin.Enabled = false;

					// 계좌정보 가져오기 - 처음꺼 하나만 가져옴
					setting.login_account = mSession.GetAccountList(0);
					mfLogin.TextLoginAccount.Text = setting.login_account;

					// 종목 기본 정보 저장 호출
					setting.mxTr8430.call_request();

					// 트레이로 이동이 체크되어 있다면...
					if (setting.login_tray_yn)
					{
						mfMain.fnTrayTo();
					}
					// 트레이 이동이 아니라면 화면에 보이도록 변경, 트레이 아이콘은 숨김
					else
					{
						mfMain.notifyIcon1.Visible = false;

						// 로그인 폼 비활성
						mfLogin.Hide();
						mfMain.CheckShowFormLogin.Checked = false;
					}
				}
				// 5201 :: [  -13] Request ID가 부족합니다.
				else if (szCode == "5201")
				{
					mfMain.fnRestartProgram();
				}
				// 2003 :: [공인인증] Full 인증과정에서 다음과 같은 오류가 발생하였습니다.
				// 인증서가 없습니다.
				else if (szCode == "2003")
				{
					Log.WriteLine("자동 로그인 중지..!!");
					mfMain.TimerLogin.Stop();
				}
				// 2005 :: 공인인증 비밀번호가 맞지 않습니다.
				else if (szCode == "2005")
				{
					Log.WriteLine("자동 로그인 중지..!!");
					mfMain.TimerLogin.Stop();
				}
				// 8004 :: 모의투자에 등록되지 않은 ID입니다. 
				// 8004 :: 모의투자 로그인 비밀번호를 확인해 주세요. 
				// 8004 :: 모의투자 접속절차오류입니다. 재접속하시기 바랍니다. 
				else if (szCode == "8004")
				{
					Log.WriteLine("자동 로그인 중지..!!");

					if (szMsg.IndexOf("모의투자에 등록되지 않은 ID입니다") >= 0)
					{
						mfMain.TimerLogin.Stop();
					}
					else if (szMsg.IndexOf("모의투자 로그인 비밀번호를 확인해 주세요") >= 0)
					{
						mfMain.TimerLogin.Stop();
					}
					else if (szMsg.IndexOf("모의투자 접속절차오류입니다. 재접속하시기 바랍니다") >= 0)
					{
						mfMain.fnRestartProgram();
					}
					else
					{
						;
					}
				}
				else
				{
					Log.WriteLine(szCode + " :: " + szMsg);
				}
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
		}	// end function


		/// <summary>
		/// 로그아웃 이벤트 처리 
		/// </summary>
		void _IXASessionEvents.Logout()
		{
            try
            {
                Log.WriteLine("로그아웃 되었습니다..!!");
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
		}   // end function


		/// <summary>
		/// 서버와의 접속 종료시 이벤트 처리 
		/// </summary>
		void _IXASessionEvents.Disconnect()
		{
			try
			{
				// 프로그램 재시작
				//mfMain.fnRestartProgram();

				// 재 로그인 시도
				fnLogin();
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}   // end function

	}	// end class
}	// end namespace
