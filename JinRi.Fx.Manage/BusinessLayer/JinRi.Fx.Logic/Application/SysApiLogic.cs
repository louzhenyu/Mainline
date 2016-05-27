using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System.Collections.Generic;

namespace JinRi.Fx.Logic
{
    /// <summary>
    /// 接口注册
    /// </summary>
    public class SysApiLogic
    {
        SysApiDal sysApiDal = new SysApiDal();
        /// <summary>
        /// 获取一个接口注册列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysApiEntity> GetSysApiList(int appId, int status, string aipName, PageItem pageItem = null)
        {
            return sysApiDal.GetSysApiList(appId, status, aipName, pageItem);
        }
        /// <summary>
        /// 获取单个接口注册信息
        /// </summary>
        /// <param name="sysApiId">接口注册编号</param>
        /// <returns>NULL未获取到接口注册信息，其他返回相应的系统接口信息</returns>
        public SysApiEntity GetSysApiInfo(int sysApiId)
        {
            return sysApiDal.GetSysApiInfo(sysApiId);
        }

        public int UpdateSysApi(SysApiEntity model)
        {
            return sysApiDal.UpdateSysApi(model);
        }
        public int AddSysApi(SysApiEntity model)
        {
            return sysApiDal.AddSysApi(model);
        }

        public int DeleteSysApiList(List<int> ids)
        {
            return sysApiDal.DeleteSysApiList(ids);
        }
    }
}
