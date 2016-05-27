using Flight.Product.DBEntity;
using Flight.Product.Entity;
using Flight.Product.Entity.RequestDTO;
using Flight.Product.Entity.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight.Product.DB
{
    public class JinRiRateDBFacade
    {
        static readonly JinRiRateDBCMD dbCMD = new JinRiRateDBCMD();
        static readonly JinRiRateDBQuery dbQuery = new JinRiRateDBQuery();

        /// <summary>
        /// 保存政策备注
        /// </summary>
        /// <param name="operateType"></param>
        /// <param name="rateRemark"></param>
        /// <returns></returns>
        public int PolicyRemarkSave(OperateType operateType, PolicyRemark rateRemark)
        {
            int result = 0;
            switch (operateType)
            {
                case OperateType.Add:
                    result = dbCMD.AddPolicyRemark(rateRemark);
                    break;
                case OperateType.Modify:
                    result = dbCMD.UpdatePolicyRemark(rateRemark);
                    break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// 分页查询政策备注列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        public PolicyRemarkSearchResponse PolicyRemarkSearch(PolicyRemarkSearchRequest request)
        {
            PolicyRemarkSearchResponse response = new PolicyRemarkSearchResponse();
            try
            {
                response.Success = true;
                response.Paging.TotalCount = dbQuery.GetPolicyRemarkTotalCount(request);
                if (response.Paging.TotalCount > 0)
                {
                    List<PolicyRemark> list = dbQuery.GetPolicyRemarkPageData(request);
                    foreach (PolicyRemark rateRemark in list)
                    {
                        response.Data.Add(new PolicyRemarkDTO() { PolicyRemarkId = rateRemark.ID, ProviderId = rateRemark.AgentID, PolicyType = rateRemark.RateType, Remark = rateRemark.Info });
                    }
                }
            }
            catch (Exception ex)
            {
                //记录日志
                response.Success = false;
                response.ErrMsg = "获取数据失败。";
            }
            return response;
        }
    }
}
