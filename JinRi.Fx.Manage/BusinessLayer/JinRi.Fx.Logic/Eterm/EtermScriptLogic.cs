using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;

namespace JinRi.Fx.Logic
{
    public class EtermScriptLogic
    {
        EtermScriptDal dal = new EtermScriptDal();

        public EtermScript GetEtermScript(int etermScriptId)
        {
            return dal.GetEtermScript(etermScriptId);
        }

        public EtermScript GetEtermScript(string methodName)
        {
            return dal.GetEtermScript(methodName);
        }

        public IEnumerable<EtermScript> GetEtermScriptPageList(EtermScript searchCondition, PageItem pageItem)
        {
            return dal.GetEtermScriptList(searchCondition, pageItem);
        }

        public int AddEtermScript(EtermScript item)
        {
            return dal.AddEtermScript(item);
        }

        public int UpdateEtermScript(EtermScript item)
        {
            return dal.UpdateEtermScript(item);
        }

        public int DeleteEtermScriptList(List<EtermScript> list)
        {
            return dal.DeleteEtermScriptList(list);
        }
    }
}
