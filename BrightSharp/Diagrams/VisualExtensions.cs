using BrightSharp;
using BrightSharp.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Diagrams
{
    /// <summary>
    /// If starts with a(A) then use animation,
    /// $FromZoom-$ToZoom pattern
    /// </summary>
    public class VisualExtensions : DependencyObject
    {
        #region LevelOfDetails Attached Property

        public static readonly DependencyProperty LODZoomProperty =
            DependencyProperty.RegisterAttached("LODZoom",
            typeof(string),
            typeof(VisualExtensions),
            new UIPropertyMetadata(null, OnChangeLODZoomProperty));

        public static void SetLODZoom(UIElement element, string o)
        {
            element.SetValue(LODZoomProperty, o);
        }

        public static string GetLODZoom(UIElement element)
        {
            return (string)element.GetValue(LODZoomProperty);
        }
        #endregion

        public static bool GetCanRotate(DependencyObject obj) {
            return (bool)obj.GetValue(CanRotateProperty);
        }

        public static void SetCanRotate(DependencyObject obj, bool value) {
            obj.SetValue(CanRotateProperty, value);
        }

        public static readonly DependencyProperty CanRotateProperty =
            DependencyProperty.RegisterAttached("CanRotate", typeof(bool), typeof(VisualExtensions), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        private static void OnChangeLODZoomProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;

            var window = Window.GetWindow(element);
            if (window == null) return;

            if (e.NewValue != e.OldValue)
            {
                var zoomControl = element.FindAncestor<ZoomControl>();
                if (zoomControl == null) return;

                var zoomChangedHandler = new RoutedEventHandler((sender, args) =>
                {
                    var lodInfo = new LodInfo(GetLODZoom(element));

                    var transform = element.TransformToVisual(window) as MatrixTransform;
                    var scaleX = transform.Matrix.M11;

                    var newOpacity = (scaleX >= lodInfo.StartRange && scaleX <= lodInfo.EndRange) ? 1 : 0;

                    if (lodInfo.UseAnimation && args.RoutedEvent != FrameworkElement.LoadedEvent)
                    {
                        element.Visibility = Visibility.Visible;
                        var animation = new DoubleAnimation(newOpacity, TimeSpan.FromSeconds(.5));
                        element.BeginAnimation(UIElement.OpacityProperty, animation);
                    }
                    else
                    {
                        element.Visibility = newOpacity == 1 ? Visibility.Visible : Visibility.Hidden;
                        element.Opacity = newOpacity;
                    }
                });


                if (string.IsNullOrWhiteSpace((string)e.NewValue))
                {
                    element.Opacity = 1;
                    zoomControl.ZoomChanged -= zoomChangedHandler;
                    zoomControl.Loaded -= zoomChangedHandler;
                }
                else
                {
                    zoomControl.ZoomChanged += zoomChangedHandler;
                    zoomControl.Loaded += zoomChangedHandler;
                }
            }
        }

        sealed class LodInfo
        {
            public LodInfo(string lod)
            {
                UseAnimation = lod.StartsWith("a", true, CultureInfo.InvariantCulture);
                lod = lod.TrimStart('a', 'A').Trim();

                double rangeStart = 0;
                double rangeEnd = double.PositiveInfinity;
                var vals = lod.Split('-');

                double.TryParse(vals[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rangeStart);

                if (vals.Length > 1)
                {
                    if (!double.TryParse(vals[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rangeEnd))
                    {
                        rangeEnd = double.PositiveInfinity;
                    }
                }
                EndRange = rangeEnd;
                StartRange = rangeStart;
            }
            public double StartRange { get; set; }
            public double EndRange { get; set; }
            public bool UseAnimation { get; set; }
        }
    }

}
