using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Logic
{
    public class SysMenuLogic
    {
        SysMenuDal menuDal = new SysMenuDal();
        public IEnumerable<SysMenu> GetMenuList(int status = -1, string menuName = "", int moduleId = -1, PageItem pageItem = null)
        {
            return menuDal.GetMenuList(menuName, moduleId, status, pageItem);
        }
        public IEnumerable<SysMenu> GetUserMenuList(int userId)
        {
            return menuDal.GetUserMenuList(userId);
        }       

        /// <summary>
        /// 获取单个菜单信息
        /// </summary>
        /// <param name="menuId">菜单编号</param>
        /// <returns>NULL未获取到菜单信息，其他返回相应的菜单信息</returns>
        public SysMenu GetMenuInfo(int menuId)
        {
            return menuDal.GetMenuInfo(menuId);
        }
        public int UpdateMenu(SysMenu model)
        {
            return menuDal.UpdateMenu(model);
        }
        public int DeleteMenuList(List<int> ids)
        {
            return menuDal.DeleteMenuList(ids);
        }
        public int AddMenu(SysMenu model)
        {
            return menuDal.AddMenu(model);
        }
    }
}
