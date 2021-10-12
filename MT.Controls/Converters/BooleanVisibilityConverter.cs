using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MT.Controls.Converters {
    public class BooleanVisibilityConverter : BaseValueConverter<BooleanVisibilityConverter> {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return Visibility.Hidden;
            var p = parameter?.ToString();
            var v = (bool)value;
            if (p == "1")// 参数为1，反转
                v = !v;
            if (v) {
                return Visibility.Visible;
            } else {
                return Visibility.Collapsed;
            }
        }
    }
}
