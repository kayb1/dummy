using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel;
using System.Windows.Forms;

using System.Net.Json;

using XA_DATASETLib;

namespace xing
{
	public class xing_tr_CSPAT00600 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		public IXAQuery mTr;

		/// <summary>
		/// 생성자 - 현물정상주문
		/// </summary>
		public xing_tr_CSPAT00600()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\CSPAT00600.res";
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
		/// 현물정상주문 메세지 처리
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
				// 시간당 전송 제한에 걸렸습니다
				else if (nMessageCode == "  -21")
				{
					Log.WriteLine("CSPAT00600 :: " + nMessageCode + " :: " + szMessage);
				}
				// 01222 :: 모의투자 매도잔고가 부족합니다  
				else if (nMessageCode == "01222")
				{
					;
				}
				// 00040 :: 모의투자 매수주문 입력이 완료되었습니다.
				else if (nMessageCode == "00040")
				{
					;
				}
				// 00039 :: 모의투자 매도주문 입력이 완료되었습니다. 
				else if (nMessageCode == "00039")
				{
					;
				}

				//// 01221 :: 모의투자 증거금부족으로 주문이 불가능합니다
				//else if (nMessageCode == "01221")
				//{
				//    ;
				//} 
				//// 01219 :: 모의투자 매매금지 종목
				//else if (nMessageCode == "01219")
				//{
				//    ;
				//}
				else
				{
					Log.WriteLine("CSPAT00600 :: " + nMessageCode + " :: " + szMessage);
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
		/// 현물정상주문
		/// </summary>
		/// <param name="IsuNo">종목번호</param>
		/// <param name="Quantity">수량</param>
		/// <param name="Price">가격</param>
		/// <param name="DivideBuySell">매매구분 : 1-매도, 2-매수</param>
		/// <param name="szDescription">매매 내역 : [매도] 당일시가갱신</param>
		/// <param name="hname">종목명</param>
		public void call_request(string shcode, string quantity, string price, string divide, string szDescription, string hname)
		{
			// 종목코드 포멧 맞추기
			shcode = shcode.Replace("A", "");

			string jstatus = setting.mxRealJif.mjstatus;
			
			// 장중, 장전/장마감 동시호가 10초전
			if ("21,22,42".IndexOf(jstatus) > -1)
			{
				// 매수
				if (divide == "2")
				{
					JsonObject obj = setting.mxTr0424.mJson[shcode];

					if (obj == null)
					{
						setting.mxTr0424.mJson.Add(new JsonStringValue(shcode, "0|0|0"));
					}
				}

				mTr.SetFieldData("CSPAT00600Inblock1", "AcntNo", 0, setting.login_account);			// 계좌번호
				mTr.SetFieldData("CSPAT00600Inblock1", "InptPwd", 0, setting.login_account_pw);		// 입력비밀번호
				mTr.SetFieldData("CSPAT00600Inblock1", "IsuNo", 0, "A" + shcode);					// 종목번호
				mTr.SetFieldData("CSPAT00600Inblock1", "OrdQty", 0, quantity);						// 주문수량
				mTr.SetFieldData("CSPAT00600Inblock1", "OrdPrc", 0, price);							// 주문가
				mTr.SetFieldData("CSPAT00600Inblock1", "BnsTpCode", 0, divide);						// 매매구분: 1-매도, 2-매수
				mTr.SetFieldData("CSPAT00600Inblock1", "MgntrnCode", 0, "000");						// 신용거래코드: 000-보통
				mTr.SetFieldData("CSPAT00600Inblock1", "LoanDt", 0, "");							// 대출일 : 신용주문이 아닐 경우 SPACE
				mTr.SetFieldData("CSPAT00600Inblock1", "OrdCndiTpCode", 0, "0");					// 주문조건구분 : 0-없음
				mTr.SetFieldData("CSPAT00600Inblock1", "OrdprcPtnCode", 0, "00");					// 호가유형코드: 00-지정가, 05-조건부지정가, 06-최유리지정가, 07-최우선지정가

				mTr.Request(false);

				Log.WriteLine(szDescription + " :: " + hname + " 가격:" + price);
			}
		}	// end function
	}	// end class
}	// end namespace
