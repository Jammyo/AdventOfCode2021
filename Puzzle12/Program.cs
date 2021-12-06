using System;
using System.Threading.Tasks;
using Shared;
using Shared.LanternFish;

namespace Puzzle12
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(6);

            var initialFish = LanternFish.ParseInput(input);

            var finalFish = LanternFish.CountFishAfterDays(initialFish, 256);
            
            Console.WriteLine($"Lantern fish: {finalFish}.");
        }
    }
}