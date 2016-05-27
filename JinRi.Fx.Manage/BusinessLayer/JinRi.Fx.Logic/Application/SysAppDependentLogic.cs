using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System.Collections.Generic;

namespace JinRi.Fx.Logic
{
    /// <summary>
    /// 应用依赖关系
    /// </summary>
    public class SysAppDependentLogic
    {
        SysAppDependentDal appDependentDal = new SysAppDependentDal();
        public int BindingDependent(int appId, int dependentId)
        {
            return appDependentDal.BindingDependent(appId, dependentId);
        }
        public int DeleteDependent(int appId, int dependentId)
        {
            return appDependentDal.DeleteDependent(appId, dependentId);
        }
        public IEnumerable<SysAppDependentEntity> GetAppDependentList(int id = -1)
        {
            return appDependentDal.GetAppDependentList(id);
        }
    }
}
