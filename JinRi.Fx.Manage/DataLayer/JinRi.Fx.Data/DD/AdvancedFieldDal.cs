using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Data.DD
{
    public class AdvancedFieldDal
    {
        IList<Field> _fields = new List<Field>();
        private string _advServerName;
        private string _advDatabaseName;
        private string _advTableName;
        private string _advFieldNameKeyword;
        private string _advFieldDescriptionKeyword;
        public IEnumerable<Field> GetField(string serverName, string databaseName, string tableName, string fieldNameKeyword, string fieldDescriptionKeyword, int pageIndex, int pageSize, out int totalRecords)
        {
            if (string.IsNullOrWhiteSpace(fieldNameKeyword) && string.IsNullOrWhiteSpace(fieldDescriptionKeyword))
            {
                totalRecords = 0;
                return new List<Field>();
            }
            if (serverName != _advServerName || databaseName != _advDatabaseName || tableName != _advTableName ||
                fieldNameKeyword != _advFieldNameKeyword || fieldDescriptionKeyword != _advFieldDescriptionKeyword)
            {
                _fields = new List<Field>();
                using (IDbConnection connection = new SqlConnection(ConnStrHelper.GetConnStr(serverName)))
                {
                    connection.Open();
                    var databases = new List<Database>();
                    if (string.IsNullOrWhiteSpace(databaseName))
                    {
                        const string queryDbName =
                            @"SELECT name AS 'DatabaseName' FROM sys.sysdatabases WITH(NOLOCK) WHERE dbid > 4";
                        databases = connection.Query<Database>(queryDbName).ToList();
                    }
                    else
                    {
                        databases.Add(new Database()
                        {
                            DatabaseName = databaseName
                        });
                    }
                    foreach (var i in databases)
                    {
                        try
                        {
                            connection.ChangeDatabase(i.DatabaseName);
                        }
                        catch
                        {
                            continue;
                        }
                        var sb = new StringBuilder();
                        sb.Append(@"SELECT
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
     c.colid AS 'FieldPosition',
     tb.name AS 'TableName'
FROM sys.syscolumns c
INNER JOIN sys.systypes t ON c.xusertype = t.xusertype 
LEFT JOIN sys.extended_properties etp ON etp.major_id = c.id AND etp.minor_id = c.colid AND etp.name = 'MS_Description' 
LEFT JOIN sys.syscomments cm ON c.cdefault = cm.id
LEFT JOIN sys.tables tb ON C.id = tb.object_id
LEFT JOIN sys.schemas sc ON sc.schema_id = tb.schema_id
WHERE sc.name = 'dbo'");
                        if (!string.IsNullOrWhiteSpace(tableName))
                        {
                            sb.Append(@" AND c.id = OBJECT_ID('");
                            sb.Append(tableName);
                            sb.Append(@"')");
                        }
                        if (!string.IsNullOrWhiteSpace(fieldNameKeyword))
                        {
                            string[] fieldNameKeywords = Regex.Split(fieldNameKeyword, @"\s+");
                            foreach (var fieldName in fieldNameKeywords)
                            {
                                sb.Append(@" AND c.name LIKE '%");
                                sb.Append(fieldName.Trim());
                                sb.Append(@"%'");
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(fieldDescriptionKeyword))
                        {
                            string[] fieldDescriptionKeywords = Regex.Split(fieldDescriptionKeyword, @"\s+");
                            foreach (var fieldDescription in fieldDescriptionKeywords)
                            {
                                sb.Append(@" AND CONVERT(VARCHAR(MAX),ISNULL(etp.value,'')) LIKE '%");
                                sb.Append(fieldDescription.Trim());
                                sb.Append(@"%'");
                            }
                        }

                        var query = sb.ToString();
                        var resultFields =
                            connection.Query<Field>(query).ToList();
                        foreach (var j in resultFields)
                        {
                            j.DatabaseName = i.DatabaseName;
                            j.FieldDescription = j.FieldDescription ?? "";
                            j.FieldDescription = j.FieldDescription.Trim().IsEmpty() ? "" : j.FieldDescription;
                            _fields.Add(j);
                        }
                    }
                }
                _advServerName = serverName;
                _advDatabaseName = databaseName;
                _advTableName = tableName;
                _advFieldNameKeyword = fieldNameKeyword;
                _advFieldDescriptionKeyword = fieldDescriptionKeyword;
            }
            totalRecords = _fields.Count;
            return _fields.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
