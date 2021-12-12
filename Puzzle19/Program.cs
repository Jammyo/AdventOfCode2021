using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Syntax;

namespace Puzzle19
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(10);

            var linesOfCode = Syntax.ParseInput(input);

            var illegalCharacters = FindIllegalCharacters(linesOfCode);
            var illegalCharacterScore = ScoreIllegalCharacters(illegalCharacters);
            
            Console.WriteLine($"Illegal character score {illegalCharacterScore}.");
        }

        private static IReadOnlyList<char> FindIllegalCharacters(IReadOnlyList<IReadOnlyList<char>> linesOfCode)
        {
            return linesOfCode
                .Select(Syntax.FindFirstIllegalCharacter)
                .Where(firstIllegalCharacter => firstIllegalCharacter != null)
                .Select(firstIllegalCharacter => firstIllegalCharacter.Value)
                .ToList();
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