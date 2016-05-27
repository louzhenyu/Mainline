using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.CTI.Entity.DTO
{
    /// <summary>
    /// SMSDTO
    /// 用做RabbitMQ发送端到接收端的通信契约
    /// </summary>
    public class SMSDTO
    {
        /// <summary>
        /// SMS编号
        /// 可否为null:否
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用编号
        /// 可否为null:否
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 手机号
        /// 可否为null:否
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 短信内容
        /// 可否为null:否
        /// </summary>
        public string Content { get; set; }
    }
}
