using MT.Controls.MsgBox;
using System;
using System.Threading.Tasks;

namespace MT.Controls
{
    public class ConfirmBox
    {
        public static async Task<IDialogResult> IsConfirm(string msg, string title = "", MessageStyle messageStyle = MessageStyle.Warning, Action<FontConfig> configAction = null)
        {
            TaskCompletionSource<IDialogResult> tcs = new TaskCompletionSource<IDialogResult>();
            if (TextView.Instance == null)
            {
                var config = new FontConfig();
                configAction?.Invoke(config);
                TextView.Instance = new TextView(true)
                {
                    Title = title,
                    MessageStyle = messageStyle,
                    Message = msg,
                    Config = config
                };
                var dialog = ContentDialog.CreateDialog(TextView.Instance, null, null);
                await dialog.Show(tcs);
            }
            return await tcs.Task;

        }
    }
}
