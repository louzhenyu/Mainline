using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.RequestDTO;
using ServiceContract.ResponseDTO;

namespace ServiceImpl.Business
{
    public interface IProductRepository
    {
        ProductListResponse GetProductList();
        ProductResponse GetProduct(GetProduct request);
        ProductResponse GetProduct(int productId);
    }
}
