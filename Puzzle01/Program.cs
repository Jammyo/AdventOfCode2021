using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle01
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(1);
            var inputs = AdventOfCode.ParseInput<int>(input);
            
            var increases = CalculateIncreases(inputs);
            
            Console.WriteLine($"Increases: {increases}");
        }

        static int CalculateIncreases(IReadOnlyList<int> inputs)
        {
            // Count all cases where the value increases from the previous.
            var result = inputs
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