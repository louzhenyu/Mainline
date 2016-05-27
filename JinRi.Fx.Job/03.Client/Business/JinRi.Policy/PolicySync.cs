using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Policy
{
    /// <summary>
    /// 政策同步业务类
    /// </summary>
    public class PolicySync
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PolicySync));
        /// <summary>
        /// 政策同步业务方法
        /// </summary>
        public void PolicySyncProcess()
        {
            logger.Info(string.Format("[{0}] 政策同步处理中...", DateTime.Now.ToString("HH:mm:ss")));
            //政策同步业务逻辑处理
            System.Threading.Thread.Sleep(3000);
        }
    }
}
