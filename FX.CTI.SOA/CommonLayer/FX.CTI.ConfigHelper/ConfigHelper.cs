using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using FX.CTI.DB.JinRi;

namespace FX.CTI.ConfigHelper
{
    public static class ConfigMgr
    {
        private static string _RabbitMQHost;
        
        private static string _WXTAccount;
        private static string _WXTPassword;

        private static string _SxunUrl;
        private static int _SxunUserId;
        private static string _SxunAccount;
        private static string _SxunPassword;

        private static DateTime _LastUpdateTime = DateTime.Now.AddYears(-1);
        private static int _IntervalMins = 10;//重新加载配置时间间隔分钟数
        private static int _SmsChannel;
        public static string RabbitMQHost
        {
            get
            {
                if (string.IsNullOrEmpty(_RabbitMQHost))
                {
                    _RabbitMQHost = ConfigurationManager.AppSettings["RabbitMQHost"];
                }
                return _RabbitMQHost;
            }
        }
        public static int SmsChannel
        {
            get
            {
                if (IsOverInterval())
                {
                    try
                    {
                        _LastUpdateTime = DateTime.Now;
                        var facade = new tblWebConfigFacade();
                        var result = facade.GetWebConfigRecord("CTI_SmsChannel");
                        _SmsChannel = int.Parse(result.Value);
                    }
                    catch (Exception)
                    {
                        _SmsChannel = 1;
                    }
                }
                return _SmsChannel;
            }
        }
        public static string WXTAccount
        {
            get
            {
                if (string.IsNullOrEmpty(_WXTAccount))
                {
                    _WXTAccount = ConfigurationManager.AppSettings["WXTAccount"];
                }
                return _WXTAccount;
            }
        }
        public static string WXTPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_WXTPassword))
                {
                    _WXTPassword = ConfigurationManager.AppSettings["WXTPassword"];
                }
                return _WXTPassword;
            }
        }
        public static string SxunUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_SxunUrl))
                {
                    _SxunUrl = ConfigurationManager.AppSettings["SxunUrl"];
                }
                return _SxunUrl;
            }
        }

        public static string SxunAccount
        {
            get
            {
                if (string.IsNullOrEmpty(_SxunAccount))
                {
                    _SxunAccount = ConfigurationManager.AppSettings["SxunAccount"];
                }
                return _SxunAccount;
            }
        }
        public static string SxunPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_SxunPassword))
                {
                    _SxunPassword = ConfigurationManager.AppSettings["SxunPassword"];
                }
                return _SxunPassword;
            }
        }
        public static int SxunUserId
        {
            get
            {
                if (_SxunUserId == 0)
                {
                    _SxunUserId = int.Parse(ConfigurationManager.AppSettings["SxunUserId"]);
                }
                return _SxunUserId;
            }
        }
       
        private static bool IsOverInterval()
        {
            if ((DateTime.Now - _LastUpdateTime).TotalMinutes > _IntervalMins)
            {
                return true;
            }
            return false;
        }

    }
}
