using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net.Json;

namespace xing.cs.form
{
	public partial class FormTrading : Form
	{
		/// <summary>메인 폼 참조시 사용</summary>
		public FormMain mfMain;

        /// <summary>매수할 종목 그리드 헤더 json</summary>
        public JsonObjectCollection mhBuyList;

		/// <summary>주식잔고 그리드 헤더 json</summary>
		public JsonObjectCollection mhAccount;


		public FormTrading()
		{
			InitializeComponent();

			// 그리드 초기화
			fnInitGridTitle();

			#region 폼 위치/사이즈 복원

			if (Properties.Settings.Default.FORM_TRADING_LEFT >= 0)
			{
				this.Left = Properties.Settings.Default.FORM_TRADING_LEFT;
			}
			else
			{
				this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width - 150;
			}

			if (Properties.Settings.Default.FORM_TRADING_TOP >= 0)
			{
				this.Top = Properties.Settings.Default.FORM_TRADING_TOP;
			}
			else
			{
				this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height - 29;
			}

			if (Properties.Settings.Default.FORM_TRADING_WIDTH >= 0 && Properties.Settings.Default.FORM_TRADING_HEIGHT >= 0)
			{
				this.Size = new System.Drawing.Size(
						Properties.Settings.Default.FORM_TRADING_WIDTH,
						Properties.Settings.Default.FORM_TRADING_HEIGHT
					);
			}

			#endregion

			
		}

		
		/// <summary>
		/// 그리드 초기화 - 타이틀 세팅 
		/// </summary>
		private void fnInitGridTitle()
		{
            // 매수할 종목 그리드
            string[] aBuyListTitle = { "종목코드", "종목명", "현재가", "등락율", "검색가", "검색등락율" };

            mhBuyList = new JsonObjectCollection();
            GridBuy.ColumnCount = aBuyListTitle.Length;

            for (int i = 0; i < aBuyListTitle.Length; i++)
            {
                mhBuyList.Add(new JsonNumericValue(aBuyListTitle[i], i));

                GridBuy.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                GridBuy.Columns[i].Name = aBuyListTitle[i];
                GridBuy.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                GridBuy.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (aBuyListTitle[i] == "종목코드" || aBuyListTitle[i] == "종목명")
                {
                    GridBuy.Columns[i].Frozen = true;
                }
            }

			// 미체결(매수) 그리드
			GridOrderBuy.ColumnCount = 3;
            GridOrderBuy.Columns[0].Name = "종목명";
            GridOrderBuy.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			GridOrderBuy.Columns[1].Name = "수량";
            GridOrderBuy.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			GridOrderBuy.Columns[2].Name = "주문가";
            GridOrderBuy.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			// 미체결(매도) 그리드
			GridOrderSell.ColumnCount = 3;
            GridOrderSell.Columns[0].Name = "종목명";
            GridOrderSell.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			GridOrderSell.Columns[1].Name = "수량";
            GridOrderSell.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            GridOrderSell.Columns[2].Name = "주문가";
            GridOrderSell.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			// 실시간 잔고 그리드
            string[] aAccountTitle = { "종목코드", "종목명", "수량", "현재가", "등락율", "평균단가", "수익율", "평가손익", "체결강도", "거래량", "거래대금", "평가금액", "시가", "고가", "저가", "전일종가", "상한가", "하한가" };

			mhAccount = new JsonObjectCollection();
			GridAccount.ColumnCount = aAccountTitle.Length;

			for (int i = 0; i < aAccountTitle.Length; i++)
			{
				mhAccount.Add(new JsonNumericValue(aAccountTitle[i], i));

				GridAccount.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

				GridAccount.Columns[i].Name = aAccountTitle[i];
				GridAccount.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				GridAccount.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

				if (aAccountTitle[i] == "종목코드" || aAccountTitle[i] == "종목명")
				{
					GridAccount.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					GridAccount.Columns[i].Frozen = true;
				}
				else
				{
					GridAccount.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

					if (aAccountTitle[i] == "수익율")
					{
						GridAccount.Columns[i].DefaultCellStyle.Font = new System.Drawing.Font(GridAccount.Font, System.Drawing.FontStyle.Bold);
					}
				}
			}

			GridAccount.SelectionMode = DataGridViewSelectionMode.FullRowSelect;		// 행단위 선택
		}

		

		/// <summary>
		/// 자동 매매거래 시작/중지 지정
		/// </summary>
		/// <param name="flag"></param>
		public void fnAutoTrading(bool flag)
		{
			if (flag)
			{
				Log.WriteLine("자동거래 시작..!!");

				mfMain.TimerBuyRun.Start();

				ButtonAutoBuyStart.Enabled = false;
				ButtonAutoBuyStop.Enabled = true;
			}
			else
			{
				Log.WriteLine("자동거래 중지..!!");

				mfMain.TimerBuyRun.Stop();
                
				ButtonAutoBuyStart.Enabled = true;
				ButtonAutoBuyStop.Enabled = false;
			}
		}

		
		/// <summary>
		/// 잔고정보 그리드 셀 클릭시 이벤트 처리
		/// HTS 프로그램과 종목 연동
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GridAccountList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				int iRow = e.RowIndex;

