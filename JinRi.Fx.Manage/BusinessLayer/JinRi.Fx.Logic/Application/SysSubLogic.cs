using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System.Collections.Generic;

namespace JinRi.Fx.Logic
{
    public class SysSubLogic
    {
        SysSubDal subSystemDal = new SysSubDal();
        /// <summary>
        /// 获取子系统列表
        /// </summary>
        /// <param name="productId">负数表示不限制</param>
        /// <param name="systemName">子系统名称</param>
        /// <returns></returns>
        public IEnumerable<SysSubEntity> GetSubSystemList(int productId = -1, string systemName = "", PageItem pageItem = null)
        {
            return subSystemDal.GetSubSystemList(productId, systemName, pageItem);
        }
        /// <summary>
        /// 获取单个子系统信息
        /// </summary>
        /// <param name="ProductId">子系统编号</param>
        /// <returns>NULL未获取到子系统信息，其他返回相应的子系统信息</returns>
        public SysSubEntity GetSubSystemInfo(int subSystemId)
        {
            return subSystemDal.GetSubSystemInfo(subSystemId);
        }

        public int UpdateSubSystem(SysSubEntity model)
        {
            return subSystemDal.UpdateSubSystem(model);
        }
        public int AddSubSystem(SysSubEntity model)
        {
            return subSystemDal.AddSubSystem(model);
        }

        public int DeleteSubSystemList(List<int> ids)
        {
            return subSystemDal.DeleteSubSystemList(ids);
        }
    }
}
