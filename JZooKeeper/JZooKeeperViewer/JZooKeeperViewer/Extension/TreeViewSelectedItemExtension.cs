using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JZooKeeperViewer.Extension
{
    public class TreeViewSelectedItemExtension
    {
        public static readonly DependencyProperty TreeViewSelectedItemProperty =
            DependencyProperty.RegisterAttached("TreeViewSelectedItem", typeof(object), typeof(TreeViewSelectedItemExtension), new PropertyMetadata(new object(), TreeViewSelectedItemChanged));

        static void TreeViewSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView == null)
            {
                return;
            }

            treeView.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(treeView_SelectedItemChanged);
            treeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(treeView_SelectedItemChanged);

            //以下代码可不加
            //modified by Yang Li
            Common.Common.SelectItem(treeView, e.NewValue);
            //TreeViewItem thisItem = treeView.ItemContainerGenerator.ContainerFromItem(e.NewValue) as TreeViewItem;
            //if (thisItem != null)
            //{
            //    thisItem.IsSelected = true;
            //    return;
            //}

            //for (int i = 0; i < treeView.Items.Count; i++)
            //    SelectItem(e.NewValue, treeView.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem);
            //以上代码可不加

        }

        static void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = sender as TreeView;
            SetTreeViewSelectedItem(treeView, e.NewValue);
        }

        public static object GetTreeViewSelectedItem(DependencyObject obj)
        {
            return (object)obj.GetValue(TreeViewSelectedItemProperty);
        }

        public static void SetTreeViewSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(TreeViewSelectedItemProperty, value);
        }
    } 
}
