using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace BrightSharp.Themes
{

    internal partial class Theme
    {
        public void CalendarPreviewMouseUp(object sender, MouseEventArgs e) {
            if (Mouse.Captured is CalendarItem) { Mouse.Capture(null); }
        }
        public void DatePickerUnloaded(object sender, RoutedEventArgs e) {
            if (sender == null) return;
            DependencyObject child = ((Popup)((DatePicker)sender).Template.FindName("PART_Popup", (FrameworkElement)sender))?.Child;
            while (child != null && !(child is AdornerDecorator))
                child = VisualTreeHelper.GetParent(child) ?? LogicalTreeHelper.GetParent(child);
            if (((AdornerDecorator)child)?.Child is Calendar) ((AdornerDecorator)child).Child = null;
        }


        private void closeButton_Click(object sender, RoutedEventArgs e) {
            var window = Window.GetWindow((DependencyObject)sender);
            window.Close();
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e) {
            var window = Window.GetWindow((DependencyObject)sender);
            window.WindowState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e) {
            var window = Window.GetWindow((DependencyObject)sender);
            window.WindowState = WindowState.Minimized;
        }

        
    }
}
