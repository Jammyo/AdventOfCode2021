using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Risk;

namespace Puzzle30
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(15);

            var map = Map.ParseInput(input);
            map = ExtendInputTile(map);

            var risk = Map.FindLeastRiskyPath(map);
            
            Console.WriteLine($"Path risk: {risk}");
        }

        private static IReadOnlyDictionary<(int y, int x), int> ExtendInputTile(IReadOnlyDictionary<(int y, int x), int> singleTile)
        {
            var width = singleTile.Max(pair => pair.Key.x) + 1;
            var height = singleTile.Max(pair => pair.Key.y) + 1;
            var result = new Dictionary<(int y, int x), int>();
            foreach (var i in Enumerable.Range(0, 5))
            {
                foreach (var j in Enumerable.Range(0, 5))
                {
                    foreach (var ((y, x), value) in singleTile)
                    {
                        var adjustedValue = value + i + j;
                        if (adjustedValue > 9)
                        {
                            adjustedValue -= 9;
                        }
                        result[(y + height * i, x + width * j)] = adjustedValue;
                    }
                }
            }

            return result;
        }
    }
}