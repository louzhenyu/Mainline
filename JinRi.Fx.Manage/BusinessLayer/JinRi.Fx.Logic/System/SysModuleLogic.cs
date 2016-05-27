using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Logic
{
    public class SysModuleLogic
    {
        SysModuleDal moduleDal = new SysModuleDal();
        /// <summary>
        /// 获取一个系统模块列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysModule> GetModuleList(int status = -1, string moduleName = "", int systemId = -1, PageItem pageItem = null)
        {
            return moduleDal.GetModuleList(moduleName, systemId, status, pageItem);
        }
        /// <summary>
        /// 获取单个模块信息
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <returns>NULL未获取到菜单信息，其他返回相应的菜单信息</returns>
        public SysModule GetModuleInfo(int moduleId)
        {
            return moduleDal.GetModuleInfo(moduleId);
        }
        public int UpdateModule(SysModule model)
        {
            return moduleDal.UpdateModule(model);
        }
        public int DeleteModuleList(List<int> ids)
        {
            return moduleDal.DeleteModuleList(ids);
        }
        public int AddModule(SysModule model)
        {
            return moduleDal.AddModule(model);
        }
    }
}
