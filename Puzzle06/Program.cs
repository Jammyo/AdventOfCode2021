using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle06
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
                .SelectMany(s => s.Select((digit, columnIndex) => (digit, columnIndex)))
                .GroupBy(tuple => tuple.columnIndex, tuple => tuple.digit)
                .Select(columns => columns.ToList())
                .Select(column => column.Select((cell, rowIndex) => (cell, rowIndex)).ToList())
                .ToList();
            
            //Get the index for each rating type.
            var oxygenIndex = GetRatingIndex(columns, BitCriteria.Oxygen);
            var co2Index = GetRatingIndex(columns, BitCriteria.Co2);

            var oxygen = ConvertIndexToRating(oxygenIndex, columns);
            var co2 = ConvertIndexToRating(co2Index, columns);

            return (oxygen, co2);
        }

        private static int ConvertIndexToRating(int index, List<List<(char cell, int rowIndex)>> columns)
        {
            var rating = string.Join("", columns.Select(rows => rows[index].cell));
            return Convert.ToInt32(rating, 2);
        }

        private static int GetRatingIndex(List<List<(char cell, int rowIndex)>> columns, BitCriteria bitCriteria)
        {
            var seedIndices = columns.First().Select(tuple => tuple.rowIndex).ToList();
            var resultingIndices = columns.Aggregate(seedIndices, (remainingRows, column) =>
            {
                if (remainingRows.Count == 1)
                {
                    // We've found the answer, return early.
                    return remainingRows;
                }

                // Filter out rows which have been eliminated by a previous iteration.
                var filteredColumn = column.Where(tuple => remainingRows.Contains(tuple.rowIndex)).ToList();

                // Calculate the most common bit.
                var zeroes = filteredColumn.Count(row => row.cell.Equals('0'));
                var ones = filteredColumn.Count(row => row.cell.Equals('1'));
                var criteriaBit = EstablishBitFromCriteria(bitCriteria, ones, zeroes);
                
                // Filter out the relevant bit.
                return filteredColumn.Where(row => row.cell.Equals(criteriaBit))
                    .Select(row => row.rowIndex)
                    .ToList();
            });
            if (resultingIndices.Count != 1)
            {
                throw new Exception( $"Expected just one index, but found {resultingIndices.Count}. Diagnostics: {string.Join(", ", resultingIndices)}");
            }

            return resultingIndices.First();
        }

        private static char EstablishBitFromCriteria(BitCriteria bitCriteria, int ones, int zeroes)
        {
            return bitCriteria switch
            {
                BitCriteria.Oxygen => ones >= zeroes ? '1' : '0', // Oxygen prefers most and 1's.
                BitCriteria.Co2 => zeroes <= ones ? '0' : '1', // Co2 prefers least and 0's.
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