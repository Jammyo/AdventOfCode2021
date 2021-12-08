using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle15
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(8);

            var entries = ParseInput(input);

            var uniqueDigits = CountUniqueDigits(entries);
            
            Console.WriteLine($"Unique digits: {uniqueDigits}.");
        }

        private static IReadOnlyList<Entry> ParseInput(string input)
        {
            return input
                .Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Split(" | "))
                .Select(signalsAndOutputs => new Entry(ParseSegments(signalsAndOutputs[0]), ParseSegments(signalsAndOutputs[1])))
                .ToList();
        }

        private static IReadOnlyList<string> ParseSegments(string input)
        {
            return input
                .Split(' ')
                .ToList();
        }

        private static int CountUniqueDigits(IReadOnlyList<Entry> entries)
        {
            return entries
                .SelectMany(entry => entry.Outputs)
                .Select(s => s.Length)
                .Count(length => length is 2 or 3 or 4 or 7);
        }
    }

    class Entry
    {
        public IReadOnlyList<string> Signals { get; }
        public IReadOnlyList<string> Outputs { get; }

        public Entry(IReadOnlyList<string> signals, IReadOnlyList<string> outputs)
        {
            Signals = signals;
            Outputs = outputs;
        }
    }
}