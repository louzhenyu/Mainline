using Flight.Provider.Entity.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity.Response
{
    [Serializable]
    public class PolicyRemarkSearchResponse : ResponseBase
    {
        public PolicyRemarkSearchResponse()
        {
            Paging = new ResponsePaging();
            Data = new List<PolicyRemarkDTO>();
        }
        public ResponsePaging Paging { get; set; }
        public List<PolicyRemarkDTO> Data { get; set; }
    }
}
