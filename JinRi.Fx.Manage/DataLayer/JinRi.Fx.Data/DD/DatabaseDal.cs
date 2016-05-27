using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Dapper;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Data.DD
{
    public class DatabaseDal
    {
        public IEnumerable<Database> GetAll(string serverName)
        {
            using (IDbConnection connection = new SqlConnection(ConnStrHelper.GetConnStr(serverName)))
            {
                const string queryDbName = @"SELECT name AS 'DatabaseName' FROM sys.sysdatabases WITH(NOLOCK) WHERE dbid > 4";
                connection.Open();
                var resultDatabases = connection.Query<Database>(queryDbName).ToList();
                const string queryDbDesc = @"SELECT value FROM sys.fn_listextendedproperty ('MS_Description', default, default, default, default, default, default)";
                const string queryDbOwner = @"SELECT value FROM sys.fn_listextendedproperty ('JR_Owner', default, default, default, default, default, default)";
                foreach (var i in resultDatabases)
                {
                    try
                    {
                        connection.ChangeDatabase(i.DatabaseName);
                    }
                    catch
                    {
                        i.ServerName = serverName;
                        i.DatabaseDescription = @"<font color='red'>脱机</font>";
                        continue;
                    }
                    var dbDesc = connection.Query<string>(queryDbDesc).FirstOrDefault();
                    var dbOwner = connection.Query<string>(queryDbOwner).FirstOrDefault();
                    i.ServerName = serverName;
                    i.DatabaseOwner = dbOwner ?? "";
                    i.DatabaseOwner = i.DatabaseOwner.Trim().IsEmpty() ? "" : i.DatabaseOwner;
                    i.DatabaseDescription = dbDesc ?? "";
                    i.DatabaseDescription = i.DatabaseDescription.Trim().IsEmpty() ? "" : i.DatabaseDescription;
                }
                return resultDatabases.AsEnumerable();
            }
        }

        public void UpdateDbDesc(Database db, string dealer)
        {
            using (IDbConnection connection = new SqlConnection(ConnStrHelper.GetConnStr(db.ServerName)))
            {
                connection.Open();
                connection.ChangeDatabase(db.DatabaseName);
                const string queryDbDesc = @"SELECT value FROM sys.fn_listextendedproperty ('MS_Description', default, default, default, default, default, default)";
                var dbDesc = connection.Query<string>(queryDbDesc).FirstOrDefault();
                const string queryDbOwner = @"SELECT value FROM sys.fn_listextendedproperty ('JR_Owner', default, default, default, default, default, default)";
                var dbOwner = connection.Query<string>(queryDbOwner).FirstOrDefault();
                if (dbDesc.IsNull() && db.DatabaseDescription.IsNull() && dbOwner.IsNull() && db.DatabaseOwner.IsNull())
                {
                    return;
                }
                if (!dbDesc.IsNull() && !db.DatabaseDescription.IsNull() && !dbOwner.IsNull() && !db.DatabaseOwner.IsNull())
                {
                    if (dbDesc == db.DatabaseDescription && dbOwner == db.DatabaseOwner)
                        return;
                }
                var logDal = new LogDal();
                var log = new Log()
                {
                    ColName = "",
                    DbName = db.DatabaseName,
                    Dealer = dealer,
                    DealTime = DateTime.Now,
                    NewDesc = db.DatabaseDescription??"",
                    OldDesc = dbDesc ?? "",
                    SchemaName = "",
                    SvrName = db.ServerName,
                    TblName = "",
                    OldOwner = dbOwner ?? "",
                    NewOwner = db.DatabaseOwner ?? ""
                };
                logDal.Add(log);
                var p = new DynamicParameters();
                p.Add("@name", "MS_Description");
                p.Add("@value", db.DatabaseDescription ?? "");
                connection.Execute(dbDesc.IsNull() ? "sys.sp_addextendedproperty" : "sys.sp_updateextendedproperty", p,
                    commandType: CommandType.StoredProcedure);
                p = new DynamicParameters();
                p.Add("@name", "JR_Owner");
                p.Add("@value", db.DatabaseOwner ?? "");
                connection.Execute(dbOwner.IsNull() ? "sys.sp_addextendedproperty" : "sys.sp_updateextendedproperty", p,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
