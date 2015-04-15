using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace xing
{
	/// <summary>환경설정 변수 값을 저장하는 클래스</summary>
	class setting
	{


		#region 로그인/세션 관련 설정

		/// <summary>서버 접속 주소</summary>
		public static string login_server;

		/// <summary>로그인 아뒤</summary>
		public static string login_id;

		/// <summary>로그인 비밀번호</summary>
		public static string login_pw;

		/// <summary>공인인증서 비밀번호</summary>
		public static string login_public_pw;

		/// <summary>계좌</summary>
		public static string login_account;

		/// <summary>계좌 비밀번호</summary>
		public static string login_account_pw;

		/// <summary>자동 로그인 유무</summary>
		public static bool login_auto_yn;

		/// <summary>자동 매매 시작 유무</summary>
		public static bool login_trading_yn;

		/// <summary>프로그램 최소화시 트레이로 이동 유무</summary>
		public static bool login_tray_yn;

		#endregion

		#region 매수관련 설정

		/// <summary>추정자산을 기준으로 1/n 여부</summary>
		public static bool buy_count_yn;

		/// <summary>추정자산을 기준으로 최대 매수 가능 금액을 1/n 값으로 계산할 값</summary>
		public static int buy_count;

		/// <summary>매수 금액 지정 여부</summary>
		public static bool buy_cost_yn;

		/// <summary>매수 금액 지정 값</summary>
		public static double buy_cost;

		/// <summary>당일 같은 종목 재매수 가능 여부</summary>
		public static bool buy_rebuy_yn;

		/// <summary>자동 정정 주문 발행 여부</summary>
		public static bool buy_re_order_yn;

		/// <summary>장마감 동시호가 진입시 미체결(매수) 종목 취소 주문 발행</summary>
		public static bool buy_order_cancel_31_yn;

		/// <summary>미수 주문 가능 여부</summary>
		public static bool buy_misu_yn;

		/// <summary>중목 매수 가능 여부</summary>
		public static bool buy_duplicate_yn;

		/// <summary>당일 목표 수익 달성시 매수 주문 안함</summary>
		public static bool buy_max_yn;

		/// <summary>당일 목표 수익 달성시 매수 주문 안함 :: 목표 수익율 값</summary>
		public static double buy_max;

		/// <summary>장 시작 후 지정시간 동안 매수 진행 여부</summary>
		public static bool buy_time_yn;

		/// <summary>장 시작 후 지정시간 동안 매수 진행할 시간 값</summary>
		public static double buy_time;

		
		#endregion 

		#region 매도관련 설정

		/// <summary>손절 매도 사용 유무</summary>
		public static bool sell_min_yn;

		/// <summary>손절할 수익율</summary>
		public static double sell_min_rate;

		/// <summary>고정 매도 사용 유무</summary>
		public static bool sell_fix_yn;

		/// <summary>고정 매도할 수익율 버퍼</summary>
		public static double sell_fix_buffer;

		/// <summary>목표달성 매도 사용 유무</summary>
		public static bool sell_max_yn;

		/// <summary>목표달성 수익율</summary>
		public static double sell_max_rate;

		/// <summary>목표달성 후 매도를 위한 버퍼</summary>
		public static double sell_max_buffer;

		/// <summary>시가 기준 매도 사용 여부</summary>
		public static bool sell_open_yn;

		/// <summary>시가 기준 매도할 버퍼 값</summary>
		public static double sell_open_buffer;

		/// <summary>상한가 기준 매도 사용 여부</summary>
		public static bool sell_uplmt_yn;

		/// <summary>상한가 기준 매도할 버퍼 값</summary>
		public static double sell_uplmt_buffer;

		/// <summary>장마감 동시호가 진입시 미체결(매도) 종목 취소 주문 발행</summary>
		public static bool sell_order_cancel_31_yn;

		/// <summary>자동 정정 주문 발행 여부</summary>
		public static bool sell_re_order_yn;

		/// <summary>당일 청산 여부</summary>
		public static bool sell_today_yn;

		/// <summary>종목검색 매도 사용 유무</summary>
		public static bool sell_1833_yn;

		/// <summary>종목검색 매도시 버퍼 값</summary>
		public static double sell_1833_buffer;

		/// <summary>절반 매도 사용 유무</summary>
		public static bool sell_half_yn;

		/// <summary>절반 매도시 목표 값</summary>
		public static double sell_half;

		#endregion

		#region 나머지 시스템 관련 설정

		/// <summary>프로그램 실행 디렉토리</summary>
		public static string program_execute_dir = "";

		/// <summary>현재 실행중인 프로그램 경로/이름 </summary>
		public static string program_full_name;

		/// <summary>API -> HTS 연동 유무</summary>
		public static bool program_api_2_hts_yn;

		/// <summary>HTS -> API 연동 유무</summary>
		public static bool program_hts_2_api_yn;

		/// <summary>PC 시간을 서버와 동기화 유무</summary>
		public static bool program_sync_time_yn;

		
		#endregion

		#region TR 관련 설정

		/// <summary>세션</summary>
		public static xing_session mxSession;

		/// <summary>장 운영 정보</summary>
		public static xing_real_jif mxRealJif;

		/// <summary>서버 시간 조회</summary>
		public static xing_tr_0167 mxTr0167;

		/// <summary>주식 멀티현재가</summary>
		public static xing_tr_8407 mxTr8407;

		/// <summary>주식 체결/미체결</summary>
		public static xing_tr_0425 mxTr0425;

		/// <summary>주식 잔고 2</summary>
		public static xing_tr_0424 mxTr0424;

		/// <summary>종목검색(씽API용)</summary>
		public static xing_tr_1833 mxTr1833;

        /// <summary>주식차트(일주월)</summary>
        public static xing_tr_8413 mxTr8413;

		/// <summary>주식종목조회</summary>
		public static xing_tr_8430 mxTr8430;

		/// <summary>현물취소주문</summary>
		public static xing_tr_CSPAT00800 mxTrCSPAT00800;

		/// <summary>현물정정주문</summary>
		public static xing_tr_CSPAT00700 mxTrCSPAT00700;

		/// <summary>현물정상주문</summary>
		public static xing_tr_CSPAT00600 mxTrCSPAT00600;

		/// <summary>현물계좌 증거금별 주문가능 수량 조회</summary>
		public static xing_tr_CSPAQ02200 mxTrCSPAQ02200;

		/// <summary>1833 종목검색을 위한 ADF 파일</summary>
		public static string t1833_files;

		/// <summary>1833 종목검색을 위한 ADF 파일이 있는 폴더 위치</summary>
		public static string t1833_dir;

		/// <summary>현물계좌 증거금별 주문가능 수량 조회</summary>
		public static xing_tr_CSPBQ00200 mxTrCSPBQ00200;

        /// <summary>주식 멀티현재가 - ocr로 얻어온 종목코드로 현재가 불러와 매수처리전용</summary>
        public static xing_tr_8407_kiwum mxTr8407_kiwum;

		#endregion

	}	// end class
}	// end namespace
