using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;

namespace BrightSharp.Diagrams
{
    [ToolboxItem(false)]
    public class ResizeThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private double angle;
        private Adorner adorner;
        private Point transformOrigin;
        private ContentControl designerItem;
        private Canvas canvas;

        public ResizeThumb()
        {
            DragStarted += new DragStartedEventHandler(ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
            DragCompleted += new DragCompletedEventHandler(ResizeThumb_DragCompleted);
            MouseRightButtonDown += new MouseButtonEventHandler(ResizeThumb_MouseRightButtonDown);
        }

        private void ResizeThumb_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            designerItem = designerItem ?? DataContext as ContentControl;

            if (VerticalAlignment == VerticalAlignment.Top || VerticalAlignment == VerticalAlignment.Bottom)
            {
                designerItem.Height = double.NaN;
            }
            if (HorizontalAlignment == HorizontalAlignment.Left || HorizontalAlignment == HorizontalAlignment.Right)
            {
                designerItem.Width = double.NaN;
            }
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            designerItem = DataContext as ContentControl;

            if (designerItem != null)
            {
                canvas = VisualTreeHelper.GetParent(designerItem) as Canvas;

                if (canvas != null)
                {
                    transformOrigin = designerItem.RenderTransformOrigin;

                    rotateTransform = designerItem.RenderTransform as RotateTransform;
                    if (rotateTransform != null)
                    {
                        angle = rotateTransform.Angle * Math.PI / 180.0;
                    }
                    else
                    {
                        angle = 0.0d;
                    }

                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(canvas);
                    if (adornerLayer != null)
                    {
                        adorner = new SizeAdorner(designerItem);
                        adornerLayer.Add(adorner);
                    }
                }
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (designerItem != null)
            {
                double deltaVertical, deltaHorizontal;
                if (double.IsNaN(Canvas.GetTop(designerItem))) Canvas.SetTop(designerItem, 0);
                if (double.IsNaN(Canvas.GetLeft(designerItem))) Canvas.SetLeft(designerItem, 0);
                if ((VerticalAlignment == VerticalAlignment.Top || VerticalAlignment == VerticalAlignment.Bottom) && double.IsNaN(designerItem.Height)) designerItem.Height = designerItem.ActualHeight;
                if ((HorizontalAlignment == HorizontalAlignment.Left || HorizontalAlignment == HorizontalAlignment.Right) && double.IsNaN(designerItem.Width)) designerItem.Width = designerItem.ActualWidth;

                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                        deltaVertical = Math.Max(deltaVertical, designerItem.ActualHeight - designerItem.MaxHeight);
                        Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + (transformOrigin.Y * deltaVertical * (1 - Math.Cos(-angle))));
                        Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) - deltaVertical * transformOrigin.Y * Math.Sin(-angle));
                        designerItem.Height -= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                        deltaVertical = Math.Max(deltaVertical, designerItem.ActualHeight - designerItem.MaxHeight);
                        Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + deltaVertical * Math.Cos(-angle) + (transformOrigin.Y * deltaVertical * (1 - Math.Cos(-angle))));
                        Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaVertical * Math.Sin(-angle) - (transformOrigin.Y * deltaVertical * Math.Sin(-angle)));
                        designerItem.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                        deltaHorizontal = Math.Max(deltaHorizontal, designerItem.ActualWidth - designerItem.MaxWidth);
                        Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + deltaHorizontal * Math.Sin(angle) - transformOrigin.X * deltaHorizontal * Math.Sin(angle));
                        Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaHorizontal * Math.Cos(angle) + (transformOrigin.X * deltaHorizontal * (1 - Math.Cos(angle))));
                        designerItem.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                        deltaHorizontal = Math.Max(deltaHorizontal, designerItem.ActualWidth - designerItem.MaxWidth);
                        Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) - transformOrigin.X * deltaHorizontal * Math.Sin(angle));
                        Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + (deltaHorizontal * transformOrigin.X * (1 - Math.Cos(angle))));
                        designerItem.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (adorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(canvas);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(adorner);
                }

                adorner = null;
            }
        }
    }
}
