using MT.Controls.MsgBox;
using System;
using System.Windows.Media;

namespace MT.Controls
{
    public enum MessageStyle
    {
        Success,
        Warning,
        Error,
        Info,
    }
    public class FontConfig
    {
        public double FontSize { get; set; } = 24d;
        public Brush Color { get; set; } = new SolidColorBrush(System.Windows.Media.Color.FromRgb(62, 62, 62));
    }
    /// <summary>
    /// 消息框提示服务
    /// </summary>
    public class RemindBox
    {
        public static void Success(string msg, string title = "", Action<FontConfig> config = null)
        {
            ShowMessage(msg, title, MessageStyle.Success, config);
        }
        public static void Warning(string msg, string title = "", Action<FontConfig> config = null)
        {
            ShowMessage(msg, title, MessageStyle.Warning, config);
        }
        public static void Error(string msg, string title = "", Action<FontConfig> config = null)
        {
            ShowMessage(msg, title, MessageStyle.Error, config);
        }

        public static void Info(string msg, string title = "", Action<FontConfig> config = null)
        {
            ShowMessage(msg, title, MessageStyle.Info, config);
        }

        public static void ShowMessage(string msg, string title, MessageStyle messageStyle, Action<FontConfig> configAction)
        {
            if (TextView.Instance != null)
            {
                return;
            }
            var config = new FontConfig();
            configAction?.Invoke(config);
            TextView.Instance = new TextView()
            {
                Title = title,
                MessageStyle = messageStyle,
                Message = msg,
                Config = config
            };
            var dialog = ContentDialog.CreateDialog(TextView.Instance, null, null);
            dialog.Show();
        }
    }
}
