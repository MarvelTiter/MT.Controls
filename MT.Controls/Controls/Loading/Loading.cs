using MT.Controls.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MT.Controls
{
    public enum LoadingStyle
    {
        Standard,
        Wave,
        Ring,
        Classic
    }
    public class Loading : Control
    {
        static Loading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Loading), new FrameworkPropertyMetadata(typeof(Loading)));
        }

        private AdornerMask _container;
        private AdornerDecorator decorator;
        private static Loading instance = new Loading();

        #region IsLoading
        public static bool GetIsLoading(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsLoadingProperty);
        }

        public static void SetIsLoading(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLoadingProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.RegisterAttached("IsLoading", typeof(bool), typeof(Loading), new PropertyMetadata(false, propertyChangedCallback));
        #endregion

        #region LoadingStyle
        public static LoadingStyle GetLoadingStyle(DependencyObject obj)
        {
            return (LoadingStyle)obj.GetValue(LoadingStyleProperty);
        }

        public static void SetLoadingStyle(DependencyObject obj, LoadingStyle value)
        {
            obj.SetValue(LoadingStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for LoadingStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingStyleProperty =
            DependencyProperty.RegisterAttached("LoadingStyle", typeof(LoadingStyle), typeof(Loading), new PropertyMetadata(LoadingStyle.Standard));
        #endregion

        #region Color

        public static SolidColorBrush GetColor(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ColorProperty);
        }

        public static void SetColor(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.RegisterAttached("Color", typeof(SolidColorBrush), typeof(Loading), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(63, 63, 63))));

        #endregion

        #region Text


        public static string GetLoadingText(DependencyObject obj)
        {
            return (string)obj.GetValue(LoadingTextProperty);
        }

        public static void SetLoadingText(DependencyObject obj, string value)
        {
            obj.SetValue(LoadingTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for LoadingText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.RegisterAttached("LoadingText", typeof(string), typeof(Loading), new PropertyMetadata("Loading"));



        internal string LoadingTextForBind
        {
            get { return (string)GetValue(LoadingTextForBindProperty); }
            set { SetValue(LoadingTextForBindProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadingTextForBind.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty LoadingTextForBindProperty =
            DependencyProperty.Register("LoadingTextForBind", typeof(string), typeof(Loading), new PropertyMetadata("",(d,e)=> { 
                if (string.IsNullOrEmpty(e.NewValue?.ToString()))
                {
                    d.SetValue(TextVisibleProperty, Visibility.Collapsed);
                }
                else
                {
                    d.SetValue(TextVisibleProperty, Visibility.Visible);

                }
            }));



        public Visibility TextVisible
        {
            get { return (Visibility)GetValue(TextVisibleProperty); }
            set { SetValue(TextVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextVisibleProperty =
            DependencyProperty.Register(nameof(TextVisible), typeof(Visibility), typeof(Loading), new PropertyMetadata(Visibility.Collapsed));



        #endregion

        private static void propertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is IAddChild container && d is FrameworkElement ele) {
            //    instance.Arrange(new Rect(0, 0, ele.ActualWidth, ele.ActualHeight));
            //    container.AddChild(instance);
            //}
            FrameworkElement element = d as FrameworkElement;
            AdornerDecorator decorator;
            /*
             * find adorner
             */
            decorator = VisualHelper.GetChild<AdornerDecorator>(element);
            if (decorator == null)
                decorator = VisualHelper.GetChild<AdornerDecorator>(WindowHelper.GetActiveWindow());

            if (decorator != null)
            {
                if (!(bool)e.NewValue)
                {
                    Close();
                    return;
                }
                instance = new Loading();
                if (decorator.Child != null)
                {
                    decorator.Child.IsEnabled = false;
                }
                var layer = decorator.AdornerLayer;
                if (layer != null)
                {
                    var color = GetColor(element);
                    instance.Foreground = color;
                    instance.LoadingTextForBind = GetLoadingText(element);
                    var container = new AdornerMask(layer)
                    {
                        Child = instance
                    };
                    instance._container = container;
                    instance.decorator = decorator;
                    layer.Add(container);
                }
            }
        }

        public static void Close()
        {
            if (instance != null)
            {
                var decorator = instance.decorator;
                if (decorator != null)
                {
                    if (decorator.Child != null)
                    {
                        decorator.Child.IsEnabled = true;
                    }
                    var layer = decorator.AdornerLayer;
                    layer?.Remove(instance._container);
                }
            }
        }
    }
}
