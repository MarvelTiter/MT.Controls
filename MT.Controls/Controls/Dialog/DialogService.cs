using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace MT.Controls
{
    public class DialogService
    {
        static Dictionary<string, DialogRegister> cache = new Dictionary<string, DialogRegister>();

        /// <summary>
        /// 注册Dialog服务
        /// </summary>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <typeparam name="TViewModel">视图ViewModel类型</typeparam>
        /// <param name="key"></param>
        public static void RegisterService<TView, TViewModel>(string key = null) where TViewModel : IDialogActivity
            where TView : FrameworkElement, new()
        {
            if (string.IsNullOrEmpty(key))
                key = typeof(TViewModel).Name;
            if (cache.ContainsKey(key))
            {
                return;
            }
            DialogRegister context = new DialogRegister
            {
                ViewType = typeof(TView),
                ViewModelType = typeof(TViewModel),
            };
            cache.Add(key, context);
        }
        /// <summary>
        /// 注册Dialog服务
        /// </summary>
        /// <param name="key"></param>
        public static void RegisterService<T>(string key = null) where T : FrameworkElement, IDialogActivity, new()
        {
            if (string.IsNullOrEmpty(key))
                key = typeof(T).Name;
            if (cache.ContainsKey(key))
            {
                return;
            }
            DialogRegister context = new DialogRegister
            {
                ViewType = typeof(T),
                ViewModelType = null,
            };
            cache.Add(key, context);
        }
        /// <summary>
        /// Dialog窗口(自动注册)
        /// </summary>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <typeparam name="TViewModel">视图ViewModel类型</typeparam>
        /// <param name="title">Dialog标题</param>
        /// <param name="dialogIdentity">Dialog显示标识，为null则为当前window</param>
        /// <param name="p">Dialog传递参数</param>
        /// <returns>Dialog返回结果</returns>
        public static async Task<IDialogResult> Show<TView, TViewModel>(string title = null, object dialogIdentify = null, object p = null) where TViewModel : IDialogActivity
            where TView : FrameworkElement, new()
        {
            RegisterService<TView, TViewModel>();
            return await Show<TViewModel>(title, dialogIdentify, p);
        }
        /// <summary>
        /// Dialog窗口(自动注册)
        /// </summary>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <typeparam name="TViewModel">视图ViewModel类型</typeparam>
        /// <param name="title">Dialog标题</param>
        /// <param name="dialogIdentity">Dialog显示标识，为null则为当前window</param>
        /// <param name="parameter">用于传递Dialog参数的对象</param>
        /// <returns>Dialog返回结果</returns>
        public static async Task<IDialogResult> Show<TView, TViewModel>(string title, object dialogIdentity, IDialogParameter parameter) where TViewModel : IDialogActivity
            where TView : FrameworkElement, new()
        {
            RegisterService<TView, TViewModel>();
            return await Show(typeof(TViewModel).Name, title, dialogIdentity, parameter);
        }
        /// <summary>
        /// Dialog窗口(需要已注册)
        /// </summary>
        /// <typeparam name="T">已注册的ViewModel类型</typeparam>
        /// <param name="title">Dialog标题</param>
        /// <param name="dialogIdentify">Dialog显示标识，为null则为当前window</param>
        /// <param name="p">Dialog传递参数</param>
        /// <returns></returns>
        public static async Task<IDialogResult> Show<T>(string title = null, object dialogIdentify = null, object p = null)
        {
            DialogParameter dialogParameter = new DialogParameter();
            if (p != null)
                dialogParameter.Add(p);
            return await Show(typeof(T).Name, title, dialogIdentify, dialogParameter);
        }
        /// <summary>
        /// Dialog窗口(需要已注册)
        /// </summary>
        /// <param name="key">已注册的Key</param>
        /// <param name="title">Dialog标题</param>
        /// <param name="dialogIndentify">Dialog显示标识，为null则为当前window</param>
        /// <param name="parameter">用于传递Dialog参数的对象</param>
        /// <returns></returns>
        public static async Task<IDialogResult> Show(string key, string title, object dialogIndentify, IDialogParameter parameter)
        {
            TaskCompletionSource<IDialogResult> tcs = new TaskCompletionSource<IDialogResult>();
            try
            {
                if (!cache.TryGetValue(key, out var context))
                {
                    throw new Exception($"DialogService {key} did not register, please invoke DialogService.RegisterService first");
                }
                var border = new DialogBorder();
                border.Title = title;
                border.Content = Activator.CreateInstance(context.ViewType);
                //var element = Activator.CreateInstance(context.ViewType);
                object viewModel = null;
                if (border.Content is FrameworkElement fe)
                {
                    if (fe.DataContext == null)
                    {
                        if (ServiceLocator.IsLocationProviderSet)
                            viewModel = ServiceLocator.Current.GetInstance(context.ViewModelType);
                        else
                        {
                            if (context.ViewModelType != null)
                                viewModel = Activator.CreateInstance(context.ViewModelType);
                            else
                                viewModel = border.Content;
                        }
                        fe.DataContext = viewModel;
                    }
                    else
                    {
                        viewModel = fe.DataContext;
                    }
                }
                if (viewModel == null || !(viewModel is IDialogActivity activity))
                    throw new ArgumentException($"view model {viewModel} is null or is not a IDialogActivity instance");

                if (parameter == null) parameter = new DialogParameter();
                var dialog = ContentDialog.CreateDialog(border, dialogIndentify, parameter);
                dialog.OnDialogOpening += activity.OnDialogOpening;
                dialog.OnDialogClosing += activity.OnDialogClosing;
                var result = await dialog.Show(tcs);
                return result;
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return await tcs.Task;
        }
    }
}
