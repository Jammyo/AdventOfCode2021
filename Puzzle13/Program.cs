using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle13
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(7);

            var positions = ParseInput(input);

            var fuelNeededToLineUp = CalculateFuelNeededToLineUp(positions);
            
            Console.WriteLine($"Fuel needed to line up: {fuelNeededToLineUp}.");
        }

        private static IReadOnlyList<int> ParseInput(string input)
        {
            return input.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .OrderBy(i => i)
                .ToList();
        }

        private static int CalculateFuelNeededToLineUp(IReadOnlyList<int> positions)
        {
            return positions
                .Prepend(positions.Max() - positions.Min())
                .Select(position => CalculateFuelNeededToLineUp(position, positions))
                .Min();
        }

        private static int CalculateFuelNeededToLineUp(int position, IReadOnlyList<int> positions)
        {
            return positions.Sum(i => Math.Abs(i - position));
        }
    }
}