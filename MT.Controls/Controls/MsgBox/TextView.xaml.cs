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

namespace MT.Controls.MsgBox
{    
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TextView : UserControl
    {
        public TextView(bool showCancel = false)
        {
            InitializeComponent();
            Unloaded += TextView_Unloaded;
            if (showCancel)
            {
                btnCancel.Visibility = Visibility.Visible;
            }
        }

        private void TextView_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= TextView_Unloaded;
            Instance = null;
        }

        public static TextView Instance { get; set; }


        public FontConfig Config
        {
            get { return (FontConfig)GetValue(ConfigProperty); }
            set { SetValue(ConfigProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Config.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigProperty =
            DependencyProperty.Register(nameof(Config), typeof(FontConfig), typeof(TextView), new PropertyMetadata(default(FontConfig)));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(TextView), new PropertyMetadata(default(string)));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(TextView), new PropertyMetadata(default(string)));



        public MessageStyle MessageStyle
        {
            get { return (MessageStyle)GetValue(MessageStyleProperty); }
            set { SetValue(MessageStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageStyleProperty =
            DependencyProperty.Register(nameof(MessageStyle), typeof(MessageStyle), typeof(TextView), new FrameworkPropertyMetadata(default(MessageStyle)));


    }
}
