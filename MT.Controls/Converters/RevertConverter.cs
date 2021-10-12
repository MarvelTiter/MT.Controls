using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MT.Controls.Converters {
    public class RevertConverter : BaseValueConverter<RevertConverter> {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (parameter is Visibility v) {
                if (!(v is Visibility.Visible)) return Visibility.Collapsed;
            }
            if (value is Visibility visibility) {
                if (visibility == Visibility.Visible) {
                    return Visibility.Collapsed;
                } else {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }
    }
}
