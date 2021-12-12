using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Syntax
{
    public class Syntax
    {
        public static IReadOnlyList<IReadOnlyList<char>> ParseInput(string input)
        {
            return input
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.ToCharArray())
                .ToList();
        }

        private static (Stack<char> syntaxStack, char? illegalCharacter) ParseLine(IReadOnlyList<char> line)
        {
            var stack = new Stack<char>();
            foreach (var c in line)
            {
                switch (c)
                {
                    case '(':
                        stack.Push('(');
                        break;
                    case '{':
                        stack.Push('{');
                        break;
                    case '[':
                        stack.Push('[');
                        break;
                    case '<':
                        stack.Push('<');
                        break;
                    case ')':
                    {
                        if(!stack.TryPop(out var previousCharacter) || !previousCharacter.Equals('('))
                        {
                            return (stack, c);
                        }
                        break;
                    }
                    case '}':
                    {
                        if (!stack.TryPop(out var previousCharacter) || !previousCharacter.Equals('{'))
                        {
                            return (stack, c);
                        }
                        break;
                    }
                    case ']':
                    {
                        if (!stack.TryPop(out var previousCharacter) || !previousCharacter.Equals('['))
                        {
                            return (stack, c);
                        }
                        break;
                    }
                    case '>':
                    {
                        if (!stack.TryPop(out var previousCharacter) || !previousCharacter.Equals('<'))
                        {
                            return (stack, c);
                        }
                        break;
                    }
                }
            }

            return (stack, null);
        }
        
        public static char? FindFirstIllegalCharacter(IReadOnlyList<char> line)
        {
            var (syntaxStack, illegalCharacter) = ParseLine(line);
            return illegalCharacter;
        }

        public static IReadOnlyList<char> BuildClosingSyntax(IReadOnlyList<char> line)
        {
            var (syntaxStack, illegalCharacter) = ParseLine(line);
            return syntaxStack
                .Select(c =>
                {
                    switch (c)
                    {
                        case '(':
                            return ')';
                        case '{':
                            return '}';
                        case '[':
                            return ']';
                        case '<':
                            return '>';
                    }
                    throw new Exception("Invalid character found.");
                })
                .ToList();
        }
    }
}