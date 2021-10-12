using System.Windows;
using System.Windows.Media;

namespace MT.Controls.Chart
{
    public class LabelDefinition : FrameworkContentElement
    {
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(LabelDefinition), new FrameworkPropertyMetadata(14d, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(LabelDefinition), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(nameof(Step), typeof(double), typeof(LabelDefinition), new FrameworkPropertyMetadata(30d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double CenterOffset
        {
            get { return (double)GetValue(CenterOffsetProperty); }
            set { SetValue(CenterOffsetProperty, value); }
        }
        public static readonly DependencyProperty CenterOffsetProperty =
            DependencyProperty.Register(nameof(CenterOffset), typeof(double), typeof(LabelDefinition), new FrameworkPropertyMetadata(0.5d, FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
