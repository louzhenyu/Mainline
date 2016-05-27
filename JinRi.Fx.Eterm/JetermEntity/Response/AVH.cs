using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 某航司的各直达航线的各舱位剩余可订数返回对象
    /// </summary>
    [Serializable]
    public class AVH
    {
        public AVH()
        {
            DepDate = DateTime.MinValue.Date;
            AVHList = new List<AVHSingle>();
        }

        /// <summary>
        /// 起飞日期
        /// </summary>
        public DateTime DepDate { get; set; }

        /// <summary>
        /// 某航司的各直达航线的各舱位剩余可订数返回对象列表
        /// </summary>
        public List<AVHSingle> AVHList { get; set; }

        /// <summary>
        /// 【检查某航司的各直达航线的各舱位剩余可订数】指令返回结果
        /// </summary>
        public string ResultBag { get; set; }
    }

    /// <summary>
    /// 某航班的直达航线的各舱位剩余可订数对象
    /// </summary>
    [Serializable]
    public class AVHSingle
    {
        public AVHSingle()
        {
            CarbinNumList = new List<CarbinNumber>();
            ShareFltNoList = new List<string>();
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 共享航班（true表示共享航班，false表示非共享航班；默认值为false）
        /// </summary>
        public bool ShareFlight { get; set; }

        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo { get; set; }

        /// <summary>
        /// 共享航班号列表
        /// </summary>
        public List<string> ShareFltNoList { get; set; }

        /// <summary>
        /// 机型
        /// </summary>
        public string FlightModel { get; set; }

        /// <summary>
        /// 出发城市三字码
        /// </summary>
        public string SCity { get; set; }

        /// <summary>
        /// 到达城市三字码
        /// </summary>
        public string ECity { get; set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public string STime { get; set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        public string ETime { get; set; }

        /// <summary>
        /// 出发航站楼
        /// </summary>
        public string DepTerminal { get; set; }

        /// <summary>
        /// 到达航站楼
        /// </summary>
        public string ArrTerminal { get; set; }

        /// <summary>
        /// 飞行总时长
        /// </summary>
        public string FltDuration { get; set; }

        /// <summary>
        /// 舱位剩余可订数列表
        /// </summary>
        public List<CarbinNumber> CarbinNumList { get; set; }
    }

    /// <summary>
    /// 舱位剩余可订数对象
    /// </summary>
    [Serializable]
    public class CarbinNumber
    {
        /// <summary>
        /// 舱位
        /// </summary>
        public string Cabin { get; set; }
        /// <summary>
        /// 舱位数代码
        /// </summary>
        public string NumTag { get; set; }
        /// <summary>
        /// 剩余可订数（string类型）
        /// </summary>
        public string NumStr { get; set; }
    }
}
