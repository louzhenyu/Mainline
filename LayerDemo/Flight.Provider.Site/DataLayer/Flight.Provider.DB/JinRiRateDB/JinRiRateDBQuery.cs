using Dapper;
using Flight.Provider.DBEntity;
using Flight.Provider.Entity.Request;
using Flight.Provider.Entity.Response;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Flight.Provider.DB
{
    public class JinRiRateDBQuery
    {
        /// <summary>
        /// 获取政策备注分页总记录数
        /// </summary>
        /// <returns></returns>
        internal int GetPolicyRemarkTotalCount(PolicyRemarkSearchRequest request)
        {
            //Where条件必须与GetRateRemarkPageData()方法一致
            //可以减少不必要的表关联，不需要排序
            //相同查询条件的记录数可以适当缓存
            const string sql = "SELECT COUNT(1) FROM TblRateRemark WITH(NOLOCK) WHERE AgentID=@AgentID AND RateType=@RateType";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@AgentID", request.ProviderId);
            dp.Add("@RateType", request.PolicyType);
            using (var conn = new SqlConnection(ConnectionString.JinRiRateDB_SELECT))
            {
                conn.Open();
                return conn.Query<int>(sql, dp).SingleOrDefault<int>();
            }
        }

        /// <summary>
        /// 获取政策备注列表
        /// </summary>
        /// <returns>政策备注列表</returns>
        internal List<PolicyRemark> GetPolicyRemarkPageData(PolicyRemarkSearchRequest request)
        {
            const string sql =
@"WITH cte AS(
	SELECT ROW_NUMBER() OVER(ORDER BY ID DESC) AS RowID,*
	FROM TblRateRemark WITH(NOLOCK)  WHERE AgentID=@AgentID AND RateType=@RateType
)
SELECT * FROM cte WHERE RowID BETWEEN @RowBegin AND @RowEnd ORDER BY ID;";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@RowBegin", request.Paging.PageIndex * request.Paging.PageSize + 1);
            dp.Add("@RowEnd", (request.Paging.PageIndex + 1) * request.Paging.PageSize);
            dp.Add("@AgentID", request.ProviderId);
            dp.Add("@RateType", request.PolicyType);

            using (var conn = new SqlConnection(ConnectionString.JinRiRateDB_SELECT))
            {
                conn.Open();
                return conn.Query<PolicyRemark>(sql, dp).ToList<PolicyRemark>();
            }
        }
        /// <summary>
        /// 获取分页保底政策 分页方式二
        /// </summary>
        /// <returns>保底政策</returns>
        internal PolicyRemarkSearchResponse GetPolicyRemarkPageList(PolicyRemarkSearchRequest request)
        {
            PolicyRemarkSearchResponse response = new PolicyRemarkSearchResponse();

            const string sql =
@"WITH cte AS(
	SELECT ROW_NUMBER() OVER(ORDER BY ID DESC) AS RowID,*
	FROM TblRateRemark WITH(NOLOCK)  WHERE AgentID=@AgentID AND RateType=@RateType
)
SELECT * FROM cte WHERE RowID BETWEEN @RowBegin AND @RowEnd ORDER BY ID;
SELECT COUNT(1) FROM TblRateRemark WITH(NOLOCK)  WHERE AgentID=@AgentID AND RateType=@RateType;";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@RowBegin", request.Paging.PageIndex * request.Paging.PageSize + 1);
            dp.Add("@RowEnd", (request.Paging.PageIndex + 1) * request.Paging.PageSize);
            dp.Add("@AgentID", request.ProviderId);
            dp.Add("@RateType", request.PolicyType);

            using (var conn = new SqlConnection(ConnectionString.JinRiRateDB_SELECT))
            {
                conn.Open();
                using (var result = conn.QueryMultiple(sql, dp, null, null, CommandType.Text))
                {
                    response.Success = true;
                    response.Data = result.Read<PolicyRemarkDTO>().ToList<PolicyRemarkDTO>();
                    response.Paging.TotalCount = result.Read<int>().SingleOrDefault<int>();
                }
            }
            return response;
        }
    }
}
