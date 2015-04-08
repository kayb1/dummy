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
	public class xing_tr_0425 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		private IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mfTrading;

		/// <summary>현재 TR이 실행중인지 여부</summary>
		private bool mStateRun = false;

		/// <summary>현재 TR이 실행중일 동안 카운트 수</summary>
		private int mStateRunCount = 0;	

		/// <summary>
		/// 생성자 - 주식 체결/미체결
		/// </summary>
		public xing_tr_0425()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\t0425.res";
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
				string jstatus = setting.mxRealJif.mjstatus;

				// 그리드 초기화
				mfTrading.GridOrderBuy.Rows.Clear();
				mfTrading.GridOrderSell.Rows.Clear();

                string[] rowBuy = new string[3];
                string[] rowSell = new string[3];

                int iCount = mTr.GetBlockCount("t0425OutBlock1");
                
                for (int i = 0; i < iCount; i++)
                {
                    string order_no = mTr.GetFieldData("t0425OutBlock1", "ordno", i);
					string shcode = mTr.GetFieldData("t0425OutBlock1", "expcode", i);
					string quantity = mTr.GetFieldData("t0425OutBlock1", "ordrem", i);
                    string price2 = mTr.GetFieldData("t0425OutBlock1", "price", i); // 주문가
					string price = mTr.GetFieldData("t0425OutBlock1", "price1", i); // 현재가
					string order_code = mTr.GetFieldData("t0425OutBlock1", "hogagb", i);

                    // 잔고목록에서 종목찾아본다
                    int iRowGrid = mfTrading.GridAccount.Rows.Count;
                    string name = "";
                    for (int iRow = 0; iRow < iRowGrid; iRow++)
                    {
                        // 그리드에서 해당 종목을 찾아서..
                        if (shcode == mfTrading.GridAccount.Rows[iRow].Cells[0].Value.ToString())
                        {
                            // 종목명
                            name = mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["종목명"].GetValue())].Value.ToString();
                        }
                    }
                    // 종목검색에서 종목찾아본다
                    iRowGrid = mfTrading.GridBuy.Rows.Count;
                    for (int iRow = 0; iRow < iRowGrid; iRow++)
                    {
                        // 그리드에서 해당 종목을 찾아서..
                        if (shcode == mfTrading.GridBuy.Rows[iRow].Cells[0].Value.ToString())
                        {
                            // 종목명
                            name = mfTrading.GridBuy.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhBuyList["종목명"].GetValue())].Value.ToString();
                        }
                    }

                    // 미체결-매수
                    if (mTr.GetFieldData("t0425OutBlock1", "medosu", i).Substring(0, 2) == "매수")
                    {
                        JsonObject obj = setting.mxTr0424.mJson[shcode];

                        rowBuy[0] = shcode;
                        if (name.Length > 1) {
                            rowBuy[0] = name;
                        }
                        rowBuy[1] = quantity;
                        rowBuy[2] = price2;
                        mfTrading.GridOrderBuy.Rows.Add(rowBuy);

                        // 일괄 매도중이면
                        if (mfTrading.CheckSellAll.Checked == true)
                        {
                            // 미체결(매수) 종목 전량 취소
							setting.mxTrCSPAT00800.call_request(order_no, shcode, quantity);

							if (obj != null)
							{
								setting.mxTr0424.mJson.Remove(obj);
							}
                        }
						// 보유종목 당일 청산
						else if (setting.sell_today_yn && setting.mxTr0167.mTimeCur >= 144900)
						{
							// 미체결(매수) 종목 전량 취소
							setting.mxTrCSPAT00800.call_request(order_no, shcode, quantity);

							if (obj != null)
							{
								setting.mxTr0424.mJson.Remove(obj);
							}
						}
						// 일괄 매도중이 아닐 경우
                        else
                        {
							// 장마감 동시호가에 들어가면 
							if (jstatus == "31")
							{
								// 미체결 종목들 모두 취소 주문 발행
								if (setting.buy_order_cancel_31_yn)
								{
									setting.mxTrCSPAT00800.call_request(order_no, shcode, quantity);

									if (obj != null)
									{
										setting.mxTr0424.mJson.Remove(obj);
									}
								}
							}
							else
							{
								// 매도 시그널 발생한 종목은..
								if (obj == null)
								{
									// 미체결 종목 전량 취소
									setting.mxTrCSPAT00800.call_request(order_no, shcode, quantity);
								}
								else
								{
									// 미체결 전량 정정 주문(현재가)
									if (setting.buy_re_order_yn)
									{
										setting.mxTrCSPAT00700.call_request(order_no, shcode, quantity, price, order_code);
									}
								}
							}
                        }
                    }
					// 미체결-매도
                    else
                    {
                        rowSell[0] = shcode;
                        if (name.Length > 1)
                        {
                            rowSell[0] = name;
                        }
                        rowSell[1] = quantity;
                        rowSell[2] = price2;
                        mfTrading.GridOrderSell.Rows.Add(rowSell);

						// 장마감 동시호가에 들어가면 미체결 종목들 모두 취소
						if (jstatus == "31")
						{
							// 미체결 취소 또는 당일 청산일 경우도 취소 주문 날림
							if (setting.sell_order_cancel_31_yn || setting.sell_today_yn)
							{
								// 미체결 취소
								setting.mxTrCSPAT00800.call_request(order_no, shcode, quantity);
							}
						}
						else
						{
							if (setting.sell_re_order_yn)
							{
								// 미체결 전량 정정 주문(현재가)
								setting.mxTrCSPAT00700.call_request(order_no, shcode, quantity, price, order_code);
							}
						}
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
		/// 주식 체결/미체결 메세지 처리
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
				// 03669 :: 비밀번호 오류입니다. (5회중 4회 남았습니다)
				else if (nMessageCode == "03669")
				{
					Log.WriteLine("t0425 :: " + nMessageCode + " :: " + szMessage);

					mfTrading.fnAutoTrading(false);
				}
				else
				{
					//Log.WriteLine("t0425 :: " + nMessageCode + " :: " + szMessage);
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
		/// 주식 체결/미체결 정보 호출
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

				mTr.SetFieldData("t0425InBlock", "accno", 0, setting.login_account);		// 계좌번호
				mTr.SetFieldData("t0425InBlock", "passwd", 0, setting.login_account_pw);	// 입력비밀번호
				mTr.SetFieldData("t0425InBlock", "expcode", 0, "");							// 종목번호
				mTr.SetFieldData("t0425InBlock", "chegb", 0, "2");							// 체결구분 : 2-미체결
				mTr.SetFieldData("t0425InBlock", "medosu", 0, "0");							// 매매구분 : 0-전체
				mTr.SetFieldData("t0425InBlock", "sortgb", 0, "2");							// 정렬순서 : 2-주문번호 순
				mTr.SetFieldData("t0425InBlock", "cts_ordno", 0, "");						// 주문번호

				mTr.Request(false);
			}
		}	// end function
	}	// end class
}	// end namespace
