using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.CTI.DB.JinRi
{
    public class tblWebConfigFacade
    {
        tblWebConfigQuery query = new tblWebConfigQuery();

        public WebConfigRecord GetWebConfigRecord(string key)
        {
            return query.GetWebConfigRecord(key);
        }
    }
}
