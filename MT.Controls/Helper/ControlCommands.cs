using System.Windows.Input;

namespace MT.Controls {
    public static class ControlCommands {
        public static RoutedCommand Close { get; } = new RoutedCommand(nameof(Close), typeof(ControlCommands));
        public static RoutedCommand Open { get; } = new RoutedCommand(nameof(Open), typeof(ControlCommands));
        public static RoutedCommand Save { get; } = new RoutedCommand(nameof(Save), typeof(ControlCommands));
        public static RoutedCommand Toggle { get; } = new RoutedCommand(nameof(Toggle), typeof(ControlCommands));

    }
}
