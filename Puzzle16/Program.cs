using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle16
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(8);

            var entries = ParseInput(input);

            var actualValues = SumActualValues(entries);
            
            Console.WriteLine($"Sum actual values: {actualValues}.");
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

        private static int SumActualValues(IReadOnlyList<Entry> entries)
        {
            var sum = 0;
            foreach (var entry in entries)
            {
                var (top, topLeft, topRight, middle, bottomLeft, bottomRight, bottom) = MapSegments(entry);

                var value = 0;
                foreach (var output in entry.Outputs)
                {
                    value *= 10;
                    switch (output.Length)
                    {
                        case 2: // 1
                            value += 1;
                            break;
                        case 3: // 7
                            value += 7;
                            break;
                        case 4: // 4
                            value += 4;
                            break;
                        case 5: // 2, 3, or 5.
                            if (output.Contains(top) && 
                                output.Contains(topRight) && 
                                output.Contains(middle) &&
                                output.Contains(bottomLeft) && 
                                output.Contains(bottom))
                            {
                                value += 2;
                            }
                            else if(output.Contains(top) && 
                                    output.Contains(topRight) &&
                                    output.Contains(middle) &&
                                    output.Contains(bottomRight) &&
                                    output.Contains(bottom))
                            {
                                value += 3;
                            }
                            else
                            {
                                value += 5;
                            }
                            break;
                        case 6: // 6, 9, or 0.
                            if (!output.Contains(topRight))
                            {
                                value += 6;
                            }
                            else if (!output.Contains(bottomLeft))
                            {
                                value += 9;
                            }
                            break;
                        case 7: // 8
                            value += 8;
                            break;
                    }
                }

                sum += value;
            }

            return sum;
        }

        private static (char top, char topLeft, char topRight, char middle, char bottomLeft, char bottomRight, char bottom)
            MapSegments(Entry entry)
        {
            var (_1, _4, _7, _8, _235, _690) = GetKnownSegments(entry.Signals);

            // The top segment will be the bit of 7 which isn't in 1.
            var top = _7.Except(_1).Single();

            // Middle comes from 6,9,0 because 0 is the only digit which has its missing segment (middle) represented in all of 2,3,5.
            var middle = _690
                .Select(s => _8.Except(s).Single())
                .Single(c => _235.All(s => s.Contains(c)));

            // Top left is the bit of 4 which isn't in 1 or the middle segment.
            var topLeft = _4.Except(_1).Except(new List<char>{middle}).Single();

            // Top right can be got from the 6 by excluding the two values (9, and 0) which fully contain 1.
            var topRight = _690
                .Select(s => _1.Except(s).ToList())
                .Single(list => list.Any())
                .Single();

            // Bottom left will be the missing segment from 9, taken from 6,9,0 where the string contains top right and middle.
            var _9 = _690.Single(s => s.Contains(topRight) && s.Contains(middle));
            var bottomLeft = _8.Except(_9).Single();

            // Bottom right can be found by removing the top right character from 1.
            var bottomRight = _1.Except(new List<char>{topRight}).Single();

            // Bottom is the last remaining, use the other segments to validate they are all right.
            var bottom = _8.Except(new List<char> { top, topLeft, topRight, middle, bottomLeft, bottomRight }).Single();
            return (top, topLeft, topRight, middle, bottomLeft, bottomRight, bottom);
        }

        private static (string _1, string _4, string _7, string _8, IReadOnlyList<string> _235, IReadOnlyList<string> _690) GetKnownSegments(IReadOnlyList<string> segments)
        {
            return (
                segments.First(s => s.Length is 2),
                segments.First(s => s.Length is 4),
                segments.First(s => s.Length is 3),
                segments.First(s => s.Length is 7),
                segments.Where(s => s.Length is 5).ToList(),
                segments.Where(s => s.Length is 6).ToList());
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