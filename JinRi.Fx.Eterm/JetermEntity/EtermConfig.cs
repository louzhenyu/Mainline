using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity
{
    /// <summary>
    /// Eterm配置模型
    /// </summary>
    public class EtermConfig
    {
        public EtermConfig()
        {
            IsSSL = false;
            AutoSI = false;
            KeepAlive = true;
            Port = 350;
            Interval = 0;
            MaxCount = 130000;
            Count = 0;
        }
        /// <summary>
        /// 是否加密连接
        /// </summary>
        public bool IsSSL { get; set; }
        /// <summary>
        /// 是否自动SI
        /// </summary>
        public bool AutoSI { get; set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public uint Port { get; set; }
        /// <summary>
        /// 指令间隔(毫秒)
        /// </summary>
        public uint Interval { get; set; }
        /// <summary>
        /// 最大使用配置流量
        /// </summary>
        public uint MaxCount { get; set; }
        /// <summary>
        /// 当前当月流量
        /// </summary>
        public uint Count { get; set; }
        /// <summary>
        /// 配置用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 配置密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// SI
        /// </summary>
        public string SI { get; set; }
        /// <summary>
        /// 保持连接（心跳包）
        /// </summary>
        public bool KeepAlive { get; set; }
    }
}
