using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    public class EtermConfig
    {
        private int _ConfigID = 0;
        private string _ServerUrl = "";
        private string _OfficeNo = "";
        private string _ConfigType = "";
        private int _ConfigState = 0;
        private DateTime _OperDate;
        private string _AllowAirLine = "";
        private string _DenyAirLine = "";
        private string _Remark = "";
        private ConfigLevel _ConfigLevel = ConfigLevel.A;

        public int ConfigID
        {
            get
            {
                return _ConfigID;
            }
            set
            {
                _ConfigID = value;
            }
        }

        [Required(ErrorMessage = "服务器地址不能为空")]
        /// <summary>
        /// 服务器路径
        /// </summary>
        public string ServerUrl
        {
            get
            {
                return _ServerUrl;
            }
            set
            {
                _ServerUrl = value;
            }
        }

        /// <summary>
        ///  配置OFFICE号
        /// </summary>
        public string OfficeNo
        {
            get
            {
                return _OfficeNo;
            }
            set
            {
                _OfficeNo = value;
            }
        }

        /// <summary>
        /// 配置支持的功能
        /// </summary>
        public string ConfigType
        {
            get
            {
                return _ConfigType;
            }
            set
            {
                _ConfigType = value;
            }
        }
        /// <summary>
        /// 状态 0 启用 1关闭
        /// </summary>
        public int ConfigState
        {
            get
            {
                return _ConfigState;
            }
            set
            {
                _ConfigState = value;
            }
        }
        /// <summary>
        /// 添加日期 
        /// </summary>
        public DateTime OperDate
        {
            get
            {
                return _OperDate;
            }
            set
            {
                _OperDate = value;
            }
        }
        /// <summary>
        /// 支持的航司，*支持全部航司
        /// </summary>
        public string AllowAirLine
        {
            get
            {
                return _AllowAirLine;
            }
            set
            {
                _AllowAirLine = value;
            }
        }

        /// <summary>
        /// 不支持的航司
        /// </summary>
        public string DenyAirLine
        {
            get
            {
                return _DenyAirLine;
            }
            set
            {
                _DenyAirLine = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                _Remark = value;
            }
        }

        /// <summary>
        /// 配置级别
        /// </summary>
        public ConfigLevel ConfigLevel
        {
            get
            {
                return _ConfigLevel;
            }
            set
            {
                _ConfigLevel = value;
            }
        }

        /// <summary>
        /// 配置功能描述
        /// </summary>
        public string TypeDesc
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigType))
                {
                    return "不支持任何功能";
                }

                Dictionary<int, string> dicTypes = EnumHelper.GetItemValueList<CmdType>();
                StringBuilder sb = new StringBuilder();
                foreach (var item in dicTypes.Keys)
                {
                    if (ConfigType.Contains(item + ","))
                        sb.Append(dicTypes[item] + ",");
                }
                return sb.ToString();
            }
        }

    }
}
