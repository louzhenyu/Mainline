using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.RequestDTO;
using JinRi.Fx.Utility;
using System.Collections.Generic;

namespace JinRi.Fx.Logic
{
    /// <summary>
    /// 应用管理
    /// </summary>
    public class SysApplicationLogic
    {
        SysApplicationDal sysApplicationDal = new SysApplicationDal();
        /// <summary>
        /// 获取应用列表
        /// </summary>      
        /// <param name="productIdList">1表示国内机票；2表示国际机票；3表示酒店；4表示公共服务；5表示框架；6表示手机</param>
        /// <returns></returns>
        public IEnumerable<SysApplicationEntity> GetSysApplicationList(int appId = -1, int subSystemId = -1, string appName = "", int appTypeId = -1, int status = -1, PageItem pageItem = null, List<int> productIdList = null, string owner = "")
        {
            return sysApplicationDal.GetSysApplicationList(appId, subSystemId, appName, appTypeId, status, pageItem, productIdList, owner);
        }
        public IEnumerable<SysApplicationEntity> GetCanBindApplication(DependentSearchRequest request, PageItem pageItem = null)
        {
            return sysApplicationDal.GetCanBindApplication(request, pageItem);
        }
        /// <summary>
        /// 获取单个应用信息
        /// </summary>
        /// <param name="appId">应用编号</param>
        /// <returns></returns>
        public SysApplicationEntity GetSysApplicationInfo(int appId)
        {
            return sysApplicationDal.GetSysApplicationInfo(appId);
        }

        public int UpdateSysApplication(SysApplicationEntity model)
        {
            return sysApplicationDal.UpdateSysApplication(model);
        }
        public int AddSysApplication(SysApplicationEntity model)
        {
            return sysApplicationDal.AddSysApplication(model);
        }

        public int DeleteApplicationList(List<int> ids)
        {
            return sysApplicationDal.DeleteApplicationList(ids);
        }
    }
}
