using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MT.Controls.Common
{
    public static class DispatcherHelper
    {
        private static Dispatcher dispatcher;
        public static void Init()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public static async Task InvokeAsync(Action action)
        {
            if (action == null)
            {
                return;
            }
            CheckDispatcher();
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                await dispatcher.BeginInvoke(action);
            }
        }
        public static async Task<T> InvokeAsync<T>(Func<T> callback)
        {
            if (callback == null)
            {
                return default;
            }
            CheckDispatcher();
            if (dispatcher.CheckAccess())
            {
                return callback();
            }
            else
            {
                return await dispatcher.InvokeAsync(callback);
            }
        }
        private static void CheckDispatcher()
        {
            if (dispatcher == null)
            {
                throw new InvalidOperationException("The DispatcherHelper is not initialized.");
            }
        }

    }
}
