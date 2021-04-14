using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace BrightSharp.Diagrams
{
    [ToolboxItem(false)]
    public class RotateThumb : Thumb
    {
        private double initialAngle;
        private RotateTransform rotateTransform;
        private Vector startVector;
        private Point centerPoint;
        private ContentControl designerItem;
        private Canvas canvas;

        public RotateThumb()
        {
            DragDelta += new DragDeltaEventHandler(RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(RotateThumb_DragStarted);
            MouseRightButtonDown += RotateThumb_MouseLeftButtonDown;
        }

        private void RotateThumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                rotateTransform = designerItem.RenderTransform as RotateTransform;
                if (rotateTransform != null)
                {
                    rotateTransform.Angle = 0;
                    designerItem.InvalidateMeasure();
                }
            }
        }

        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            designerItem = DataContext as ContentControl;

            if (designerItem != null)
            {
                canvas = VisualTreeHelper.GetParent(designerItem) as Canvas;

                if (canvas != null)
                {
                    centerPoint = designerItem.TranslatePoint(
                        new Point(designerItem.ActualWidth * designerItem.RenderTransformOrigin.X,
                                  designerItem.ActualHeight * designerItem.RenderTransformOrigin.Y),
                                  canvas);

                    Point startPoint = Mouse.GetPosition(canvas);
                    startVector = Point.Subtract(startPoint, centerPoint);

                    rotateTransform = designerItem.RenderTransform as RotateTransform;
                    if (rotateTransform == null)
                    {
                        designerItem.RenderTransform = new RotateTransform(0);
                        initialAngle = 0;
                    }
                    else
                    {
                        initialAngle = rotateTransform.Angle;
                    }
                }
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (designerItem != null && canvas != null)
            {
                Point currentPoint = Mouse.GetPosition(canvas);
                Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

                double angle = Vector.AngleBetween(startVector, deltaVector);

                RotateTransform rotateTransform = designerItem.RenderTransform as RotateTransform;
                rotateTransform.Angle = initialAngle + Math.Round(angle, 0);
                designerItem.InvalidateMeasure();
            }
        }
    }
}
