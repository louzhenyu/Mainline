using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Web;
using Dapper;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Data.DD
{
    public class FieldDal
    {
        public IEnumerable<Field> GetAll(string serverName, string databaseName, string schemaName, string tableName)
        {
            using (IDbConnection connection = new SqlConnection(ConnStrHelper.GetConnStr(serverName)))
            {
                connection.Open();
                connection.ChangeDatabase(databaseName);
                const string query = @"SELECT
     c.name AS 'FieldName',
     c.colorder AS 'FieldOrder',
     t.name AS 'FieldType',
     CONVERT(BIT, c.IsNullable) AS 'IsFieldNullable',
     CONVERT(BIT,CASE WHEN EXISTS(SELECT 1 FROM sysobjects WHERE xtype='PK' AND parent_obj=c.id AND name IN (
         SELECT name FROM sysindexes WHERE indid IN(
             SELECT indid FROM sysindexkeys WHERE id = c.id AND colid=c.colid))) THEN 1 ELSE 0 END) 
                 AS 'IsFieldPrimaryKey'
     ,CONVERT(BIT,COLUMNPROPERTY(c.id,c.name,'IsIdentity')) AS 'IsFieldAutoIncrement',
     c.length AS 'FieldMaxLength',
     ISNULL(cm.text,'') AS 'FieldDefaultValue',
     ISNULL(etp.value,'') AS 'FieldDescription',
     c.colid AS 'FieldPosition'
FROM sys.syscolumns c
INNER JOIN sys.systypes t ON c.xusertype = t.xusertype 
LEFT JOIN sys.extended_properties etp ON etp.major_id = c.id AND etp.minor_id = c.colid AND etp.name = 'MS_Description' 
LEFT JOIN sys.syscomments cm ON c.cdefault = cm.id
LEFT JOIN sys.tables tb ON C.id = tb.object_id
LEFT JOIN sys.schemas sc ON sc.schema_id = tb.schema_id
WHERE c.id = OBJECT_ID(@tablename) AND sc.name = @schemaname";
                var resultFields = connection.Query<Field>(query, new { schemaname = schemaName, tablename = tableName }).ToList();
                foreach (var i in resultFields)
                {
                    i.ServerName = serverName;
                    i.DatabaseName = databaseName;
                    i.SchemaName = schemaName;
                    i.TableName = tableName;
                    i.FieldDescription = i.FieldDescription ?? "";
                    i.FieldDescription = i.FieldDescription.Trim().IsEmpty() ? "" : i.FieldDescription;
                }
                return resultFields.AsEnumerable();
            }
        }

        public void UpdateFieldDesc(Field field, string dealer)
        {
            using (IDbConnection connection = new SqlConnection(ConnStrHelper.GetConnStr(field.ServerName)))
            {
                connection.Open();
                connection.ChangeDatabase(field.DatabaseName);
                const string query = "SELECT value FROM sys.fn_listextendedproperty ('MS_Description', 'schema', @schemaname, 'table', @tablename, 'column', @fieldname)";
                var fieldDescription =
                    connection.Query<string>(query, new { schemaname = field.SchemaName, tablename = field.TableName, fieldname = field.FieldName }).FirstOrDefault();
                if (fieldDescription.IsNull() && field.FieldDescription.IsNull())
                {
                    return;
                }
                if (!fieldDescription.IsNull() && !field.FieldDescription.IsNull())
                {
                    if (fieldDescription == field.FieldDescription)
                        return;
                }
                if (!field.FieldDescription.IsNull())
                {
                    field.FieldDescription = HttpUtility.UrlDecode(field.FieldDescription);
                }
                var logDal = new LogDal();
                var log = new Log()
                {
                    ColName = "",
                    DbName = field.DatabaseName,
                    Dealer = dealer,
                    DealTime = DateTime.Now,
                    NewDesc = field.FieldDescription??"",
                    OldDesc = fieldDescription ?? "",
                    SchemaName = "dbo",
                    SvrName = field.ServerName,
                    TblName = field.TableName,
                    OldOwner = "",
                    NewOwner = ""
                };
                logDal.Add(log);
                var p = new DynamicParameters();
                p.Add("@name", "MS_Description");
                p.Add("@level0type", "schema");
                p.Add("@level0name", field.SchemaName);
                p.Add("@level1type", "table");
                p.Add("@level1name", field.TableName);
                p.Add("@level2type", "column");
                p.Add("@level2name", field.FieldName);
                p.Add("@value", field.FieldDescription??"");
                connection.Execute(fieldDescription.IsNull() ? "sys.sp_addextendedproperty" : "sys.sp_updateextendedproperty", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
