using Flight.Product.DB;
using Flight.Product.DBEntity;
using Flight.Product.Entity.RequestDTO;
using Flight.Product.Entity.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight.Product.Business
{
    public class PolicyRemarkLogic
    {
        static readonly JinRiRateDBFacade JinRiRateFacade = new JinRiRateDBFacade();

        public PolicyRemarkApplyResponse PolicyRemarkApply(PolicyRemarkApplyRequest request)
        {
            PolicyRemarkApplyResponse response = new PolicyRemarkApplyResponse();
            response.Success = false;
            if (string.IsNullOrEmpty(request.PolicyRemark.Remark))
            {
                response.ErrMsg = "无效的备注信息。";
                return response;
            }
            if (request.PolicyRemark.ProviderId <= 0)
            {
                response.ErrMsg = "无效的供应商编号。";
                return response;
            }
            PolicyRemark rateRemark = new PolicyRemark();
            rateRemark.ID = request.PolicyRemark.PolicyRemarkId;
            rateRemark.AgentID = request.PolicyRemark.ProviderId;
            rateRemark.Info = request.PolicyRemark.Remark;
            rateRemark.RateType = request.PolicyRemark.PolicyType;
            int result = JinRiRateFacade.PolicyRemarkSave(request.Operate, rateRemark);
            response.Success = result > 0;
            if (result == 0)
            {
                response.ErrMsg = "未知错误。";
            }
            return response;
        }

        public PolicyRemarkSearchResponse PolicyRemarkSearch(PolicyRemarkSearchRequest request)
        {
            PolicyRemarkSearchResponse response = new PolicyRemarkSearchResponse();
            response.Success = false;
            if (request.Paging == null)
            {
                response.ErrMsg = "无效的查询参数，Paging不能为空。";
                return response;
            }
            if (request.Paging.PageSize <= 0)
            {
                response.ErrMsg = "无效的查询参数Paging.PageSize。";
                return response;
            }
            response = JinRiRateFacade.PolicyRemarkSearch(request);
            response.Paging.PageSize = request.Paging.PageSize;
            response.Paging.PageIndex = request.Paging.PageIndex;
            return response;
        }
    }
}
