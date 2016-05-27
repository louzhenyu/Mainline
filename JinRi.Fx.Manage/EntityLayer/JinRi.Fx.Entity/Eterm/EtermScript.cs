using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    public class EtermScript
    {
        public int EtermScriptID { get; set; }

        [Display(Name = "方法名")]
        [Required(ErrorMessage = "方法名不能为空")]
        public string MethodName { get; set; }

        [Display(Name = "脚本内容")]
        [Required(ErrorMessage = "脚本内容不能为空")]
        public string ScriptContent { get; set; }     

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public string FormatedRemark
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Remark))
                {
                    return string.Empty;
                }

                if (Remark.Length > 100)
                {
                    return string.Format("{0}...", Remark.Substring(0, 99));
                }

                return Remark;
            }
        }

        private DateTime updateTime = DateTime.Now;
        public DateTime UpdateTime
        {
            get
            {
                return updateTime;
            }
            set
            {
                updateTime = value;
            }
        }
    }
}
