using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace PuzzleTwo
{
    class Program
    {
        static async Task Main()
        {
            var cookie = AdventOfCode.GetCookie();
            var input = await AdventOfCode.GetInput(cookie, 1);
            var increases = CalculateIncreases(input);
            
            Console.WriteLine($"Increases: {increases}");
        }

        const int Groupings = 3;
        
        static int CalculateIncreases(string input)
        {
            //Clean and convert input.
            var inputs = input.Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .ToList();

            //Apply the window and get the sum of each group.
            var windowedInputs = new List<int>();
            for (var i = 0; i + Groupings <= inputs.Count; ++i)
            {
                windowedInputs.Add(inputs.GetRange(i, Groupings).Sum());
            }
            
            //Count each occurence where the depth increases within the window.
            var result = windowedInputs
                .Aggregate((count: 0, previous: int.MaxValue), (tuple, current) =>
                {
                    var (count, previous) = tuple;
                    if (previous < current)
                    {
                        ++count;
                    }
                    return (count, current);
                });
            return result.count;
        }
    }
}