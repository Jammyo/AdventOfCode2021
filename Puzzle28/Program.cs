using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle28
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(14);

            var (template, insertionRules) = ParseInput(input);

            var polymers = GetHighestLowestPolymerDifference(template, insertionRules);

            Console.WriteLine($"Polymer difference: {polymers}");
        }

        private static (string template, IReadOnlyDictionary<(char pairOne, char pairTwo), char> insertionRules) ParseInput(string input)
        {
            var inputs = input.Split("\n\n", 2);
            var template = inputs[0].Trim();
            var insertionRules = inputs[1]
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split(" -> "))
                .ToDictionary(strings => (strings[0].First(), strings[0].Last()), strings => strings[1].First());
            return (template, insertionRules);
        }

        private static long GetHighestLowestPolymerDifference(string template, IReadOnlyDictionary<(char pairOne, char pairTwo), char> insertionRules)
        {
            var pairs = BuildStartingPairs(template, insertionRules);
            var polymerCounts = insertionRules.Values.Distinct().ToDictionary(c => c, c => 0L);
            foreach (var c in template)
            {
                ++polymerCounts[c];
            }

            foreach (var _ in Enumerable.Range(0, 40))
            {
                pairs = BuildPairs(pairs, insertionRules, polymerCounts);
            }

            return polymerCounts.Values.Max() - polymerCounts.Values.Min();
        }

        private static IReadOnlyDictionary<(char pairOne, char pairTwo), long> BuildStartingPairs(string template, IReadOnlyDictionary<(char pairOne, char pairTwo), char> insertionRules)
        {
            var pairs = insertionRules.Keys.ToDictionary(tuple => tuple, _ => 0L); 
            
            for (var i = 1; i < template.Length; i++)
            {
                ++pairs[(template[i - 1], template[i])];
            }

            return pairs;
        }

        private static IReadOnlyDictionary<(char pairOne, char pairTwo), long> BuildPairs(IReadOnlyDictionary<(char pairOne, char pairTwo), long> inputPairs, IReadOnlyDictionary<(char pairOne, char pairTwo), char> insertionRules, Dictionary<char,long> polymerCounts)
        {
            var resultingPairs = insertionRules.Keys.ToDictionary(tuple => tuple, _ => 0L);
            
            foreach (var (key, quantity) in inputPairs.ToList())
            {
                var newChar = insertionRules[(key.pairOne, key.pairTwo)];
                polymerCounts[newChar] += quantity;
                resultingPairs[(key.pairOne, newChar)] += quantity;
                resultingPairs[(newChar, key.pairTwo)] += quantity;
            }

            return resultingPairs;
        }
    }
}