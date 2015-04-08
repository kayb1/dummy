using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel;
using System.Windows.Forms;

using System.Net.Json;

using XA_DATASETLib;

using System.Collections;
using xing.cs.form;

namespace xing
{
	public class xing_tr_1833 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		private IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mfTrading;

		/// <summary>설정 폼 참조</summary>
		public FormSetting mfSetting;

		/// <summary>이탈 종목 구분을 위함 - {종목코드:"거래량", ... }</summary>
		public JsonObjectCollection mJson;	

		/// <summary>종목검색 파일이 들어있는 폴더 경로</summary>
		public string mFilePath = "";

		/// <summary>21-장중 종목검색을 위한 파일 목록</summary>
		public ArrayList mFile21 = new ArrayList();

		/// <summary>21-장중 종목검색시 현재 인덱스</summary>
		public int mFile21Index = 0;

		/// <summary>22-장전 동시호가 종목검색을 위한 파일 목록</summary>
		public ArrayList mFile22 = new ArrayList();

		/// <summary>22-장전 동시호가 종목검색시 현재 인덱스</summary>
		public int mFile22Index = 0;

		/// <summary>42-장마감 동시호가 종목검색을 위한 파일 목록</summary>
		public ArrayList mFile42 = new ArrayList();

		/// <summary>42-장마감 동시호가 종목검색시 현재 인덱스</summary>
		public int mFile42Index = 0;

		/// <summary>현재 종목 검색중인 파일명</summary>
		private string mFileName = "";

		/// <summary>현재 TR이 실행중인지 여부</summary>
		private bool mStateRun = false;

		/// <summary>현재 TR이 실행중일 동안 카운트 수</summary>
		private int mStateRunCount = 0;

        /// <summary>당일 종목별 계산 정보들(한번만 계산) - {종목코드:"60고가","60저가" ... }</summary>
        public JsonObjectCollection mT1833Json;

		/// <summary>
		/// 생성자 - 종목검색
		/// </summary>
		public xing_tr_1833()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\t1833.res";
			icpc = (IConnectionPointContainer)mTr;
			Guid IID_QueryEvents = typeof(_IXAQueryEvents).GUID;
			icpc.FindConnectionPoint(ref IID_QueryEvents, out icp);
			icp.Advise(this, out iCookie);

			// json 초기화
			mJson = new JsonObjectCollection();

			#region 1833 종목 검색을 위한 파일 설정 로딩

			string[] files = setting.t1833_files.Split('■');

			foreach (string filename in files)
			{
				if (filename.IndexOf("21_") == 0)
				{
					mFile21.Add(filename);
				}
				else if (filename.IndexOf("22_") == 0)
				{
					mFile22.Add(filename);
				}
				else if (filename.IndexOf("42_") == 0)
				{
					mFile42.Add(filename);
				}
			}

			#endregion

            #region 1833 종목 검색후 당일 종목마다 가지고 있어야할 종목 정보 처리

            string date_saved = Properties.Settings.Default.T1833_DATE;
            string date_now = util_datetime.GetFormatNow("yyyyMMdd");

            // 오늘 프로그램이 실행된적이 있다면...
            if (date_now == date_saved)
            {
                // 설정파일에 저장된 값을 로딩				
                try
                {
                    mT1833Json = (JsonObjectCollection)new JsonTextParser().Parse(Properties.Settings.Default.T1833_JSON);
                }
                // 설정 파일에 잘못된 json 값이 들어가 있을 경우 예외 처리
                catch (Exception ex)
                {
                    // json 초기화
                    mT1833Json = new JsonObjectCollection();

                    Log.WriteLine("t1833 종목 계산 정보 json 초기화 :: " + ex.Message);
                }
            }
            // 오늘 처음 실행되는 것이라면..
            else
            {
                // json 초기화
                mT1833Json = new JsonObjectCollection();

                // 설정파일에 금일 날짜값 저장
                Properties.Settings.Default.T1833_DATE = date_now;
                Properties.Settings.Default.Save();
            }

