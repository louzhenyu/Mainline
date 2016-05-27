using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    public enum CmdType
    {
        [EnumTitle("订位")]
        SS = 0,
        [EnumTitle("P特价")]
        SS_PAT = 1,
        [EnumTitle("RT编码")]
        /// <summary>
        /// RT编码
        /// </summary>
        RT = 2,
        [EnumTitle("DETR大编码")]
        /// <summary>
        /// DETR大编码
        /// </summary> 
        DETR_CN =3,
        [EnumTitle("DETR票号状态")]
        /// <summary>
        /// DETR票号 查票号状态
        /// </summary> 
        DETR_TN = 4,
        [EnumTitle("DETR票号,S")]
        /// <summary>
        /// DETR票号,S
        /// </summary> 
        DETR_TNS = 5,
        [EnumTitle("DETR票号,F")]
        /// <summary>
        /// DETR票号,F
        /// </summary> 
        DETR_TNF = 6,
        [EnumTitle("DETR票号,H")]
        /// <summary>
        /// DETR票号,H 查票号历史记录
        /// </summary> 
        DETR_TNH = 7,
        [EnumTitle("授权")]
        /// <summary>
        /// 授权
        /// </summary>
        RMK = 8,
        [EnumTitle("擦编码")]
        /// <summary>
        /// 擦编码
        /// </summary>
        XEPNR = 9,
        [EnumTitle("自动出票")]
        /// <summary>
        /// 自动出票
        /// </summary>
        AutoTicket = 10,
        [EnumTitle("AV查询")]
        /// <summary>
        /// AV查询
        /// </summary>
        AV = 11,
        [EnumTitle("AVH查询")]
        /// <summary>
        /// AVH查询
        /// </summary>
        AVH = 12
    }
    //婴儿项：
    //自动补位:
    //清Q：QT QS SC
    //BSP自动出票:
    //查航班：AVH
    //P价格
    //

    public enum ConfigLevel
    {
        [EnumTitle("A")]
        A= 1,
        [EnumTitle("B")]
        B = 2,
        [EnumTitle("C")]
        C= 3,
        [EnumTitle("D")]
        D = 4,
        [EnumTitle("E")]
        E = 5
    }
   
}
