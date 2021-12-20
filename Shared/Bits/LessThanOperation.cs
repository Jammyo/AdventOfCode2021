using System.Collections.Generic;

namespace Shared.Bits
{
    internal class LessThanOperation : Operation
    {
        public LessThanOperation(int version, int identifier, IReadOnlyList<Token> subTokens) : base(version, identifier, subTokens)
        {
        }

        public override long GetValue()
        {
            var first = SubTokens[0].GetValue();
            var last = SubTokens[^1].GetValue();
            return first < last ? 1 : 0;
        }
    }
}