using JFx;
using System.Configuration;

namespace Flight.Provider.DB
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public class ConnectionString
    {
        static string encryptKey = ConfigurationManager.AppSettings["EncryptKey"];

        #region JinRiDB
        private static string m_JinRiDB_SELECT;

        /// <summary>
        /// JinRi库只读连接
        /// </summary>
        public static string JinRiDB_SELECT
        {
            get
            {
                if (string.IsNullOrEmpty(m_JinRiDB_SELECT))
                {
                    m_JinRiDB_SELECT = JinRiEncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["JinRiDB_SELECT"].ConnectionString, encryptKey);
                }
                return m_JinRiDB_SELECT;
            }
        }
        private static string m_JinRiDB_INSERT;

        /// <summary>
        /// JinRi库写连接
        /// </summary>
        public static string JinRiDB_INSERT
        {
            get
            {
                if (string.IsNullOrEmpty(m_JinRiDB_INSERT))
                {
                    m_JinRiDB_INSERT = JinRiEncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["JinRiDB_INSERT"].ConnectionString, encryptKey);
                }
                return m_JinRiDB_INSERT;
            }
        }
        #endregion

        #region JinRiRateDB
        private static string m_JinRiRateDB_SELECT;

        /// <summary>
        /// JinRiRate库只读连接
        /// </summary>
        public static string JinRiRateDB_SELECT
        {
            get
            {
                if (string.IsNullOrEmpty(m_JinRiRateDB_SELECT))
                {
                    m_JinRiRateDB_SELECT = JinRiEncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["JinRiRateDB_SELECT"].ConnectionString, encryptKey);
                }
                return m_JinRiRateDB_SELECT;
            }
        }
        private static string m_JinRiRateDB_INSERT;

        /// <summary>
        /// JinRiRate库写连接
        /// </summary>
        public static string JinRiRateDB_INSERT
        {
            get
            {
                if (string.IsNullOrEmpty(m_JinRiRateDB_INSERT))
                {
                    m_JinRiRateDB_INSERT = JinRiEncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["JinRiRateDB_INSERT"].ConnectionString, encryptKey);
                }
                return m_JinRiRateDB_INSERT;
            }
        }
        #endregion
    }
}
