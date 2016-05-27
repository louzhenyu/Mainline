using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity.RequestDTO
{
    [DataContract]
    [Serializable]
    public class PolicyRemarkSearchRequest : RequestBase
    {
        public PolicyRemarkSearchRequest()
        {
            Paging = new RequestPaging();
        }
        [DataMember]
        public RequestPaging Paging { get; set; }

        [DataMember]
        public int ProviderId { get; set; }

        [DataMember]
        public int PolicyType { get; set; }
    }
}
