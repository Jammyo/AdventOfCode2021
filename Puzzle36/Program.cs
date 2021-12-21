using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Snailfish;

namespace Puzzle36
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(18);

            var snailfishNumbers = Snailfish.ParseInput(input);
            
            var magnitude = LargestMagnitude(snailfishNumbers);
            
            Console.WriteLine($"Magnitude: {magnitude}.");
        }

        private static int LargestMagnitude(IReadOnlyList<IReadOnlyList<(int? value, int depth)>> snailfishNumbers)
        {
            return snailfishNumbers
                .SelectMany(_ => snailfishNumbers,
                    (first, second) => new List<IReadOnlyList<(int? value, int depth)>> { first, second })
                .Where(list => !list.First().Equals(list.Last()))
                .Select(Snailfish.AddSnailfishNumbers)
                .Select(Snailfish.CalculateMagnitude)
                .Prepend(0)
                .Max();
        }
    }
}