using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace BrightSharp.Converters
{
    public class IntToValueConverter : IValueConverter
    {
        public ulong? TrueValueInt { get; set; } = null;
        public ulong? FalseValueInt { get; set; } = 0;


        public object TrueValue { get; set; } = true;
        public object FalseValue { get; set; } = false; 


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                value = false;
            }
            if (value is string valueStr)
            {
                value = valueStr.Length > 0;
            }
            if (value is bool valueBool)
            {
                value = valueBool ? 1 : 0;
            }
            if (value is ICollection valueCol)
            {
                value = valueCol.Count;
            }
            if (value is Enum en)
            {
                value = System.Convert.ToInt32(en);
            }
            if (ulong.TryParse(value.ToString(), out ulong valueLong))
            {
                // Exact match for false
                if (FalseValueInt.HasValue && FalseValueInt == valueLong) return FalseValue;
                // Exact match for true
                if (TrueValueInt.HasValue && TrueValueInt == valueLong) return TrueValue;
                // Any value for false
                if (!FalseValueInt.HasValue) return FalseValue;
                // Any value for true
                if (!TrueValueInt.HasValue) return TrueValue;
            }
            return TrueValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
			// Convert with potential lose exact value match
	        return Equals(value, TrueValue) ? TrueValueInt : FalseValueInt;
        }
    }
}
