using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Shared.HydrothermalVents
{
    public class VentLines
    {
        public static IReadOnlyList<VentLine> ParseInput(string input)
        {
            return input.Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(ventLine =>
                {
                    var vents = ventLine.Split(" -> ");
                    var ventStart = vents[0].Split(',').Select(int.Parse).ToList();
                    var ventEnd = vents[1].Split(',').Select(int.Parse).ToList();
                    return new VentLine(new Point(ventStart[0], ventStart[1]), new Point(ventEnd[0], ventEnd[1]));
                })
                .ToList();
        }

        public static Dictionary<int, Dictionary<int, int>> BuildGrid(IReadOnlyList<VentLine> ventLines)
        {
            //Build grid.
            var gridPoints = new Dictionary<int, Dictionary<int, int>>();
            foreach (var ventLine in ventLines)
            {
                foreach (var linePoint in CalculateLinePoints(ventLine))
                {
                    if (!gridPoints.ContainsKey(linePoint.X))
                    {
                        gridPoints[linePoint.X] = new Dictionary<int, int>();
                    }

                    if (!gridPoints[linePoint.X].ContainsKey(linePoint.Y))
                    {
                        gridPoints[linePoint.X][linePoint.Y] = 0;
                    }

                    gridPoints[linePoint.X][linePoint.Y] += 1;
                }
            }

            return gridPoints;
        }
        private static IEnumerable<Point> CalculateLinePoints(VentLine ventLine)
        {
            var currentPoint = ventLine.From;
            while (!currentPoint.Equals(ventLine.To))
            {
                yield return currentPoint;
                if (currentPoint.X < ventLine.To.X)
                {
                    currentPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                }
                else if (currentPoint.X > ventLine.To.X)
                {
                    currentPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                }

                if (currentPoint.Y < ventLine.To.Y)
                {
                    currentPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                }
                else if (currentPoint.Y > ventLine.To.Y)
                {
                    currentPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                }
            }
            yield return ventLine.To;
        }
    }
}