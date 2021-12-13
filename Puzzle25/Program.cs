using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Instructions;

namespace Puzzle25
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(13);

            var manual = Manual.ParseInput(input);

            var page = Manual.FoldPage(manual.page, manual.instructions.First());

            Console.WriteLine($"Total dots: {page.Count}.");
        }
    }
}