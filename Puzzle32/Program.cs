using System;
using System.Threading.Tasks;
using Shared;
using Shared.Bits;

namespace Puzzle32
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(16);

            var token = Bits.ParseInput(input);

            var result = token.GetValue();

            Console.WriteLine($"Result: {result}.");
        }
    }
}