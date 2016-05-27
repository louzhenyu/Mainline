using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace JZooKeeperViewer.Common
{
    //added by Yang Li
    public partial class Common
    {
        public static void SelectItem(TreeView treeView, object searchValue)
        {
            TreeViewItem thisItem = treeView.ItemContainerGenerator.ContainerFromItem(searchValue) as TreeViewItem;
            if (thisItem != null)
            {
                thisItem.IsSelected = true;
                return;
            }

            for (int i = 0; i < treeView.Items.Count; i++)
            {
                SelectItem(searchValue, treeView.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem);
            }
        }

        private static bool SelectItem(object o, TreeViewItem parentItem)
        {
            if (parentItem == null)
                return false;

            bool isExpanded = parentItem.IsExpanded;
            if (!isExpanded)
            {
                parentItem.IsExpanded = true;
                parentItem.UpdateLayout();
            }

            TreeViewItem item = parentItem.ItemContainerGenerator.ContainerFromItem(o) as TreeViewItem;
            if (item != null)
            {
                item.IsSelected = true;
                return true;
            }

            bool wasFound = false;
            for (int i = 0; i < parentItem.Items.Count; i++)
            {
                TreeViewItem itm = parentItem.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                var found = SelectItem(o, itm);
                if (!found)
                    itm.IsExpanded = false;
                else
                    wasFound = true;
            }

            return wasFound;
        }        
    }
}
