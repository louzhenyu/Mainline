using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Logic
{
    public class SysRoleLogic
    {
        SysRoleDal roleDal = new SysRoleDal();
        /// <summary>
        /// 获取一个系统角色列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysRole> GetRoleList(int status = -1, string roleName = "", int systemId = -1, PageItem pageItem = null)
        {
            return roleDal.GetRoleList(roleName, systemId, status, pageItem);
        }
        /// <summary>
        /// 获取单个角色信息
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>NULL未获取到角色信息，其他返回相应的角色信息</returns>
        public SysRole GetRoleInfo(int roleId)
        {
            return roleDal.GetRoleInfo(roleId);
        }
        public int UpdateRole(SysRole model)
        {
            return roleDal.UpdateRole(model);
        }
        public int DeleteRoleList(List<int> ids)
        {
            return roleDal.DeleteRoleList(ids);
        }
        public int AddRole(SysRole model)
        {
            return roleDal.AddRole(model);
        }
    }
}
