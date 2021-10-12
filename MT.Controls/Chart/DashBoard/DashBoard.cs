using MT.Controls.Helper;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Range = MT.Controls.Common.Range;

namespace MT.Controls.Chart
{
    [TemplatePart(Name = "PART_PieSection", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_TickSection", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_Lable", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Needle", Type = typeof(Grid))]
    public class DashBoard : Control
    {
        public Range ValueRange
        {
            get { return (Range)GetValue(ValueRangeProperty); }
            set { SetValue(ValueRangeProperty, value); }
        }
        public static readonly DependencyProperty ValueRangeProperty =
            DependencyProperty.Register(nameof(ValueRange), typeof(Range), typeof(DashBoard), new PropertyMetadata(null));

        public Range AngleRange
        {
            get { return (Range)GetValue(AngleRangeProperty); }
            set { SetValue(AngleRangeProperty, value); }
        }
        public static readonly DependencyProperty AngleRangeProperty =
            DependencyProperty.Register(nameof(AngleRange), typeof(Range), typeof(DashBoard), new PropertyMetadata(null));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(DashBoard), new PropertyMetadata(0d, OnValueChanged));

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(DashBoard), new PropertyMetadata(0d));

        public PieSectionCollection PieSectionDefinitions
        {
            get { return (PieSectionCollection)GetValue(PieSectionDefinitionsProperty); }
            set { SetValue(PieSectionDefinitionsProperty, value); }
        }
        public static readonly DependencyProperty PieSectionDefinitionsProperty =
            DependencyProperty.Register(nameof(PieSectionDefinitions), typeof(PieSectionCollection), typeof(DashBoard), new PropertyMetadata(null, (d, e) => (d as DashBoard)?.RenderPies()));

        public PieTickCollection PieTickDefinitions
        {
            get { return (PieTickCollection)GetValue(PieTickDefinitionsProperty); }
            set { SetValue(PieTickDefinitionsProperty, value); }
        }
        public static readonly DependencyProperty PieTickDefinitionsProperty =
            DependencyProperty.Register(nameof(PieTickDefinitions), typeof(PieTickCollection), typeof(DashBoard), new PropertyMetadata(null, (d, e) => (d as DashBoard)?.RenderTicks()));

        public LabelDefinition LabelConfig
        {
            get { return (LabelDefinition)GetValue(LabelConfigProperty); }
            set { SetValue(LabelConfigProperty, value); }
        }
        public static readonly DependencyProperty LabelConfigProperty =
            DependencyProperty.Register(nameof(LabelConfig), typeof(LabelDefinition), typeof(DashBoard), new PropertyMetadata(null, (d, e) => (d as DashBoard)?.RenderLabel()));

        public NeedleDefinition NeedleConfig
        {
            get { return (NeedleDefinition)GetValue(NeedleConfigProperty); }
            set { SetValue(NeedleConfigProperty, value); }
        }
        public static readonly DependencyProperty NeedleConfigProperty =
            DependencyProperty.Register(nameof(NeedleConfig), typeof(NeedleDefinition), typeof(DashBoard), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(DashBoard), new PropertyMetadata(""));

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(nameof(Unit), typeof(string), typeof(DashBoard), new PropertyMetadata(""));

        public bool AutoRaise
        {
            get { return (bool)GetValue(AutoRaiseProperty); }
            set { SetValue(AutoRaiseProperty, value); }
        }
        public static readonly DependencyProperty AutoRaiseProperty =
            DependencyProperty.Register(nameof(AutoRaise), typeof(bool), typeof(DashBoard), new PropertyMetadata(false));

        private Grid pieSectionGrid;
        private Grid tickSectionGrid;
        private Canvas labelCanvas;

        public override void OnApplyTemplate()
        {
            pieSectionGrid = GetTemplateChild("PART_PieSection") as Grid;
            tickSectionGrid = GetTemplateChild("PART_TickSection") as Grid;
            labelCanvas = GetTemplateChild("PART_Lable") as Canvas;
        }

        public DashBoard()
        {
            SizeChanged += (s, e) =>
            {
                Render();
            };
        }

        private void Render()
        {
            RenderPies();
            RenderTicks();
            RenderLabel();
        }

        private void RenderTicks()
        {
            if (PieTickDefinitions != null && tickSectionGrid != null)
            {
                tickSectionGrid.Children.Clear();
                foreach (var tick in PieTickDefinitions)
                {
                    var t = new PieTick
                    {
                        AngleRange = tick.AngleRange,
                        RadiusRange = tick.RadiusRange,
                        Stroke = tick.Fill,
                        Step = tick.Step,
                        StrokeThickness = tick.Thickness,
                    };
                    tickSectionGrid.Children.Add(t);
                }
            }
        }

        private void RenderPies()
        {
            if (PieSectionDefinitions != null && pieSectionGrid != null)
            {
                pieSectionGrid.Children.Clear();
                foreach (var pie in PieSectionDefinitions)
                {
                    var p = new PieSection
                    {
                        AngleRange = pie.AngleRange,
                        RadiusRange = pie.RadiusRange,
                        Fill = pie.Fill,
                    };
                    pieSectionGrid.Children.Add(p);
                }
            }
        }

        private void RenderLabel()
        {
            if (labelCanvas != null && (ActualHeight * ActualWidth != 0.0 || Height * Width != 0.0) && LabelConfig != null)
            {
                // 获取实际渲染的边长;

                var zero = ActualHeight * ActualWidth == 0.0;
                var minWidth = zero ? Width : ActualWidth;
                var minHeight = zero ? Height : ActualHeight;
                double radius = Math.Min(minWidth, minHeight) / 2.0 * LabelConfig.CenterOffset;
                Point origin = new Point(minWidth / 2, minHeight / 2);

                //var minSide = ActualHeight * ActualWidth == 0.0 ? (Width, Height) : (ActualWidth, ActualHeight);
                //double radius = Math.Min(minSide.Item1, minSide.Item2) / 2.0 * LabelConfig.CenterOffset;
                //Point origin = new Point(minSide.Item1 / 2, minSide.Item2 / 2);

                labelCanvas.Children.Clear();
                int labelCount = (int)(ValueRange.Distance / LabelConfig.Step);
                double labelAngle = MathHelper.Normalize(ValueRange.Min + LabelConfig.Step, ValueRange.Min, ValueRange.Max, AngleRange.Min, AngleRange.Max) - AngleRange.Min;
                double angleStart = AngleRange.Min;
                double valueStart = ValueRange.Min;
                do
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Text = valueStart.ToString(),
                        FontSize = LabelConfig.FontSize,
                        Foreground = LabelConfig.Foreground,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextAlignment = TextAlignment.Center
                    };
                    labelCanvas.Children.Add(textBlock);
                    textBlock.UpdateLayout();
                    Point offset = MathHelper.PolarToCartesian(angleStart, radius);
                    // 移动到原点 => 偏移 => 居中
                    Canvas.SetLeft(textBlock, origin.X + offset.X - textBlock.ActualWidth / 2.0);
                    Canvas.SetTop(textBlock, origin.Y + offset.Y - textBlock.ActualHeight / 2.0);
                    angleStart += labelAngle;
                    valueStart += LabelConfig.Step;
                }
                while (valueStart <= ValueRange.Max);
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 判断是增加还是减少
            var raise = ((double)e.NewValue).GreaterThan((double)e.OldValue, 1000);
            //Debug.WriteLine($"{raise} new {e.NewValue} old {e.OldValue} diff {(double)e.NewValue- (double)e.OldValue}");
            (d as DashBoard)?.UpdateAngle(raise);
        }