            #endregion
        
        }	// end function


		/// <summary>
		/// 데이터 응답 처리
		/// </summary>
		/// <param name="szTrCode">호출된 TrCode</param>
		void _IXAQueryEvents.ReceiveData(string szTrCode)
		{
            try
            {
				// 증거금 100% 종목 매입가능 금액
				double iCost100 = setting.mxTrCSPAQ02200.m100;

				// 그리드 초기화
				//mfTrading.GridBuy.Rows.Clear();

				// 서버에서 받은 응답 결과를 디비에 저장
				int iCount = mTr.GetBlockCount("t1833OutBlock1");

				// 파일명에서 매수/매도 구분값 가져옴
				string divide = mFileName.Substring(3, 2);

                // 파일명에서 이름 구분값 가져옴
                string logicName = mFileName.Substring(6, 4);
                if (String.Compare(logicName, "피보나치") == 0)
                {
                    for (int i = 0; i < iCount; i++)
                    {
                        string shcode = mTr.GetFieldData("t1833OutBlock1", "shcode", i);
                        string hname = mTr.GetFieldData("t1833OutBlock1", "hname", i);
                        if (mT1833Json[shcode] == null)
                        {
                            try
                            {
                                // 주식 차트 요청
                                setting.mxTr8413.call_request(shcode);
                                //Log.WriteLine("주식차트 요청 " + hname + " " + shcode);
                            }
                            catch (Exception ex)
                            {
                                Log.WriteLine(ex.Message);
                                Log.WriteLine(ex.StackTrace);
                            }

                            break;
                        }
                    }
                }

                shBuyAndSell(iCount, divide, iCost100, logicName);

				// 다시 실행가능하도록 초기화
				mStateRun = false;
				mStateRunCount = 0;

				mFileName = "";
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
                    //Log.WriteLine("t1833 :: " + nMessageCode + " :: " + szMessage);
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
		/// 종목검색 호출
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

				string jstatus = setting.mxRealJif.mjstatus;
				string filename = "";

				// 장중
				/*if (jstatus == "21")
				{
					if (mFile21.Count >= 1)
					{
						if (mFile21Index == mFile21.Count)
						{
							mFile21Index = 0;
						}

						filename = mFile21[mFile21Index++].ToString();
					}
				}
				// 장전 동시호가
				else if ("22,23,24,25,11".IndexOf(jstatus) >= 0)
				{
					if (mFile22.Count >= 1)
					{
						if (mFile22Index == mFile22.Count)
						{
							mFile22Index = 0;
						}

						filename = mFile22[mFile22Index++].ToString();
					}
				}
				// 장마감 동시호가
				else if ("42,43,44,31".IndexOf(jstatus) >= 0)
				{
					if (mFile42.Count >= 1)
					{
						if (mFile42Index == mFile42.Count)
						{
							mFile42Index = 0;
						}

						filename = mFile42[mFile42Index++].ToString();
					}
				}*/

                if (mFile21.Count >= 1) // 상태에 상관없이 종목은 항상 검색 시도
                {
                    if (mFile21Index == mFile21.Count)
                    {
                        mFile21Index = 0;
                    }

                    filename = mFile21[mFile21Index++].ToString();
                }

				// 종목검색 호출
				if (filename != "")
				{
					mFileName = filename;
					mfTrading.Group1833.Text = filename;

					mTr.RequestService("t1833", setting.t1833_dir + filename);
				}
				else
				{
					mfTrading.Group1833.Text = "종목검색 중지";
				}
			}
		}	// end function

        private void shBuyAndSell(int iCount, string divide, double iCost100, string logicName)
        {
            for (int i = 0; i < iCount; i++)
            {
                string shcode = mTr.GetFieldData("t1833OutBlock1", "shcode", i);
                string hname = mTr.GetFieldData("t1833OutBlock1", "hname", i);
                string close = mTr.GetFieldData("t1833OutBlock1", "close", i);
                string volume = mTr.GetFieldData("t1833OutBlock1", "volume", i);
                string diff = mTr.GetFieldData("t1833OutBlock1", "diff", i);

                if (String.Compare(logicName, "피보나치") == 0 && mT1833Json[shcode] == null)
                {
                    continue;
                }
                else if (String.Compare(logicName, "피보나치") == 0 && mT1833Json[shcode] != null)
                {
                    string[] arr1833 = mT1833Json[shcode].GetValue().ToString().Split('|');
                    int high = Convert.ToInt32(arr1833[0]);
                    int low = Convert.ToInt32(arr1833[1]);
                    int realOpen = Convert.ToInt32(arr1833[2]);

                    // 피보나치 되돌림
                    double p236 = (double)high - (double)(high - low) * 0.236;
                    double p382 = (double)high - (double)(high - low) * 0.382;
                    double p50 = (double)high - (double)(high - low) * 0.5;
                    double p618 = (double)high - (double)(high - low) * 0.618;

                    int realClose = Convert.ToInt32(close);
                    bool isPibonacci = false;
                    if (realOpen > realClose && ((realOpen > p236 * 1.01 && realClose < p236 * 1.003) ||
                        (realOpen > p382 * 1.01 && realClose < p382 * 1.003) ||
                        (realOpen > p50 * 1.01 && realClose < p50 * 1.003) ||
                        (realOpen > p618 * 1.01 && realClose < p618 * 1.003)))
                    {
                        //Log.WriteLine("t1833 주문가능 " + hname + " " + shcode + " " + high + " " + low + " " + realOpen + " " + realClose + " " + (p236 * 1.003) + " " + (p382 * 1.003) + " " + (p50 * 1.003) + " " + (p618 * 1.003));
                        isPibonacci = true;
                    }

                    //Log.WriteLine("t1833 주문가능 " + hname + " " + shcode + " " + high + " " + low + " " + close + " " + isPibo);

                    if (!isPibonacci)
                    {
                        continue;
                    }
                }

                // json에 저장된 종목정보 가져 옴
                JsonObject obj = mJson[shcode];

                // json에 종목정보가 없으면 추가
                if (obj == null)
                {
                    mJson.Add(new JsonStringValue(shcode, volume));

                    // 종목검색 그리드에 표시
                    string[] row = new string[mfTrading.GridBuy.ColumnCount];
                    for (int j = 0; j < row.Length; j++)
                    {
                        row[j] = "0";
                    }
                    row[Convert.ToInt32(mfTrading.mhBuyList["종목코드"].GetValue())] = shcode;
                    row[Convert.ToInt32(mfTrading.mhBuyList["종목명"].GetValue())] = hname;
                    row[Convert.ToInt32(mfTrading.mhBuyList["현재가"].GetValue())] = Util.GetNumberFormat(close);
                    row[Convert.ToInt32(mfTrading.mhBuyList["등락율"].GetValue())] = Util.GetNumberFormat2(diff);
                    row[Convert.ToInt32(mfTrading.mhBuyList["검색가"].GetValue())] = Util.GetNumberFormat(close);
                    row[Convert.ToInt32(mfTrading.mhBuyList["검색등락율"].GetValue())] = Util.GetNumberFormat2(diff);

                    mfTrading.GridBuy.Rows.Add(row);
                }
                // 있으면 현재 검색된 기준으로 변경
                else
                {
                    if (volume == obj.GetValue().ToString())
                    {
                        continue;
                    }
                    else
                    {
                        ((JsonStringValue)obj).Value = volume;
                    }
                }
            }

            for (int iRow = 0; iRow < mfTrading.GridBuy.Rows.Count; iRow++)
            {
                bool isExist = false;
                string curHname = mfTrading.GridBuy.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhBuyList["종목명"].GetValue())].Value.ToString();
                string tempPrice = "";
                string tempRate = "";
                for (int i = 0; i < iCount; i++)
                {
                    string shcode = mTr.GetFieldData("t1833OutBlock1", "shcode", i);
                    string hname = mTr.GetFieldData("t1833OutBlock1", "hname", i);
                    string close = mTr.GetFieldData("t1833OutBlock1", "close", i);
                    string diff = mTr.GetFieldData("t1833OutBlock1", "diff", i);

                    if (String.Compare(logicName, "피보나치") == 0 && mT1833Json[shcode] == null)
                    {
                        continue;
                    }

                    if (string.Compare(curHname, hname) == 0)
                    {
                        isExist = true;
                        tempPrice = close;
                        tempRate = diff;
                        break;
                    }
                }

                if (!isExist)
                {
                    mfTrading.GridBuy.Rows[iRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Salmon;
                    mfTrading.GridBuy.Rows[iRow].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                }
                else
                {   // 존재할때 기존 종목 현재가와 등락율 갱신
                    mfTrading.GridBuy.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhBuyList["현재가"].GetValue())].Value = Util.GetNumberFormat(tempPrice);
                    mfTrading.GridBuy.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhBuyList["등락율"].GetValue())].Value = Util.GetNumberFormat2(tempRate);
                    mfTrading.GridBuy.Rows[iRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

                    double tempCur = Convert.ToDouble(mfTrading.GridBuy.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhBuyList["등락율"].GetValue())].Value);
                    double tempFirst = Convert.ToDouble(mfTrading.GridBuy.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhBuyList["검색등락율"].GetValue())].Value);
                    if (tempCur > tempFirst) // 검색출현때보다 올라있으면 빨간배경
                    {
                        mfTrading.GridBuy.Rows[iRow].DefaultCellStyle.BackColor = System.Drawing.Color.PeachPuff;
                    }
                    else if (tempCur < tempFirst) // 검색출현때보다 내려있으면 하늘배경
                    {
                        mfTrading.GridBuy.Rows[iRow].DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
                    }
                }
            }

            if (!mfTrading.ButtonAutoBuyStart.Enabled)
            {
                for (int i = 0; i < iCount; i++)
                {
                    string shcode = mTr.GetFieldData("t1833OutBlock1", "shcode", i);
                    string hname = mTr.GetFieldData("t1833OutBlock1", "hname", i);
                    string close = mTr.GetFieldData("t1833OutBlock1", "close", i);
                    string volume = mTr.GetFieldData("t1833OutBlock1", "volume", i);

                    if (String.Compare(logicName, "피보나치") == 0 && mT1833Json[shcode] == null)
                    {
                        continue;
                    }

                    // 일괄 매도중이면.. 패쓰~~
                    // 매수 종목된 항목 정보는 보기 위해 continue 사용
                    if (mfTrading.CheckSellAll.Checked == true)
                    {
                        continue;
                    }

                    #region 매수 주문
                    if (divide == "매수")
                    {
                        // 보유종목 당일 청산
                        if (setting.sell_today_yn)
                        {
                            if (setting.mxTr0167.mTimeCur >= 144858)
                            {
                                continue;
                            }
                        }

                        // 당일 목표 달성
                        if (setting.buy_max_yn)
                        {
                            // 수익율 상/하양 값이 목표값 이상이면..패쓰~~~
                            if (Math.Abs(setting.mxTr0424.mAccountRate) > setting.buy_max)
                            {
                                Log.WriteLine("당일 최고/최저 목표 초과..!!");
                                continue;
                            }
                        }
                        Log.WriteLine("당일 재매수 여부:" + setting.buy_rebuy_yn + " 당일 매수된 종목수:" + setting.mxTr0424.mRebuyJson.Count);
                        // 매수 시간 지정 사용
                        if (setting.buy_time_yn)
                        {
                            // JIF 에서 장시작 시간이 지정된 경우
                            if (setting.mxRealJif.mTimeStart21 > 0)
                            {
                                // 현재 시간이 설정값 보다 크면 패쓰~~
                                if (setting.mxTr0167.mTimeCur > (setting.mxRealJif.mTimeStart21 + (setting.buy_time * 10000)))
                                {
                                    Log.WriteLine("JIF == TRUE :: 매수 가능 시간이 지났습니다..!!");
                                    continue;
                                }
                            }
                            else
                            {
                                // 현재 시간이 설정값 보다 크면 패쓰~~
                                if (setting.mxTr0167.mTimeCur > (90000 + (setting.buy_time * 10000)))
                                {
                                    Log.WriteLine("JIF == FALSE :: 매수 가능 시간이 지났습니다..!!");
                                    continue;
                                }
                            }
                        }

                        // 미수 주문일 경우
                        if (setting.buy_misu_yn)
                        {
                            if (iCount == 1)
                            {
                                setting.mxTrCSPBQ00200.call_request(shcode, close);
                            }
                        }
                        // 미수 주문이 아닐 경우 - 증거금 100% 기준
                        else
                        {
                            // 중복 매수 불가능한 상태
                            if (setting.buy_duplicate_yn == false)
                            {
                                // 잔고에 종목이 있다면 패쓰~~
                                if (setting.mxTr0424.mJson[shcode] != null)
                                {
                                    continue;
                                }
                            }

                            // 당일 같은 종목 재매수 불가능한 상태
                            if (setting.buy_rebuy_yn == false)
                            {
                                // 당일에 한번 샀던 종목이 있다면 패쓰~~
                                if (setting.mxTr0424.mRebuyJson[shcode] != null)
                                {
                                    continue;
                                }
                            }

                            // 증거금 100% 매입가능 금액 세팅
                            double price = iCost100;

                            // 매수 금액 지정 값 사용 여부
                            if (setting.buy_cost_yn)
                            {
                                if (price > setting.buy_cost)
                                {
                                    price = setting.buy_cost;
                                }
                            }

                            // 추정자산 1/n 금액 사용 여부
                            if (setting.buy_count_yn)
                            {
                                double price_count = Convert.ToDouble(mfTrading.TextSunamt.Text.Replace(",", "")) / setting.buy_count;

                                // 매수 금액 지정 값이 없으면 추정자산 기준 1/n 값을 세팅
                                if (price > price_count)
                                {
                                    price = price_count;
                                }
                            }

                            // 매수 가능 수량 계산
                            int quantity = (int)(price / Convert.ToInt32(close));

                            // 매매단위에 맞도록 수량 재계산
                            int memedan = Convert.ToInt32(setting.mxTr8430.mJson[shcode].GetValue());

                            quantity = (int)(Math.Truncate((double)quantity / memedan) * memedan);

                            // 매수 진행
                            if (quantity > 0)
                            {
                                iCost100 -= quantity * Convert.ToDouble(close);

                                setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), close, "2", "[매수]", hname);
                            }
                        }
                    }
                    #endregion

                    #region 매도 주문
                    else if (divide == "매도")
                    {
                        for (int iRow = 0; iRow < mfTrading.GridAccount.Rows.Count; iRow++)
                        {
                            if (shcode == mfTrading.GridAccount.Rows[iRow].Cells[0].Value.ToString())
                            {
                                string quantity = mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["수량"].GetValue())].Value.ToString();

                                if (quantity != "0")
                                {
                                    string rate = mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["수익율"].GetValue())].Value.ToString();

                                    // 종목검색 매도 옵션 사용 유무
                                    if (setting.sell_1833_yn)
                                    {
                                        if (setting.sell_fix_buffer <= Convert.ToDouble(rate))
                                        {
                                            setting.mxTrCSPAT00600.call_request(shcode, quantity, close, "1", "[매도] 종목검색[이익실현] :: " + mFileName, hname);
                                        }
                                    }
                                    else
                                    {
                                        setting.mxTrCSPAT00600.call_request(shcode, quantity, close, "1", "[매도] 종목검색[단순] :: " + mFileName, hname);
                                    }
                                }

                                break;
                            }
                        }
                    }
                    #endregion
                }	// end for
            }
        }
	}	// end class
}	// end namespace
