using System.Collections.Generic;
using System.Linq;

namespace Shared.Cave
{
    public class Cave
    {
        public static Dictionary<Node, List<Node>> ParseInput(string input)
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
    }
}