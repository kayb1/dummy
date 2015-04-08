using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel;
using System.Windows.Forms;

using System.Net.Json;

using XA_DATASETLib;
using xing.cs.form;

namespace xing
{
	public class xing_tr_0167 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		private IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mfTrading;

		/// <summary>메인 폼 참조</summary>
		public FormMain mfMain;

		/// <summary>날짜</summary>
		public double mDateCur = 0;

		/// <summary>시간</summary>
		public double mTimeCur = 0;

		/// <summary>현재 TR이 실행중인지 여부</summary>
		private bool mStateRun = false;

		/// <summary>현재 TR이 실행중일 동안 카운트 수</summary>
		private int mStateRunCount = 0;			

		/// <summary>
		/// 생성자 - 시간조회
		/// </summary>
		public xing_tr_0167()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;

			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\t0167.res";
			icpc = (IConnectionPointContainer)mTr;
			Guid IID_QueryEvents = typeof(_IXAQueryEvents).GUID;
			icpc.FindConnectionPoint(ref IID_QueryEvents, out icp);
			icp.Advise(this, out iCookie);
		}	// end function

		/// <summary>
		/// 데이터 응답 처리
		/// </summary>
		/// <param name="szTrCode">호출된 TrCode</param>
		void _IXAQueryEvents.ReceiveData(string szTrCode)
		{
            try
            {
				// 시간 값 세팅
				string szDate = mTr.GetFieldData("t0167OutBlock", "dt", 0);
				string szTimeCur = mTr.GetFieldData("t0167OutBlock", "time", 0);

				
				// 타임 값이 정상적으로 존재하면..
				if(szTimeCur.Length >= 6)
				{
					// PC 시간을 서버 시간으로 변경
					if (mTimeCur == 0)
					{
						if (setting.program_sync_time_yn)
						{
							util_system_time.set_system_time(szDate, szTimeCur);
						}
					}

					mDateCur = Convert.ToDouble(szDate);
					mTimeCur = Convert.ToDouble(szTimeCur.Substring(0, 6));

					// 우측 상단에 서버 시간 표기
					mfTrading.Text = String.Format("Trading;  {0}[ {1:##:##:##} ]", setting.mxRealJif.mlabel, mTimeCur);

					// 오후 6시에 자동매매 스탑
					if (mTimeCur == 180000)
					{
						mfTrading.fnAutoTrading(false);
					}
				}

				// 다시 실행가능하도록 초기화
				mStateRun = false;
				mStateRunCount = 0;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
		}	// end function

		/// <summary>
		/// 시간조회 메세지 처리
		/// </summary>
		/// <param name="blsSystemError">시스템 에러</param>
		/// <param name="nMessageCode">응답코드</param>
		/// <param name="szMessage">메세지 내용</param>
		void _IXAQueryEvents.ReceiveMessage(bool blsSystemError, string nMessageCode, string szMessage)
		{
            try
            {
				// 00000 :: 조회완료
				if (nMessageCode == "00000")
				{
					;
				}
				// 로그인을 해야 사용 가능합니다
				else if (nMessageCode == "   -7")
				{
					Log.WriteLine("t0167 :: " + nMessageCode + " :: " + szMessage);

					mfMain.fnRestartProgram();
				}
				// 서버 접속에 실패하였습니다
				else if (nMessageCode == "   -2")
				{
					Log.WriteLine("t0167 :: " + nMessageCode + " :: " + szMessage);

					mfMain.fnRestartProgram();
				}
				// Request ID가 부족합니다
				else if (nMessageCode == "  -13")
				{
					Log.WriteLine("t0167 :: " + nMessageCode + " :: " + szMessage);

					mfMain.fnRestartProgram();
				}
				else 
				{
					//Log.WriteLine("t0167 :: " + nMessageCode + " :: " + szMessage);
				}
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
		}	// end function

        void _IXAQueryEvents.ReceiveChartRealData(string szTrCode)
        {

        }	// end function

		/// <summary>
		/// 서버에 시간 요청 호출
		/// </summary>
		public void call_request()
		{
			// 응답 결과를 아직 실행중이라면
			if (mStateRun)
			{
				if (mStateRunCount < 3)
				{
					mStateRunCount++;
				}
				else
				{
					mStateRun = false;
					mStateRunCount = 0;
				}
			}
			// 정상적으로 응답 처리가 끝난 상태라면 다시 호출을 시도
			else
			{
				mStateRun = true;

				mTr.SetFieldData("t0167InBlock", "id", 0, "");
				mTr.Request(false);
			}
		}	// end function


		/// <summary>
		/// API -> HTS 종목코드 연동
		/// HTS 프로그램이 실행되어 있을 경우에만 동작
		/// HTS 프로그램이 실행되지 않은 상태에서는 팝업 메세지가 계속 발생 - 확인 클릭하기 귀찮음..ㅡㅡ;;
		/// HTS 프로그램 :: xingQ 마스터 기준
		/// </summary>
		public void call_api_2_hts(string shcode)
		{
			if (setting.program_api_2_hts_yn)
			{
				// HTS 프로그램이 실행중인지 검사
				System.Diagnostics.Process[] arr = System.Diagnostics.Process.GetProcessesByName("xingqsmartmain");

				if (arr.Length > 0)
				{
					mTr.RequestLinkToHTS("&STOCK_CODE", shcode, "0");
				}
			}
		}	// end function
	}	// end class
}	// end namespace
