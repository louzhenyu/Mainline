using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using ServiceContract.ResponseDTO;
using ServiceStack;

namespace ServiceContract.RequestDTO
{
    [DataContract]
    public class GetProductList : IReturn<ProductListResponse>
    {
    }

    [DataContract]
    public class GetProduct : IReturn<ProductResponse>
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
    }
}
