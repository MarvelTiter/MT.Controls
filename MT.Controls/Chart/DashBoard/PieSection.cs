using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using MT.Controls.Common;
using MT.Controls.Helper;
using Range = MT.Controls.Common.Range;

namespace MT.Controls.Chart
{
    public class PieSectionDefinition : SectionDefinition
    {
        public Range ValueRange
        {
            get { return (Range)GetValue(ValueRangeProperty); }
            set { SetValue(ValueRangeProperty, value); }
        }
        public static readonly DependencyProperty ValueRangeProperty =
            DependencyProperty.Register(nameof(ValueRange), typeof(Range), typeof(PieSectionDefinition), new PropertyMetadata(new Range(0, 100)));

    }
    public class PieSectionCollection : ObservableCollection<PieSectionDefinition> { }

    internal class PieSection : PieCompoment
    {
        public PieSection()
        {
            SetCurrentValue(FillProperty, Brushes.Chocolate);
        }
        protected override Geometry GeometryGenerator()
        {
            if (ActualHeight * ActualWidth == 0.0 || Height * Width == 0.0)
            {
                return Geometry.Empty;
            }
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext ctx = streamGeometry.Open())
            {

                double min = AngleRange.Min;
                double max = AngleRange.Max;
                double actualRadius = (ActualHeight * ActualWidth != 0.0) ? (Math.Min(ActualWidth, ActualHeight) / 2.0) : (Math.Min(Width, Height) / 2.0);
                double minRadius = RadiusRange.Min * actualRadius;
                double maxRadius = RadiusRange.Max * actualRadius;
                Size size = new Size(minRadius, minRadius);
                Size size2 = new Size(maxRadius, maxRadius);
                bool isLargeArc = max - min > 180.0;
                Point p1 = MathHelper.PolarToCartesian(min, minRadius);
                Point p2 = MathHelper.PolarToCartesian(min, maxRadius);
                Point p3 = MathHelper.PolarToCartesian(max, maxRadius);
                Point p4 = MathHelper.PolarToCartesian(max, minRadius);
                double offsetX = ActualWidth / 2.0;
                double offsetY = ActualHeight / 2.0;
                p1.Offset(offsetX, offsetY);
                p2.Offset(offsetX, offsetY);
                p3.Offset(offsetX, offsetY);
                p4.Offset(offsetX, offsetY);
                ctx.BeginFigure(p1, isFilled: true, isClosed: true);
                ctx.LineTo(p2, isStroked: true, isSmoothJoin: false);
                ctx.ArcTo(p3, size2, 0.0, isLargeArc, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
                ctx.LineTo(p4, isStroked: true, isSmoothJoin: false);
                ctx.ArcTo(p1, size, 0.0, isLargeArc, SweepDirection.Counterclockwise, isStroked: true, isSmoothJoin: false);
            }
            return streamGeometry;
        }
    }
}
