using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace MetricsServer.Utils
{
    public class DBUtil
    {
        public static int ExecuteNonQuery(string connectionStr, string commandText, object paras)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                return conn.Execute(commandText, paras);
            }
        }
    }
}