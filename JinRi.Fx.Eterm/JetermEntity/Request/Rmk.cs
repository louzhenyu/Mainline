using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 授权请求对象
    /// </summary>
    [Serializable]
    public class Rmk
    {
        /// <summary>
        /// 记录编码
        /// </summary>
        public string Pnr { get; set; }

        /// <summary>
        /// 需授权OFFICE号
        /// </summary>
        public List<string> RmkOfficeNoList { get; set; }

        public Rmk()
        {
            RmkOfficeNoList = new List<string>();
        }
    }
}
