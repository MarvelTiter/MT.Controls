using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace MT.Controls.Converters
{

    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter where T : class, new() {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        private static T _instance;
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return _instance ?? (_instance = new T());
        }
    }
}
