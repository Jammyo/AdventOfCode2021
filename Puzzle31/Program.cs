using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Puzzle31
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(16);

            var token = ParseInput(input);

            var versionSums = SumVersions(token);
            
            Console.WriteLine($"Versions sums: {versionSums}.");
        }

        private static Token ParseInput(string input)
        {
            var binary = input
                .Trim()
                .SelectMany(c => Convert
                    .ToString(Convert.ToInt32($"{c}", 16), 2)
                    .PadLeft(4, '0'))
                .GetEnumerator();
            binary.MoveNext();
            var (tokenLength, token) = ParseToken(binary);
            return token;
        }

        private static int ParseInt(IEnumerator<char> input, int count)
        {
            var segment = new StringBuilder();
            
            foreach (var i in Enumerable.Range(0, count))
            {
                segment.Append(input.Current);
                input.MoveNext();
            }
            
            return Convert.ToInt32(segment.ToString(), 2);
        }

        private static (int tokenLength, Token token) ParseToken(IEnumerator<char> input)
        {
            var version = ParseInt(input, 3);
            var identifier = ParseInt(input, 3);
            switch (identifier)
            {
                case 4: // Literal
                {
                    var (tokenLength, literal) = ParseLiteral(input, version, identifier);
                    return (tokenLength, literal);
                }
                default: // Operation
                {
                    var (tokenLength, operation) = ParseOperation(input, version, identifier);
                    return (tokenLength, operation);
                }
            }
        }

        private static (int tokenLength, Literal literal) ParseLiteral(IEnumerator<char> input, int version, int identifier)
        {
            var literal = new StringBuilder();
            var tokenLength = 3 + 3; // Version + identifier.
            while (true)
            {
                var hasMore = ParseInt(input, 1);
                var digit = ParseInt(input, 4);
                literal.Append(digit);
                tokenLength += 5;
                if (hasMore == 0)
                {
                    break;
                }
            }

            return (tokenLength, new Literal(version, identifier, long.Parse(literal.ToString())));
        }

        private static (int tokenLength, Operation operation) ParseOperation(IEnumerator<char> input, int version, int identifier)
        {
            var lengthTypeId = ParseInt(input, 1);
            var tokens = new List<Token>();
            var subPacketBitLength = 0;
            switch (lengthTypeId)
            {
                case 0:
                    var totalBitLength = ParseInt(input, 15);
                    while (subPacketBitLength < totalBitLength)
                    {
                        var (tokenLength, token) = ParseToken(input);
                        subPacketBitLength += tokenLength;
                        tokens.Add(token);
                    }

                    const int subPacketBitLengthHeader = 3 + 3 + 1 + 15; // Version + identifier + length type + sub packet bit length.
                    return (subPacketBitLength + subPacketBitLengthHeader, new Operation(version, identifier, tokens));
                
                case 1:
                    var numberOfPackets = ParseInt(input, 11);
                    foreach (var _ in Enumerable.Range(0, numberOfPackets))
                    {
                        var (tokenLength, token) = ParseToken(input);
                        subPacketBitLength += tokenLength;
                        tokens.Add(token);
                    }

                    const int subPacketCountHeader = 3 + 3 + 1 + 11; // Version + identifier + length type + sub pack count.
                    return (subPacketBitLength + subPacketCountHeader, new Operation(version, identifier, tokens));
            }

            throw new Exception($"Length type id {lengthTypeId} not recognised.");
        }

        private static int SumVersions(Token token)
        {
            return token.Version + token.GetChildren().Sum(SumVersions);
        }
    }

    abstract class Token
    {
        public Token(int version, int identifier)
        {
            Version = version;
            Identifier = identifier;
        }

        public int Version { get; }
        public int Identifier { get; }

        public abstract IEnumerable<Token> GetChildren();
    }

    class Operation : Token
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

    class Literal : Token
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
    }
}