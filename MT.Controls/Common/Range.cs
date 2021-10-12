using System;
using System.ComponentModel;

namespace MT.Controls.Common
{
    [TypeConverter(typeof(RangeConverter))]
    public class Range
    {
        public double From { get; set; }
        public double To { get; set; }
        public double Max => Math.Max(From, To);
        public double Min => Math.Min(From, To);
        public double Distance => Math.Abs(To - From);
        public Range()
        {

        }

        public Range(double from, double to)
        {
            From = from;
            To = to;
        }
    }
}
