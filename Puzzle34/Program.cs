using System;
using System.Threading.Tasks;
using Shared;
using Shared.Probe;

namespace Puzzle34
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(17);

            var targetArea = Probe.ParseInput(input);

            var validInitialVelocities = Probe.CountValidInitialVelocities(targetArea);

            Console.WriteLine($"Valid initial velocities {validInitialVelocities}.");
        }
    }
}