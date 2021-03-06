using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Shared;
using Shared.Probe;

namespace Puzzle33
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(17);

            var targetArea = Probe.ParseInput(input);

            var highestYPosition = Probe.CalculateHighestYPosition(targetArea);

            Console.WriteLine($"Highest y position {highestYPosition}.");
        }
    }
}