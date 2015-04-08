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
	public class xing_tr_8407 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		private IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mFormTrading;

		/// <summary>현재 TR이 실행중인지 여부</summary>
		private bool mStateRun = false;

		/// <summary>현재 TR이 실행중일 동안 카운트 수</summary>
		private int mStateRunCount = 0;

		/// <summary>잔고 종목의 매도시 필요한 정보 저장 - {종목코드:"최종변경시간", ... }</summary>
		public JsonObjectCollection mJson;	

		

		/// <summary>
		/// 생성자 - 멀티현재가
		/// </summary>
		public xing_tr_8407()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\t8407.res";
			icpc = (IConnectionPointContainer)mTr;
			Guid IID_QueryEvents = typeof(_IXAQueryEvents).GUID;
			icpc.FindConnectionPoint(ref IID_QueryEvents, out icp);
			icp.Advise(this, out iCookie);

			// json 초기화
			mJson = new JsonObjectCollection();
		}	// end function


		/// <summary>
		/// 데이터 응답 처리
		/// </summary>
		/// <param name="szTrCode">호출된 TrCode</param>
		void _IXAQueryEvents.ReceiveData(string szTrCode)
		{
            try
            {
                int iCount = mTr.GetBlockCount("t8407OutBlock1");

                for (int i = 0; i < iCount; i++)
                {
                    string shcode = mTr.GetFieldData("t8407OutBlock1", "shcode", i);			// 종목코드
					string open = mTr.GetFieldData("t8407OutBlock1", "open", i);				// 당일시가
					string uplmtprice = mTr.GetFieldData("t8407OutBlock1", "uplmtprice", i);	// 상한가
					string dnlmtprice = mTr.GetFieldData("t8407OutBlock1", "dnlmtprice", i);	// 하한가

					// json에 저장된 종목정보 가져 옴
					JsonObject obj = mJson[shcode];

					// json에 종목정보가 없으면 추가
					if (obj == null)
					{
						mJson.Add(new JsonStringValue(shcode, DateTime.Now.ToString()));
					}
					// 있으면 현재 검색된 기준으로 변경
					else
					{
						((JsonStringValue)obj).Value = DateTime.Now.ToString();
					}

					// 잔고 그리드에 반영
					for (int iRow = 0; iRow < mFormTrading.GridAccount.Rows.Count; iRow++)
					{
						// 그리드에서 해당 종목을 찾아서..
						if (mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["종목코드"].GetValue())].Value.ToString() == shcode)
						{
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["등락율"].GetValue())].Value = Util.GetNumberFormat2(mTr.GetFieldData("t8407OutBlock1", "diff", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["거래량"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "volume", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["거래대금"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "value", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["체결강도"].GetValue())].Value = Util.GetNumberFormat2(mTr.GetFieldData("t8407OutBlock1", "chdegree", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["시가"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "open", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["고가"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "high", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["저가"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "low", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["전일종가"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "jnilclose", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["상한가"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "uplmtprice", i));
							mFormTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mFormTrading.mhAccount["하한가"].GetValue())].Value = Util.GetNumberFormat(mTr.GetFieldData("t8407OutBlock1", "dnlmtprice", i));
							
							// 루프 빠져 나감
							break;
						}	// end if
					}	// end for
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
                if (nMessageCode != "00000")
                {
					//Log.WriteLine("t8407 :: " + nMessageCode + " :: " + szMessage);
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
		/// 멀티현재가 검색 호출
		/// </summary>
		/// <param name="nrec">검색 종목 수 - 최대 50건</param>
		/// <param name="shcode">종복번호 병합한 거</param>
		public void call_request(string nrec, string shcodes)
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
				if (shcodes != "")
				{
					mStateRun = true;

					mTr.SetFieldData("t8407InBlock", "nrec", 0, nrec);
					mTr.SetFieldData("t8407InBlock", "shcode", 0, shcodes);

					mTr.Request(false);

//					Log.WriteLine("call_request 8407 :: " + nrec.ToString());
				}
			}
		}	// end function
	}	// end class
}	// end namespace
