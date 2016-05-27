using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JetermClient.Utility;
using JetermClient.Common;
using JetermEntity;

namespace JetermClient.DAL
{
    public class EtermConfig
    {
        private const string SQL_SELECT_ETERM_CONFIG = "SELECT ServerUrl,OfficeNo,ConfigType,AllowAirLine,DenyAirLine,ConfigList,ConfigLevel FROM dbo.EtermConfig WHITH(NOLOCK) WHERE ConfigState=0";

        /// <summary>
        /// 获取所有配置
        /// </summary>
        /// <returns></returns>
        public List<Config> GetConfigs()
        {
            List<Config> configs = new List<Config>();

            using(SqlDataReader reader=JetermClient.Utility.SqlHelper.ExecuteReader(Common.Common.ConnectString,CommandType.Text,SQL_SELECT_ETERM_CONFIG))
            {
                while(reader.Read())
                {
                    Config config = new Config();
                    config.AllowAirLine = getAirlines(reader["AllowAirLine"].ToString());
                    config.DenyAirLine = getAirlines(reader["DenyAirLine"].ToString());
                    config.OfficeNo = reader["OfficeNo"].ToString();
                    config.ServerUrl = reader["ServerUrl"].ToString();
                    config.ConfigLevel = Convert.ToInt32(reader["ConfigLevel"]);
                    config.cmdType = gettypes(reader["ConfigType"].ToString());
                    config.ConfigList = getconfigs(reader["ConfigList"].ToString());
                    configs.Add(config);
                }
                reader.Close();
            }

            return configs;
        }

        /// <summary>
        /// 获取航空公司集合
        /// </summary>
        /// <param name="strAirlines"></param>
        /// <returns></returns>
        private List<string> getAirlines(string strAirlines)
        {
            List<string> airlines = new List<string>();
            if (string.IsNullOrEmpty(strAirlines)) return airlines;
            string[] ss = strAirlines.Split(',');
            foreach(string al in ss)
            {
                if (string.IsNullOrEmpty(al)) continue;
                airlines.Add(al);
            }
            return airlines;
        }
        
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="strTypes"></param>
        /// <returns></returns>
        private List<EtermCommand.CmdType> gettypes(string strTypes)
        {
            List<EtermCommand.CmdType> types = new List<EtermCommand.CmdType>();

            Array arr = Enum.GetValues(typeof(EtermCommand.CmdType));

            foreach (object o in arr)
            {
                if (strTypes.IndexOf(((int)o).ToString() + ",") != -1 || strTypes.IndexOf("*") != -1)
                    types.Add((EtermCommand.CmdType)o);
            }
         
            return types;
        }

        /// <summary>
        /// 获取配置名称
        /// </summary>
        /// <param name="configs"></param>
        /// <returns></returns>
        private List<string> getconfigs(string configs)
        {
            List<string> rconfigs = new List<string>();
            string[] configlist = configs.Split(',');
            foreach (string config in configlist)
                if (!string.IsNullOrEmpty(config))
                    rconfigs.Add(config);
            return rconfigs;
        }
    }
}
