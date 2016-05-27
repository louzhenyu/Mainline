using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using JFx;

namespace FX.CTI.DB
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public class ConnectionString
    {
        static string encryptKey = ConfigurationManager.AppSettings["EncryptKey"];

        #region FxDB
        private static string m_FxDB_SELECT;
        private static string m_FxDB_INSERT;

        /// <summary>
        /// Fx库只读连接
        /// </summary>
        public static string FxDB_SELECT
        {
            get
            {
                if (string.IsNullOrEmpty(m_FxDB_SELECT))
                {
                    m_FxDB_SELECT = JinRiEncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["FxDB_SELECT"].ConnectionString, encryptKey);
                }
                return m_FxDB_SELECT;
            }
        }

        /// <summary>
        /// FxDB库写连接
        /// </summary>
        public static string FxDB_INSERT
        {
            get
            {
                if (string.IsNullOrEmpty(m_FxDB_INSERT))
                {
                    m_FxDB_INSERT = JinRiEncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["FxDB_INSERT"].ConnectionString, encryptKey);
                }
                return m_FxDB_INSERT;
            }
        }
        #endregion
        #region JinRi
        private static string m_JinRi_SELECT;

        /// <summary>
        /// Fx库只读连接
        /// </summary>
        public static string JinRi_SELECT
        {
            get
            {
                if (string.IsNullOrEmpty(m_JinRi_SELECT))
                {
                    m_JinRi_SELECT = JinRiEncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["JinRi_SELECT"].ConnectionString, encryptKey);
                }
                return m_JinRi_SELECT;
            }
        }
        #endregion
    }
}
