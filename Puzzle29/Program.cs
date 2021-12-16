using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Risk;

namespace Puzzle29
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(15);

            var map = Map.ParseInput(input);

            var risk = Map.FindLeastRiskyPath(map);
            
            Console.WriteLine($"Path risk: {risk}");
        }
    }
}