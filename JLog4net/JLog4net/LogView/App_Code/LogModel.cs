using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogView
{
    #region 查询结果集实体类
    ///<summary>
    /// 查询结果集实体类
    ///</summary>
    public class ResultSet
    {
        public ResultSet()
        {

        }

        public long resultCount
        {
            get;
            set;
        }

        public List<Log4NetEntiy> result
        {
            get;
            set;
        }
    }
    #endregion

    #region 日志实体类
    ///<summary>
    /// 日志实体类
    ///</summary>
    public class Log4NetEntiy
    {
        public object _id
        {
            get;
            set;
        }
        public Log4NetEntiy()
        {

        }

        public string level
        {
            get;
            set;
        }



        public string message
        {
            get;
            set;
        }


        public string ip
        {
            get;
            set;
        }

        public string appid
        {
            get;
            set;
        }


        //public string fileName
        //{
        //    get;
        //    set;
        //}

        public DateTime timestamp
        {
            get;
            set;
        }

        public string method
        { get; set; }

        public string traceId
        { get; set; }

        public string className
        {
            get;
            set;
        }

    }
    #endregion

}