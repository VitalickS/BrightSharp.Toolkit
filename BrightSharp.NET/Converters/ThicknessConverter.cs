using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BrightSharp.Converters
{
    internal class ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Thickness)
            {
                var offset = double.Parse(parameter.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                var thick = (Thickness)value;

                return new Thickness(Math.Max(0, thick.Left + offset),
                    Math.Max(0, thick.Top + offset),
                    Math.Max(0, thick.Right + offset),
                    Math.Max(0, thick.Bottom + offset));
            }
            else if(value is CornerRadius)
            {
                var offset = double.Parse(parameter.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                var thick = (CornerRadius)value;

                return new CornerRadius(Math.Max(0, thick.TopLeft + offset),
                    Math.Max(0, thick.TopRight + offset),
                    Math.Max(0, thick.BottomRight + offset),
                    Math.Max(0, thick.BottomLeft + offset));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
