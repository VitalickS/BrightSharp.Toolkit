using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BrightSharp.Extensions
{
    public static class MarkupExtensionProperties
    {
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
        public static object GetHeader(DependencyObject obj)
        {
            return obj.GetValue(HeaderProperty);
        }
        public static void SetHeader(DependencyObject obj, object value)
        {
            obj.SetValue(HeaderProperty, value);
        }
        public static object GetLeadingElement(DependencyObject obj)
        {
            return obj.GetValue(CornerRadiusProperty);
        }
        public static void SetLeadingElement(DependencyObject obj, object value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
        public static object GetTrailingElement(DependencyObject obj)
        {
            return obj.GetValue(CornerRadiusProperty);
        }
        public static void SetTrailingElement(DependencyObject obj, object value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
        public static object GetIcon(DependencyObject obj)
        {
            return obj.GetValue(IconProperty);
        }
        public static void SetIcon(DependencyObject obj, object value)
        {
            obj.SetValue(IconProperty, value);
        }
        public static void SetHeaderHeight(DependencyObject obj, double value)
        {
            obj.SetValue(HeaderHeightProperty, value);
        }
        public static double GetHeaderHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(HeaderHeightProperty);
        }

        public static void SetHeaderVerticalAlignment(DependencyObject obj, VerticalAlignment value)
        {
            obj.SetValue(HeaderVerticalAlignmentProperty, value);
        }
        public static VerticalAlignment GetHeaderVerticalAlignment(DependencyObject obj)
        {
            return (VerticalAlignment)obj.GetValue(HeaderVerticalAlignmentProperty);
        }
        public static void SetHeaderHorizontalAlignment(DependencyObject obj, HorizontalAlignment value)
        {
            obj.SetValue(HeaderHorizontalAlignmentProperty, value);
        }
        public static HorizontalAlignment GetHeaderHorizontalAlignment(DependencyObject obj)
        {
            return (HorizontalAlignment)obj.GetValue(HeaderHorizontalAlignmentProperty);
        }
        public static void SetSpecialHeight(DependencyObject obj, double value)
        {
            obj.SetValue(SpecialHeightProperty, value);
        }
        public static double GetSpecialHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(SpecialHeightProperty);
        }
        public static void SetSpecialWidth(DependencyObject obj, double value)
        {
            obj.SetValue(SpecialWidthProperty, value);
        }
        public static double GetSpecialWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(SpecialWidthProperty);
        }
        public static Dock GetDocking(DependencyObject obj)
        {
            return (Dock)obj.GetValue(DockingProperty);
        }
        public static void SetDocking(DependencyObject obj, Dock value)
        {
            obj.SetValue(DockingProperty, value);
        }
        public static Thickness GetHeaderPadding(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(HeaderPaddingProperty);
        }
        public static void SetHeaderPadding(DependencyObject obj, Thickness value)
        {
            obj.SetValue(HeaderPaddingProperty, value);
        }
        public static bool GetIsDragHelperVisible(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragHelperVisibleProperty);
        }
        public static void SetIsDragHelperVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragHelperVisibleProperty, value);
        }
        public static Brush GetSpecialBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(SpecialBrushProperty);
        }
        public static void SetSpecialBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SpecialBrushProperty, value);
        }

        public static readonly DependencyProperty SpecialBrushProperty = DependencyProperty.RegisterAttached("SpecialBrush", typeof(Brush), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty IsDragHelperVisibleProperty = DependencyProperty.RegisterAttached("IsDragHelperVisible", typeof(bool), typeof(MarkupExtensionProperties), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.RegisterAttached("HeaderPadding", typeof(Thickness), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty LeadingElementProperty = DependencyProperty.RegisterAttached("LeadingElement", typeof(object), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty TrailingElementProperty = DependencyProperty.RegisterAttached("TrailingElement", typeof(object), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.RegisterAttached("Header", typeof(object), typeof(MarkupExtensionProperties), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty DockingProperty = DependencyProperty.RegisterAttached("Docking", typeof(Dock), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.RegisterAttached("HeaderHeight", typeof(double), typeof(MarkupExtensionProperties), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderVerticalAlignmentProperty = DependencyProperty.RegisterAttached("HeaderVerticalAlignment", typeof(VerticalAlignment), typeof(MarkupExtensionProperties), new PropertyMetadata(VerticalAlignment.Center));
        public static readonly DependencyProperty HeaderHorizontalAlignmentProperty = DependencyProperty.RegisterAttached("HeaderHorizontalAlignment", typeof(HorizontalAlignment), typeof(MarkupExtensionProperties), new PropertyMetadata(HorizontalAlignment.Left));
        public static readonly DependencyProperty SpecialHeightProperty = DependencyProperty.RegisterAttached("SpecialHeight", typeof(double), typeof(MarkupExtensionProperties), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty SpecialWidthProperty = DependencyProperty.RegisterAttached("SpecialWidth", typeof(double), typeof(MarkupExtensionProperties), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        
    }

}
