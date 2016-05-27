using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 通用解析方法基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    [Serializable]
    public class ParserBase<T, R>
        where T : new()
        where R : new()
    {
        public virtual T Request { get; set; }

        public virtual R Response { get; set; }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>若请求参数验证通过，则返回Eterm指令；否则返回为空。</returns>
        public virtual string ParseCmd(T request)
        {
            return string.Empty;
        }

        /// <summary>
        /// 解析Eterm指令返回结果
        /// </summary>
        /// <param name="cmdResult">Eterm指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public virtual R ParseCmdResult(string cmdResult)
        {
            return new R();
        }

        #region Helper

        protected internal virtual bool ValidRequest()
        {
            return false;
        }

        protected internal virtual bool ValidCmdResult(string cmdResult)
        {
            return false;
        }

        #endregion
    }
}
