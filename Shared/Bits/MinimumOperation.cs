using System.Collections.Generic;
using System.Linq;

namespace Shared.Bits
{
    internal class MinimumOperation : Operation
    {
        public MinimumOperation(int version, int identifier, IReadOnlyList<Token> subTokens) : base(version, identifier, subTokens)
        {
        }

        public override long GetValue()
        {
            return SubTokens.Select(token => token.GetValue()).Min();
        }
    }
}