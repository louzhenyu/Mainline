using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Logic.DD
{
    public class LogLogic
    {
        private readonly LogDal _logDal = new LogDal();
        public void Add(Log log)
        {
            _logDal.Add(log);
        }
        public IEnumerable<Log> Query(string dealer, string beginTime, string endTime, string server, string db, string table, string col, int pageIndex, int pageSize, out int totalRecords)
        {
            return _logDal.Query(dealer, beginTime, endTime, server, db, table, col, pageIndex, pageSize, out totalRecords);
        }
    }
}
