using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity
{
    /// <summary>
    /// 乘机人
    /// </summary>
    [Serializable]
    public class Passenger
    {
        /// <summary>
        /// 乘机人姓名
        /// </summary>
        public string name { get; set; }

        ///// <summary>
        ///// 性别
        ///// </summary>
        //public EtermCommand.Sexual sexual {get;set;}

        /// <summary>
        /// 证件类型
        /// </summary>
        public EtermCommand.IDtype idtype { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string cardno { get; set; }

        /// <summary>
        /// 乘客类型
        /// </summary>
        public EtermCommand.PassengerType PassType { get; set; }

        /// <summary>
        /// 乘客名拼音（婴儿）使用
        /// </summary>
        public string Ename { get; set; }
        /// <summary>
        /// 婴儿出生日期
        /// </summary>
        public DateTime BabyBirthday { get; set; }

        /// <summary>
        /// 儿童出生日期
        /// </summary>
        public DateTime ChildBirthday { get; set; }

        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNo { get; set; }

        public Passenger()
        {
            name = string.Empty;
            //sexual = EtermCommand.Sexual.female;
            idtype = EtermCommand.IDtype.NotSet;
            cardno = string.Empty;
            PassType = EtermCommand.PassengerType.NotSet;
            Ename = string.Empty;
            BabyBirthday = DateTime.MinValue;
            ChildBirthday = DateTime.MinValue;
            TicketNo = string.Empty;            
        }
    }
}
