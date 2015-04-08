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
	public class xing_tr_0424 : _IXAQueryEvents
	{
		/// <summary>xing component</summary>
		private IXAQuery mTr;

		/// <summary>Trading 폼 참조</summary>
		public FormTrading mfTrading;

		/// <summary>매도시 활용 {"종목코드":"고가|최고수익율"}</summary>
		public JsonObjectCollection mJson;

        /// <summary>당일 같은 종목 재매수 세팅시 활용 {"종목코드"}</summary>
        public JsonObjectCollection mRebuyJson;

		/// <summary>현재 TR이 실행중인지 여부</summary>
		private bool mStateRun = false;

		/// <summary>현재 TR이 실행중일 동안 카운트 수</summary>
		private int mStateRunCount = 0;

		/// <summary>당일 계좌잔고 수익율</summary>
		public double mAccountRate = 0;		

        /// <summary>
        /// 생성자 - 주식잔고2
        /// </summary>
		public xing_tr_0424()
		{
			IConnectionPoint icp;
			IConnectionPointContainer icpc;

			int iCookie = 0;
			mTr = new XAQuery();
			mTr.ResFileName = "\\res\\t0424.res";
			icpc = (IConnectionPointContainer)mTr;
			Guid IID_QueryEvents = typeof(_IXAQueryEvents).GUID;
			icpc.FindConnectionPoint(ref IID_QueryEvents, out icp);
			icp.Advise(this, out iCookie);

			#region 매도를 위한 JSON 로딩

			string date_saved = Properties.Settings.Default.T0424_DATE;
			string date_now = util_datetime.GetFormatNow("yyyyMMdd");

			// 오늘 프로그램이 실행된적이 있다면...
			if (date_now == date_saved)
			{
				// 설정파일에 저장된 값을 로딩				
				try
				{
					mJson = (JsonObjectCollection)new JsonTextParser().Parse(Properties.Settings.Default.T0424_JSON);
                    mRebuyJson = (JsonObjectCollection)new JsonTextParser().Parse(Properties.Settings.Default.REBUY_JSON);
				}
				// 설정 파일에 잘못된 json 값이 들어가 있을 경우 예외 처리
				catch (Exception ex)
				{
					// json 초기화
					mJson = new JsonObjectCollection();
                    mRebuyJson = new JsonObjectCollection();

					Log.WriteLine("t0424 json 초기화 :: " + ex.Message);
				}
			}
			// 오늘 처음 실행되는 것이라면..
			else
			{
				// json 초기화
				mJson = new JsonObjectCollection();
                mRebuyJson = new JsonObjectCollection();

				// 설정파일에 금일 날짜값 저장
				Properties.Settings.Default.T0424_DATE = date_now;
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
				string t8407_shcode = "";						// 8407 멀티현재가 파라미터로 던질 종목코드 
				int t8407_count = 0;							// 8407 멀티현재가 파라미터로 던지는 종목 수

				// 계좌정보 써머리
				string mamt = mTr.GetFieldData("t0424OutBlock", "mamt", 0);
				if (mamt != "")
				{
					double sunamt = Convert.ToDouble(mTr.GetFieldData("t0424OutBlock", "sunamt", 0));		// 추정자산
					double dtsunik = Convert.ToDouble(mTr.GetFieldData("t0424OutBlock", "dtsunik", 0));		// 실현손익
					double tdtsunik = Convert.ToDouble(mTr.GetFieldData("t0424OutBlock", "tdtsunik", 0));	// 평가손익

					mAccountRate = (dtsunik + tdtsunik) / (sunamt - dtsunik - tdtsunik) * 100;

					mfTrading.TextSunamt.Text = Util.GetNumberFormat(sunamt);												// 추정순자산
					mfTrading.TextDtsunik.Text = Util.GetNumberFormat(dtsunik);												// 실현손익
					mfTrading.TextMamt.Text = Util.GetNumberFormat(mTr.GetFieldData("t0424OutBlock", "mamt", 0));			// 매입금액
					mfTrading.TextSunamt1.Text = Util.GetNumberFormat(mTr.GetFieldData("t0424OutBlock", "sunamt1", 0));		// 추정D2예수금
					mfTrading.TextTappamt.Text = Util.GetNumberFormat(mTr.GetFieldData("t0424OutBlock", "tappamt", 0));		// 평가금액
					mfTrading.TextTdtsunik.Text = Util.GetNumberFormat(tdtsunik);											// 평가손익
					mfTrading.TextRate.Text = Util.GetNumberFormat2(mAccountRate);											// 수익율
				}

				// 잔고 리스트 종목 수
				int count_0424 = mTr.GetBlockCount("t0424OutBlock1");

				if (count_0424 > 0)
				{
					// 잔고 그리드 초기화
					for (int iRow = 0; iRow < mfTrading.GridAccount.Rows.Count; iRow++)
					{
						// 비교용으로 사용할 row header 값 초기화
						mfTrading.GridAccount.Rows[iRow].HeaderCell.Value = "";

						// 배경색 초기화
						mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.BackColor = System.Drawing.Color.White;
					}
				}

				// 종목별 잔고내역 세팅 및 옵션에 따른 매도 주문
                for (int i = 0; i < count_0424; i++)
                {
					
                    // 잔고정보 가져오기
                    string shcode = mTr.GetFieldData("t0424OutBlock1", "expcode", i);						// 종목코드
					string hname = mTr.GetFieldData("t0424OutBlock1", "hname", i);							// 종목명
                    double price = Double.Parse(mTr.GetFieldData("t0424OutBlock1", "price", i));			// 현재가
                    double quantity = Double.Parse(mTr.GetFieldData("t0424OutBlock1", "mdposqt", i));		// 매도가능 수량
                    double rate = Double.Parse(mTr.GetFieldData("t0424OutBlock1", "sunikrt", i));			// 수익율
					string appamt = mTr.GetFieldData("t0424OutBlock1", "appamt", i);						// 평가금액
					string dtsunik = mTr.GetFieldData("t0424OutBlock1", "dtsunik", i);						// 평가손익
                    string pamt = mTr.GetFieldData("t0424OutBlock1", "pamt", i);						    // 평균단가

					#region 8407 호출을 위한 데이터 수집

					// 8407 json에 저장된 종목정보 가져 옴
					JsonObject obj8407 = setting.mxTr8407.mJson[shcode];

					// 8407 json에 종목 정보가 없으면.. 멀티현재가 검색을 위한 파라미터값 추가
					if (obj8407 == null)
					{
						// 멀티현재가 검색시 최대 50건 제약이 있음
						if (t8407_count < 50)
						{
							t8407_shcode += shcode;
							t8407_count++;
						}
					}
					// 8407 json에 종목정보가 있다면
					else
					{
						// 멀티현재가에서 최종 변경된 시간
						DateTime dtTime = Convert.ToDateTime(obj8407.GetValue().ToString());		

						// 2초 이상 지났으면 재검색을 위해 종목추가
						if (DateTime.Now.Subtract(dtTime).Seconds >= 2)
						{
							// 멀티현재가 검색시 최대 50건 제약이 있음
							if (t8407_count < 50)
							{
								t8407_shcode += shcode;
								t8407_count++;
							}
						}
					}

					#endregion

					#region 0424 json 데이터 처리

					double json_price_high = 0;
					double json_rate_high = 0;

					// json에 저장된 잔고 종목 정보 가져 옴
					JsonObject json0424 = mJson[shcode];

					// json에 잔고 종목 정보가 없으면 추가
					if (json0424 == null)
					{
						json_price_high = price;
						json_rate_high = rate;

						mJson.Add(new JsonStringValue(shcode, price.ToString() + "|" + rate.ToString() + "|" + quantity.ToString()));
					}
					// 있으면 변경
					else
					{
						string[] arr0424 = json0424.GetValue().ToString().Split('|');

						json_price_high = Convert.ToDouble(arr0424[0]);
						json_rate_high = Convert.ToDouble(arr0424[1]);

						// json 저장된 고가 < 현재가
						if (json_price_high < price)
						{
							// json 에 현재가 정보를 저장
							json_price_high = price;
							json_rate_high = rate;

							((JsonStringValue)json0424).Value = price.ToString() + "|" + rate.ToString() + "|" + quantity.ToString();
						}
					}

					#endregion

                    #region 0424 당일 재매수 종목 json 데이터 처리
                    // json에 저장된 잔고 종목 정보 가져 옴
                    JsonObject jsonRebuy0424 = mRebuyJson[shcode];

                    // json에 잔고 종목 정보가 없으면 추가
                    if (jsonRebuy0424 == null)
                    {
                        mRebuyJson.Add(new JsonStringValue(shcode));
                    }
                    #endregion

                    // 잔고정보 그리드에 종목정보가 이미 있다면 현재값 기준으로 수정
					bool flagModify = false;
					
					int iRowGrid = mfTrading.GridAccount.Rows.Count;

					for (int iRow = 0; iRow < iRowGrid; iRow++)
					{
						// 그리드에서 해당 종목을 찾아서..
						if (shcode == mfTrading.GridAccount.Rows[iRow].Cells[0].Value.ToString())
						{
							// 상한가
							double uplmt_grid = Convert.ToDouble(mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["상한가"].GetValue())].Value);

							// 평가금액
							double appamt_grid = Convert.ToDouble(mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["평가금액"].GetValue())].Value);

							#region 매도주문 

							// 매도가능 수량이 있다면 설정값에 따른 매도 진행
							if (quantity > 0)
							{
								// 일괄매도 체크되어 있으면 보유종목 전량 매도
								if (mfTrading.CheckSellAll.Checked)
								{
									setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 일괄 :: " + rate.ToString(), hname);
								}

								// 보유종목 당일 청산
								if (setting.sell_today_yn)
								{
									// 장중
									if (setting.mxRealJif.mjstatus == "21" && setting.mxTr0167.mTimeCur >= 144900)
									{
										setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 당일청산 :: " + rate.ToString(), hname);
									}
									// 장 마감 동시호가
									else if (setting.mxRealJif.mjstatus == "42")
									{
										double dnlmt_grid = Convert.ToDouble(mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["하한가"].GetValue())].Value);
										setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), dnlmt_grid.ToString(), "1", "[매도] 당일청산 :: " + rate.ToString(), hname);
									}
								}

								// 손실제한 사용
								if (setting.sell_min_yn)
								{
									// 수익율이 손실제한 설정값보다 작다면 매도
									if (rate < -setting.sell_min_rate)
									{
										setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 손절 :: " + rate.ToString(), hname);
									}
								}

								// 목표 수익율 달성시 매도 사용
								if (setting.sell_max_yn)
								{
									// 최고 수익율이 목표 수익율을 넘겼다면..
									if (json_rate_high > setting.sell_max_rate)
									{
										// 목표 수익율 도달 후 버퍼값 만큼 하락시 매도
										if ((rate + 100) < (json_rate_high + 100 - setting.sell_max_buffer))
										{
											setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 목표 :: " + rate.ToString(), hname);
										}
									}
								}

								// 고정진폭 매도
								if (setting.sell_fix_yn)
								{
									// 현재 수익율 < 최고 수익율 - 버퍼
									if ((rate + 100) < (json_rate_high + 100 - setting.sell_fix_buffer))
									{
										setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 고정 :: " + rate.ToString(), hname);
									}
								}

								// 절반 매도 사용
								if (setting.sell_half_yn)
								{
									// 최고 수익율이 목표 수익율을 넘겼다면..
									if (json_rate_high > setting.sell_half)
									{
										// 최고 수익율 대비 절반만큼 하락시 매도
										if (rate <= (json_rate_high - (json_rate_high / 2)))
										{
											setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 절반 :: " + rate.ToString(), hname);
										}
									}
								}

								// 상한가 이탈 종목 잡기
								if (setting.sell_uplmt_yn)
								{
									// 고가 == 상한가 찍었을 경우
									if (json_price_high == uplmt_grid)
									{
										// 지정한 버퍼만큼 하락하면 매도
										if ((rate + 100) < (json_rate_high + 100 - setting.sell_uplmt_buffer))
										{
											setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 상한가 이탈 :: " + rate.ToString(), hname);
										}
									}
								}

								// 시가 기준 매도
								if (setting.sell_open_yn)
								{
									double open_grid = Convert.ToDouble(mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["시가"].GetValue())].Value);
									double dnlmt_grid = Convert.ToDouble(mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["하한가"].GetValue())].Value);

									// 시가 == 하한가 경우는 무조건 매도
									if (open_grid == dnlmt_grid)
									{
										setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 시가 == 하한가 :: " + rate.ToString(), hname);
									}
									// 시가 기준 버퍼값 하락시 매도
									else
									{
										if (price < Util.GetPricePercent(open_grid, -setting.sell_open_buffer))
										{
											setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.ToString(), "1", "[매도] 현재가 < 당일시가 :: " + rate.ToString(), hname);
										}
									}
								}
							}

							#endregion

							#region 그리드에 잔고 변경 정보 반영

							// 수정될 종목이 있다고 flag 값 변경
							flagModify = true;

							// 루프 종료 후 삭제시 참고하기 위해 현재 정보를 row header 값에 세팅
							mfTrading.GridAccount.Rows[iRow].HeaderCell.Value = "U";

							// 그리드의 값 변경
							mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["수량"].GetValue())].Value = Util.GetNumberFormat(quantity);
							mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["현재가"].GetValue())].Value = Util.GetNumberFormat(price);
							mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["평가금액"].GetValue())].Value = Util.GetNumberFormat(appamt);
							mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["평가손익"].GetValue())].Value = Util.GetNumberFormat(dtsunik);
							mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["수익율"].GetValue())].Value = Util.GetNumberFormat2(rate);
                            mfTrading.GridAccount.Rows[iRow].Cells[Convert.ToInt32(mfTrading.mhAccount["평균단가"].GetValue())].Value = Util.GetNumberFormat(pamt);

							// 고가 == 상한가
							if (json_price_high == uplmt_grid)
							{
								mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.Font = new System.Drawing.Font("굴림", 9, System.Drawing.FontStyle.Italic);

								// 현재가 == 상한가
								if (price == uplmt_grid)
								{
									mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
									mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.BackColor = System.Drawing.Color.IndianRed;
								}
								// 상한가 이탈
								else
								{
									// 평가손익 대비 row 색상 지정
									if (Convert.ToDouble(dtsunik) > 0)
									{
										mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
									}
									else
									{
										mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
									}
								}
							}
							else
							{
								// 평가금액 변동이 있다면..
								if (Convert.ToDouble(appamt) != appamt_grid)
								{
									// 평가손익 대비 row 색상 지정
									if (Convert.ToDouble(dtsunik) > 0)
									{
										mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
									}
									else
									{
										mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
									}

									// 변경된 항목 확인을 위해 배경색 지정
									mfTrading.GridAccount.Rows[iRow].DefaultCellStyle.BackColor = System.Drawing.Color.Wheat;
								}	// end if
							}
							
							// 그리드 루프 빠져 나감
							break;

							#endregion
						}	// end if
					}	// end for

					#region 신규 종목이면 그리드에 추가

					if (flagModify == false)
					{
						string[] row = new string[mfTrading.GridAccount.ColumnCount];

						for (int j = 0; j < row.Length; j++)
						{
							row[j] = "0";
						}

						row[Convert.ToInt32(mfTrading.mhAccount["종목코드"].GetValue())] = shcode;								
						row[Convert.ToInt32(mfTrading.mhAccount["종목명"].GetValue())] = hname;									
						row[Convert.ToInt32(mfTrading.mhAccount["수량"].GetValue())] = Util.GetNumberFormat(quantity);		
						row[Convert.ToInt32(mfTrading.mhAccount["현재가"].GetValue())] = Util.GetNumberFormat(price);		
						row[Convert.ToInt32(mfTrading.mhAccount["평가금액"].GetValue())] = Util.GetNumberFormat(appamt);	
						row[Convert.ToInt32(mfTrading.mhAccount["평가손익"].GetValue())] = Util.GetNumberFormat(dtsunik);
						row[Convert.ToInt32(mfTrading.mhAccount["수익율"].GetValue())] = Util.GetNumberFormat2(rate);
                        row[Convert.ToInt32(mfTrading.mhAccount["평균단가"].GetValue())] = Util.GetNumberFormat(pamt);

						mfTrading.GridAccount.Rows.Add(row);

						int iRowLast = mfTrading.GridAccount.Rows.Count-1;

						// 평가손익 대비 row 색상 지정
						if (Convert.ToDouble(dtsunik) > 0)
						{
							mfTrading.GridAccount.Rows[iRowLast].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
						}
						else
						{
							mfTrading.GridAccount.Rows[iRowLast].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
						}

						// 추가된 종목 확인을 위해 배경색 지정
						mfTrading.GridAccount.Rows[iRowLast].DefaultCellStyle.BackColor = System.Drawing.Color.Wheat;

						// 루프 종료 후 삭제시 참고하기 위해 현재 정보를 row header 값에 세팅
						mfTrading.GridAccount.Rows[iRowLast].HeaderCell.Value = "I";
					}

					#endregion

				}	// end for

				#region 목록에 없는 종목 그리드에서 제거

				// 서버에서 응답 결과가 있거나 매입 금액이 없는 경우
				if (count_0424 > 0 || mamt == "0")
				{
					for (int iRow = mfTrading.GridAccount.Rows.Count - 1; iRow >= 0; iRow--)
					{
						bool flag = false;

						if (mamt == "0")
						{
							flag = true;
						}
						else
						{ 
							// 종목 추가 또는 변경이 없었던 종목들
							if (mfTrading.GridAccount.Rows[iRow].HeaderCell.Value.ToString() == "")
							{
								flag = true;
							}
						}

						if (flag)
						{
							// 0424 json 삭제
							JsonObject obj = mJson[mfTrading.GridAccount.Rows[iRow].Cells[0].Value.ToString()];

							if (obj != null)
							{
								mJson.Remove(obj);
							}

							// 그리드에서 삭제
							mfTrading.GridAccount.Rows.Remove(mfTrading.GridAccount.Rows[iRow]);
						}
					}
				}

				#endregion


				// 멀티현재가 검색할 종목이 있다면 호출
				// 현재가 < 당일시가, 상한가 이탈 매도시 사용을 위해 검색
				if (t8407_count > 0)
				{
					setting.mxTr8407.call_request(t8407_count.ToString(), t8407_shcode);
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
				// 03669 :: 비밀번호 오류입니다. (5회중 4회 남았습니다)
				else if (nMessageCode == "03669")
				{
					Log.WriteLine("t0424 :: " + nMessageCode + " :: " + szMessage);

					mfTrading.fnAutoTrading(false);
				}
				else
				{
					//Log.WriteLine("t0424 :: " + nMessageCode + " :: " + szMessage);
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
		/// 주식 잔고 정보 요청 호출
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

				mTr.SetFieldData("t0424InBlock", "accno", 0, setting.login_account);		// 계좌번호
				mTr.SetFieldData("t0424InBlock", "passwd", 0, setting.login_account_pw);	// 비밀번호
				mTr.SetFieldData("t0424InBlock", "prcgb", 0, "1");							// 단가구분 : 1-평균단가, 2:BEP단가
				mTr.SetFieldData("t0424InBlock", "chegb", 0, "2");							// 체결구분 : 0-결제기준, 2-체결기준
				mTr.SetFieldData("t0424InBlock", "dangb", 0, "0");							// 단일가구분 : 0-정규장, 1-시간외단일가 
				mTr.SetFieldData("t0424InBlock", "charge", 0, "1");							// 제비용포함여부 : 0-미포함, 1-포함
				mTr.SetFieldData("t0424InBlock", "cts_expcode", 0, "");						// CTS종목번호 : 처음 조회시는 SPACE

				mTr.Request(false);

//				Log.WriteLine("t0424 call_request()");
			}
		}	// end function
	}	// end class
}	// end namespace
