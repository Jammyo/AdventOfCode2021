using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Syntax;

namespace Puzzle20
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(10);

            var linesOfCode = Syntax.ParseInput(input);

            var score = FindMiddleScoringIncompleteLine(linesOfCode);
            
            Console.WriteLine($"Middle scoring autocomplete statement {score}.");
        }

        private static long FindMiddleScoringIncompleteLine(IReadOnlyList<IReadOnlyList<char>> linesOfCode)
        {
            var incompleteScores = StripIllegalLines(linesOfCode)
                .Select(Syntax.BuildClosingSyntax)
                .Select(Score)
                .OrderBy(i => i)
                .ToList();

            return incompleteScores[incompleteScores.Count / 2];
        }

        private static long Score(IReadOnlyList<char> line)
        {
            var points = new Dictionary<char, long>
            {
                {')', 1},
                {']', 2},
                {'}', 3},
                {'>', 4}
            };
            
            var score = 0L;
            
            foreach (var c in line)
            {
                score *= 5;
                score += points[c];
            }

            return score;
        }

        private static IReadOnlyList<IReadOnlyList<char>> StripIllegalLines(IReadOnlyList<IReadOnlyList<char>> linesOfCode)
        {
            return linesOfCode
                .Where(line => Syntax.FindFirstIllegalCharacter(line) == null)
                .ToList();
        }
    }
}