using System;

using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

using System.Net.Json;

using XA_DATASETLib;
using xing.cs.form;

namespace xing
{
	public class xing_real_jif : _IXARealEvents
	{
		/// <summary>xing component</summary>
		private IXAReal mReal;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mfTrading;

		/// <summary>장 상태 코드 값</summary>
		public string mjstatus = "";

		/// <summary>장 상태 :: 설명 포함</summary>
		public string mlabel = "";

		/// <summary>장 시작 시간 :: 서버 시간 기준</summary>
		public double mTimeStart21 = 0;

		/// <summary>생성자</summary>
		public xing_real_jif()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mReal = new XAReal();
			mReal.ResFileName = "\\res\\JIF.res";
			icpc = (IConnectionPointContainer)mReal;
			Guid IID_RealEvents = typeof(_IXARealEvents).GUID;
			icpc.FindConnectionPoint(ref IID_RealEvents, out icp);
			icp.Advise(this, out iCookie);

			// 장 상태 값 초기화
			init_jstatus();
		}	// end function


		/// <summary>
		/// HTS -> API 연동시 이벤트 처리
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		void _IXARealEvents.RecieveLinkData(string szLinkName, string szData, string szFiller)
		{
			Log.WriteLine("## HTS -> API :: " + szLinkName + " :: " + szData + " :: " + szFiller);

			if (szLinkName == "&STOCK_CODE")
			{
				// 잔고정보 그리드에 해당 종목 선택시켜 줌
				for (int iRow = 0; iRow < mfTrading.GridAccount.Rows.Count; iRow++)
				{
					// 그리드에서 해당 종목을 찾아서..
					if (szData == mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["종목코드"].GetValue())].Value.ToString())
					{
						mfTrading.GridAccount.Rows[iRow].Selected = true;
						break;
					}
				}
			}
		}	// end function


		/// <summary>
		/// 실시간 데이터 처리 
		/// </summary>
		/// <param name="szTrCode"></param>
		void _IXARealEvents.ReceiveRealData(string szTrCode)
		{
			try
			{
                string jangubun = mReal.GetFieldData("OutBlock", "jangubun");
				string jstatus = mReal.GetFieldData("OutBlock", "jstatus");

				// 코스피 기준
				if (jangubun == "1")
				{
					set_jstatus(jstatus);

					// 설정 파일에 저장
					Properties.Settings.Default.JIF_DATE = util_datetime.GetFormatNow("yyyyMMdd");
					Properties.Settings.Default.JIF_JSTATUS = jstatus;

					if (jstatus == "21")
					{
						mTimeStart21 = setting.mxTr0167.mTimeCur;
						Properties.Settings.Default.JIF_TIME_START_21 = mTimeStart21;
					}

					Properties.Settings.Default.Save();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
			}
		}	// end function

		/// <summary>
		/// 장 상태 변경 내역 세팅
		/// </summary>
		/// <param name="time">시간 :: 12:30:21</param>
		/// <param name="jangubun">장 구분 : 1-코스피</param>
		/// <param name="jstatus">장 상태 : 21-장시작</param>
		public void set_jstatus(string jstatus)
		{
			try
			{
				mjstatus = jstatus;
				mlabel = jstatus + "-" + get_label(jstatus);

				Log.WriteLine("JIF :: " + mlabel);
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}	// end function


		/// <summary>
		/// 장운영 정보 실시간 호출 등록/해지
		/// 옵션에 따른 자동거래 시작
		/// </summary>
		/// <param name="flag">장운영 정보 실시간 등록 여부</param>
		public void call_advise(bool flag)
		{
			if (flag)
			{
				mReal.SetFieldData("InBlock", "jangubun", "0");
				mReal.AdviseRealData();

				Log.WriteLine("JIF :: 장 운영정보 실시간 접수 등록");
			}
			else
			{
				mReal.UnadviseRealData();
				Log.WriteLine("JIF :: 장 운영정보 실시간 접수 해제");
			}

			// HTS -> API 연동 등록
			setting.mxRealJif.call_hts_2_api(setting.program_hts_2_api_yn);

			// 자동거래 시작
			mfTrading.fnAutoTrading(setting.login_trading_yn);
		}	// end function


		/// <summary>
		/// HTS -> API 연동 등록/해지
		/// </summary>
		/// <param name="flag">등록/해지 유무</param>
		public void call_hts_2_api(bool flag)
		{
			if (flag)
			{
				mReal.AdviseLinkFromHTS();
				Log.WriteLine("JIF :: HTS -> API 연동 등록");
			}
			else
			{
				mReal.UnAdviseLinkFromHTS();
				Log.WriteLine("JIF :: HTS -> API 연동 해제");
			}
		}	// end function


		/// <summary>
		/// 장 상태값에 해당하는 라벨값 리턴
		/// </summary>
		/// <param name="jstatus">장 정보 코드 값</param>
		/// <returns>장 정보 설명 값</returns>
		public string get_label(string jstatus)
		{
			if (jstatus == "11")
			{
				return "장전 동시호가 개시";
			}
			else if (jstatus == "21")
			{
				return "장 시작";
			}
			else if (jstatus == "22")
			{
				return "장 개시 10초 전";
			}
			else if (jstatus == "23")
			{
				return "장 개시 1분 전";
			}
			else if (jstatus == "24")
			{
				return "장 개시 5분 전";
			}
			else if (jstatus == "25")
			{
				return "장 개시 10분 전";
			}
			else if (jstatus == "31")
			{
				return "장후 동시호가 개시";
			}
			else if (jstatus == "41")
			{
				return "장 마감";
			}
			else if (jstatus == "42")
			{
				return "장 마감 10초 전";
			}
			else if (jstatus == "43")
			{
				return "장 마감 1분 전";
			}
			else if (jstatus == "44")
			{
				return "장 마감 5분 전";
			}
			else if (jstatus == "51")
			{
				return "시간외종가 매매 개시";
			}
			else if (jstatus == "52")
			{
				return "시간외종가 매매 종료";
			}
			else if (jstatus == "53")
			{
				return "시간외단일가 매매 개시";
			}
			else if (jstatus == "54")
			{
				return "시간외단일가 매매 종료";
			}
			else if (jstatus == "61")
			{
				return "서킷브레이크 발동";
			}
			else if (jstatus == "62")
			{
				return "서킷브레이크 해제";
			}
			else if (jstatus == "63")
			{
				return "서킷브레이크 단일가 접수";
			}
			else if (jstatus == "64")
			{
				return "사이드카 매도 발동";
			}
			else if (jstatus == "65")
			{
				return "사이드카 매도 해제";
			}
			else if (jstatus == "66")
			{
				return "사이드카 매수 발동";
			}
			else if (jstatus == "67")
			{
				return "사이드카 매수 해제";
			}
			else
			{
				return jstatus + " :: 해당 코드 없음";
			}
		}	// end function


		/// <summary>
		/// 장 상태값 초기화
		/// </summary>
		public void init_jstatus()
		{
			string jif_date = Properties.Settings.Default.JIF_DATE;
			string jif_jstatus = Properties.Settings.Default.JIF_JSTATUS;

			// 당일 장정보 등록 내용이 있을 경우
			if (jif_date == util_datetime.GetFormatNow("yyyyMMdd"))
			{
				mTimeStart21 = Properties.Settings.Default.JIF_TIME_START_21;

				set_jstatus(jif_jstatus);
			}
			// 장정보 등록된 적이 없다면..
			else
			{
				string jstatus = "";
				double time = Convert.ToDouble(util_datetime.GetFormatNow("HHmmss"));

				if (time < 73000)
				{
					jstatus = "41";
				}
				else if (time >= 73000 && time < 80000)		// 7:30 ~ 8:30 시간외종가
				{
					jstatus = "51";
				}
				else if (time >= 80000 && time < 85000)		// 8:00 ~ 9:00 장전 동시호가(시간외종가와 중첩 구간 있음)
				{
					jstatus = "11";
				}
				else if (time >= 85000 && time < 85500)
				{
					jstatus = "25";
				}
				else if (time >= 85500 && time < 85900)
				{
					jstatus = "24";
				}
				else if (time >= 85900 && time < 85950)
				{
					jstatus = "23";
				}
				else if (time >= 85950 && time < 90000)
				{
					jstatus = "22";
				}
				else if (time >= 90000 && time < 145000)
				{
					jstatus = "21";
				}
				else if (time >= 145000 && time < 145500)
				{
					jstatus = "31";
				}
				else if (time >= 145500 && time < 145900)
				{
					jstatus = "44";
				}
				else if (time >= 145900 && time < 145950)
				{
					jstatus = "43";
				}
				else if (time >= 145950 && time < 150000)
				{
					jstatus = "42";
				}
				else
				{
					jstatus = "41";
				}

				set_jstatus(jstatus);
			}
		}	// end function
	}	// end class
}	// end namespace
