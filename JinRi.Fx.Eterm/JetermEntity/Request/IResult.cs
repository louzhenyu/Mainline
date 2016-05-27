using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Bussiness
{
    public interface IResult<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        bool state { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        List<T> result { get; set; }
    }
}
