using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BrightSharp.Diagrams
{
    [ToolboxItem(false)]
    public class MoveThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private FrameworkElement designerItem;
        private static int? zIndex = null;

        public MoveThumb()
        {
            DragStarted += MoveThumb_DragStarted;
            DragDelta += MoveThumb_DragDelta;
            DragCompleted += MoveThumb_DragCompleted;
        }

        private void MoveThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            //TODO Need think about ZIndex changes
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            designerItem = DataContext as FrameworkElement;

            if (designerItem != null)
            {
                rotateTransform = designerItem.RenderTransform as RotateTransform;
                if (designerItem.GetBindingExpression(Panel.ZIndexProperty) == null)
                {
                    zIndex = Math.Max(zIndex ?? 0, Panel.GetZIndex(designerItem));
                    Panel.SetZIndex(designerItem, zIndex.Value + 1);
                }
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (designerItem != null)
            {
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                double gridSize = 0;

                var zoomControl = designerItem.Parent as ZoomControl;
                if (zoomControl != null) gridSize = zoomControl.GridSize;

                if (rotateTransform != null)
                {
                    dragDelta = rotateTransform.Transform(dragDelta);
                }
                if (double.IsNaN(Canvas.GetLeft(designerItem))) Canvas.SetLeft(designerItem, 0);
                if (double.IsNaN(Canvas.GetTop(designerItem))) Canvas.SetTop(designerItem, 0);

                var newLeft = Canvas.GetLeft(designerItem) + dragDelta.X;
                var newTop = Canvas.GetTop(designerItem) + dragDelta.Y;
                if (gridSize > 0)
                {
                    newLeft = Math.Truncate(newLeft / gridSize) * gridSize;
                    newTop = Math.Truncate(newTop / gridSize) * gridSize;
                }
                Canvas.SetLeft(designerItem, newLeft);
                Canvas.SetTop(designerItem, newTop);

            }
        }
    }
}
