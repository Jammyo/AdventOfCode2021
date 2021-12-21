using System;
using System.Threading.Tasks;
using Shared;
using Shared.Snailfish;

namespace Puzzle35
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(18);

            var snailfishNumbers = Snailfish.ParseInput(input);

            var finalSum = Snailfish.AddSnailfishNumbers(snailfishNumbers);
            var magnitude = Snailfish.CalculateMagnitude(finalSum);

            Console.WriteLine($"Magnitude: {magnitude}.");
        }
    }
}