using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MT.Controls.Chart
{
    internal class VerticalTick : AxisTick
    {
        protected override double Normalize(double v)
        {
            return (v - Range.Min) / Range.Distance * RenderSize.Height;
        }

        protected override void Render()
        {
            root.Children.Clear();
            if (CanNotRender()) return;
            var v = Range.Min;
            double y;
            do
            {
                y = Normalize(v);
                root.Children.Add(new Line()
                {
                    X1 = 0,
                    X2 = -10,
                    Y1 = y,
                    Y2 = y,
                    Stroke = Brushes.LimeGreen,
                    StrokeThickness = 2
                });
                v += Step;
            } while (v <= Range.Max);
        }
    }

    internal class HorizontialTick : AxisTick
    {
        protected override double Normalize(double v)
        {
            return (v - Range.Min) / Range.Distance * RenderSize.Width;
        }

        protected override void Render()
        {
            root.Children.Clear();
            if (CanNotRender()) return;
            var v = Range.Min;
            double x;
            do
            {
                x = Normalize(v);
                root.Children.Add(new Line()
                {
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = 10,
                    Stroke = Brushes.LimeGreen,
                    StrokeThickness = 2
                });
                v += Step;
            } while (v <=Range.Max);
        }
    }
}
