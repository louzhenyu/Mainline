using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using JinRi.Fx.Utility;

namespace JinRi.Fx.Data
{
    public class DapperHelper<T>
    {
        public static IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection(ConnectionStr.FxDb);
            connection.Open();
            return connection;
        }

        public static IEnumerable<T> GetPageList(string connectionStr, string commandText, PageItem pager = null)
        {
            IEnumerable<T> returnValue = new List<T>();

            using (var conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                if (pager != null)
                {
                    //分页
                    DynamicParameters paras = new DynamicParameters();
                    paras.Add("@sqlstr", commandText);
                    paras.Add("@currentpage", pager.PageIndex);
                    paras.Add("@pagesize", pager.PageSize);

                    using (var result = conn.QueryMultiple("USP_PAGER", paras, null, null, CommandType.StoredProcedure))
                    {
                        returnValue = result.Read<T>();
                        pager.TotalCount = result.Read<int>().SingleOrDefault<int>();
                    }
                }
                else
                {
                    //不分页
                    return conn.Query<T>(commandText);
                }
            }         

            return returnValue;
        }

        public static IEnumerable<T> GetList(string connectionStr, string commandText)
        {
            using (IDbConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                return conn.Query<T>(commandText);
            }
        }
    }
}
