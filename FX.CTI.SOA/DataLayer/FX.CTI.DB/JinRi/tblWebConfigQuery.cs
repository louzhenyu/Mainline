using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using FX.CTI.DBEntity;

namespace FX.CTI.DB.JinRi
{
    public class tblWebConfigQuery
    {
        internal WebConfigRecord GetWebConfigRecord(string key)
        {
            const string sql = "SELECT settingKey AS 'Key', settingValue AS 'Value' FROM tblWebConfig WITH(NOLOCK) WHERE settingKey=@settingKey";
            using (var conn = new SqlConnection(ConnectionString.JinRi_SELECT))
            {
                conn.Open();
                return conn.Query<WebConfigRecord>(sql, new { settingKey = key }).SingleOrDefault<WebConfigRecord>();
            }
        }
    }

    public class WebConfigRecord
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
