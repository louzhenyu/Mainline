using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JinRi.Job.HttpScheduler.Utils
{
    public class ConfigManager
    {
        private static string useLocalSwitchConfig;
        /// <summary>
        /// 是否使用本地开关(True：表示强制指定本应用为Master 需手动停止其他Slave  False：表示使用Zookeeper Master选举，)
        /// </summary>
        public static bool UseLocalSwitch
        {
            get
            {
                if (useLocalSwitchConfig == null)
                {
                    useLocalSwitchConfig = ConfigurationManager.AppSettings["UseLocalSwitch"] == null ? "" : ConfigurationManager.AppSettings["UseLocalSwitch"].ToUpper();
                }
                return useLocalSwitchConfig == "TRUE";
            }
        }

        private static long refreshTime = -1;
        /// <summary>
        /// Job更新频率，单位：毫秒，默认60秒
        /// </summary>
        public static long RefreshTime
        {
            get
            {
                if (refreshTime < 0)
                {
                    long configTime = 0;
                    //Job更新频率不得小于1分钟
                    if (!long.TryParse(ConfigurationManager.AppSettings["RefreshTimes"], out configTime) || configTime < 60)
                    {
                        configTime = 60;
                    }
                    refreshTime = configTime * 1000;//秒->毫秒
                }
                return ConfigManager.refreshTime;
            }
        }
    }
}
