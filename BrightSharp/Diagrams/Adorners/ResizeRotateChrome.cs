using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BrightSharp.Diagrams
{
    [ToolboxItem(false)]
    public class ResizeRotateChrome : Control
    {
        static ResizeRotateChrome()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeRotateChrome), new FrameworkPropertyMetadata(typeof(ResizeRotateChrome)));
        }
    }
}
