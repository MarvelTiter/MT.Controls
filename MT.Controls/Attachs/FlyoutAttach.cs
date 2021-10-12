using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MT.Controls.Attachs
{
    public class FlyoutAttach
    {
        public static bool GetRegisterFlyoutArea(DependencyObject obj)
        {
            return (bool)obj.GetValue(RegisterFlyoutAreaProperty);
        }

        public static void SetRegisterFlyoutArea(DependencyObject obj, bool value)
        {
            obj.SetValue(RegisterFlyoutAreaProperty, value);
        }

        // Using a DependencyProperty as the backing store for RegisterFlyoutArea.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegisterFlyoutAreaProperty =
            DependencyProperty.RegisterAttached("RegisterFlyoutArea", typeof(bool), typeof(FlyoutAttach), new PropertyMetadata(false));
    }
}
