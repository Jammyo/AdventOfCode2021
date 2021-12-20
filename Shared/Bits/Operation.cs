using System.Collections.Generic;

namespace Shared.Bits
{
    internal abstract class Operation : Token
    {
        public IReadOnlyList<Token> SubTokens { get; }
        
        public Operation(int version, int identifier, IReadOnlyList<Token> subTokens) : base(version, identifier)
        {
            SubTokens = subTokens;
        }

        public override IEnumerable<Token> GetChildren()
        {
            return SubTokens;
        }
    }
}