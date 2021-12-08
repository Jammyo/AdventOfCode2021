using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle14
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
            return Enumerable.Range(positions.Min(), positions.Max())
                .Select(position => CalculateFuelNeededToLineUp(position, positions))
                .Min();
        }

        private static int CalculateFuelNeededToLineUp(int goalPosition, IReadOnlyList<int> positions)
        {
            return positions.Sum(position =>
            {
                var distance = 0;
                for (var i = 1; i <= Math.Abs(position - goalPosition); ++i)
                {
                    distance += i;
                }
                return distance;
            });
        }
    }
}