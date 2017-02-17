using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace BrightSharp.Converters
{
    public class StringHelpConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string && value is string)
            {
                var parStr = (string)parameter;
                switch (parStr)
                {
                    case "SpaceBetweenCaps":
                        var src = (value ?? string.Empty).ToString();
                        return Regex.Replace(src, "([a-z])([A-Z])", "$1 $2");
                    default:
                        break;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
