using MT.Controls.Helper;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace MT.Controls.Override
{
    public class PopupEx : Popup
    {
        private static MethodInfo method = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Window win;
        public PopupEx()
        {
            Loaded += PopupEx_Loaded;
        }

        private void PopupEx_Loaded(object sender, RoutedEventArgs e)
        {
            Popup pup = sender as Popup;
            win = WindowHelper.GetActiveWindow();

            if (win != null)
            {
                win.LocationChanged -= PositionChanged;
                win.SizeChanged -= PositionChanged;
                if (IsPositionUpdate)
                {
                    win.LocationChanged += PositionChanged;
                    win.SizeChanged += PositionChanged;
                }
            }
        }


        /// <summary>  
        /// 刷新位置  
        /// </summary>  
        private void PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (IsOpen)
                {
                    method.Invoke(this, null);
                }
            }
            catch
            {
                return;
            }
        }

        //是否最前默认为非最前（false）  
        public static DependencyProperty TopmostProperty = Window.TopmostProperty.AddOwner(typeof(Popup), new FrameworkPropertyMetadata(false, OnTopmostChanged));
        public bool Topmost
        {
            get { return (bool)GetValue(TopmostProperty); }
            set { SetValue(TopmostProperty, value); }
        }
        private static void OnTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as PopupEx).UpdateWindow();
        }

        /// <summary>  
        /// 重写拉开方法，置于非最前  
        /// </summary>  
        /// <param name="e"></param>  
        protected override void OnOpened(EventArgs e)
        {
            UpdateWindow();
        }

        /// <summary>  
        /// 刷新Popup层级  
        /// </summary>  
        private void UpdateWindow()
        {
            var hwnd = ((HwndSource)PresentationSource.FromVisual(Child)).Handle;
            RECT rect;
            if (NativeMethods.GetWindowRect(hwnd, out rect))
            {
                NativeMethods.SetWindowPos(hwnd, Topmost ? -1 : -2, rect.Left, rect.Top, (int)this.Width, (int)this.Height, 0);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #region P/Invoke imports & definitions  
        public static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
            [DllImport("user32", EntryPoint = "SetWindowPos")]
            internal static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        }
        #endregion
        public bool IsPositionUpdate
        {
            get { return (bool)GetValue(IsPositionUpdateProperty); }
            set { SetValue(IsPositionUpdateProperty, value); }
        }
        public static readonly DependencyProperty IsPositionUpdateProperty =
            DependencyProperty.Register("IsPositionUpdate", typeof(bool), typeof(PopupEx), new PropertyMetadata(true, IsPositionUpdateChanged));

        private static void IsPositionUpdateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PopupEx).PopupEx_Loaded(d as PopupEx, null);
        }
    }
}
