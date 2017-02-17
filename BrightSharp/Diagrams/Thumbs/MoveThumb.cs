using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Diagrams
{
    [ToolboxItem(false)]
    public class MoveThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private ContentControl designerItem;

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }


        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as ContentControl;

            if (this.designerItem != null)
            {
                this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                if (this.rotateTransform != null)
                {
                    dragDelta = this.rotateTransform.Transform(dragDelta);
                }
                if (double.IsNaN(Canvas.GetLeft(this.designerItem))) Canvas.SetLeft(this.designerItem, 0);
                if (double.IsNaN(Canvas.GetTop(this.designerItem))) Canvas.SetTop(this.designerItem, 0);

                Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + dragDelta.X);
                Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + dragDelta.Y);
            }
        }
    }
}
