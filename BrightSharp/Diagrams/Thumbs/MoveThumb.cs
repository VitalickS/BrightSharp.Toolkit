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
        private ContentControl designerItem;
        private static int? zIndex = null;

        public MoveThumb() {
            DragStarted += MoveThumb_DragStarted;
            DragDelta += MoveThumb_DragDelta;
            DragCompleted += MoveThumb_DragCompleted;
        }

        private void MoveThumb_DragCompleted(object sender, DragCompletedEventArgs e) {
            //TODO Need think about ZIndex changes
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e) {
            designerItem = DataContext as ContentControl;

            if (designerItem != null) {
                rotateTransform = designerItem.RenderTransform as RotateTransform;
                if (designerItem.GetBindingExpression(Panel.ZIndexProperty) == null) {
                    zIndex = Math.Max(zIndex ?? 0, Panel.GetZIndex(designerItem));
                    Panel.SetZIndex(designerItem, zIndex.Value + 1);
                }
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e) {
            if (designerItem != null) {
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                if (rotateTransform != null) {
                    dragDelta = rotateTransform.Transform(dragDelta);
                }
                if (double.IsNaN(Canvas.GetLeft(designerItem))) Canvas.SetLeft(designerItem, 0);
                if (double.IsNaN(Canvas.GetTop(designerItem))) Canvas.SetTop(designerItem, 0);

                Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + dragDelta.X);
                Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + dragDelta.Y);


            }
        }
    }
}
