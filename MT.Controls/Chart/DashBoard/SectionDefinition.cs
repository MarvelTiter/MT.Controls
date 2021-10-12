using MT.Controls.Common;
using System.Windows;
using System.Windows.Media;

namespace MT.Controls.Chart
{
    public class SectionDefinition : FrameworkContentElement
    {
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(PieSectionDefinition), new FrameworkPropertyMetadata(Brushes.LimeGreen, FrameworkPropertyMetadataOptions.AffectsRender));

        public Range AngleRange
        {
            get { return (Range)GetValue(AngleRangeProperty); }
            set { SetValue(AngleRangeProperty, value); }
        }
        public static readonly DependencyProperty AngleRangeProperty =
            DependencyProperty.Register(nameof(AngleRange), typeof(Range), typeof(PieSectionDefinition), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public Range RadiusRange
        {
            get { return (Range)GetValue(RadiusRangeProperty); }
            set { SetValue(RadiusRangeProperty, value); }
        }
        public static readonly DependencyProperty RadiusRangeProperty =
            DependencyProperty.Register(nameof(RadiusRange), typeof(Range), typeof(PieSectionDefinition), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
