using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Logic
{
    public class SysRoleRightLogic
    {
        SysRoleRightDal roleRightDal = new SysRoleRightDal();
        /// <summary>
        /// 获取一个系统角色权限列表
        /// </summary>     
        /// <returns></returns>
        public IEnumerable<SysRoleRight> GetRoleRightList(int roleId, bool isAppId = false, int userId = -1, PageItem pageItem = null)
        {
            return roleRightDal.GetRoleRightList(roleId, isAppId, userId, pageItem);
        }

        public SysRoleRight GetRoleRight(bool isMenuRight, int roleId, int userId, int menuId, string menuName, int appId)
        {
            return roleRightDal.GetRoleRight(isMenuRight, roleId, userId, menuId, menuName, appId);
        }

        public int UpdateRoleRight(SysRoleRight model)
        {
            return roleRightDal.UpdateRoleRight(model);
        }
        public int DeleteRoleRightList(List<int> ids)
        {
            return roleRightDal.DeleteRoleRightList(ids);
        }
        public int AddRoleRight(SysRoleRight model)
        {
            return roleRightDal.AddRoleRight(model);
        }
        public bool SaveRoleRight(int roleId, List<SysRoleRight> list, bool isAppId = false, int userId = 0)
        {
            return roleRightDal.SaveRoleRight(roleId, list, isAppId, userId);
        }
    }
}
