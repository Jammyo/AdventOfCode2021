using System.Collections.Generic;

namespace Shared.Bits
{
    public abstract class Token
    {
        public Token(int version, int identifier)
        {
            Version = version;
            Identifier = identifier;
        }

        public int Version { get; }
        public int Identifier { get; }

        public abstract IEnumerable<Token> GetChildren();

        public abstract long GetValue();
    }
}