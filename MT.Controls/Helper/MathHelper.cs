using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MT.Controls.Helper
{
    public static class MathHelper
    {
        private const double D2R = Math.PI / 180.0;

        /// <summary>
        ///     极坐标系转笛卡尔坐标系
        /// </summary>
        /// <param name="deg"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Point PolarToCartesian(double deg, double radius)
        {
            var d = deg * D2R;
            return new Point(Math.Cos(d) * radius, Math.Sin(d) * radius);
        }

        /// <summary>
        /// 按比例转换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double Normalize(double x, double x1, double x2, double y1, double y2)
        {
            return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
        }
        static Random random = new Random();
        public static double Random(int size)
        {
            return random.NextDouble() * size;
        }

        private const double DOUBLE_DELTA = 1E-06;
        public static bool AreEqual(double value1, double value2)
        {
            return (value1 == value2)
                || Math.Abs(value1 - value2) < DOUBLE_DELTA;
        }

        /// <summary>
        /// double比较
        /// </summary>
        /// <param name="precision">精度</param>
        /// <returns></returns>
        public static bool GreaterThan(this double value1, double value2, int precision)
        {
            return ((value1 > value2) && !AreEqual(value1, value2));
        }
    }
}
