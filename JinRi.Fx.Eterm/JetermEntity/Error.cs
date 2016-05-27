using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity
{
    /// <summary>
    /// 错误信息
    /// </summary>
    [Serializable]
    public class Error
    {
        public Error(EtermCommand.ERROR error)
        {
            ErrorCode = error;
        }

        /// <summary>
        /// 错误代码号
        /// </summary>
        public EtermCommand.ERROR ErrorCode { get; set; }

        private string _ErrorMessage = string.Empty;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return string.IsNullOrEmpty(_ErrorMessage) ? DescriptionAttribute.GetEnumDescrition<EtermCommand.ERROR>(ErrorCode) : _ErrorMessage;
            }
            set
            {
                _ErrorMessage = value;
            }
        }

        /// <summary>
        /// Eterm指令返回结果
        /// </summary>
        public string CmdResultBag { get; set; }

        /// <summary>
        /// JEtermClient内部记录的具体错误信息
        /// </summary>
        public string InnerDetailedErrorMessage { get; set; }
    }
}
