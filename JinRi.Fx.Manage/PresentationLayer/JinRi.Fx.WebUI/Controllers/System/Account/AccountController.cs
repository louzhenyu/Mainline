using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using JinRi.Fx.Web;
using JinRi.Fx.Entity;
using JinRi.Fx.Logic;

namespace JinRi.Fx.WebUI.Controllers.Account
{
    public class AccountController : ControllerBaseAdmin
    {
        SysUserLogic logic = new SysUserLogic();
        //默认跳转到登录
        // GET: /Account/
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Login()
        {
            return View();
        }
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("error", "用户名或密码错误");
                return View(); 
            }
            username = username.Trim();
            var loginInfo = logic.GetUserInfo(username);
            if (loginInfo != null && loginInfo.PassWord.Equals(password))
            {
                switch (loginInfo.Status)
                {
                    case 0:
                        LoginUserInfo loginUser = new LoginUserInfo() { UserId = loginInfo.UserId, RoleId = loginInfo.RoleId, UserName = loginInfo.UserName, RealName = loginInfo.RealName };
                        string UserData = JinRi.Fx.Utility.JsonHelper.DataContractJsonSerialize<LoginUserInfo>(loginUser).Replace("\0", "").Trim();
                        //保存序列化的用户信息票据
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, loginInfo.UserName, DateTime.Now, DateTime.Now.AddHours(1), false, UserData);
                        //将票据加密并存到cookie
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                        //输出到浏览器
                        this.HttpContext.Response.Cookies.Add(cookie);
                        return RedirectToAction("Index","Home");
                    case 1:
                        ModelState.AddModelError("error", "登录失败，用户已被禁用。");
                        return View();
                    default:
                        ModelState.AddModelError("error", "无效的用户状态。");
                        return View();
                }
            }
            else
            {
                ModelState.AddModelError("error", "用户名或密码错误");
                return View();
            }
        }
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            if (Request.IsAuthenticated)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                authCookie.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(authCookie);
            }
            return RedirectToAction("Login","Account");
        }

        [UserAuthentication]
        public ActionResult Home()
        {
            return View(); 
        }

        [UserAuthentication]
        public ActionResult ModifyPwd()
        {
            SysUser user = logic.GetUserInfo(this.WorkContext.CurrentUser.UserId);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Login", "Account");
        }

        [UserAuthentication]
        [HttpPost]
        public ActionResult ModifyPwd(FormCollection collection)
        {
            try
            {
                int userId = Convert.ToInt32(collection["UserId"]);
                string password = collection["Password"];
                string newPassowrd = collection["NewPassword"];
                string newPassowrd2 = collection["NewPassword2"];
                if (newPassowrd != newPassowrd2)
                {
                    return this.Back("新密码不一致！");
                }
                if (newPassowrd.Length < 4)
                {
                    return this.Back("新密码长度太短！");
                }
                SysUser user = logic.GetUserInfo(userId);
                if (user != null && user.PassWord.Equals(password))
                {
                    logic.ModifyPassWord(user.UserId, newPassowrd);
                    return this.RefreshParent();
                }
                else
                {
                    return this.Back("用户名密码不匹配，修改密码失败！");
                }
            }
            catch (Exception ex)
            {
                return this.Back("修改密码发生异常。" + ex.Message);
            }
        }

        [UserAuthentication]
        public ActionResult AccountInfo()
        {
            SysUser user = logic.GetUserInfo(this.WorkContext.CurrentUser.UserId);
            if (user != null)
            {
                Dictionary<int, SysRole> dictionaryRole = new Dictionary<int, SysRole>();
                SysRoleLogic roleLogic = new SysRoleLogic();
                List<SysRole> roleList = roleLogic.GetRoleList().ToList<SysRole>();
                if (roleList != null)
                {
                    for (int index = 0; index < roleList.Count; index++)
                    {
                        dictionaryRole.Add(roleList[index].RoleId, roleList[index]);
                    }
                }
                ViewBag.DictionaryRole = dictionaryRole;

                return View(user);
            }
            return RedirectToAction("Login", "Account");
        }        
    }
}