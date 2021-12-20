using System.Collections.Generic;

namespace Shared.Bits
{
    internal class Literal : Token
    {
        public long Value { get; }
        
        public Literal(int version, int identifier, long value) : base(version, identifier)
        {
            Value = value;
        }

        public override IEnumerable<Token> GetChildren()
        {
            return new List<Token>();
        }

        public override long GetValue()
        {
            return Value;
        }
    }
}