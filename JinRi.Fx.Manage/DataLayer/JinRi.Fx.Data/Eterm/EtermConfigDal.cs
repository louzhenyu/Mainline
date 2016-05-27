using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System.Data.SqlClient;

namespace JinRi.Fx.Data
{
    public class EtermConfigDal
    {
        public IEnumerable<EtermConfig> GetEtermConfigList(int state, string serverUrl = "", string officeNo = "", PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM EtermConfig WITH(NOLOCK) WHERE 1=1 ");
            if (state >= 0)
            {
                sql.AppendFormat("AND ConfigState={0} ", state);
            }
            if (!string.IsNullOrEmpty(serverUrl))
            {
                sql.AppendFormat("AND serverUrl like '%{0}%' ", serverUrl);
            }
            if (!string.IsNullOrEmpty(officeNo))
            {
                sql.AppendFormat("AND officeNo ='{0}' ", officeNo);
            }
            return DapperHelper<EtermConfig>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }


        public EtermConfig GetEtermConfig(int id)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStr.FxDb))
                {
                    conn.Open();
                    string sql = "SELECT * FROM EtermConfig WITH(NOLOCK) WHERE ConfigID =@id";
                    return conn.Query<EtermConfig>(sql, new { id = id }).SingleOrDefault<EtermConfig>();
                }
            }
            catch (Exception ex) { return null; }
        }

        public int UpdateEtermConfig(EtermConfig etermConfig)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = " UPDATE   dbo.EtermConfig  SET  ServerUrl=@ServerUrl,ConfigState=@ConfigState, OfficeNo=@OfficeNo,ConfigType=@ConfigType,AllowAirLine=@AllowAirLine,DenyairLine=@DenyairLine,ConfigLevel=@ConfigLevel,Remark=@Remark,OperDate=GetDate() WHERE ConfigID=@ConfigID ";
                return conn.Execute(sql,
                    new
                    {
                        ServerUrl = etermConfig.ServerUrl,
                        OfficeNo = etermConfig.OfficeNo,
                        ConfigState = etermConfig.ConfigState,
                        ConfigType = etermConfig.ConfigType,
                        AllowAirLine = etermConfig.AllowAirLine == null ? "*" : etermConfig.AllowAirLine,
                        DenyairLine = etermConfig.DenyAirLine == null ? "" : etermConfig.DenyAirLine,
                        ConfigLevel = (int)etermConfig.ConfigLevel,
                        Remark = etermConfig.Remark,
                        ConfigID = etermConfig.ConfigID
                    });
            }
        }

        public int AddEtermConfig(EtermConfig etermConfig)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = " INSERT INTO dbo.EtermConfig (ServerUrl,OfficeNo,ConfigType,ConfigState,OperDate,AllowAirLine,DenyAirLine,ConfigLevel,Remark)     VALUES (@ServerUrl,@OfficeNo,@ConfigType,@ConfigState,GETDATE(),@AllowAirLine,@DenyAirLine,@ConfigLevel,@Remark) ";
                return conn.Execute(sql,
                    new
                    {
                        ServerUrl = etermConfig.ServerUrl,
                        OfficeNo = etermConfig.OfficeNo,
                        ConfigState = etermConfig.ConfigState,
                        ConfigType = etermConfig.ConfigType,
                        AllowAirLine = etermConfig.AllowAirLine == null ? "*" : etermConfig.AllowAirLine,
                        DenyairLine = etermConfig.DenyAirLine == null ? "" : etermConfig.DenyAirLine,
                        ConfigLevel = (int)etermConfig.ConfigLevel,
                        Remark = etermConfig.Remark
                    });
            }
        }

        public int DeleteEtermConfigList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = string.Format("DELETE dbo.EtermConfig WHERE ConfigID IN ({0})", id.Trim(','));
                return conn.Execute(sql);
            }
        }
    }
}
