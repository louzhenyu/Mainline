using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System.Collections.Generic;

namespace JinRi.Fx.Logic
{
    /// <summary>
    /// 应用类型数据访问类
    /// </summary>
    public class SysAppTypeLogic
    {
        SysAppTypeDal appTypeDal = new SysAppTypeDal();
        /// <summary>
        /// 获取应用类型列表
        /// </summary>
        /// <param name="typeName">应用类型名称</param>
        /// <returns></returns>
        public IEnumerable<SysAppTypeEntity> GetAppTypeList(string typeName = "", PageItem pageItem = null)
        {
            return appTypeDal.GetAppTypeList(typeName, pageItem);
        }
        /// <summary>
        /// 获取应用类型信息
        /// </summary>
        /// <param name="TypeName">应用类型名称</param>
        /// <returns></returns>
        public SysAppTypeEntity GetAppTypeInfo(int AppTypeId)
        {
            return appTypeDal.GetAppTypeInfo(AppTypeId);
        }

        public int UpdateAppType(SysAppTypeEntity model)
        {
            return appTypeDal.UpdateAppType(model);
        }
        public int AddAppType(SysAppTypeEntity model)
        {
            return appTypeDal.AddAppType(model);
        }

        public int DeleteAppTypeList(List<int> ids)
        {
            return appTypeDal.DeleteAppTypeList(ids);
        }
    }
}
