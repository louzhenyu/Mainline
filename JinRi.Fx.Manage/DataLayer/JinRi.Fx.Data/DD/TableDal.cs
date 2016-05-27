using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Data.DD
{
    public class TableDal
    {
        public IEnumerable<Table> GetAll(string serverName, string databaseName, string schemaName)
        {
            using (IDbConnection connection = new SqlConnection(ConnStrHelper.GetConnStr(serverName)))
            {
                connection.Open();
                try
                {
                    connection.ChangeDatabase(databaseName);
                }
                catch
                {
                    return new List<Table>();
                }
                const string query = @"SELECT a.name AS 'TableName', e.ColCount AS 'ColCount', c.value AS 'TableOwner', b.value AS 'TableDescription' FROM sys.tables a LEFT JOIN sys.fn_listextendedproperty ('MS_Description', 'schema', @schemaname, 'table', default, default, default) b
 on a.name = b.objname COLLATE Chinese_PRC_CI_AS LEFT JOIN sys.fn_listextendedproperty ('JR_Owner', 'schema', @schemaname, 'table', default, default, default) c
 on a.name = c.objname COLLATE Chinese_PRC_CI_AS LEFT JOIN sys.schemas d ON a.schema_id = d.schema_id LEFT JOIN (select g.name AS 'TableName', COUNT(*) AS 'ColCount' from sys.syscolumns f left join sys.tables g on f.id = g.object_id group by g.name) e ON a.name = e.TableName WHERE d.name = @schemaname";
                var resultTables = connection.Query<Table>(query, new { schemaname = schemaName }).ToList();
                foreach (var i in resultTables)
                {
                    i.ServerName = serverName;
                    i.DatabaseName = databaseName;
                    i.SchemaName = schemaName;
                    i.TableOwner = i.TableOwner ?? "";
                    i.TableOwner = i.TableOwner.Trim().IsEmpty() ? "" : i.TableOwner;
                    i.TableDescription = i.TableDescription ?? "";
                    i.TableDescription = i.TableDescription.Trim().IsEmpty() ? "" : i.TableDescription;
                }
                return resultTables.AsEnumerable();
            }
        }

        public void UpdateTableDesc(Table table, string dealer)
        {
            using (IDbConnection connection = new SqlConnection(ConnStrHelper.GetConnStr(table.ServerName)))
            {
                connection.Open();
                connection.ChangeDatabase(table.DatabaseName);
                const string query = "SELECT value FROM sys.fn_listextendedproperty ('MS_Description', 'schema', @schemaname, 'table', @tablename, default, default)";
                var tableDescription =
                    connection.Query<string>(query, new { schemaname = table.SchemaName, tablename = table.TableName }).FirstOrDefault();
                const string queryOwner = "SELECT value FROM sys.fn_listextendedproperty ('JR_Owner', 'schema', @schemaname, 'table', @tablename, default, default)";
                var tableOwner =
                    connection.Query<string>(queryOwner, new { schemaname = table.SchemaName, tablename = table.TableName }).FirstOrDefault();
                if (tableDescription.IsNull() && tableOwner.IsNull() && table.TableDescription.IsNull() && table.TableOwner.IsNull())
                {
                    return;
                }
                if (!tableDescription.IsNull() && !tableOwner.IsNull() && !table.TableDescription.IsNull() && !table.TableOwner.IsNull())
                {
                    if (tableDescription == table.TableDescription && tableOwner == table.TableOwner)
                        return;
                }
                var logDal = new LogDal();
                var log = new Log()
                {
                    ColName = "",
                    DbName = table.DatabaseName,
                    Dealer = dealer,
                    DealTime = DateTime.Now,
                    NewOwner = table.TableOwner??"",
                    OldOwner = tableOwner ?? "",
                    NewDesc = table.TableDescription??"",
                    OldDesc = tableDescription ?? "",
                    SchemaName = "dbo",
                    SvrName = table.ServerName,
                    TblName = table.TableName
                };
                logDal.Add(log);
                var p = new DynamicParameters();
                p.Add("@name", "MS_Description");
                p.Add("@level0type", "schema");
                p.Add("@level0name", table.SchemaName);
                p.Add("@level1type", "table");
                p.Add("@level1name", table.TableName);
                p.Add("@value", table.TableDescription??"");
                connection.Execute(tableDescription.IsNull() ? "sys.sp_addextendedproperty" : "sys.sp_updateextendedproperty", p, commandType: CommandType.StoredProcedure);
                p = new DynamicParameters();
                p.Add("@name", "JR_Owner");
                p.Add("@level0type", "schema");
                p.Add("@level0name", table.SchemaName);
                p.Add("@level1type", "table");
                p.Add("@level1name", table.TableName);
                p.Add("@value", table.TableOwner??"");
                connection.Execute(tableOwner.IsNull() ? "sys.sp_addextendedproperty" : "sys.sp_updateextendedproperty", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
