using System;

using System.Runtime.InteropServices.ComTypes;

using System.Net.Json;

using XA_DATASETLib;

namespace xing
{
	public class xing_tr_8430 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		public IXAQuery mTr;

		/// <summary>메인 폼 참조</summary>
		public FormMain mfMain;

		/// <summary>종목 기본 정보 저장 {"종목코드":"매매단위"} </summary>
		public JsonObjectCollection mJson;

		/// <summary>
		/// 생성자 - 주식종목조회
		/// </summary>
		public xing_tr_8430()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\t8430.res";
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
		/// <param name="szTrCode"></param>
		void _IXAQueryEvents.ReceiveData(string szTrCode)
		{
            try
            {
                int iCount = mTr.GetBlockCount("t8430OutBlock");

                for (int i = 0; i < iCount; i++)
                {
					mJson.Add(
						new JsonStringValue(
							mTr.GetFieldData("t8430OutBlock", "shcode", i), 
							mTr.GetFieldData("t8430OutBlock", "memedan", i)
						)
					);
                }	// end for

				Log.WriteLine("t8430 :: 종목 기본 정보 저장 완료..!!");

				// 서버의 시간 검색 타이머 스타트 - 여기서 PC의 시간을 서버 시간과 동기화 시킴
				mfMain.Timer0167.Start();
				Log.WriteLine("t8430 :: 시간 검색 타이머 구동");

				// 장 운영 정보 실시간 등록
				// 자동거래 시작도 이 함수 안에서 실행 시킴
				setting.mxRealJif.call_advise(true);
                mfMain.TimerRealSh.Start();
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
                if (nMessageCode == "00000")
                {
					;
                }
				// 
				else if (nMessageCode == "00007")
				{
					mfMain.fnRestartProgram();
				}
				else
				{
					Log.WriteLine("t8430 :: " + nMessageCode + " :: " + szMessage);
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
		/// 종목 기본 정보 검색 호출
		/// </summary>
		public void call_request()
		{
			mTr.SetFieldData("t8430InBlock", "gubun", 0, "0");		// 구분: 0-전체
			mTr.Request(false);
		}	// end function
	}	// end class
}	// end namespace
