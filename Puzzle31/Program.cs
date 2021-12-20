using System;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Bits;

namespace Puzzle31
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(16);

            var token = Bits.ParseInput(input);

            var versionSums = SumVersions(token);

            Console.WriteLine($"Versions sums: {versionSums}.");
        }

        private static int SumVersions(Token token)
        {
            return token.Version + token.GetChildren().Sum(SumVersions);
        }
    }
}