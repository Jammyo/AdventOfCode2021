using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle9
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(5);

            var ventLines = ParseInput(input);

            var pointsWithMultipleOverlaps = CalculatePointsWithMultipleOverlaps(ventLines);

            Console.WriteLine($"Points with multiple overlaps {pointsWithMultipleOverlaps}.");
        }

        private static IReadOnlyList<VentLine> ParseInput(string input)
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

        private static int CalculatePointsWithMultipleOverlaps(IReadOnlyList<VentLine> ventLines)
        {
            //Build grid.
            var gridPoints = new Dictionary<int, Dictionary<int, int>>();
            foreach (var ventLine in ventLines.Where(line => line.From.X.Equals(line.To.X) || line.From.Y.Equals(line.To.Y)))
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

            return gridPoints.SelectMany(pair => pair.Value)
                .Select(pair => pair.Value)
                .Count(overlaps => overlaps > 1);
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

    class VentLine
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