using MT.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIPreview.DialogDemo;

namespace UIPreview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RemindBox.Success("hello");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RemindBox.Error("hello");
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogService.RegisterService<HelloDialog>();
            await DialogService.Show<HelloDialog>();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            await Task.Delay(2000);
            IsLoading = false;
        }
    }
}
