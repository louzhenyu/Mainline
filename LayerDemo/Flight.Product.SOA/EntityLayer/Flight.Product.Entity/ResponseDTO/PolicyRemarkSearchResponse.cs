using Flight.Product.Entity.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity.ResponseDTO
{
    [DataContract]
    [Serializable]
    public class PolicyRemarkSearchResponse : ResponseBase
    {
        public PolicyRemarkSearchResponse()
        {
            Paging = new ResponsePaging();
            Data = new List<PolicyRemarkDTO>();
        }

        [DataMember]
        public ResponsePaging Paging { get; set; }
        [DataMember]
        public List<PolicyRemarkDTO> Data { get; set; }
    }
}
