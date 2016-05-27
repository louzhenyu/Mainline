using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.CTI.Entity.DTO
{
    /// <summary>
    /// EmailDTO
    /// 用做RabbitMQ发送端到接收端的通信契约
    /// </summary>
    public class EmailDTO
    {
        /// <summary>
        /// Email编号
        /// 可否为null:否
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用编号
        /// 可否为null:否
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 收件人Email地址
        /// 可否为null:否
        /// </summary>
        public string ToAddr { get; set; }

        /// <summary>
        /// 抄送地址
        /// 可否为null:是
        /// 多个地址用","分隔
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// 标题
        /// 可否为null:否
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// 可否为null:否
        /// </summary>
        public string Content { get; set; }
    }
}
