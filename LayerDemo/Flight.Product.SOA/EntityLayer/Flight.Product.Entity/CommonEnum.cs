using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity
{
    /// <summary>
    /// 操作类型
    /// </summary>
    [DataContract]
    [Serializable]
    public enum OperateType
    {
        Add,
        Modify
    }
}
