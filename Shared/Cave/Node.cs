using System.Linq;

namespace Shared.Cave
{
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