using System.Collections.Generic;
using System.Linq;

namespace Shared.Octopus
{
    public class Octopus
    {
        public static int[][] ParseInput(string input)
        {
            return input
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray())
                .ToArray();
        }

        public static int SimulateOctopusGrowth(int[][] octopusTable)
        {
            // Increase all energy values.
            foreach (var octopusRow in octopusTable)
            {
                for (var x = 0; x < octopusRow.Length; x++)
                {
                    ++octopusRow[x];
                }
            }

            // Flash, and apply energy increase to surrounding octopus until we have an iteration without a flash.
            bool flashedDuringIteration;
            var flashedOctopus = new List<(int y, int x)>();
            do
            {
                flashedDuringIteration = false;
                for (var y = 0; y < octopusTable.Length; y++)
                {
                    for (var x = 0; x < octopusTable[y].Length; x++)
                    {
                        //Only flash if we haven't already flashed, and our energy is high enough.
                        if (!flashedOctopus.Contains((y, x)) && octopusTable[y][x] > 9)
                        {
                            flashedOctopus.Add((y, x));
                            flashedDuringIteration = true;
                            IncreaseSurroundingEnergy(octopusTable, y, x);
                        }
                    }
                }
            } while (flashedDuringIteration);

            // Reset any relevant energy values.
            foreach (var octopusRow in octopusTable)
            {
                for (var x = 0; x < octopusRow.Length; x++)
                {
                    if (octopusRow[x] > 9)
                    {
                        octopusRow[x] = 0;
                    }
                }
            }
            
            return flashedOctopus.Count;
        }
        
        private static void IncreaseSurroundingEnergy(int[][] octopusTable, int y, int x)
        {
            var topLeft =     (y: y - 1, x: x - 1);
            var top =         (y: y - 1, x: x);
            var topRight =    (y: y - 1, x: x + 1);
            var left =        (y: y,     x: x - 1);
            var right =       (y: y,     x: x + 1);
            var bottomLeft =  (y: y + 1, x: x - 1);
            var bottom =      (y: y + 1, x: x);
            var bottomRight = (y: y + 1, x: x + 1);

            if (topLeft.x >= 0 && 
                topLeft.y >= 0)
            {
                ++octopusTable[topLeft.y][topLeft.x];
            }

            if (top.y >= 0)
            {
                ++octopusTable[top.y][top.x];
            }
            
            if (topRight.x < octopusTable.First().Length && 
                topRight.y >= 0)
            {
                ++octopusTable[topRight.y][topRight.x];
            }

            if (left.x >= 0)
            {
                ++octopusTable[left.y][left.x];
            }

            if (right.x < octopusTable.First().Length)
            {
                ++octopusTable[right.y][right.x];
            }

            if (bottomLeft.x >= 0 && 
                bottomLeft.y < octopusTable.Length)
            {
                ++octopusTable[bottomLeft.y][bottomLeft.x];
            }

            if (bottom.y < octopusTable.Length)
            {
                ++octopusTable[bottom.y][bottom.x];
            }
            
            if (bottomRight.x < octopusTable.First().Length && 
                bottomRight.y < octopusTable.Length)
            {
                ++octopusTable[bottomRight.y][bottomRight.x];
            }
        }
    }
}