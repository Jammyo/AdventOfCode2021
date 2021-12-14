using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle27
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(14);

            var (template, insertionRules) = ParseInput(input);

            var polymers = InsertPolymers(40, template, insertionRules);

            PrintAnswer(polymers);
        }

        private static (string template, IReadOnlyDictionary<string, string> insertionRules) ParseInput(string input)
        {
            var inputs = input.Split("\n\n", 2);
            var template = inputs[0].Trim();
            var insertionRules = inputs[1]
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split(" -> "))
                .ToDictionary(strings => strings[0], strings => strings[1]);
            return (template, insertionRules);
        }

        private static string InsertPolymers(int insertions, string polymers, IReadOnlyDictionary<string, string> insertionRules)
        {
            return Enumerable
                .Range(0, insertions)
                .Aggregate(polymers, (current, _) => InsertPolymers(current, insertionRules));
        }

        private static string InsertPolymers(string polymers, IReadOnlyDictionary<string, string> insertionRules)
        {
            // Figure out what needs to be inserted.
            var insertions = new List<(string insertion, int position)>();
            for (var i = 1; i < polymers.Length; i++)
            {
                var polymerSegment = string.Join("", polymers[i - 1], polymers[i]);
                var insertion = insertionRules[polymerSegment];
                insertions.Add((insertion, i));
            }

            // Reverse the list of instructions so that we do not need to worry about indices W.R.T. changing size.
            insertions.Reverse();
            
            // Perform the insertion.
            foreach (var (insertion, position) in insertions)
            {
                polymers = polymers.Insert(position, insertion);
            }

            return polymers;
        }

        private static void PrintAnswer(string polymers)
        {
            var mostCommon = (polymer: ' ', count: 0);
            var leastCommon = (polymer: ' ', count: int.MaxValue);
            
            foreach (var polymer in polymers.Distinct())
            {
                var count = polymers.Count(c => c.Equals(polymer));
                if (mostCommon.count < count)
                {
                    mostCommon = (polymer, count);
                }
                if (leastCommon.count > count)
                {
                    leastCommon = (polymer, count);
                }
            }
            
            Console.WriteLine($"Polymer difference: ({mostCommon.polymer} - {leastCommon.polymer}) {mostCommon.count - leastCommon.count}.");
        }
    }
}