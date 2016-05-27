using Flight.Provider.ProductWS.PolicyServiceNS;
using Flight.Provider.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight.Provider.ProductWS
{
    public class PolicyService
    {
        PolicyServiceNS.PolicyServiceClient client = new PolicyServiceNS.PolicyServiceClient();

        public Flight.Provider.Entity.Response.PolicyRemarkSearchResponse PolicyRemarkSearch(Flight.Provider.Entity.Request.PolicyRemarkSearchRequest requestEntity)
        {
            PolicyRemarkSearchResponse response = client.PolicyRemarkSearch(ConvertPolicyRemarkSearchRequest(requestEntity));
            return ConvertPolicyRemarkSearchResponse(response);
        }

        /// <summary>
        /// 请求对象转换成服务定义的DTO
        /// </summary>
        /// <param name="requestEntity">请求对象</param>
        /// <returns>DTO</returns>
        PolicyRemarkSearchRequest ConvertPolicyRemarkSearchRequest(Flight.Provider.Entity.Request.PolicyRemarkSearchRequest requestEntity)
        {
            return new PolicyRemarkSearchRequest()
            {
                AppID = requestEntity.AppID,
                Paging = new RequestPaging() { PageIndex = requestEntity.Paging.PageIndex, PageSize = requestEntity.Paging.PageSize },
                PolicyType = requestEntity.PolicyType,
                ProviderId = requestEntity.ProviderId
            };
        }

        /// <summary>
        /// 将服务返回的DTO转换成内部的Entity
        /// </summary>
        /// <param name="responseDTO">DTO</param>
        /// <returns>Entity</returns>
        Flight.Provider.Entity.Response.PolicyRemarkSearchResponse ConvertPolicyRemarkSearchResponse(PolicyRemarkSearchResponse responseDTO)
        {
            Entity.Response.PolicyRemarkSearchResponse responseEntity = new Entity.Response.PolicyRemarkSearchResponse()
            {
                Success = responseDTO.Success,
                ErrMsg = responseDTO.ErrMsg,
                Paging = new Entity.ResponsePaging()
                {
                    PageIndex = responseDTO.Paging.PageIndex,
                    PageSize = responseDTO.Paging.PageSize,
                    TotalCount = responseDTO.Paging.TotalCount
                }
            };

            foreach (PolicyRemarkDTO dto in responseDTO.Data)
            {
                responseEntity.Data.Add(new Entity.Request.PolicyRemarkDTO() { PolicyRemarkId = dto.PolicyRemarkId, PolicyType = dto.PolicyType, ProviderId = dto.ProviderId, Remark = dto.Remark });
            }
            return responseEntity;
        }

        public void Test(string para1, string para2, string para3)
        {
            //将para1、para2、para3转换成Service定义的RequestDTO
            //调用服务提供的方法
        }
    }
}
