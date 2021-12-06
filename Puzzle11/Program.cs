using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle11
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(6);

            var initialFish = ParseInput(input);

            var finalFish = SimulateFishReproductionOverDays(initialFish, 80);
            
            Console.WriteLine($"Lantern fish: {finalFish.Count}.");
        }

        private static IReadOnlyList<int> ParseInput(string input)
        {
            return input.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .ToList();
        }

        private static IReadOnlyList<int> SimulateFishReproductionOverDays(IReadOnlyList<int> initialFish, int days)
        {
            var fish = initialFish.ToList();
            for (var day = 0; day < days; day++)
            {
                var fishAtTheBeginningOfTheDay = fish.Count;
                for (var i = 0; i < fishAtTheBeginningOfTheDay; i++)
                {
                    --fish[i];
                    if (fish[i] < 0)
                    {
                        fish[i] = 6;
                        fish.Add(8);
                    }
                }
            }
            return fish;
        }
    }
}