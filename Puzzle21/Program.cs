using System;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Octopus;

namespace Puzzle21
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(11);

            var octopusTable = Octopus.ParseInput(input);

            var numberOfFlashes = SimulateOctopusFlashes(octopusTable, 100);
            
            Console.WriteLine($"Total flashes: {numberOfFlashes}.");
        }

        private static int SimulateOctopusFlashes(int[][] octopusTable, int iterations)
        {
            return Enumerable
                .Range(0, iterations)
                .Sum(_ => Octopus.SimulateOctopusGrowth(octopusTable));
        }
    }
}