				if (iRow >= 0)
				{
					string shcode = GridAccount.Rows[iRow].Cells[Convert.ToInt32(mhAccount["종목코드"].GetValue())].Value.ToString();

					setting.mxTr0167.call_api_2_hts(shcode);
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 잔고정보 그리드 셀 더블 클릭시 이벤트 처리
		/// 선택된 종목 매도 주문
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GridAccountList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				int iRow = e.RowIndex;

				if (iRow >= 0)
				{
					DataGridViewRow row = GridAccount.Rows[iRow];

					string shcode = row.Cells[Convert.ToInt32(mhAccount["종목코드"].GetValue())].Value.ToString();
					string hname = row.Cells[Convert.ToInt32(mhAccount["종목명"].GetValue())].Value.ToString();
					string quantity = row.Cells[Convert.ToInt32(mhAccount["수량"].GetValue())].Value.ToString();
					string price = row.Cells[Convert.ToInt32(mhAccount["현재가"].GetValue())].Value.ToString();
					string account_rate = row.Cells[Convert.ToInt32(mhAccount["수익율"].GetValue())].Value.ToString();

					// 매도가능 수량이 있을 경우
					if (quantity != "0")
					{
						string msg = "";
						msg += hname + "[" + shcode + "]\n\n";
						msg += "수   량 :: " + quantity + "\n";
						msg += "가   격 :: " + price + "\n";
						msg += "수익율 :: " + account_rate + "\n\n";
						msg += "매도 하시겠습니까..!?";

						if (MessageBox.Show(msg, "매도주문", MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							setting.mxTrCSPAT00600.call_request(shcode, quantity.Replace(",", ""), price.Replace(",", ""), "1", "[매도] 직접", hname);
						}
					}
					else
					{
						MessageBox.Show("매도가능 수량이 없습니다..!!");
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 자동거래 중지 버튼 클릭 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonAutoBuyStop_Click_1(object sender, EventArgs e)
		{
			try
			{
				fnAutoTrading(false);
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 자동거래 시작 버튼 클릭 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonAutoBuyStart_Click_1(object sender, EventArgs e)
		{
			try
			{
				fnAutoTrading(true);
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

        /// <summary>
        /// 조건검색종목 그리드 셀 클릭시 이벤트 처리
        /// HTS 프로그램과 종목 연동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridBuyList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iRow = e.RowIndex;

                if (iRow >= 0)
                {
                    string shcode = GridBuy.Rows[iRow].Cells[0].Value.ToString();

                    setting.mxTr0167.call_api_2_hts(shcode);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 매수종목 그리드 셀 더블 클릭시 이벤트 처리
        /// 선택된 종목 매수 주문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridBuyList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iRow = e.RowIndex;

                if (iRow >= 0)
                {
                    DataGridViewRow row = GridBuy.Rows[iRow];

                    string shcode = row.Cells[Convert.ToInt32(mhBuyList["종목코드"].GetValue())].Value.ToString();
                    string hname = row.Cells[Convert.ToInt32(mhBuyList["종목명"].GetValue())].Value.ToString();
                    string price = row.Cells[Convert.ToInt32(mhBuyList["현재가"].GetValue())].Value.ToString();

                    //TODO : 잔고있을경우
                    // 매수가능 수량이 있을 경우
                    
                    // 중복 매수 불가능한 상태거나 잔고에 종목이 있다면 패쓰~~
                    // 직접 종목을 개별 선택 매수시에는 당일 재매수 가능 여부에 영향받지 않음
                    if ((setting.buy_duplicate_yn == true || setting.mxTr0424.mJson[shcode] == null))
                    {
                        // 증거금 100% 종목 매입가능 금액
                        double iCost100 = setting.mxTrCSPAQ02200.m100;

                        // 증거금 100% 매입가능 금액 세팅
                        double buyPrice = iCost100;

                        // 매수 금액 지정 값 사용 여부
                        if (setting.buy_cost_yn)
                        {
                            if (buyPrice > setting.buy_cost)
                            {
                                buyPrice = setting.buy_cost;
                            }
                        }

                        // 추정자산 1/n 금액 사용 여부
                        if (setting.buy_count_yn)
                        {
                            double price_count = Convert.ToDouble(TextSunamt.Text.Replace(",", "")) / setting.buy_count;

                            // 매수 금액 지정 값이 없으면 추정자산 기준 1/n 값을 세팅
                            if (buyPrice > price_count)
                            {
                                buyPrice = price_count;
                            }
                        }

                        // 매수 가능 수량 계산
                        int quantity = (int)(buyPrice / Convert.ToInt32(price.Replace(",", "")));

                        // 매매단위에 맞도록 수량 재계산
                        int memedan = Convert.ToInt32(setting.mxTr8430.mJson[shcode].GetValue());

                        quantity = (int)(Math.Truncate((double)quantity / memedan) * memedan);

                        // 매수 진행
                        if (quantity > 0)
                        {
                            string msg = "";
                            msg += hname + "\n\n";
                            msg += "수   량 :: " + quantity + "\n";
                            msg += "가   격 :: " + price + "\n\n";
                            msg += "매수 하시겠습니까..!?";

                            if (MessageBox.Show(msg, "매수주문", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                iCost100 -= quantity * Convert.ToDouble(price);
                                setting.mxTrCSPAT00600.call_request(shcode, quantity.ToString(), price.Replace(",", ""), "2", "[매수] 직접", hname);
                            }
                        }
                        else
                        {
                            MessageBox.Show("매수가능 수량이 없습니다..!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                Log.WriteLine(ex.StackTrace);
            }
        }
		
	}	// end class
}	// end namespace
