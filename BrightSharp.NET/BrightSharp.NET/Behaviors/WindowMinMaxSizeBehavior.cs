using System.Windows;
using System.Windows.Interactivity;

namespace BrightSharp.Behaviors
{
    public class WindowMinMaxSizeBehavior : Behavior<Window>
    {
        private MinMaxSize_Logic _logic;
        protected override void OnAttached()
        {
            base.OnAttached();
            _logic = new MinMaxSize_Logic(AssociatedObject);
            _logic.OnAttached();
        }

        
        protected override void OnDetaching()
        {
            _logic.OnDetaching();
            base.OnDetaching();
        }
    }
}
