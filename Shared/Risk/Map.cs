using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Risk
{
    public class Map
    {
        public static IReadOnlyDictionary<(int y, int x), int> ParseInput(string input)
        {
            return input
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .SelectMany((s, y) => s
                    .Trim()
                    .ToCharArray()
                    .Select(c => (int)char.GetNumericValue(c))
                    .Select((i, x) => (y, x, i)))
                .ToDictionary(tuple => (y: tuple.y, x: tuple.x), tuple => tuple.i);
        }

        public static int FindLeastRiskyPath(IReadOnlyDictionary<(int y, int x), int> map)
        {
            var partiallyVisitedCells = new HashSet<(int y, int x)>(); // Ones that have been seen but not visited.
            var start = (y: 0, x: 0);
            var end = (y: map.Max(pair => pair.Key.y), x: map.Max(pair => pair.Key.x));
            var width = end.x + 1;
            var height = end.y + 1;
            var current = start;
            var totalRiskMap = new Dictionary<(int y, int x), int>
            {
                [start] = 0
            };
            while (current != end)
            {
                //Update the risk of surrounding cells if needed.
                UpdateTotalRisk(
                    (y: current.y - 1, x: current.x), 
                    position => position.y >= 0, 
                    map, current, totalRiskMap, partiallyVisitedCells);
                UpdateTotalRisk(
                    (y: current.y + 1, x: current.x), 
                    position => position.y < height, 
                    map, current, totalRiskMap, partiallyVisitedCells);
                UpdateTotalRisk(
                    (y: current.y, x: current.x - 1), 
                    position => position.x >= 0, 
                    map, current, totalRiskMap, partiallyVisitedCells);
                UpdateTotalRisk(
                    (y: current.y, x: current.x + 1), 
                    position => position.x < width, 
                    map, current, totalRiskMap, partiallyVisitedCells);

                //Find the new lowest risk cell.
                var lowestRisk = (y: 0, x: 0, value: int.MaxValue);
                foreach (var (y, x) in partiallyVisitedCells)
                {
                    var value = totalRiskMap[(y, x)];
                    if (lowestRisk.value > value)
                    {
                        lowestRisk = (y, x, value);
                    }
                }
                current = (lowestRisk.y, lowestRisk.x);
                partiallyVisitedCells.Remove(current);
            }

            return totalRiskMap[(current.y, current.x)];
        }

        public static void UpdateTotalRisk((int y, int x) cell, Func<(int y, int x), bool> isWithinBoundary, IReadOnlyDictionary<(int y, int x), int> map, (int y, int x) current, Dictionary<(int y, int x), int> totalRiskMap, HashSet<(int y, int x)> partiallyVisitedCells)
        {
            if (!isWithinBoundary(cell))
            {
                return;
            }
            
            var totalCurrentRisk = totalRiskMap[(current.y, current.x)];
            var cellRisk = map[(cell.y, cell.x)];
            var cellRiskFromCurrent = totalCurrentRisk + cellRisk;
                
            if (!totalRiskMap.ContainsKey((cell.y, cell.x)))
            {
                totalRiskMap[(cell.y, cell.x)] = cellRiskFromCurrent;
                partiallyVisitedCells.Add(cell);
            }
            else if (cellRiskFromCurrent < totalRiskMap[(cell.y, cell.x)])
            {
                totalRiskMap[(cell.y, cell.x)] = cellRiskFromCurrent;
            }
        }
    }
}