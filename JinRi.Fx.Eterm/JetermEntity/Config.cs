using System;
using System.Collections.Generic;
using JetermEntity;

namespace JetermEntity
{
    [Serializable]
    public class Config
    {
        public Config()
        {
            ConfigLevel = 5;
            AllowAirLine = new List<string>();
            DenyAirLine = new List<string>();
            ConfigList = new List<string>();
        }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// OfficeNo
        /// </summary>
        public string OfficeNo { get; set; }

        /// <summary>
        /// 配置功能列表
        /// </summary>
        public List<EtermCommand.CmdType> cmdType { get; set; }
        
        /// <summary>
        /// Eterm状态
        /// </summary>
        public EtermCommand.ConfigState State { get; set; }

        /// <summary>
        /// 配置级别
        /// </summary>
        public int ConfigLevel { get; set; }

        /// <summary>
        /// 允许航班
        /// </summary>
        public List<string> AllowAirLine { get; set; }

        /// <summary>
        /// 禁止航班
        /// </summary>
        public List<string> DenyAirLine { get; set; }

        /// <summary>
        /// 当前可用配置列表
        /// </summary>
        public List<string> ConfigList { get; set; }
    }
}
