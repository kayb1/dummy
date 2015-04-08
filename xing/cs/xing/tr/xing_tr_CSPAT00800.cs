using System;
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
	public class xing_tr_CSPAT00800 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		public IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mfTrading;

		/// <summary>
		/// 생성자 - 현물취소주문
		/// </summary>
		public xing_tr_CSPAT00800()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\CSPAT00800.res";
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
                //			Log.WriteLine(szTrCode);

				
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
		}	// end function


		/// <summary>
		/// 현물취소주문 메세지 처리
		/// </summary>
		/// <param name="blsSystemError">시스템 에러</param>
		/// <param name="nMessageCode">응답코드</param>
		/// <param name="szMessage">메세지 내용</param>
		void _IXAQueryEvents.ReceiveMessage(bool blsSystemError, string nMessageCode, string szMessage)
		{
            try
            {
                if (nMessageCode != "00000")
                {
					// 00156 :: 모의투자 취소주문 입력이 완료되었습니다. 
					if (nMessageCode == "00156")
					{
						;
					}
					// 01218 :: 모의투자 장종료 상태입니다          
					else if (nMessageCode == "01218")
					{
						mfTrading.fnAutoTrading(false);
					}
					else
					{
						Log.WriteLine("CSPAT00800 :: " + nMessageCode + " :: " + szMessage);
					}
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
		/// 현물 취소 주문
		/// </summary>
		/// <param name="OrgOrdNo">원주문번호</param>
		/// <param name="IsuNo">종목번호</param>
		/// <param name="OrdQty">주문수량</param>
		public void call_request(string OrgOrdNo, string IsuNo, string OrdQty)
		{
			mTr.SetFieldData("CSPAT00800InBlock1", "AcntNo", 0, setting.login_account);			// 계좌번호
			mTr.SetFieldData("CSPAT00800InBlock1", "InptPwd", 0, setting.login_account_pw);		// 입력비밀번호
			mTr.SetFieldData("CSPAT00800InBlock1", "OrgOrdNo", 0, OrgOrdNo);					// 원주문번호
			mTr.SetFieldData("CSPAT00800InBlock1", "IsuNo", 0, "A" + IsuNo);					// 종목번호
			mTr.SetFieldData("CSPAT00800InBlock1", "OrdQty", 0, OrdQty);						// 주문수량

			mTr.Request(false);
		}	// end function
	}	// end class
}	// end namespace
