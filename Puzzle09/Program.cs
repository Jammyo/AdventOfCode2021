using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.HydrothermalVents;

namespace Puzzle09
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(5);

            var ventLines = VentLines.ParseInput(input);

            var pointsWithMultipleOverlaps = CalculatePointsWithMultipleOverlaps(ventLines);

            Console.WriteLine($"Points with multiple overlaps {pointsWithMultipleOverlaps}.");
        }

        private static int CalculatePointsWithMultipleOverlaps(IReadOnlyList<VentLine> ventLines)
        {
            var nonDiagonalVentLines = ventLines.Where(line => line.From.X.Equals(line.To.X) || line.From.Y.Equals(line.To.Y)).ToList();
            var gridPoints = VentLines.BuildGrid(nonDiagonalVentLines);

            return gridPoints.SelectMany(pair => pair.Value)
                .Select(pair => pair.Value)
                .Count(overlaps => overlaps > 1);
        }
    }
}