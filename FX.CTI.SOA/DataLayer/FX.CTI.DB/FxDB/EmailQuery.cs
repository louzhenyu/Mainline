using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FX.CTI.DBEntity;

namespace FX.CTI.DB.FxDB
{
    public class EmailQuery
    {
        /// <summary>
        /// 获取所有Email
        /// </summary>
        /// <returns>所有Email</returns>
        internal List<Email> GetEmailList()
        {
            const string sql = "SELECT EmailId,AppId,EmailToAddr,EmailCC,EmailSubject,EmailContent,EmailStatus,EmailErrMsg,EmailReceivedTime,EmailSentTime FROM Email WITH(NOLOCK)";
            using (var conn = new SqlConnection(ConnectionString.FxDB_SELECT))
            {
                conn.Open();
                return conn.Query<Email>(sql).ToList<Email>();
            }
        }

        /// <summary>
        /// 获取单个Email
        /// </summary>
        /// <param name="emailId">邮件编号</param>
        /// <returns>NULL未获取到Email，其他返回相应的Email</returns>
        internal Email GetEmail(string emailId)
        {
            const string sql = "SELECT EmailId,AppId,EmailToAddr,EmailCC,EmailSubject,EmailContent,EmailStatus,EmailErrMsg,EmailReceivedTime,EmailSentTime FROM Email WITH(NOLOCK) WHERE EmailId=@EmailId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_SELECT))
            {
                conn.Open();
                return conn.Query<Email>(sql, new { EmailId = emailId }).SingleOrDefault<Email>();
            }
        }

        /// <summary>
        /// 检查emailId是否已存在
        /// </summary>
        /// <param name="emailId">邮件编号</param>
        /// <returns>true:已存在，false:不存在</returns>
        internal bool IsEmailIdExist(string emailId)
        {
            const string sql = "SELECT COUNT(*) FROM Email WITH(NOLOCK) WHERE EmailId=@EmailId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_SELECT))
            {
                conn.Open();
                var existCount = conn.ExecuteScalar<int>(sql, new {EmailId = emailId});
                return existCount > 0;
            }
        }
    }
}
