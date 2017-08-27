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
        /// <summary>
        /// Returns Parent item of DependencyObject
        /// </summary>
        /// <param name="obj">Child object</param>
        /// <param name="useLogicalTree">Prefer LogicalTree when need.</param>
        /// <returns>Found item</returns>
        public static DependencyObject GetParent(this DependencyObject obj, bool useLogicalTree = false)
        {
            if (!useLogicalTree && (obj is Visual || obj is Visual3D))
                return VisualTreeHelper.GetParent(obj) ?? LogicalTreeHelper.GetParent(obj);
            else
                return LogicalTreeHelper.GetParent(obj);
        }

        public static IEnumerable<T> GetAncestors<T>(this DependencyObject obj, bool useLogicalTree = false) where T : DependencyObject
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
            foreach (var item in GetAncestors<T>(x, useLogicalTree))
            {
                yield return item;
            }
        }

        public static T FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
        {
            return GetAncestors<T>(obj).FirstOrDefault();
        }

        /// <summary>
        /// Try find ItemsPanel of ItemsControl
        /// </summary>
        /// <param name="itemsControl">Where to search</param>
        /// <returns>Panel of ItemsControl</returns>
        public static Panel GetItemsPanel(DependencyObject itemsControl)
        {
            if (itemsControl is Panel) return (Panel)itemsControl;
            ItemsPresenter itemsPresenter = FindVisualChildren<ItemsPresenter>(itemsControl).FirstOrDefault();
            if (itemsPresenter == null) return null;
            var itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;
            return itemsPanel;
        }

        /// <summary>
        /// Check if object is valid (no errors found in logical children)
        /// </summary>
        /// <param name="obj">Object to search</param>
        /// <returns>True if no errors found</returns>
        public static bool IsValid(this DependencyObject obj) {
            if (obj == null) return true;
            if (Validation.GetHasError(obj)) return false;
            foreach (var child in LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>()) {
                if (child == null) continue;
                if (child is UIElement ui) {
                    if (!ui.IsVisible || !ui.IsEnabled) continue;
                }
                if (!IsValid(child)) return false;
            }
            return true;
        }

        /// <summary>
        /// Search all textboxes to update invalid with UpdateSource(). For ex. when need update validation messages.
        /// </summary>
        /// <param name="obj">Object where to search TextBoxes</param>
        public static void UpdateSources(this DependencyObject obj) {
            if (obj == null) return;
            if (Validation.GetHasError(obj)) {
                //TODO Any types?
                if (obj is TextBox tb) tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }
            foreach (var item in FindLogicalChildren<DependencyObject>(obj)) {
                UpdateSources(item);
            }
        }

        /// <summary>
        /// Find all visual children of type T
        /// </summary>
        /// <typeparam name="T">Type to search</typeparam>
        /// <param name="depObj">Object where to search</param>
        /// <returns>Enumerator for items</returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject {
            if (depObj != null && (depObj is Visual || depObj is Visual3D)) {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child)) {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Find all logical children of type T
        /// </summary>
        /// <typeparam name="T">Type to search</typeparam>
        /// <param name="depObj">Object where to search</param>
        /// <returns>Enumerator for items</returns>
        public static IEnumerable<T> FindLogicalChildren<T>(this DependencyObject depObj) where T : DependencyObject {
            if (depObj != null) {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj)) {
                    if (rawChild is DependencyObject child) {
                        if (child is T) {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in FindLogicalChildren<T>(child)) {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }
    }

}
