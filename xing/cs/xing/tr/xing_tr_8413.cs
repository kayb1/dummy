using System;

using System.Runtime.InteropServices.ComTypes;

using System.Net.Json;

using XA_DATASETLib;

namespace xing
{
	public class xing_tr_8413 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		public IXAQuery mTr;

		/// <summary>
		/// 생성자 - 주식종목조회
		/// </summary>
		public xing_tr_8413()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\t8413.res";
			icpc = (IConnectionPointContainer)mTr;
			Guid IID_QueryEvents = typeof(_IXAQueryEvents).GUID;
			icpc.FindConnectionPoint(ref IID_QueryEvents, out icp);
			icp.Advise(this, out iCookie);
		}	// end function


		/// <summary>
		/// 데이터 응답 처리 
		/// </summary>
		/// <param name="szTrCode"></param>
		void _IXAQueryEvents.ReceiveData(string szTrCode)
		{
            try
            {
                String shcode = mTr.GetFieldData("t8413OutBlock", "shcode", 0);

                if (setting.mxTr1833.mT1833Json[shcode] == null)
                {
                    int highPrice = 0;
                    int lowPrice = 99999999;
                    int iCount = mTr.GetBlockCount("t8413OutBlock1");
                    for (int i = 0; i < iCount - 1; i++) // 당일 봉 무시
                    {
					    String date = mTr.GetFieldData("t8413OutBlock1", "date", i);
                        String high = mTr.GetFieldData("t8413OutBlock1", "high", i);
                        String low = mTr.GetFieldData("t8413OutBlock1", "low", i);

                        int realHigh = Convert.ToInt32(high);
                        int realLow = Convert.ToInt32(low);
                        if (highPrice < realHigh) // 고가 갱신
                        {
                            highPrice = realHigh;
                        }
                        if (lowPrice > realLow) // 저가 갱신
                        {
                            lowPrice = realLow;
                        }
                        //Log.WriteLine("t8413 :: 종목 차트 일봉 데이타 수신 " + iCount + " " + date + " " + high + " " + low);
                    }	// end for

                    int dayOpen = Convert.ToInt32(mTr.GetFieldData("t8413OutBlock", "disiga", 0));
                    //int dayClose = Convert.ToInt32(mTr.GetFieldData("t8413OutBlock", "diclose", 0));

                    //Log.WriteLine("t8413 :: 종목 차트 일봉 데이타 60봉고가/저가 " + " " + shcode + " " + highPrice + " " + lowPrice + " " + p236 + " " + p382 + " " + p50 + " " + p618 + " " + isPibonacci);
//                    setting.mxTr1833.mT1833Json.Add(new JsonStringValue(shcode, highPrice.ToString() + "|" + lowPrice.ToString() + "|" + isPibonacci.ToString()));
                    setting.mxTr1833.mT1833Json.Add(new JsonStringValue(shcode, highPrice.ToString() + "|" + lowPrice.ToString() + "|" + dayOpen.ToString()));
                }
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
		/// <param name="blsSystemError"></param>
		/// <param name="nMessageCode"></param>
		/// <param name="szMessage"></param>
		void _IXAQueryEvents.ReceiveMessage(bool blsSystemError, string nMessageCode, string szMessage)
		{
            try
            {
                
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
		/// 종목 기본 정보 검색 호출
		/// </summary>
		public void call_request(string shcode)
		{
            mTr.SetFieldData("t8413InBlock", "shcode", 0, shcode);
            mTr.SetFieldData("t8413InBlock", "gubun", 0, "2"); // 일
            mTr.SetFieldData("t8413InBlock", "qrycnt", 0, "137"); // 137일
            mTr.SetFieldData("t8413InBlock", "sdate", 0, " ");
            mTr.SetFieldData("t8413InBlock", "edate", 0, "당일"); // 당일 포함 137일 가져옴
            mTr.SetFieldData("t8413InBlock", "cts_date", 0, " ");
            mTr.SetFieldData("t8413InBlock", "comp_yn", 0, "N"); // 압축 x

			mTr.Request(false);
		}	// end function
	}	// end class
}	// end namespace
