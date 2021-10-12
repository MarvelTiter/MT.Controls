using System;
using System.Globalization;

namespace MT.Controls.Converters
{
    public class IntBooleanConverter : BaseValueConverter<IntBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                if (parameter != null)
                {
                    return value == parameter;
                }
                return i > 0;
            }
            return false;
        }
    }
}
