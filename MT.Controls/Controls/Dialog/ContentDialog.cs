using MT.Controls.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace MT.Controls
{
    public class ContentDialog : ContentControl
    {
        private object _token;
        private readonly IDialogParameter parameter;
        private AdornerMask _container;
        private AdornerDecorator _decorator;

        public event Action<IDialogParameter> OnDialogOpening;
        public event Action<IDialogParameter> OnDialogClosing;
        public bool RequiredResult { get; set; } = false;
        private TaskCompletionSource<IDialogResult> tcs;
        private IDialogResult dialogResult;
        private static readonly Dictionary<object, FrameworkElement> ContainerDic = new Dictionary<object, FrameworkElement>();

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(ContentDialog), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            internal set => SetValue(IsOpenProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.RegisterAttached(
            "Identifier", typeof(string), typeof(ContentDialog), new PropertyMetadata(default(string), OnIdentifierChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ContentDialog), new PropertyMetadata(""));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(ContentDialog), new PropertyMetadata(null));


        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Footer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(object), typeof(ContentDialog), new PropertyMetadata(null));

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (e.NewValue == null)
                {
                    Unregister(element);
                }
                else
                {
                    Register(e.NewValue.ToString(), element);
                }
            }
        }

        public static void SetIdentifier(DependencyObject element, string value)
            => element.SetValue(IdentifierProperty, value);

        public static string GetIdentifier(DependencyObject element)
            => (string)element.GetValue(IdentifierProperty);

        public static void Register(string token, FrameworkElement element)
        {
            if (string.IsNullOrEmpty(token) || element == null) return;
            ContainerDic[token] = element;
        }

        public static void Unregister(string token, FrameworkElement element)
        {
            if (string.IsNullOrEmpty(token) || element == null) return;

            if (ContainerDic.ContainsKey(token))
            {
                if (ReferenceEquals(ContainerDic[token], element))
                {
                    ContainerDic.Remove(token);
                }
            }
        }

        public static void Unregister(FrameworkElement element)
        {
            if (element == null) return;
            var first = ContainerDic.FirstOrDefault(item => ReferenceEquals(element, item.Value));
            if (!string.IsNullOrEmpty(first.Key?.ToString()))
            {
                ContainerDic.Remove(first.Key);
            }
        }

        public static void Unregister(string token)
        {
            if (string.IsNullOrEmpty(token)) return;

            if (ContainerDic.ContainsKey(token))
            {
                ContainerDic.Remove(token);
            }
        }
        internal ContentDialog(object content, object token = null, IDialogParameter parameter = null)
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) => Close()));
            CommandBindings.Add(new CommandBinding(ControlCommands.Save, (s, e) => DialogResult()));
            _token = token;
            this.parameter = parameter;
            Content = content;

            FrameworkElement element;
            AdornerDecorator decorator;
            if (token == null)
            {
                element = WindowHelper.GetMainWindow();
            }
            else
            {
                ContainerDic.TryGetValue(token, out element);
            }
            decorator = VisualHelper.GetChild<AdornerDecorator>(element);

            if (decorator != null)
            {
                if (decorator.Child != null)
                {
                    decorator.Child.IsEnabled = false;
                }
                var layer = decorator.AdornerLayer;
                if (layer != null)
                {
                    var container = new AdornerMask(layer)
                    {
                        Child = this
                    };
                    _container = container;
                    _decorator = decorator;
                    //Loaded += OnLoaded;
                    Unloaded += OnUnloaded;
                }
            }
        }
        private void OnLoaded(object s, EventArgs e)
        {
            Loaded -= OnLoaded;
            OnDialogOpening?.Invoke(parameter);
        }
        private void OnUnloaded(object s, RoutedEventArgs e)
        {
            Unloaded -= OnUnloaded;
            OnDialogClosing?.Invoke(parameter);
        }
        public static ContentDialog CreateDialog(object content, object token = null, IDialogParameter parameter = null)
        {
            var dialog = new ContentDialog(content, token, parameter);
            return dialog;
        }

        public Task<IDialogResult> Show(TaskCompletionSource<IDialogResult> taskCompletionSource)
        {
            if (taskCompletionSource == null)
                tcs = new TaskCompletionSource<IDialogResult>();
            else tcs = taskCompletionSource;
            Open();
            return tcs.Task;
        }

        private void Open()
        {
            if (_decorator != null)
            {
                if (_decorator.Child != null)
                {
                    _decorator.Child.IsEnabled = false;
                }
                var layer = _decorator.AdornerLayer;
                OnDialogOpening?.Invoke(parameter);
                layer?.Add(_container);
                IsOpen = true;
            }
        }

        public void Show()
        {
            Open();
        }

        public void Close()
        {
            if (tcs != null)
            {
                dialogResult = new DialogResult();
                dialogResult.DialogAction = DialogFeedback.Cancel;
                tcs.SetResult(dialogResult);
            }
            _close();
        }

        private void _close()
        {
            // 关闭Dialog
            if (_decorator == null)
            {
                return;
            }
            if (_decorator.Child != null)
            {
                _decorator.Child.IsEnabled = true;
            }
            var layer = _decorator.AdornerLayer;
            layer?.Remove(_container);
            IsOpen = false;
        }

        public void DialogResult()
        {
            // 关闭之前设置返回值
            if (tcs != null)
            {
                dialogResult = new DialogResult();
                dialogResult.DialogAction = DialogFeedback.Confirm;
                if (Content is DialogBorder border && border.Content is FrameworkElement fe && fe.DataContext is IDialogActivity activity)
                {
                    object v = activity.GetDialogResult();
                    dialogResult.Result = v;
                }
                tcs.SetResult(dialogResult);
            }
            _close();
        }
    }
}
