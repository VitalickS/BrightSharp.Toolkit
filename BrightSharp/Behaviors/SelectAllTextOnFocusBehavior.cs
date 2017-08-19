using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace BrightSharp.Behaviors
{
    public class SelectAllTextOnFocusBehavior : Behavior<TextBoxBase>
    {
        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.GotKeyboardFocus += AssociatedObjectGotKeyboardFocus;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObjectPreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= AssociatedObjectGotKeyboardFocus;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObjectPreviewMouseLeftButtonDown;
        }

        private void AssociatedObjectGotKeyboardFocus(object sender,
            KeyboardFocusChangedEventArgs e) {
            AssociatedObject.SelectAll();
        }

        private void AssociatedObjectPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (!AssociatedObject.IsKeyboardFocusWithin) {
                AssociatedObject.Focus();
                e.Handled = true;
            }
        }
    }
}
