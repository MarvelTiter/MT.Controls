using System.Linq;
using System.Windows;

namespace MT.Controls.Helper {
    public static class WindowHelper {
        public static Window GetActiveWindow() => Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        public static Window GetMainWindow() => Application.Current.MainWindow;
    }
}
