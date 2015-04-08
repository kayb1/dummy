using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xing.cs.form
{
	public partial class FormLogin : Form
	{
		/// <summary>메인 폼 참조</summary>
		public FormMain mFormMain;

		public FormLogin()
		{
			InitializeComponent();

			#region 로그인 설정 값 가져오기

			// 서버 주소
			setting.login_server = Properties.Settings.Default.LOGIN_SERVER;
			ComboLoginServer.SelectedIndex = ComboLoginServer.FindStringExact(setting.login_server);

			// 아이디
			setting.login_id = Util.Decrypt(Properties.Settings.Default.LOGIN_ID);
			TextLoginID.Text = setting.login_id;

			// 비번
			setting.login_pw = Util.Decrypt(Properties.Settings.Default.LOGIN_PW);
			TextLoginPW.Text = setting.login_pw;

			// 공인인증 비번
			setting.login_public_pw = Util.Decrypt(Properties.Settings.Default.LOGIN_PUBLIC_PW);
			TextLoginPublicPW.Text = setting.login_public_pw;

			// 계좌 비번
			setting.login_account_pw = Util.Decrypt(Properties.Settings.Default.LOGIN_ACCOUNT_PW);
			TextLoginAccountPW.Text = setting.login_account_pw;

			// 자동 로그인
			setting.login_auto_yn = Properties.Settings.Default.LOGIN_AUTO_YN;
			CheckLoginAutoYN.Checked = setting.login_auto_yn;

			// 자동거래
			setting.login_trading_yn = Properties.Settings.Default.LOGIN_TRADING_YN;
			CheckLoginTradingYN.Checked = setting.login_trading_yn;

			// 트레이
			setting.login_tray_yn = Properties.Settings.Default.LOGIN_TRAY_YN;
			CheckLoginTrayYN.Checked = setting.login_tray_yn;

			#endregion
		}

		/// <summary>
		/// 종료 버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonExitProgram_Click(object sender, EventArgs e)
		{
			try
			{
				mFormMain.fnExitProgram();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);

				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 로그인 버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonLogin_Click(object sender, EventArgs e)
		{
			try
			{
				setting.mxSession.fnLogin();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);

				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// 로그인 설정 저장
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonSaveSettingLogin_Click(object sender, EventArgs e)
		{
			try
			{
				// 서버
				setting.login_server = ComboLoginServer.Text;
				Properties.Settings.Default.LOGIN_SERVER = setting.login_server;

				// 아이디
				setting.login_id = TextLoginID.Text.Trim();
				Properties.Settings.Default.LOGIN_ID = Util.Encrypt(setting.login_id);

				// 비번
				setting.login_pw = TextLoginPW.Text.Trim();
				Properties.Settings.Default.LOGIN_PW = Util.Encrypt(setting.login_pw);

				// 공인인증
				setting.login_public_pw = TextLoginPublicPW.Text.Trim();
				Properties.Settings.Default.LOGIN_PUBLIC_PW = Util.Encrypt(setting.login_public_pw);

				// 계좌비번
				setting.login_account_pw = TextLoginAccountPW.Text.Trim();
				Properties.Settings.Default.LOGIN_ACCOUNT_PW = Util.Encrypt(setting.login_account_pw);

				// 자동 로그인
				setting.login_auto_yn = CheckLoginAutoYN.Checked;
				Properties.Settings.Default.LOGIN_AUTO_YN = setting.login_auto_yn;

				// 자동거래
				setting.login_trading_yn = CheckLoginTradingYN.Checked;
				Properties.Settings.Default.LOGIN_TRADING_YN = setting.login_trading_yn;

				// 트레이
				setting.login_tray_yn = CheckLoginTrayYN.Checked;
				Properties.Settings.Default.LOGIN_TRAY_YN = setting.login_tray_yn;

				Properties.Settings.Default.Save();

				MessageBox.Show("로그인 설정 저장 완료..!!");
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.Message);
				Log.WriteLine(ex.StackTrace);
			}
		}

		
	}
}
