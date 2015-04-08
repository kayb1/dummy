using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xing
{
    class Log
    {
		/// <summary>파일 스트림</summary>
        private static FileStream mStream;

		/// <summary>파일 스트림(쓰기용)</summary>
        private static StreamWriter mStreamWriter;

		/// <summary>로그 파일명(경로 포함)</summary>
		public static string mLogFile;

        /// <summary>
        /// Log 파일 생성
        /// </summary>
        /// <returns>StreamWriter</returns>
        private static StreamWriter GetStreamWriter()
        {
		    if (mStreamWriter == null)
            {
				string szDirLog = "log_files";
				string szPath = setting.program_execute_dir + @"\" + szDirLog;
				
				DirectoryInfo dir = new DirectoryInfo(szPath);

				// 로그 폴더가 없으면 생성해 주공
				if (dir.Exists == false)
				{
					dir.Create();
				}
				// 로그 폴더가 있으면 과거 로그 파일 삭제 - 파일 생성 후 2일 동안 보관
				else
				{
					foreach (FileInfo f in dir.GetFiles())
					{
						if (f.CreationTime < DateTime.Now.AddDays(-2))
						{
							f.Delete();
						}
					}
				}

                mLogFile = szPath + "\\" + util_datetime.GetFormatNow("yyyyMMdd_HHmmss") + ".txt";

				// 기존에 생성된 로그 파일이 있다면... 로그 내용 추가
				FileInfo file = new FileInfo(mLogFile);
				if (file.Exists)
				{
					mStream = new FileStream(mLogFile, FileMode.Append);
				}
				// 신규로 파일 생성
				else
				{
					mStream = new FileStream(mLogFile, FileMode.Create);
				}

				mStreamWriter = new StreamWriter(mStream, Encoding.GetEncoding("euc-kr"));
				mStreamWriter.AutoFlush = true;
            }

            return mStreamWriter;
        }   // end function


        /// <summary>
        /// Log 파일에 메세지 기록
        /// </summary>
        /// <param name="szMsg"></param>
        public static void WriteLine(string szMsg)
        {
			GetStreamWriter().WriteLine(util_datetime.GetFormatNow("HH:mm:ss.fff") + " :: " + szMsg);

			LogGrid.AddRowDebug(szMsg);
        }   // end function


        /// <summary>
        /// Log 파일 쓰기 스트림 종료
        /// </summary>
        public static void Close()
        {
            if (mStreamWriter != null)
            {
                mStreamWriter.Close();
                mStream.Close();
            }
        }   // end function


    }   // end class
}   // end namespace
