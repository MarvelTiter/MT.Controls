using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MT.Controls.Chart
{
    internal class GridLineShape : Shape
    {
        public GridLineShape()
        {
            Loaded += (s, e) =>
            {
                var panel = this.Parent as Panel;
                if (panel == null) return;
                Binding binding = new Binding(nameof(ActualWidth))
                {
                    Source = panel
                };
                SetBinding(WidthProperty, binding);

                binding = new Binding(nameof(ActualHeight))
                {
                    Source = panel
                };
                SetBinding(HeightProperty, binding);
            };
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry stream = new StreamGeometry();
                using (var context = stream.Open())
                {
                    Point p1, p2;
                    double width = double.IsNaN(Width) ? RenderSize.Width : Width;
                    double height = double.IsNaN(Height) ? RenderSize.Height : Height;
                    var offsetX = width / HorizontalCount;
                    var offsetY = height / VerticalCount;
                    for (int i = 0; i < HorizontalCount; i++)
                    {
                        p1 = new Point((i + 1) * offsetX, 0);
                        p2 = new Point((i + 1) * offsetX, height);
                        context.BeginFigure(p1, false, false);
                        context.LineTo(p2, true, false);
                    }
                    for (int i = 0; i < VerticalCount; i++)
                    {
                        p1 = new Point(0, (i + 1) * offsetY);
                        p2 = new Point(width, (i + 1) * offsetY);
                        context.BeginFigure(p1, false, false);
                        context.LineTo(p2, true, false);
                    }
                }
                stream.Freeze();
                return stream;
            }
        }


        public int HorizontalCount
        {
            get { return (int)GetValue(HorizontalCountProperty); }
            set { SetValue(HorizontalCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalCountProperty =
            DependencyProperty.Register(nameof(HorizontalCount), typeof(int), typeof(GridLineShape), new FrameworkPropertyMetadata(5));



        public int VerticalCount
        {
            get { return (int)GetValue(VerticalCountProperty); }
            set { SetValue(VerticalCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerticalCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalCountProperty =
            DependencyProperty.Register(nameof(VerticalCount), typeof(int), typeof(GridLineShape), new FrameworkPropertyMetadata(5));


    }
}
