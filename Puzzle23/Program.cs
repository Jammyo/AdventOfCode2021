using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle23
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(12);

            var map = ParseInput(input);

            var distinctPaths = GatherDistinctPaths(map);
            
            Console.WriteLine($"Distinct paths: {distinctPaths.Count}.");
        }

        private static Dictionary<Node, List<Node>> ParseInput(string input)
        {
            // Get the raw input data.
            var connections = input
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split('-'))
                .ToList();

            // Setup our known variables and output/result.
            var startNode = Node.StartNode;
            var nodeConnections = new Dictionary<Node, List<Node>>();
            var newNodes = new Queue<Node>();
            newNodes.Enqueue(startNode);
            // Until we don't see any new nodes, explore connections beginning at the start node.
            while (newNodes.Any())
            {
                var newNode = newNodes.Dequeue();
                // Select connections by filtering only ones with this new node name, then selecting the other node name.
                var newConnections = connections
                    .Where(strings => strings[0].Equals(newNode.Name) || strings[1].Equals(newNode.Name))
                    .Select(strings => strings[0].Equals(newNode.Name) ? strings[1] : strings[0])
                    .Select(s => new Node(s))
                    .ToList();
                nodeConnections[newNode] = newConnections;
                // Nodes are new if they are not yet in the dictionary, so add them.
                foreach (var node in newConnections.Where(node => !nodeConnections.ContainsKey(node)))
                {
                    newNodes.Enqueue(node);
                }
            }

            return nodeConnections;
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

    public class Node
    {
        private const string Start = "start";
        private const string End = "end";

        public static Node StartNode => new Node(Start);
        public static Node EndNode => new Node(End);
        
        public Node(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public bool IsStart => Name.Equals(Start);
        public bool IsEnd => Name.Equals(End);
        public bool IsLarge => char.IsUpper(Name.First());
        public bool IsSmall => char.IsLower(Name.First());

        protected bool Equals(Node other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}