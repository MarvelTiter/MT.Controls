using System;
using System.Windows;
using System.Windows.Controls;
using Range = MT.Controls.Common.Range;

namespace MT.Controls.Chart
{
    internal abstract class AxisTick : UserControl
    {
        protected Canvas root;
        public AxisTick()
        {
            root = new Canvas();
            Content = root;
            SizeChanged += (s, e) =>
            {
                (s as AxisTick)?.Render();
            };
        }

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(AxisTick), new PropertyMetadata(10d));

        public Range Range
        {
            get { return (Range)GetValue(RangeProperty); }
            set { SetValue(RangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Range.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeProperty =
            DependencyProperty.Register("Range", typeof(Range), typeof(AxisTick), new PropertyMetadata(new Range(0d, 100d)));

        protected abstract void Render();

        protected abstract double Normalize(double v);

        protected virtual bool CanNotRender() => RenderSize.Width <= 0 || RenderSize.Height <= 0;

    }
}
