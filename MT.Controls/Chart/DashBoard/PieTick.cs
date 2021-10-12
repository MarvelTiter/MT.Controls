using MT.Controls.Helper;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MT.Controls.Chart
{
    public class PieTickDefinition : SectionDefinition
    {
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(nameof(Step), typeof(double), typeof(PieTickDefinition), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.AffectsRender));


        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(PieTickDefinition), new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.AffectsRender));



    }
    public class PieTickCollection : ObservableCollection<PieTickDefinition> { }
    internal class PieTick : PieCompoment
    {
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(nameof(Step), typeof(double), typeof(PieTick), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public PieTick()
        {
            SetCurrentValue(StrokeProperty, Brushes.LimeGreen);
            SetCurrentValue(StrokeThicknessProperty, 2d);
        }

        protected override Geometry GeometryGenerator()
        {
            if (ActualHeight * ActualWidth == 0.0 && Width * Height == 0.0)
            {
                return Geometry.Empty;
            }
            StreamGeometry stream = new StreamGeometry();

            using (StreamGeometryContext ctx = stream.Open())
            {
                double min = AngleRange.Min;
                double max = AngleRange.Max;
                double actualRadius = Math.Min(ActualWidth, ActualHeight) / 2.0;
                double minRadius = RadiusRange.Min * actualRadius;
                double maxRadius = RadiusRange.Max * actualRadius;
                double step = Step;
                double offsetX = ActualWidth / 2.0;
                double offsetY = ActualHeight / 2.0;
                int tickCount = (int)((max - min) / step);
                for (int i = 0; i <= tickCount; i++)
                {
                    Point startPoint = MathHelper.PolarToCartesian(min + i * step, minRadius);
                    Point point = MathHelper.PolarToCartesian(min + i * step, maxRadius);
                    startPoint.Offset(offsetX, offsetY);
                    point.Offset(offsetX, offsetY);
                    ctx.BeginFigure(startPoint, isFilled: false, isClosed: false);
                    ctx.LineTo(point, isStroked: true, isSmoothJoin: false);
                }
            }
            return stream;
        }
    }
}
