using System.Collections.Generic;
using System.Linq;

namespace Shared.Bits
{
    internal class SumOperation : Operation
    {
        public SumOperation(int version, int identifier, IReadOnlyList<Token> subTokens) : base(version, identifier, subTokens)
        {
        }

        public override long GetValue()
        {
            return SubTokens.Select(token => token.GetValue()).Aggregate((result, current) => result + current);
        }
    }
}