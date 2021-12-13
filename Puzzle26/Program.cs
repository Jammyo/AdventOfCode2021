using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Instructions;

namespace Puzzle26
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(13);

            var manual = Manual.ParseInput(input);

            var page = manual.instructions.Aggregate(manual.page, Manual.FoldPage);

            PrintPage(page);
        }
    
        private static void PrintPage(IReadOnlyList<(int x, int y)> page)
        {
            Console.WriteLine("Output: ");
            var width = page.Max(tuple => tuple.x);
            var height = page.Max(tuple => tuple.y);
            foreach (var y in Enumerable.Range(0, height + 1))
            {
                foreach (var x in Enumerable.Range(0, width + 1))
                {
                    if (page.Contains((x, y)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}