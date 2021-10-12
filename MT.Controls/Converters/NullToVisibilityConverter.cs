using System;
using System.Globalization;
using System.Windows;

namespace MT.Controls.Converters
{
    public class NullToVisibilityConverter : BaseValueConverter<NullToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
    }
}
