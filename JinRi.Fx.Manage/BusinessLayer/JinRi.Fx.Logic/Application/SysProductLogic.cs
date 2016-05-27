using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System.Collections.Generic;

namespace JinRi.Fx.Logic
{
    /// <summary>
    /// 产品线
    /// </summary>
    public class SysProductLogic
    {
        SysProductDal sysProductDal = new SysProductDal();
        /// <summary>
        /// 获取一个产品线列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysProductEntity> GetProductList(string productName = "", PageItem pageItem = null)
        {
            return sysProductDal.GetProductList(productName, pageItem);
        }
        /// <summary>
        /// 获取单个产品线信息
        /// </summary>
        /// <param name="ProductId">产品线编号</param>
        /// <returns>NULL未获取到产品线信息，其他返回相应的产品线信息</returns>
        public SysProductEntity GetProductInfo(int ProductId)
        {
            return sysProductDal.GetProductInfo(ProductId);
        }

        public int UpdateProduct(SysProductEntity model)
        {
            return sysProductDal.UpdateProduct(model);
        }
        public int AddProduct(SysProductEntity model)
        {
            return sysProductDal.AddProduct(model);
        }

        public int DeleteProductList(List<int> ids)
        {
            return sysProductDal.DeleteProductList(ids);
        }
    }
}
