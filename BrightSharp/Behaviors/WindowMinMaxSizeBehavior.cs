using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace BrightSharp.Behaviors
{
    public class WindowMinMaxSizeBehavior : Behavior<Window>
    {
        private MinMaxSize_Logic logic;
        protected override void OnAttached()
        {
            base.OnAttached();
            logic = new MinMaxSize_Logic(AssociatedObject);
            logic.OnAttached();
        }

        
        protected override void OnDetaching()
        {
            logic.OnDetaching();
            base.OnDetaching();
        }
    }
}
