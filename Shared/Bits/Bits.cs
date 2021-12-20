using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Bits
{
    public class Bits
    {
        public static Token ParseInput(string input)
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
            return Convert.ToInt32(RetrieveCount(input, count), 2);
        }

        private static string RetrieveCount(IEnumerator<char> input, int count)
        {
            var segment = new StringBuilder();
            
            foreach (var _ in Enumerable.Range(0, count))
            {
                segment.Append(input.Current);
                input.MoveNext();
            }
            
            return segment.ToString();
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
                var segment = RetrieveCount(input, 4);
                literal.Append(segment);
                tokenLength += 5;
                if (hasMore == 0)
                {
                    break;
                }
            }

            return (tokenLength, new Literal(version, identifier, Convert.ToInt64(literal.ToString(), 2)));
        }

        private static (int tokenLength, Operation operation) ParseOperation(IEnumerator<char> input, int version, int identifier)
        {
            var lengthTypeId = ParseInt(input, 1);
            var tokens = new List<Token>();
            var headerLength = 0;
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
                    headerLength = subPacketBitLengthHeader;
                    break;
                
                case 1:
                    var numberOfPackets = ParseInt(input, 11);
                    foreach (var _ in Enumerable.Range(0, numberOfPackets))
                    {
                        var (tokenLength, token) = ParseToken(input);
                        subPacketBitLength += tokenLength;
                        tokens.Add(token);
                    }

                    const int subPacketCountHeader = 3 + 3 + 1 + 11; // Version + identifier + length type + sub pack count.
                    headerLength = subPacketCountHeader;
                    break;
                
                default:
                    throw new Exception($"Length type id {lengthTypeId} not recognised.");
            }

            Operation operation;
            switch (identifier)
            {
                case 0: // Sum
                    operation = new SumOperation(version, identifier, tokens);
                    break;
                case 1: // Product
                    operation = new ProductOperation(version, identifier, tokens);
                    break;
                case 2: // Minimum
                    operation = new MinimumOperation(version, identifier, tokens);
                    break;
                case 3: // Maximum
                    operation = new MaximumOperation(version, identifier, tokens);
                    break;
                case 5: // Greater than
                    operation = new GreaterThanOperation(version, identifier, tokens);
                    break;
                case 6: // Less than
                    operation = new LessThanOperation(version, identifier, tokens);
                    break;
                case 7: // Equal to
                    operation = new EqualToOperation(version, identifier, tokens);
                    break;
                default:
                    throw new Exception($"Unknown identifier: {identifier}.");
            }
            return (subPacketBitLength + headerLength, operation);
        }
    }
}