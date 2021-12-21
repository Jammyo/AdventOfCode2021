using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Puzzle35
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(18);

            var snailfishNumbers = ParseInput(input);
            
            var finalSum = AddSnailfishNumbers(snailfishNumbers);
            var magnitude = CalculateMagnitude(finalSum);
            
            Console.WriteLine($"Magnitude: {magnitude}.");
        }

        private static string PrintSnailfishNumber(SnailfishNumber snailfishNumber)
        {
            var result = new StringBuilder();
            var currentDepth = 0;
            foreach (var (value, depth) in snailfishNumber.Numbers)
            {
                while (currentDepth < depth)
                {
                    ++currentDepth;
                    result.Append('[');
                }
                while (currentDepth > depth)
                {
                    --currentDepth;
                    result.Append(']');
                }
                if (value == null)
                {
                    result.Append(',');
                }
                else
                {
                    result.Append(value);
                }
            }
            while (currentDepth > 0)
            {
                --currentDepth;
                result.Append(']');
            }
            return result.ToString();
        }

        private static IReadOnlyList<SnailfishNumber> ParseInput(string input)
        {
            return input
                .Trim()
                .Split('\n')
                .Select(ParseSnailfishNumber)
                .ToList();
        }

        private static SnailfishNumber ParseSnailfishNumber(string line)
        {
            var depth = 0;
            var numbers = new List<(int? value, int depth)>();
            foreach (var c in line)
            {
                switch (c)
                {
                    case '[':
                        ++depth;
                        break;
                    case ']':
                        --depth;
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        numbers.Add((int.Parse($"{c}"), depth));
                        break;
                    case ',':
                        // A bit naughty, but using null as the separator allows us to remember pairs easily.
                        numbers.Add((null, depth));
                        break;
                }
            }
            return new SnailfishNumber(numbers);
        }

        private static SnailfishNumber AddSnailfishNumbers(IReadOnlyList<SnailfishNumber> rows)
        {
            return rows.Aggregate((result, current) =>
            {
                var workingResult = result.Numbers
                    .Append((null, 0)) // This 0 will be increased to depth 1 in the select.
                    .Concat(current.Numbers)
                    .Select(tuple => (value: tuple.value, depth: tuple.depth + 1))
                    .ToList();

                bool appliedReduction;
                do
                {
                    appliedReduction = false;
                    if (ApplyExplode(workingResult))
                    {
                        appliedReduction = true;
                    }
                    else if (ApplySplit(workingResult))
                    {
                        appliedReduction = true;
                    }
                } while (appliedReduction);

                return new SnailfishNumber(workingResult);
            });
        }

        private static bool ApplyExplode(List<(int? value, int depth)> workingResult)
        {
            var leftExplodeIndex = workingResult.FindIndex(tuple => tuple.depth > 4);
            if (leftExplodeIndex == -1)
            {
                return false; // No explosion needed.
            }
            
            if (leftExplodeIndex > 0) // We can add this value to the left.
            {
                var previousIndex = leftExplodeIndex - 1;
                // Skip past all commas until we reach a concrete number.
                while (workingResult[previousIndex].value == null)
                {
                    --previousIndex;
                }

                // Add our left value to the previous.
                workingResult[previousIndex] = (
                    workingResult[previousIndex].value + workingResult[leftExplodeIndex].value, 
                    workingResult[previousIndex].depth);
            }

            var rightExplodeIndex = leftExplodeIndex + 2; // Skip past the comma.
            if (rightExplodeIndex + 1 < workingResult.Count)
            {
                var nextIndex = rightExplodeIndex + 1;
                // Skip past all commas until we reach a concrete number.
                while (workingResult[nextIndex].value == null)
                {
                    ++nextIndex;
                }
                
                // Add our right value to the next.
                workingResult[nextIndex] = (
                    workingResult[nextIndex].value + workingResult[rightExplodeIndex].value,
                    workingResult[nextIndex].depth);
            }

            // Remove right pair and comma, replace left pair with 0.
            workingResult.RemoveAt(rightExplodeIndex);
            workingResult.RemoveAt(leftExplodeIndex + 1);

            workingResult[leftExplodeIndex] = (0, workingResult[leftExplodeIndex].depth - 1);

            return true;
        }

        private static bool ApplySplit(List<(int? value, int depth)> workingResult)
        {
            var splitIndex = workingResult.FindIndex(tuple => tuple.value > 9);
            if (splitIndex == -1)
            {
                return false; // No split needed.
            }

            var split = workingResult[splitIndex];
            var newFirst = (int)Math.Floor((float)split.value! / 2);
            var newSecond = (int)Math.Ceiling((float)split.value! / 2);
            workingResult[splitIndex] = (newFirst, split.depth + 1);
            workingResult.Insert(splitIndex + 1, (null, split.depth + 1));
            workingResult.Insert(splitIndex + 2, (newSecond, split.depth + 1));

            return true;
        }

        private static int CalculateMagnitude(SnailfishNumber finalSum)
        {
            var workingResult = finalSum.Numbers.ToList();
            while (workingResult.Count > 1)
            {
                var largestDepth = workingResult.Max(tuple => tuple.depth);
                var leftDeepestIndex = workingResult.FindIndex(tuple => tuple.depth == largestDepth);
                var commaIndex = leftDeepestIndex + 1;
                var rightDeepestIndex = commaIndex + 1;

                var magnitude = workingResult[leftDeepestIndex].value * 3 + workingResult[rightDeepestIndex].value * 2;

                // Remove right pair and comma, replace left pair with 0.
                workingResult.RemoveAt(rightDeepestIndex);
                workingResult.RemoveAt(commaIndex);

                workingResult[leftDeepestIndex] = (magnitude, largestDepth - 1);
            }

            return workingResult.First().value!.Value;
        }
    }

    public class SnailfishNumber
    {
        public SnailfishNumber(IReadOnlyList<(int? value, int depth)> numbers)
        {
            Numbers = numbers;
        }

        public IReadOnlyList<(int? value, int depth)> Numbers { get; }
    }
}