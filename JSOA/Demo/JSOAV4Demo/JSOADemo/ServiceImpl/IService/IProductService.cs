using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.RequestDTO;
using ServiceContract.ResponseDTO;

namespace ServiceImpl.IService
{
    public interface IProductService
    {
        //获取产品列表：
        ProductListResponse Get(GetProductList request);
        //获取指定产品详情：
        ProductResponse Get(GetProduct request);
    }
}
