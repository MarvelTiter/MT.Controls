using MT.Controls.Helper;
using System.Windows;
using System.Windows.Controls;

namespace MT.Controls.Attachs
{
    public class ButtonAttach
    {

        public static double GetRadius(DependencyObject obj)
        {
            return (double)obj.GetValue(RadiusProperty);
        }

        public static void SetRadius(DependencyObject obj, double value)
        {
            obj.SetValue(RadiusProperty, value);
        }
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(double), typeof(ButtonAttach), new PropertyMetadata(5d));


        public static ContentFlyout GetFlyout(DependencyObject obj)
        {
            return (ContentFlyout)obj.GetValue(FlyoutProperty);
        }

        public static void SetFlyout(DependencyObject obj, ContentFlyout value)
        {
            obj.SetValue(FlyoutProperty, value);
        }

        public static readonly DependencyProperty FlyoutProperty =
            DependencyProperty.RegisterAttached("Content", typeof(ContentFlyout), typeof(ButtonAttach), new PropertyMetadata(null, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }
            if (d is Button btn && btn != null)
            {
                btn.Loaded += Btn_Loaded;
            }
        }

        private static void Btn_Loaded(object sender, RoutedEventArgs e)
        {
            if (e.Source == null) return;
            ContentFlyout flyout = GetFlyout(e.Source as DependencyObject);
            if (flyout != null && flyout.ApplyTemplate())
            {
                var btn = sender as Button;
                flyout.SetTarget(btn);
                var panel = VisualHelper.GetParent<Panel>(btn, d => FlyoutAttach.GetRegisterFlyoutArea(d));
                panel = panel ?? WindowHelper.GetActiveWindow().Content as Panel;
                if (panel != null)
                {
                    panel.Children.Add(flyout);
                    Point point = btn.TranslatePoint(new Point(0, 0), panel);
                    flyout.TargetPoint = point;
                    flyout.Container = panel;
                    flyout.DataContext = btn.DataContext;
                    btn.Click += (s, ei) =>
                    {
                        flyout.Toggle();
                    };
                }
            }
        }
    }
}
