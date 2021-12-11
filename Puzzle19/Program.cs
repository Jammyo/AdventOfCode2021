using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle19
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(10);

            var linesOfCode = ParseInput(input);

            var illegalCharacters = FindIllegalCharacters(linesOfCode);
            var illegalCharacterScore = ScoreIllegalCharacters(illegalCharacters);
            
            Console.WriteLine($"Illegal character score {illegalCharacterScore}.");
        }

        private static IReadOnlyList<IReadOnlyList<char>> ParseInput(string input)
        {
            return input
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.ToCharArray())
                .ToList();
        }

        private static IReadOnlyList<char> FindIllegalCharacters(IReadOnlyList<IReadOnlyList<char>> linesOfCode)
        {
            var illegalCharacters = new List<char>();
            
            foreach (var line in linesOfCode)
            {
                var firstIllegalCharacter = FindFirstIllegalCharacter(line);
                if (firstIllegalCharacter != null)
                {
                    illegalCharacters.Add(firstIllegalCharacter.Value);
                }
            }

            return illegalCharacters;
        }

        private static char? FindFirstIllegalCharacter(IReadOnlyList<char> line)
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
                            return c;
                        }
                        break;
                    }
                    case '}':
                    {
                        if (!stack.TryPop(out var previousCharacter) || !previousCharacter.Equals('{'))
                        {
                            return c;
                        }
                        break;
                    }
                    case ']':
                    {
                        if (!stack.TryPop(out var previousCharacter) || !previousCharacter.Equals('['))
                        {
                            return c;
                        }
                        break;
                    }
                    case '>':
                    {
                        if (!stack.TryPop(out var previousCharacter) || !previousCharacter.Equals('<'))
                        {
                            return c;
                        }
                        break;
                    }
                }
            }

            return null;
        }

        private static int ScoreIllegalCharacters(IReadOnlyList<char> illegalCharacters)
        {
            var points = new Dictionary<char, int>
            {
                {')', 3},
                {']', 57},
                {'}', 1197},
                {'>', 25137}
            };
            
            return illegalCharacters.Sum(c => points[c]);
        }
    }
}