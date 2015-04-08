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
	public class xing_tr_CSPAT00700 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		public IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mFormTrading;

		/// <summary>메인 폼 참조</summary>
		public FormMain mFormMain;

		/// <summary>
		/// 생성자 - 현물정정주문
		/// </summary>
		public xing_tr_CSPAT00700()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\CSPAT00700.res";
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
                //Log.WriteLine("CSPAT00700 :: " + szTrCode);
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
		}	// end function


		/// <summary>
		/// 현물정정주문 메세지 처리
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
                    
                }
				//// 정정가격이 원주문 가격과 같습니다 - 패쓰
				//else if (nMessageCode == "01922")
				//{
				//    ;
				//}
				//// 원주문번호가 없습니다 - 패쓰
				//else if (nMessageCode == "01230")
				//{
				//    ;
				//}
				//// 00132 :: 모의투자 정정주문 입력이 완료되었습니다. 
				//else if (nMessageCode == "00132")
				//{
				//    ;
				//}
				//// 01923 :: 모의투자 정정수량이 정정가능수량을 초과합니다 
				//else if (nMessageCode == "01923")
				//{
				//    ;
				//}
				//// 01231 :: 모의투자 정정/취소할 수량이 없습니다
				//else if (nMessageCode == "01231")
				//{
				//    ;
				//}
				//// 장 종료시 자동거래 중지
				//else if (nMessageCode == "01218")
				//{
				//    Log.WriteLine("CSPAT00700 :: " + nMessageCode + " :: " + szMessage);

				//    mFormTrading.fnAutoTrading(false);
				//}
				else
				{
					//Log.WriteLine("CSPAT00700 :: " + nMessageCode + " :: " + szMessage);
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
		/// 현물정정주문
		/// </summary>
		/// <param name="order_no">원주문번호</param>
		/// <param name="shcode">종목번호</param>
		/// <param name="quantity">주문수량</param>
		/// <param name="price">주문가</param>
		/// <param name="order_code">호가유형코드</param>
		public void call_request(string order_no, string shcode, string quantity, string price, string order_code)
		{
			// 장중
			if ("21".IndexOf(setting.mxRealJif.mjstatus) > -1)
			{
				mTr.SetFieldData("CSPAT00700InBlock1", "AcntNo", 0, setting.login_account);			// 계좌번호
				mTr.SetFieldData("CSPAT00700InBlock1", "InptPwd", 0, setting.login_account_pw);		// 입력비밀번호
				mTr.SetFieldData("CSPAT00700InBlock1", "OrgOrdNo", 0, order_no);					// 원주문번호
				mTr.SetFieldData("CSPAT00700InBlock1", "IsuNo", 0, "A" + shcode);					// 종목번호
				mTr.SetFieldData("CSPAT00700InBlock1", "OrdQty", 0, quantity);						// 주문수량
				mTr.SetFieldData("CSPAT00700InBlock1", "OrdprcPtnCode", 0, order_code);				// 호가유형코드 : 00-지정가
				mTr.SetFieldData("CSPAT00700InBlock1", "OrdCndiTpCode", 0, "0");					// 주문조건구분 : 0-없음
				mTr.SetFieldData("CSPAT00700InBlock1", "OrdPrc", 0, price);							// 주문가

				mTr.Request(false);
			}
		}	// end function
	}	// end class
}	// end namespace
