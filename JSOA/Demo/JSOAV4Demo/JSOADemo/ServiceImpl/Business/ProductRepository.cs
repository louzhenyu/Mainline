using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract;
using ServiceContract.RequestDTO;
using ServiceContract.ResponseDTO;

namespace ServiceImpl.Business
{
    public class ProductRepository : IProductRepository
    {
        private static readonly ProductListResponse productList = new ProductListResponse();

        public ProductRepository()
        {
            if (productList.ProductList == null || !productList.ProductList.Any())
            {
                productList.ProductList = new List<ProductResponse>();

                for (int i = 1; i < 51; i++)
                {
                    productList.ProductList.Add(new ProductResponse()
                    {
                        Id = i,
                        Name = "产品_" + i,
                        StatusCode = StatusCode.Active
                    });
                }
            }
        }

        public ProductListResponse GetProductList()
        {
            return productList;
        }

        public ProductResponse GetProduct(GetProduct request)
        {
            ProductResponse result = productList.ProductList.FirstOrDefault(x => x.Id == request.Id);
            if (result != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Name) && !result.Name.Equals(request.Name))
                {
                    return null;
                }
            }

            return result;
        }

        public ProductResponse GetProduct(int productId)
        {
            return productList.ProductList.FirstOrDefault(x => x.Id == productId);
        }
    }
}
