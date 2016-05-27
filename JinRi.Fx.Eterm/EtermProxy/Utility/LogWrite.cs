using System;
using System.Text;
using System.IO;
using System.Threading;

namespace EtermProxy.Utility
{
    public class LogWrite
    {
        private static Mutex mutex = new Mutex();
        private static string LogPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\logs\\";

        /// <summary>
        /// 写错误日志方法
        /// </summary>
        /// <param name="error"></param>
        public static void WriteLog(Exception error)
        {
            string log = string.Format("Source:{0}\r\nStackTrace:{1}\r\nTargetSite:{2}\r\nMessage:{3}\r\n", error.Source, error.StackTrace, error.TargetSite, error.Message);
            WriteLog(log);
        }

        /// <summary>
        /// 写日志方法
        /// </summary>
        /// <param name="Context"></param>
        public static void WriteLog(string Context)
        {
            try
            {
                string FileName = string.Format("EP{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }

                mutex.WaitOne();

                FileStream fs = new FileStream(LogPath + FileName, FileMode.Append, FileAccess.Write, FileShare.Write);

                StreamWriter writer = new StreamWriter(fs, Encoding.Default);

                writer.Write(string.Format("{0}\r\n{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"), Context));

                writer.Close();
                fs.Close();

                mutex.ReleaseMutex();

            }
            catch { }
        }
    }
}
