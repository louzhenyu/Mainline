using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity.RequestDTO
{
    [DataContract]
    [Serializable]
    public class PolicyRemarkApplyRequest : RequestBase
    {
        [DataMember]
        public OperateType Operate { get; set; }

        [DataMember]
        public PolicyRemarkDTO PolicyRemark { get; set; }
    }
}
