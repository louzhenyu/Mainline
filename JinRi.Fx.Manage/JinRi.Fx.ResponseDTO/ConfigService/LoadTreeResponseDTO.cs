using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JinRi.Fx.Entity;

namespace JinRi.Fx.ResponseDTO.ConfigService
{
    public class LoadTreeResponseDTO
    {
        public List<TreeZNode> ZNodeList = null;

        public bool IsSuccess = false;        

        public string NodeValue = string.Empty;

        public string Message = string.Empty;
    }
}
