using BrightSharp.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using BrightSharp.Diagrams;

namespace BrightSharp
{

    public partial class ZoomControl : ItemsControl
    {
        public ZoomControl()
        {
            Loaded += ZoomControl_Loaded;
        }

        private void ZoomControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectionBehavior.Attach(this);
        }

        public ContentControl SelectedControl
        {
            get { return (ContentControl)GetValue(SelectedControlProperty); }
            set { SetValue(SelectedControlProperty, value); }
        }

        public static readonly DependencyProperty SelectedControlProperty =
            DependencyProperty.Register("SelectedControl", typeof(ContentControl), typeof(ZoomControl), new PropertyMetadata(null));

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                _panPoint = e.GetPosition(this);
            }
        }

        private Point? _panPoint;

        #region Props

        public double TranslateX
        {
            get { return (double)GetValue(TranslateXProperty); }
            set { SetValue(TranslateXProperty, value); }
        }

        public static readonly DependencyProperty TranslateXProperty =
            DependencyProperty.Register("TranslateX", typeof(double), typeof(ZoomControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange, TranslateChangedHandler));



        public double TranslateY
        {
            get { return (double)GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }

        public static readonly DependencyProperty TranslateYProperty =
            DependencyProperty.Register("TranslateY", typeof(double), typeof(ZoomControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange, TranslateChangedHandler));



        public double RenderZoom
        {
            get { return (double)GetValue(RenderZoomProperty); }
            set { SetValue(RenderZoomProperty, value); }
        }

        public static readonly DependencyProperty RenderZoomProperty =
            DependencyProperty.Register("RenderZoom", typeof(double), typeof(ZoomControl), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsArrange, ZoomChangedHandler));

        private static void ZoomChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            ZoomControl zc = (ZoomControl)d;
        }
        private static void TranslateChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            ZoomControl zc = (ZoomControl)d;
        }

        #endregion

        private static readonly RoutedEvent ZoomChangedEvent = EventManager.RegisterRoutedEvent("ZoomChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZoomControl));

        public event RoutedEventHandler ZoomChanged
        {
            add { AddHandler(ZoomChangedEvent, value); }
            remove { RemoveHandler(ZoomChangedEvent, value); }
        }

        private void RaiseZoomChangedEvent()
        {
            var newEventArgs = new RoutedEventArgs(ZoomChangedEvent);
            RaiseEvent(newEventArgs);
        }

        static ZoomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomControl), new FrameworkPropertyMetadata(typeof(ZoomControl)));
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (CheckMouseOverControlWheelHandle(_panPoint.HasValue))
                return;

            var point = e.GetPosition(this);


            var oldValue = RenderZoom;
            const double zoomPower = 1.25;
            var newValue = RenderZoom * (e.Delta > 0 ? zoomPower : 1 / zoomPower);
            if (UseAnimation)
            {
                DoubleAnimation anim = new DoubleAnimation(newValue, TimeSpan.FromSeconds(.3));
                anim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                this.BeginAnimation(ZoomControl.RenderZoomProperty, anim);
            }
            else
            {
                RenderZoom = newValue;
            }

            RaiseZoomChangedEvent();

            var translateXTo = CoercePanTranslate(TranslateX, point.X, oldValue, newValue);
            var translateYTo = CoercePanTranslate(TranslateY, point.Y, oldValue, newValue);
            if (UseAnimation)
            {
                DoubleAnimation anim = new DoubleAnimation(translateXTo, TimeSpan.FromSeconds(.3));
                anim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                this.BeginAnimation(ZoomControl.TranslateXProperty, anim);

                anim = new DoubleAnimation(translateYTo, TimeSpan.FromSeconds(.3));
                anim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                this.BeginAnimation(ZoomControl.TranslateYProperty, anim);
            }
            else
            {
                TranslateX = translateXTo; TranslateY = translateYTo;
            }

            e.Handled = true;

            base.OnPreviewMouseWheel(e);
        }
        public bool UseAnimation { get; set; }
        private static bool CheckMouseOverControlWheelHandle(bool checkFlowDoc = false)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl)) return false;
            var element = Mouse.DirectlyOver as DependencyObject;
            if (element is ScrollViewer) return true;

            var scrollViewer = element.FindAncestor<ScrollViewer>();
            if (scrollViewer != null)
            {
                return scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible
                        || scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible;
            }
            if (checkFlowDoc)
            {
                var flowDocument = element.FindAncestor<FlowDocument>();
                if (flowDocument != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static double CoercePanTranslate(double oldTranslate, double mousePos, double oldZoom, double newZoom)
        {
            return Math.Round(oldTranslate + (oldTranslate - mousePos) * (newZoom - oldZoom) / oldZoom);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.MiddleButton == MouseButtonState.Released)
            {
                _panPoint = null;
                ReleaseMouseCapture();
            }
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (e.MiddleButton == MouseButtonState.Pressed && _panPoint.HasValue)
            {
                //PAN MODE
                var vector = e.GetPosition(this) - _panPoint.Value;
                _panPoint = _panPoint + vector;
                CaptureMouse();
                TranslateX += vector.X;
                TranslateY += vector.Y;
                BeginAnimation(TranslateXProperty, null);
                BeginAnimation(TranslateYProperty, null);
            }
        }
    }
}
