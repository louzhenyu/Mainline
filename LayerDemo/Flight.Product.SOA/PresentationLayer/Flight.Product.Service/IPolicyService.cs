using Flight.Product.Entity.RequestDTO;
using Flight.Product.Entity.ResponseDTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace Flight.Product.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IPolicyService”。
    [ServiceContract]
    public interface IPolicyService
    {
        [OperationContract]
        string GetString();

        [OperationContract]
        PolicyRemarkApplyResponse PolicyRemarkApply(PolicyRemarkApplyRequest request);

        [OperationContract]
        PolicyRemarkSearchResponse PolicyRemarkSearch(PolicyRemarkSearchRequest request);
    }
}
