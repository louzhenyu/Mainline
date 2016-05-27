using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Logic
{
    public class SysUserLogic
    {
        Data.SysUserDal userDal = new Data.SysUserDal();

        /// <summary>
        /// 获取一个系统用户列表
        /// </summary>
        /// <param name="roleId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysUser> GetUserList(string userName, int roleId, int status,PageItem pageItem=null)
        {
            return userDal.GetUserList(userName, roleId, status, pageItem);
        }
        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>NULL未获取到用户信息，其他返回相应的用户信息</returns>
        public SysUser GetUserInfo(int userId)
        {
            return userDal.GetUserInfo(userId);
        }
        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>NULL未获取到用户信息，其他返回相应的用户信息</returns>
        public SysUser GetUserInfo(string userName)
        {
            return userDal.GetUserInfo(userName);
        }
        public int UpdateUser(SysUser model)
        {
            return userDal.UpdateUser(model);
        }
        public int DeleteUserList(List<int> ids)
        {
            return userDal.DeleteUserList(ids);
        }
        public int AddUser(SysUser model)
        {
            return userDal.AddUser(model);
        }
        public int ModifyPassWord(int userId, string newPassword)
        {
            return userDal.ModifyPassWord(userId, newPassword);
        }
    }
}
