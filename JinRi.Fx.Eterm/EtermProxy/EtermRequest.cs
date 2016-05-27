using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtermProxy
{
    public class EtermRequest
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string Config { get; set; }
        /// <summary>
        /// OfficeNo
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
    }
}
