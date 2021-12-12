using System;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Octopus;

namespace Puzzle22
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(11);

            var octopusTable = Octopus.ParseInput(input);

            var iterations = IterateUntilOctopusAreSynchronised(octopusTable);
            
            Console.WriteLine($"Total flashes: {iterations}.");
        }

        private static int IterateUntilOctopusAreSynchronised(int[][] octopusTable)
        {
            var totalOctopus = octopusTable.Sum(ints => ints.Length);
            var flashingOctopus = 0;
            var iterations = 0;
            
            do
            {
                ++iterations;
                flashingOctopus = Octopus.SimulateOctopusGrowth(octopusTable);
            } while (flashingOctopus < totalOctopus);

            return iterations;
        }
    }
}