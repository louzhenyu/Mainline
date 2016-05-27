using System.Data.SqlClient;
using Dapper;
using FX.CTI.DBEntity;

namespace FX.CTI.DB.FxDB
{
    public class EmailCmd
    {
        internal int UpdateEmail(Email email)
        {
            const string sql = "UPDATE Email SET AppId=@AppId,EmailToAddr=@EmailToAddr,EmailCC=@EmailCC,EmailSubject=@EmailSubject,EmailContent=@EmailContent,EmailStatus=@EmailStatus,EmailErrMsg=@EmailErrMsg,EmailReceivedTime=@EmailReceivedTime,EmailSentTime=@EmailSentTime WHERE EmailId=@EmailId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, email);
            }
        }
        internal int AddEmail(Email email)
        {
            const string sql = "INSERT INTO Email(EmailId,AppId,EmailToAddr,EmailCC,EmailSubject,EmailContent,EmailStatus,EmailErrMsg,EmailReceivedTime,EmailSentTime) VALUES(@EmailId,@AppId,@EmailToAddr,@EmailCC,@EmailSubject,@EmailContent,@EmailStatus,@EmailErrMsg,@EmailReceivedTime,@EmailSentTime)";
            using (var conn = new SqlConnection(ConnectionString.FxDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, email);
            }
        }
        internal int DeleteEmail(string emailId)
        {
            const string sql = "DELETE Email WHERE EmailId=@EmailId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, new { EmailId = emailId });
            }
        }
    }
}
