using System.Collections.Generic;
using System.Linq;

namespace Shared.LavaTubes
{
    public class LavaTubes
    {
        public static int[][] ParseInput(string input)
        {
            return input.Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.ToCharArray())
                .Select(chars => chars.Select(c => (int)char.GetNumericValue(c)))
                .Select(ints => ints.ToArray())
                .ToArray();
        }

        public static IReadOnlyList<(int x, int y)> FindLowestPoints(int[][] heightMap)
        {
            var lowestPoints = new List<(int x, int y)>();
            var height = heightMap.Length;
            var width = heightMap.First().Length;
            for (var y = 0; y < heightMap.Length; y++)
            {
                for (var x = 0; x < heightMap[y].Length; x++)
                {
                    var up = (y: y - 1, x: x);
                    var down = (y: y + 1, x: x);
                    var left = (y: y, x: x - 1);
                    var right = (y: y, x: x + 1);
                    if (up.y >= 0 && heightMap[y][x] >= heightMap[up.y][up.x])
                    {
                        continue;
                    }
                    if (down.y < height && heightMap[y][x] >= heightMap[down.y][down.x])
                    {
                        continue;
                    }
                    if (left.x >= 0 && heightMap[y][x] >= heightMap[left.y][left.x])
                    {
                        continue;
                    }
                    if (right.x < width && heightMap[y][x] >= heightMap[right.y][right.x])
                    {
                        continue;
                    }

                    lowestPoints.Add((x: x, y: y));
                }
            }

            return lowestPoints;
        }
    }
}