using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// ZNode节点类
    /// </summary>
    public class TreeZNode
    {
        public TreeZNode()
        {
            this.joinNodePath = this.NodePath == "/" ? string.Empty : this.NodePath;
            this.ChildNodeList = new List<TreeZNode>();
        }

        public TreeZNode(int id, bool isParent, string nodePath, string nodeName, int pId)
            : this()
        {
            this.id = id;
            this.isParent = isParent;
            this.NodePath = nodePath;
            this.joinNodePath = nodePath == "/" ? string.Empty : nodePath;
            this.NodeName = nodeName;
            this.name = nodeName;
            this.pId = pId;          
        }

        public int id { get; set; }

        public string name { get; set; }

        public int pId { get; set; }

        public bool isParent { get; set; }

        public string NodePath { get; set; }

        public string joinNodePath = string.Empty;

        [Display(Name = "节点名")]
        [Required(ErrorMessage = "节点名不能为空")]
        public string NodeName { get; set; }       

        public List<TreeZNode> ChildNodeList { get; set; }        
    }
}
