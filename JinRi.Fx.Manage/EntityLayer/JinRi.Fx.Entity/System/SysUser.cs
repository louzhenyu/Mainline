using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 用户信息实体
    /// </summary>
    public class SysUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [RegularExpression(@"[A-Za-z]{4,20}", ErrorMessage = "用户名格式错误，4-20位字母+数字")]
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 角色代码
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 用户状态：0正常，1禁用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegDate { get; set; }
    }
}
