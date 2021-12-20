using System.Collections.Generic;
using System.Linq;

namespace Shared.Bits
{
    internal class GreaterThanOperation : Operation
    {
        public GreaterThanOperation(int version, int identifier, IReadOnlyList<Token> subTokens) : base(version, identifier, subTokens)
        {
        }

        public override long GetValue()
        {
            var first = SubTokens[0].GetValue();
            var last = SubTokens[^1].GetValue();
            return first > last ? 1 : 0;
        }
    }
}