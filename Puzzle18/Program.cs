using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.LavaTubes;

namespace Puzzle18
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(9);

            var heightMap = LavaTubes.ParseInput(input);

            var threeLargestBasins = FindThreeLargestBasins(heightMap);
            
            Console.WriteLine($"Multiplied basin areas: {threeLargestBasins[0] * threeLargestBasins[1] * threeLargestBasins[2]}.");
        }

        private static IReadOnlyList<int> FindThreeLargestBasins(int[][] heightMap)
        {
            var lowestPoints = LavaTubes.FindLowestPoints(heightMap);
            var basinSizes = new List<int>();
            
            foreach (var (x, y) in lowestPoints)
            {
                basinSizes.Add(FindBasinSize(x, y, heightMap));
            }

            var largest = basinSizes.Max();
            var secondLargest = basinSizes.Except(new List<int> { largest }).Max();
            var thirdLargest = basinSizes.Except(new List<int> { largest, secondLargest }).Max();
            return new List<int> { largest, secondLargest, thirdLargest };
        }

        private static int FindBasinSize(int x, int y, int[][] heightMap)
        {
            var unexploredAreas = new Queue<(int y, int x)>();
            unexploredAreas.Enqueue((y, x));
            var basinAreas = new List<(int y, int x)>();

            var width = heightMap.Length;
            var height = heightMap.First().Length;
            while (unexploredAreas.Any())
            {
                var unexploredArea = unexploredAreas.Dequeue();
                basinAreas.Add(unexploredArea);
                
                var up = (y: unexploredArea.y - 1, x: unexploredArea.x);
                if (up.y >= 0 && heightMap[up.y][up.x] < 9 && !basinAreas.Contains(up) && !unexploredAreas.Contains(up))
                {
                    unexploredAreas.Enqueue((up.y, up.x));
                }
                var down = (y: unexploredArea.y + 1, x: unexploredArea.x);
                if (down.y < height && heightMap[down.y][down.x] < 9 && !basinAreas.Contains(down) && !unexploredAreas.Contains(down))
                {
                    unexploredAreas.Enqueue((down.y, down.x));
                }
                var left = (y: unexploredArea.y, x: unexploredArea.x - 1);
                if (left.x >= 0 && heightMap[left.y][left.x] < 9 && !basinAreas.Contains(left) && !unexploredAreas.Contains(left))
                {
                    unexploredAreas.Enqueue((left.y, left.x));
                }
                var right = (y: unexploredArea.y, x: unexploredArea.x + 1);
                if (right.x < width && heightMap[right.y][right.x] < 9 && !basinAreas.Contains(right) && !unexploredAreas.Contains(right))
                {
                    unexploredAreas.Enqueue((right.y, right.x));
                }
            }

            return basinAreas.Count;
        }
    }
}