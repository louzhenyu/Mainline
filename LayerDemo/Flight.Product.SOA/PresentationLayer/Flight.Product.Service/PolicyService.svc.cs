using Flight.Product.Business;
using Flight.Product.DBEntity;
using Flight.Product.Entity.RequestDTO;
using Flight.Product.Entity.ResponseDTO;
using System.Collections.Generic;

namespace Flight.Product.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“PolicyService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 PolicyService.svc 或 PolicyService.svc.cs，然后开始调试。
    public class PolicyService : IPolicyService
    {
        PolicyRemarkLogic rateRemarkLogic = new PolicyRemarkLogic();
        public string GetString()
        {
            return "OK";
        }

        public PolicyRemarkApplyResponse PolicyRemarkApply(PolicyRemarkApplyRequest request)
        {
            return rateRemarkLogic.PolicyRemarkApply(request);
        }
        public PolicyRemarkSearchResponse PolicyRemarkSearch(PolicyRemarkSearchRequest request)
        {
            return rateRemarkLogic.PolicyRemarkSearch(request);
        }
    }
}
