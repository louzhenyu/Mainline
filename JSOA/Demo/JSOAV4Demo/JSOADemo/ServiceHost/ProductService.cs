using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract.ResponseDTO;
using ServiceContract.RequestDTO;
using ServiceImpl.IService;
using ServiceImpl.Business;

using System.Net;
using ServiceStack;

namespace ServiceHost
{
    public class ProductService : Service, IProductService
    {
        private IProductRepository ProductRepository { get; set; }

        public ProductService()
        {
            ProductRepository = new ProductRepository();
        }

        //获取产品列表：
        public ProductListResponse Get(GetProductList request)
        {
            ProductListResponse result = ProductRepository.GetProductList();
            return result;
        }

        //获取指定产品详情：
        public ProductResponse Get(GetProduct request)
        {
            ProductResponse result = ProductRepository.GetProduct(request);
            if (result == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return default(ProductResponse);
            }

            return result;
        }
    }
}
