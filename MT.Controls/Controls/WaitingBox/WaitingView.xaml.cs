using MT.Controls.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MT.Controls.WaitingBox
{
    /// <summary>
    /// WaitingView.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingView : UserControl, IDialogActivity, IWaitingHandler
    {
        public static WaitingView Instance { get; set; }
        public WaitingView()
        {
            InitializeComponent();
            DataContext = this;
        }
        private Task task;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;
        public Func<IWaitingHandler, TaskCompletionSource<bool>, Task<bool>> Mission { get; set; }
        public TaskCompletionSource<bool> TaskCompletion { get; set; }
        public Action TaskFinishedAction { get; set; }
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(WaitingView), new PropertyMetadata(default(string)));

        public void OnDialogOpening(IDialogParameter parameter)
        {
            Message = "Running";
            tokenSource = new CancellationTokenSource();
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            token = tokenSource.Token;
            task = Task.Run(async () =>
            {
                await Mission(this, TaskCompletion);
            }, token);
            _ = task.ContinueWith(async t =>
             {
                 var msg = TaskCompletion.Task.Exception?.InnerException?.Message;
                 if (string.IsNullOrEmpty(msg)) msg = "Finished";
                 await DispatcherHelper.InvokeAsync(async () =>
                 {
                     loading.Visibility = Visibility.Collapsed;
                     Message = $"Mission Complete:{msg}";
                     await Task.Delay(500);
                     TaskFinishedAction?.Invoke();
                 });
             }, token);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {

        }

        public void OnDialogClosing(IDialogParameter parameter)
        {
            Instance = null;
            TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;
            Abort();
        }

        public object GetDialogResult()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateMessage(string msg)
        {
            await DispatcherHelper.InvokeAsync(() =>
            {
                Message = msg;
            });
        }
        private void Abort()
        {
            tokenSource.Cancel();
            //TaskCompletion.SetCanceled();
        }
    }
}
