using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// Job运行日志
    /// <para>2015-12-10</para>
    /// </summary>
    public class JobExecuteLog
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID{ get;set; }
        /// <summary>
        /// 任务编号
        /// </summary>
        public int JobId{ get;set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecuteTime{ get;set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public int Success{ get;set; }
    }
}
