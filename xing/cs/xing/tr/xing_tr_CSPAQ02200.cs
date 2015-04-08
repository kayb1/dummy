using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel;
using System.Windows.Forms;

using System.Net.Json;

using XA_DATASETLib;
using xing.cs.form;

namespace xing
{
	public class xing_tr_CSPAQ02200 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		public IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mfTrading;

		/// <summary>메인 폼 참조</summary>
		public FormMain mfMain;

		/// <summary>증거금 100% 주문가능 금액</summary>
		public double m100 = 0;

		/// <summary>현재 TR이 실행중인지 여부</summary>
		private bool mStateRun = false;

		/// <summary>현재 TR이 실행중일 동안 카운트 수</summary>
		private int mStateRunCount = 0;	


        /// <summary>
        /// 생성자 - 현물계좌 예수금/주문가능금액/총평가 조회
        /// </summary>
		public xing_tr_CSPAQ02200()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\CSPAQ02200.res";
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
                int iCount = mTr.GetBlockCount("CSPAQ02200OutBlock2");
                
                for (int i = 0; i < iCount; i++)
                {
					m100 = Convert.ToDouble(mTr.GetFieldData("CSPAQ02200OutBlock2", "MgnRat100pctOrdAbleAmt", i));
					mfTrading.Text100.Text = Util.GetNumberFormat(m100);
					
                }	// end for

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
        /// 메세지 처리
        /// </summary>
        /// <param name="blsSystemError">시스템 에러</param>
        /// <param name="nMessageCode">응답코드</param>
        /// <param name="szMessage">메세지 내용</param>
		void _IXAQueryEvents.ReceiveMessage(bool blsSystemError, string nMessageCode, string szMessage)
		{
            try
            {
				if (nMessageCode == "00000")
				{
					;
				}
				// 00310 :: 모의투자 조회가 완료되었습니다                      
				else if (nMessageCode == "00310")
				{
					;
				}
				// 00136 :: 조회가 완료되었습니다                      
				else if (nMessageCode == "00136")
				{
					;
				}
				// 00020 :: application program exit[TR:CSPAQ]              
				else if (nMessageCode == "00020")
				{
					Log.WriteLine("CSPAQ02200 :: " + nMessageCode + " :: " + szMessage);
					mfMain.fnRestartProgram();
				}
				// 03669 :: 비밀번호 오류입니다. (5회중 4회 남았습니다)
				else if (nMessageCode == "03669")
				{
					Log.WriteLine("CSPAQ02200 :: " + nMessageCode + " :: " + szMessage);
					mfTrading.fnAutoTrading(false);
				}
				// 01796 :: 비밀번호 연속 오류허용횟수를 초과하였습니다. 콜센터로 문의하시기 바랍니다.
				else if (nMessageCode == "01796")
				{
					Log.WriteLine("CSPAQ02200 :: " + nMessageCode + " :: " + szMessage);
					mfTrading.fnAutoTrading(false);
				}
				else
				{
					Log.WriteLine("CSPAQ02200 :: " + nMessageCode + " :: " + szMessage);
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
		/// 현물계좌 예수금/주문가능금액/총평가 조회 호출
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

				mTr.SetFieldData("CSPAQ02200InBlock1", "RecCnt", 0, "1");							// 레코드갯수
				mTr.SetFieldData("CSPAQ02200InBlock1", "MgmtBrnNo", 0, "");							// 관리지점번호
				mTr.SetFieldData("CSPAQ02200InBlock1", "AcntNo", 0, setting.login_account);			// 계좌번호
				mTr.SetFieldData("CSPAQ02200InBlock1", "Pwd", 0, setting.login_account_pw);			// 비밀번호
				mTr.SetFieldData("CSPAQ02200InBlock1", "BalCreTp", 0, "0");							// 잔고생성구분: 0-주식잔고

				mTr.Request(false);
			}
		}	// end function
	}	// end class
}	// end namespace
