using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Cave;

namespace Puzzle23
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
            CountDistinctPaths(map, Node.StartNode, new List<Node>(), distinctPaths);
            return distinctPaths;
        }
        
        private static void CountDistinctPaths(IReadOnlyDictionary<Node, List<Node>> map, Node currentNode, IReadOnlyList<Node> visitedNodes, List<string> distinctPaths)
        {
            foreach (var connection in map[currentNode].Where(node => !node.IsStart))
            {
                if (connection.IsEnd)
                {
                    distinctPaths.Add(string.Join(", ", visitedNodes.Select(node => node.Name)));
                }
                else if (connection.IsLarge || !visitedNodes.Contains(connection))
                {
                    CountDistinctPaths(map, connection, visitedNodes.Append(connection).ToList(), distinctPaths);
                }
                else
                {
                    // This connection is small and has already been visited, give up on this path.
                }
            }
        }
    }
}