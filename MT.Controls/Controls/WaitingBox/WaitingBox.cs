using MT.Controls.WaitingBox;
using System;
using System.Threading.Tasks;

namespace MT.Controls
{
    public interface IWaitingHandler
    {
        Task UpdateMessage(string msg);
    }
    public class WaitingHost
    {
        /// <summary>
        /// 调用等待窗口，注意委托返回值应该返回第二个参数的Task，并且设置结果或者异常
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task<bool> Wait(Func<IWaitingHandler, TaskCompletionSource<bool>, Task<bool>> action, bool autoClose = false)
        {
            if (WaitingView.Instance != null)
            {
                return false;
            }
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            WaitingView.Instance = new WaitingView
            {
                Mission = action,
                TaskCompletion = tcs,
            };
            var dialog = ContentDialog.CreateDialog(WaitingView.Instance, null, null);
            var activity = WaitingView.Instance as IDialogActivity;
            dialog.OnDialogOpening += activity.OnDialogOpening;
            dialog.OnDialogClosing += activity.OnDialogClosing;

            if (autoClose)
            {
                WaitingView.Instance.TaskFinishedAction = () =>
                {
                    dialog.Close();
                };
            }

            dialog.Show();
            return await tcs.Task;
        }
    }
}
