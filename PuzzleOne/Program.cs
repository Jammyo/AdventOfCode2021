using System;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace PuzzleOne
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

        static int CalculateIncreases(string input)
        {
            //Clean and convert the input, then count all cases where the value increases from the previous.
            var result = input.Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
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