using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Cave;

namespace Puzzle24
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(12);

            var map = Cave.ParseInput(input);

            var distinctPaths = GatherDistinctPaths(map);
            
            Console.WriteLine($"Distinct paths: {distinctPaths.Count}.");
        }

        private static IReadOnlyList<string> GatherDistinctPaths(IReadOnlyDictionary<Node, List<Node>> map)
        {
            var distinctPaths = new List<string>();
            CountDistinctPaths(map, Node.StartNode, new List<Node>(), distinctPaths, false);
            return distinctPaths;
        }
        
        private static void CountDistinctPaths(IReadOnlyDictionary<Node, List<Node>> map, Node currentNode, IReadOnlyList<Node> visitedNodes, List<string> distinctPaths, bool haveDoubleVisitedASmallCave)
        {
            foreach (var connection in map[currentNode].Where(node => !node.IsStart))
            {
                if (connection.IsEnd)
                {
                    // Log a complete path.
                    distinctPaths.Add(string.Join(", ", visitedNodes.Select(node => node.Name)));
                }
                else if (connection.IsLarge)
                {
                    //We can revisit as much as needed.
                    CountDistinctPaths(map, connection, visitedNodes.Append(connection).ToList(), distinctPaths, haveDoubleVisitedASmallCave);
                }
                else if (connection.IsSmall)
                {
                    if (!visitedNodes.Contains(connection))
                    {
                        CountDistinctPaths(map, connection, visitedNodes.Append(connection).ToList(), distinctPaths, haveDoubleVisitedASmallCave);
                    }
                    else if (!haveDoubleVisitedASmallCave)
                    {
                        CountDistinctPaths(map, connection, visitedNodes.Append(connection).ToList(), distinctPaths, true);
                    }
                    else
                    {
                        // The connection is small, but we've already visited it, and have an active double visit in progress.
                    }
                }
            }
        }
    }
}