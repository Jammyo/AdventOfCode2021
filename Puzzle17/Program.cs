using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.LavaTubes;

namespace Puzzle17
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(9);

            var heightMap = LavaTubes.ParseInput(input);

            var totalRiskLevel = CalculateTotalRiskLevel(heightMap);
            
            Console.WriteLine($"Sum of risk levels: {totalRiskLevel}.");
        }

        private static int CalculateTotalRiskLevel(int[][] heightMap)
        {
            var lowestPoints = LavaTubes.FindLowestPoints(heightMap);

            return lowestPoints.Sum(tuple => heightMap[tuple.y][tuple.x] + 1);
        }
    }
}