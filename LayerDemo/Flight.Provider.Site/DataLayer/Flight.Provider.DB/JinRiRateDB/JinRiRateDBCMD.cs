using Dapper;
using Flight.Provider.DBEntity;
using System.Data.SqlClient;

namespace Flight.Provider.DB
{
    public class JinRiRateDBCMD
    {
        internal int UpdatePolicyRemark(PolicyRemark rateRemark)
        {
            const string sql = "UPDATE TblRateRemark SET Info=@Info,RateType=@RateType WHERE ID=@ID AND AgentID=@AgentID";
            using (var conn = new SqlConnection(ConnectionString.JinRiRateDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, rateRemark);
            }
        }
        internal int AddPolicyRemark(PolicyRemark rateRemark)
        {
            const string sql = "INSERT INTO TblRateRemark(AgentID,Info,CreateTime,Status,RateType,ExtendInfo,TempletName) VALUES(@AgentID,@Info,GETDATE(),0,@RateType,@ExtendInfo,@TempletName)";
            using (var conn = new SqlConnection(ConnectionString.JinRiRateDB_INSERT))
            {
                conn.Open();
                return conn.Execute(sql, rateRemark);
            }
        }
    }
}
