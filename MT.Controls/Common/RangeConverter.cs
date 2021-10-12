using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Controls.Common
{
    class RangeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string rmSpace = value?.ToString().Replace(" ", "");
            if (!string.IsNullOrEmpty(rmSpace))
            {
                var arr = rmSpace.Split(new char[] { ',' });
                if (arr.Length == 2)
                {
                    if (double.TryParse(arr[0],out var d1) && double.TryParse(arr[1],out var d2))
                    {
                        return new Range(d1, d2);
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
