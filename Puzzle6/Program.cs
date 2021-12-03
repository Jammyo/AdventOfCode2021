using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle6
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(3);

            var (oxygen, co2) = CalculateLifeSupportRating(input);
            
            Console.WriteLine($"Oxygen: {oxygen}, CO2: {co2}.");
            Console.WriteLine($"Life support rating: {oxygen * co2}");
        }

        private static (int oxygen, int co2) CalculateLifeSupportRating(string input)
        {
            // Split and clean the input. The outer list represents a column, the inner list represents each row for that column.
            var columns = input.Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))

                // Handy rotation snippet from: https://stackoverflow.com/questions/39484996/rotate-transposing-a-listliststring-using-linq-c-sharp 
                .SelectMany(s => s.Select((digit, index) => (digit, index)))
                .GroupBy(tuple => tuple.index, tuple => tuple.digit)
                .Select(digits => digits.ToList())
                .ToList();

            // Create an index for each row.
            var indices = columns.First().Select((_, index) => index).ToList();
            
            //Get the index for each rating type.
            var oxygenIndex = GetRatingIndex(columns, indices, BitCriteria.Oxygen);
            var co2Index = GetRatingIndex(columns, indices, BitCriteria.Co2);

            var oxygen = ConvertIndexToRating(oxygenIndex, columns);
            var co2 = ConvertIndexToRating(co2Index, columns);

            return (oxygen, co2);
        }

        private static int ConvertIndexToRating(int index, List<List<char>> columns)
        {
            var rating = string.Join("", columns.Select(rows => rows[index]));
            return Convert.ToInt32(rating, 2);
        }

        private static int GetRatingIndex(List<List<char>> columns, List<int> indices, BitCriteria bitCriteria)
        {
            var resultingIndices = columns.Aggregate(indices, (remainingIndices, column) =>
            {
                if (remainingIndices.Count == 1)
                {
                    // We've found the answer, return early.
                    return remainingIndices;
                }

                var zeroes = column.Count(c => c.Equals('0'));
                var ones = column.Count(c => c.Equals('1'));
                var mostCommonBit = EstablishMostCommonBitFromCriteria(bitCriteria, ones, zeroes);
                return column.Select((c, index) => (c, index))
                    .Where(tuple => tuple.c.Equals(mostCommonBit))
                    .Select(tuple => tuple.index)
                    .Where(remainingIndices.Contains)
                    .ToList();
            });
            if (resultingIndices.Count != 1)
            {
                throw new Exception( $"Expected just one index, but found {resultingIndices.Count}. Diagnostics: {string.Join(", ", resultingIndices)}");
            }

            return resultingIndices.First();
        }

        private static char EstablishMostCommonBitFromCriteria(BitCriteria bitCriteria, int ones, int zeroes)
        {
            return bitCriteria switch
            {
                BitCriteria.Oxygen => ones >= zeroes ? '1' : '0', // Oxygen prefers 1's.
                BitCriteria.Co2 => zeroes >= ones ? '0' : '1', // Co2 prefers 0's.
                _ => throw new ArgumentOutOfRangeException(nameof(bitCriteria), bitCriteria, null)
            };
        }

        private enum BitCriteria
        {
            Oxygen,
            Co2
        }
    }
}