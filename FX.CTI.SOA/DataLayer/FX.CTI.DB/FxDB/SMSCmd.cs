using System.Data.SqlClient;
using Dapper;
using FX.CTI.DBEntity;

namespace FX.CTI.DB.FxDB
{
    public class SMSCmd
    {
        internal int UpdateSMS(SMS sms)
        {
            const string sql = "UPDATE SMS SET AppId=@AppId,SMSMobile=@SMSMobile,SMSContent=@SMSContent,SMSErrMsg=@SMSErrMsg,SMSStatus=@SMSStatus,SMSReceivedTime=@SMSReceivedTime,SMSSentTime=@SMSSentTime WHERE SMSId=@SMSId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, sms);
            }
        }
        internal int AddSMS(SMS sms)
        {
            const string sql = "INSERT INTO SMS(SMSId,AppId,SMSMobile,SMSContent,SMSErrMsg,SMSStatus,SMSReceivedTime,SMSSentTime) VALUES(@SMSId,@AppId,@SMSMobile,@SMSContent,@SMSErrMsg,@SMSStatus,@SMSReceivedTime,@SMSSentTime)";
            using (var conn = new SqlConnection(ConnectionString.FxDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, sms);
            }
        }
        internal int DeleteSMS(string smsId)
        {
            const string sql = "DELETE SMS WHERE SMSId=@SMSId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, new { SMSId = smsId });
            }
        }
    }
}