        private double _valueMax = double.NaN;
        private double _valueMin = double.NaN;
        /// <summary>
        /// true -> 值增加，false -> 值减少
        /// </summary>
        /// <param name="raise"></param>
        private void UpdateAngle(bool raise)
        {
            if (AutoRaise)
            {
                UpdateLabel(raise);
            }

            Angle = MathHelper.Normalize(Value, ValueRange.Min, ValueRange.Max, AngleRange.Min, AngleRange.Max);
            if (Value > ValueRange.Max)
            {
                Angle = AngleRange.Max + MathHelper.Random(10);
            }
            else if (Value < ValueRange.Min)
            {
                Angle = AngleRange.Min - MathHelper.Random(10);
            }
        }

        private void UpdateLabel(bool raise)
        {
            if (double.IsNaN(_valueMax) && double.IsNaN(_valueMin))
            {
                _valueMax = ValueRange.Max;
                _valueMin = ValueRange.Min;
            }
            if (raise && Value > ValueRange.Max)
            {
                ValueRange = new Range(ValueRange.Min, ValueRange.Max + ValueRange.Distance / 5);
                RenderLabel();
                return;
            }
            else if (!raise && Angle < AngleRange.Max - AngleRange.Distance / 5)
            {
                var max = ValueRange.Max - ValueRange.Distance / 5;
                max = max > _valueMax ? max : _valueMax;
                ValueRange = new Range(ValueRange.Min, max);
                RenderLabel();
                return;
            }
        }
    }
}
