using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ServiceContract.ResponseDTO
{
    [DataContract]
    public class ProductListResponse
    {
        [DataMember(Order = 1)]
        public List<ProductResponse> ProductList { get; set; }
    }

    [DataContract]
    public class ProductResponse
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public StatusCode StatusCode { get; set; }
    }
}
