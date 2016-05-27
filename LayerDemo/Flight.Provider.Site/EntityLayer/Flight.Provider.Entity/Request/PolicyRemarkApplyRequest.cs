using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity.Request
{
    
    [Serializable]
    public class PolicyRemarkApplyRequest : RequestBase
    {
        
        public OperateType Operate { get; set; }

        
        public PolicyRemarkDTO PolicyRemark { get; set; }
    }
}
