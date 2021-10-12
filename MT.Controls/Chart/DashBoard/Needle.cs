using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MT.Controls.Chart
{
    public class NeedleDefinition : FrameworkContentElement
    {
        public double PointLength
        {
            get { return (double)GetValue(PointLengthProperty); }
            set { SetValue(PointLengthProperty, value); }
        }
        public static readonly DependencyProperty PointLengthProperty =
            DependencyProperty.Register(nameof(PointLength), typeof(double), typeof(NeedleDefinition), new FrameworkPropertyMetadata(0.1, FrameworkPropertyMetadataOptions.AffectsRender));

        public double TailLength
        {
            get { return (double)GetValue(TailLengthProperty); }
            set { SetValue(TailLengthProperty, value); }
        }
        public static readonly DependencyProperty TailLengthProperty =
            DependencyProperty.Register(nameof(TailLength), typeof(double), typeof(NeedleDefinition), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(nameof(Width), typeof(double), typeof(NeedleDefinition), new FrameworkPropertyMetadata(0.03, FrameworkPropertyMetadataOptions.AffectsRender));

        public double CenterOffset
        {
            get { return (double)GetValue(CenterOffsetProperty); }
            set { SetValue(CenterOffsetProperty, value); }
        }
        public static readonly DependencyProperty CenterOffsetProperty =
            DependencyProperty.Register(nameof(CenterOffset), typeof(double), typeof(NeedleDefinition), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(NeedleDefinition), new FrameworkPropertyMetadata(Brushes.LimeGreen, FrameworkPropertyMetadataOptions.AffectsRender));



    }
    public class Needle : Shape
    {
        protected override Geometry DefiningGeometry => GetGeomertyGenerator();

        /// <summary>
        /// 指针头部长度(0-1)
        /// </summary>
        public double NeedleLength
        {
            get { return (double)GetValue(NeedleLengthProperty); }
            set { SetValue(NeedleLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NeedleLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeedleLengthProperty =
            DependencyProperty.Register(nameof(NeedleLength), typeof(double), typeof(Needle), new FrameworkPropertyMetadata(0.1, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 指针尾部长度(0-1)
        /// </summary>
        public double NeedleTailLength
        {
            get { return (double)GetValue(NeedleTailLengthProperty); }
            set { SetValue(NeedleTailLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NeedleTailLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeedleTailLengthProperty =
            DependencyProperty.Register(nameof(NeedleTailLength), typeof(double), typeof(Needle), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 指针宽度(0-1)
        /// </summary>
        public double NeedleWidth
        {
            get { return (double)GetValue(NeedleWidthProperty); }
            set { SetValue(NeedleWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NeedleWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeedleWidthProperty =
            DependencyProperty.Register(nameof(NeedleWidth), typeof(double), typeof(Needle), new FrameworkPropertyMetadata(0.03, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 偏移圆心距离(0-1)
        /// </summary>
        public double CenterOffset
        {
            get { return (double)GetValue(CenterOffsetProperty); }
            set { SetValue(CenterOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterOffsetProperty =
            DependencyProperty.Register(nameof(CenterOffset), typeof(double), typeof(Needle), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.AffectsRender));

        private Geometry GetGeomertyGenerator()
        {
            if (ActualHeight * ActualWidth == 0)
            {
                return Geometry.Empty;
            }
            StreamGeometry stream = new StreamGeometry();
            using (StreamGeometryContext ctx = stream.Open())
            {
                double actualRadius = Math.Min(ActualHeight, ActualWidth) / 2.0;
                double width = actualRadius * NeedleWidth / 2.0;
                double widthTail = actualRadius * NeedleTailLength;
                double length = actualRadius * NeedleLength;
                double offsetX = ActualWidth / 2.0;
                double offsetY = ActualHeight / 2.0;
                double offsetX2 = actualRadius * CenterOffset;
                Point startPoint = new Point(0.0, 0.0 - width);
                Point p1 = new Point(widthTail, 0.0 - width);
                Point p2 = new Point(widthTail + length, 0.0);
                Point p3 = new Point(widthTail, width);
                Point p4 = new Point(0.0, width);
                startPoint.Offset(offsetX, offsetY);
                startPoint.Offset(offsetX2, 0.0);
                p1.Offset(offsetX, offsetY);
                p1.Offset(offsetX2, 0.0);
                p2.Offset(offsetX, offsetY);
                p2.Offset(offsetX2, 0.0);
                p3.Offset(offsetX, offsetY);
                p3.Offset(offsetX2, 0.0);
                p4.Offset(offsetX, offsetY);
                p4.Offset(offsetX2, 0.0);
                ctx.BeginFigure(startPoint, isFilled: true, isClosed: true);
                ctx.LineTo(p1, isStroked: true, isSmoothJoin: false);
                ctx.LineTo(p2, isStroked: true, isSmoothJoin: true);
                ctx.LineTo(p3, isStroked: true, isSmoothJoin: false);
                ctx.LineTo(p4, isStroked: true, isSmoothJoin: false);
            }
            return stream;
        }
    }
}
