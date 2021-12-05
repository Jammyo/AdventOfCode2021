using System.Drawing;

namespace Shared.HydrothermalVents
{
    public class VentLine
    {
        public VentLine(Point from, Point to)
        {
            From = from;
            To = to;
        }

        public Point From { get; }
        public Point To { get; }
    }
}