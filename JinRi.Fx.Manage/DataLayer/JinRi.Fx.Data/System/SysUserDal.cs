using JinRi.Fx.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using JinRi.Fx.Utility;

namespace JinRi.Fx.Data
{
    public class SysUserDal
    {
        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>NULL未获取到用户信息，其他返回相应的用户信息</returns>
        public SysUser GetUserInfo(string userName)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "SELECT TOP 1 UserId,UserName,Password,UserId,RoleId,Status,RealName,Tel,Email,RegDate FROM SysUser WITH(NOLOCK) WHERE UserName=@UserName";
                return conn.Query<SysUser>(sql, new { UserName = userName }).SingleOrDefault<SysUser>();
            }
        }
        /// <summary>
        /// 获取一个系统用户列表
        /// </summary>
        /// <param name="systemId">负数表示不限制</param>
        /// <param name="status">负数表示不限制</param>
        /// <returns></returns>
        public IEnumerable<SysUser> GetUserList(string userName, int roleId, int status, PageItem pageItem = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT UserId,UserName,Password,RoleId,Status,RealName,Tel,Email,RegDate FROM SysUser WHERE 1=1 ");
            if (!string.IsNullOrEmpty(userName))
            {
                sql.AppendFormat("AND UserName LIKE '%{0}%' ", userName);
            }
            if (roleId >= 0)
            {
                sql.AppendFormat("AND RoleId={0} ", roleId);
            }
            if (status >= 0)
            {
                sql.AppendFormat("AND Status={0} ", status);
            }
            sql.AppendFormat(" ORDER BY UserId ");
            return DapperHelper<SysUser>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>NULL未获取到用户信息，其他返回相应的用户信息</returns>
        public SysUser GetUserInfo(int userId)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "SELECT TOP 1 UserId,UserName,Password,RoleId,Status,RealName,Tel,Email,RegDate FROM SysUser WITH(NOLOCK) WHERE UserId=@UserId";
                return conn.Query<SysUser>(sql, new { UserId = userId }).SingleOrDefault<SysUser>();
            }
        }

        public int UpdateUser(SysUser model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "UPDATE SysUser SET RoleId=@RoleId,Status=@Status,RealName=@RealName,Tel=@Tel,Email=@Email WHERE UserId=@UserId";
                return conn.Execute(sql,
                    new
                    {
                        UserId = model.UserId,
                        RoleId = model.RoleId,
                        Status = model.Status,
                        RealName = model.RealName,
                        Tel = model.Tel,
                        Email = model.Email
                    });
            }
        }
        public int AddUser(SysUser model)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "INSERT INTO SysUser(UserName,Password,RoleId,Status,RealName,Tel,Email,RegDate) VALUES(@UserName,@Password,@RoleId,@Status,@RealName,@Tel,@Email,GETDATE())";
                return conn.Execute(sql,
                    new
                    {
                        UserName = model.UserName,
                        Password = "123456",
                        RoleId = model.RoleId,
                        Status = model.Status,
                        RealName = model.RealName,
                        Tel = model.Tel,
                        Email = model.Email
                    });
            }
        }

        public int ModifyPassWord(int userId, string newPassword)
        {
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = "UPDATE TOP(1) SysUser SET PassWord=@PassWord WHERE UserId=@UserId";
                return conn.Execute(sql,
                    new
                    {
                        UserId = userId,
                        PassWord = newPassword
                    });
            }
        }

        public int DeleteUserList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                string sql = string.Format("DELETE SysUser WHERE UserId IN ({0})", id.Trim(','));
                return conn.Execute(sql);
            }
        }
    }
}
