using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Range = MT.Controls.Common.Range;

namespace MT.Controls.Chart
{
    internal abstract class PieCompoment : Shape
    {
        protected override Geometry DefiningGeometry => GeometryGenerator();


        /// <summary>
        /// 半径范围(0-1)
        /// </summary>
        public Range RadiusRange
        {
            get { return (Range)GetValue(RadiusRangeProperty); }
            set { SetValue(RadiusRangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadiusRange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusRangeProperty =
            DependencyProperty.Register(nameof(RadiusRange), typeof(Range), typeof(PieCompoment), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 角度范围
        /// </summary>
        public Range AngleRange
        {
            get { return (Range)GetValue(AngleRangeProperty); }
            set { SetValue(AngleRangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AngelRange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleRangeProperty =
            DependencyProperty.Register(nameof(AngleRange), typeof(Range), typeof(PieCompoment), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        protected abstract Geometry GeometryGenerator();

    }
}
