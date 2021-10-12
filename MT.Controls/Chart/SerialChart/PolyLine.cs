using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MT.Controls.Chart
{
    internal class PolyLine : Shape
    {
        protected override Geometry DefiningGeometry => GetGeometry();

        private Geometry GetGeometry()
        {
            StreamGeometry stream = new StreamGeometry();
            double width = double.IsNaN(Width) ? RenderSize.Width : Width;
            double height = double.IsNaN(Height) ? RenderSize.Height : Height;
            var kx = width / 100;
            var ky = height / 100;
            using (var ctx = stream.Open())
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(0 * kx, 50 * ky));
                points.Add(new Point(17 * kx, 30 * ky));
                points.Add(new Point(22 * kx, 70 * ky));
                points.Add(new Point(55 * kx, 10 * ky));
                points.Add(new Point(88 * kx, 20 * ky));
                points.Add(new Point(96 * kx, 90 * ky));
                ctx.BeginFigure(points[0], false, false);
                ctx.PolyLineTo(points, true, true);
            }
            return stream;
        }
    }
}
