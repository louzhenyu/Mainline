using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FX.CTI.DBEntity;

namespace FX.CTI.DB.FxDB
{
    public class SMSQuery
    {
        #region SMS
        /// <summary>
        /// 获取所有SMS
        /// </summary>
        /// <returns>所有SMS</returns>
        internal List<SMS> GetSMSList()
        {
            const string sql = "SELECT SMSId,AppId,SMSMobile,SMSContent,SMSErrMsg,SMSStatus,SMSReceivedTime,SMSSentTime FROM SMS WITH(NOLOCK)";
            using (var conn = new SqlConnection(ConnectionString.FxDB_SELECT))
            {
                conn.Open();
                return conn.Query<SMS>(sql).ToList<SMS>();
            }
        }

        /// <summary>
        /// 获取单个SMS
        /// </summary>
        /// <param name="smsId">SMS编号</param>
        /// <returns>NULL未获取到SMS，其他返回相应的SMS</returns>
        internal SMS GetSMS(string smsId)
        {
            const string sql = "SELECT SMSId,AppId,SMSMobile,SMSContent,SMSErrMsg,SMSStatus,SMSReceivedTime,SMSSentTime FROM SMS WITH(NOLOCK) WHERE SMSId=@SMSId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_SELECT))
            {
                conn.Open();
                return conn.Query<SMS>(sql, new { ID = smsId }).SingleOrDefault<SMS>();
            }
        }

        /// <summary>
        /// 检查smsId是否已存在
        /// </summary>
        /// <param name="smsId">短信编号</param>
        /// <returns>true:已存在，false:不存在</returns>
        internal bool IsSMSIdExist(string smsId)
        {
            const string sql = "SELECT COUNT(*) FROM SMS WITH(NOLOCK) WHERE SMSId=@SMSId";
            using (var conn = new SqlConnection(ConnectionString.FxDB_SELECT))
            {
                conn.Open();
                var existCount = conn.ExecuteScalar<int>(sql, new { SMSId = smsId });
                return existCount > 0;
            }
        }
        #endregion
    }
}
