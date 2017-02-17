using BrightSharp.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System;

namespace Diagrams
{
    public static class SelectionBehavior
    {
        public static void Attach(FrameworkElement fe)
        {
            ContentControl selectedControl = null;
            Style designerStyle = (Style)Application.Current.FindResource("DesignerItemStyle");
            if (fe is Panel)
            {
                var fePanel = (Panel)fe;
                fe.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    if (e.Source is DependencyObject)
                    {
                        var clickedElement = ((DependencyObject)e.OriginalSource).Ancestors<ContentControl>().FirstOrDefault(el => el.Style == designerStyle && WpfExtensions.GetItemsPanel(el.GetParent(true)) == fe);
                        if (clickedElement == null)
                        {
                            foreach (DependencyObject item in fePanel.Children)
                            {
                                Selector.SetIsSelected(item, false);
                            }
                            selectedControl = null;
                            return;
                        }
                        foreach (var control in fePanel.Children.OfType<ContentControl>())
                        {
                            if (ProcessControl(control, clickedElement, ref selectedControl))
                            {
                                e.Handled = selectedControl.Content is ButtonBase;
                                return;
                            }
                        }

                    }
                };
            }
            else if (fe is ItemsControl)
            {
                var feItemsControl = (ItemsControl)fe;
                fe.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    if (e.Source is DependencyObject)
                    {
                        var clickedElement = ((DependencyObject)e.Source).Ancestors<ContentControl>().FirstOrDefault(el => el.Style == designerStyle && el.Parent == fe);
                        if (clickedElement == null)
                        {
                            foreach (DependencyObject item in feItemsControl.Items)
                            {
                                Selector.SetIsSelected(item, false);
                            }
                            selectedControl = null;
                            return;
                        }
                        foreach (var control in feItemsControl.Items.OfType<ContentControl>())
                        {
                            if (ProcessControl(control, clickedElement, ref selectedControl))
                            {
                                e.Handled = selectedControl.Content is ButtonBase;
                                return;
                            }
                        }

                    }
                };
            }
        }

        private static bool ProcessControl(ContentControl control, ContentControl clickedElement, ref ContentControl selectedControl)
        {
            if (control == clickedElement && control != selectedControl)
            {
                if (selectedControl != null)
                {
                    Selector.SetIsSelected(selectedControl, false);
                }
                Selector.SetIsSelected(control, true);
                selectedControl = control;
                return true;
            }
            return false;
        }
    }
}
