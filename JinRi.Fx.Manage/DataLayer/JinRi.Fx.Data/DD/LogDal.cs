using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Data.DD
{
    public class LogDal
    {
        public void Add(Log log)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStr.FxDb))
            {
                connection.Open();
                connection.ChangeDatabase("FxDB");
                const string sql =
                    @"INSERT INTO DataDictionaryLog(SvrName, DbName, SchemaName, TblName, ColName, OldDesc, NewDesc, Dealer, DealTime, OldOwner, NewOwner) VALUES(@SvrName, @DbName, @SchemaName, @TblName, @ColName, @OldDesc, @NewDesc, @Dealer, @DealTime, @OldOwner, @NewOwner)";
                connection.Execute(sql, log);
            }
        }
        public IEnumerable<Log> Query(string dealer, string beginTime, string endTime, string server, string db, string table, string col, int pageIndex, int pageSize, out int totalRecords)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStr.FxDb))
            {
                connection.Open();
                connection.ChangeDatabase("FxDB");
                var sb = new StringBuilder();
                sb.Append(
                    @"SELECT [Id], SvrName, DbName, SchemaName, TblName, ColName, OldDesc, NewDesc, Dealer, DealTime, OldOwner, NewOwner FROM DataDictionaryLog WITH(NOLOCK) WHERE 1=1");
                if (!string.IsNullOrWhiteSpace(dealer))
                {
                    sb.Append(@" AND Dealer='");
                    sb.Append(dealer);
                    sb.Append(@"'");
                }
                if (!string.IsNullOrWhiteSpace(beginTime))
                {
                    sb.Append(@" AND DealTime >= CONVERT(DATETIME, '");
                    sb.Append(beginTime);
                    sb.Append(@"')");
                }
                if (!string.IsNullOrWhiteSpace(endTime))
                {
                    sb.Append(@" AND DealTime < DATEADD(DD, 1, CONVERT(DATETIME, '");
                    sb.Append(endTime);
                    sb.Append(@"'))");
                }
                if (!string.IsNullOrWhiteSpace(server))
                {
                    sb.Append(@" AND SvrName='");
                    sb.Append(server);
                    sb.Append(@"'");
                    if (!string.IsNullOrWhiteSpace(db))
                    {
                        sb.Append(@" AND DbName='");
                        sb.Append(db);
                        sb.Append(@"'");
                        if (!string.IsNullOrWhiteSpace(table))
                        {
                            sb.Append(@" AND TblName='");
                            sb.Append(table);
                            sb.Append(@"'");
                            if (!string.IsNullOrWhiteSpace(col))
                            {
                                sb.Append(@" AND ColName='");
                                sb.Append(col);
                                sb.Append(@"'");
                            }
                        }
                    }
                }
                sb.Append(@" ORDER BY DealTime DESC");
                string sql = sb.ToString();
                var paras = new DynamicParameters();
                paras.Add("@sqlstr", sql);
                paras.Add("@currentpage", pageIndex);
                paras.Add("@pagesize", pageSize);

                using (
                    SqlMapper.GridReader result = connection.QueryMultiple("USP_PAGER", paras, null, null,
                        CommandType.StoredProcedure))
                {
                    IEnumerable<Log> logs = result.Read<Log>();
                    totalRecords = result.Read<int>().SingleOrDefault<int>();
                    return logs;
                }
            }
        }
    }
}