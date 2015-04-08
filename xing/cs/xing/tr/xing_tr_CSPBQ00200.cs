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
	public class xing_tr_CSPBQ00200 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		public IXAQuery mTr;

		/// <summary>현재 TR이 실행중인지 여부</summary>
		private bool mStateRun = false;

		/// <summary>현재 TR이 실행중일 동안 카운트 수</summary>
		private int mStateRunCount = 0;	


        /// <summary>
        /// 생성자 - 현물계좌 증거금별 주문가능 수량 조회
        /// </summary>
		public xing_tr_CSPBQ00200()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\CSPBQ00200.res";
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
				string shcode = mTr.GetFieldData("CSPBQ00200OutBlock1", "IsuNo", 0);							// 종목코드
				string hname = mTr.GetFieldData("CSPBQ00200OutBlock2", "IsuNm", 0);								// 종목명
				string close = mTr.GetFieldData("CSPBQ00200OutBlock1", "OrdPrc", 0);							// 주문가격
				int quantity = (int)Convert.ToDouble(mTr.GetFieldData("CSPBQ00200OutBlock2", "OrdAbleQty", 0));	// 주문가능수량
				
				// 주문시 틱 변동에 따른 오차 범위를 줄이기 위해 값 조정
				quantity = (int)Math.Ceiling(quantity * 0.96);

				// 매수 진행
				if (quantity > 0)
				{
					setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), close.ToString(), "2", "[매수]", hname);
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
				else
				{
					Log.WriteLine("CSPBQ00200 :: " + nMessageCode + " :: " + szMessage);
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
		/// 현물계좌 증거금별 주문가능 수량 조회
		/// KODEX 종목 매수시 미수거래를 하기 위함
		/// </summary>
		/// <param name="shcode">종목코드</param>
		/// <param name="price">가격</param>
		public void call_request(string shcode, string price)
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

				mTr.SetFieldData("CSPBQ00200InBlock1", "RecCnt", 0, "1");							// 레코드갯수
				mTr.SetFieldData("CSPBQ00200InBlock1", "BnsTpCode", 0, "2");						// 매매구분 : 1@매도, 2@매수
				mTr.SetFieldData("CSPBQ00200InBlock1", "AcntNo", 0, setting.login_account);			// 계좌번호
				mTr.SetFieldData("CSPBQ00200InBlock1", "InptPwd", 0, setting.login_account_pw);		// 비밀번호
				mTr.SetFieldData("CSPBQ00200InBlock1", "IsuNo", 0, "A" + shcode);					// 종목코드
				mTr.SetFieldData("CSPBQ00200InBlock1", "OrdPrc", 0, price);							// 주문가격
				mTr.SetFieldData("CSPBQ00200InBlock1", "RegCommdaCode", 0, "");						// ETK_GetCommMedia() 리턴값 입력??

				mTr.Request(false);
			}
		}	// end function
	}	// end class
}	// end namespace
