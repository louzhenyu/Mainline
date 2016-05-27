using JinRi.Fx.Entity;
using JinRi.Fx.Logic;
using JinRi.Fx.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinRi.Fx.WebUI.Controllers.Home
{
    [UserAuthentication]
    public class HomeController : ControllerBaseAdmin
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Header()
        {
            return View();
        }
        public ActionResult Footer()
        {
            return View();
        }
        public ActionResult Menu()
        {
            SysModuleLogic moduleLogic = new SysModuleLogic();
            SysMenuLogic menuLogic = new SysMenuLogic();
            List<SysModule> moduleList = moduleLogic.GetModuleList(0).ToList<SysModule>();
            List<SysMenu> menuList = menuLogic.GetUserMenuList(this.WorkContext.CurrentUser.UserId).ToList<SysMenu>();

            ViewBag.ModuleList = moduleList?? new List<SysModule>();
            ViewBag.MenuList = menuList ?? new List<SysMenu>();
            return View();
        }
    }
}
