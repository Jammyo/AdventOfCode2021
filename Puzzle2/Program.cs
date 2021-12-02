using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle2
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(1);
            var inputs = AdventOfCode.ParseInput<int>(input);
            
            // Transform to a new list to gain access to GetRange().
            var increases = CalculateIncreases(inputs.ToList());
            
            Console.WriteLine($"Increases: {increases}");
        }

        const int Groupings = 3;
        
        static int CalculateIncreases(List<int> inputs)
        {
            // Apply the window and get the sum of each group.
            var windowedInputs = new List<int>();
            for (var i = 0; i + Groupings <= inputs.Count; ++i)
            {
                windowedInputs.Add(inputs.GetRange(i, Groupings).Sum());
            }
            
            // Count each occurence where the depth increases within the window.
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