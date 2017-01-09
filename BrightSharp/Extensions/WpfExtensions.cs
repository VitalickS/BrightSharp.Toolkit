using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BrightSharp.Extensions
{
    public static class WpfExtensions
    {
        public static DependencyObject GetParent(this DependencyObject obj, bool useLogicalTree = false)
        {
            if (!useLogicalTree && (obj is Visual || obj is Visual3D))
                return VisualTreeHelper.GetParent(obj) ?? LogicalTreeHelper.GetParent(obj);
            else
                return LogicalTreeHelper.GetParent(obj);
        }
        public static IEnumerable<T> Ancestors<T>(this DependencyObject obj, bool useLogicalTree = false) where T : DependencyObject
        {
            if (obj == null) yield break;

            var x = GetParent(obj, useLogicalTree);

            while (x != null && !(x is T))
            {
                x = GetParent(x, useLogicalTree);
            }
            if (x != null)
                yield return x as T;
            else
                yield break;
            foreach (var item in Ancestors<T>(x, useLogicalTree))
            {
                yield return item;
            }
        }

        public static T FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
        {
            return Ancestors<T>(obj).FirstOrDefault();
        }
        public static Panel GetItemsPanel(DependencyObject itemsControl)
        {
            if (itemsControl is Panel) return (Panel)itemsControl;
            ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(itemsControl);
            var itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;
            return itemsPanel;
        }
        public static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }

}
