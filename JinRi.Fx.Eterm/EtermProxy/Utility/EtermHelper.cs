using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.International.Converters.PinYinConverter;
using JetermEntity;

namespace EtermProxy.Utility
{
    public class EtermHelper
    {        
        public EtermHelper()
        {
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int PostMessage(IntPtr hwnd, int wMsg, uint wParam, IntPtr lParam);
        const int WM_MSG = 0x505;

        [DllImport("Kernel32.dll")]
        private static extern uint WaitForSingleObject(System.IntPtr hHandle, uint dwMilliseconds);
        [DllImport("Kernel32.dll")]
        private static extern uint ResetEvent(System.IntPtr hHandle);

        /// <summary>
        /// Eterm数据
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, string> EtermData = new Dictionary<string, string>();

        /// <summary>
        /// 发送Eterm指令
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hHandel"></param>
        /// <param name="config">配置名称</param>
        /// <param name="cmd">命令字符串</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public string system(IntPtr hwnd, IntPtr hHandel, string config, string cmd, uint timeout = 30 * 1000)
        {            
            string sret = string.Empty;
            
            string guid = Guid.NewGuid().ToString("N");

            PostMessage(hwnd, WM_MSG, timeout, Marshal.StringToCoTaskMemUni(guid + cmd));
            ResetEvent(hHandel);
            uint nret = WaitForSingleObject(hHandel, timeout);

            string key = config + guid + cmd;
            int nto = 0;
            do
            {
                if (EtermData.Keys.Contains(key))
                {
                    sret = EtermData[key];
                    EtermData.Remove(key);
                    break;
                }
                System.Threading.Thread.Sleep(10);
                nto++;
            } while (nto < timeout);

            LogWrite.WriteLog(string.Format("config={0} guid={1} cmd={2}\r\nnret={3} sret={4}", config, guid, cmd, nret, sret));

            return sret;
           
        }

        /// <summary>
        /// 获取方法名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getmethod(string url)
        {
            Regex reg = new Regex("method=.*?(&|$)");
            Regex regp = new Regex("(method=)|(&)");
            Match match = reg.Match(url);
            return (match.Success) ? regp.Replace(match.Value, "") : string.Empty;
        }

        /// <summary>
        /// 获取参数集合
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string> getparams(string url)
        {
            List<string> list = new List<string>();
            Regex reg = new Regex("param\\d+=.*?(&|$)");
            Regex regp = new Regex("(param\\d+=)|(&)");
            MatchCollection coll = reg.Matches(url);
            foreach (Match match in coll)
            {
                list.Add(regp.Replace(match.Value, ""));
            }
            return list;
        }
    }
}
