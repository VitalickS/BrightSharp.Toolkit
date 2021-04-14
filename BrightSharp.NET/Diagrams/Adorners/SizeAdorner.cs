using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BrightSharp.Diagrams
{
    [ToolboxItem(false)]
    public class SizeAdorner : Adorner
    {
        private SizeChrome chrome;
        private VisualCollection visuals;
        private FrameworkElement designerItem;

        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        public SizeAdorner(FrameworkElement designerItem)
            : base(designerItem)
        {
            SnapsToDevicePixels = true;
            this.designerItem = designerItem;
            chrome = new SizeChrome();
            chrome.DataContext = designerItem;
            visuals = new VisualCollection(this);
            visuals.Add(chrome);
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            chrome.Arrange(new Rect(new Point(0.0, 0.0), arrangeBounds));
            return arrangeBounds;
        }
    }
}
