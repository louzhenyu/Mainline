using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity.Request
{
    [Serializable]
    public class PolicyRemarkSearchRequest : RequestBase
    {
        public PolicyRemarkSearchRequest()
        {
            Paging = new RequestPaging();
        }
        public RequestPaging Paging { get; set; }

        public int ProviderId { get; set; }

        public int PolicyType { get; set; }
    }
}
