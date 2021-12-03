using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Puzzle5
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(3);

            var (gamma, epsilon) = CalculatePower(input);
            
            Console.WriteLine($"Gamma: {gamma}, Epsilon: {epsilon}.");
            Console.WriteLine($"Power consumption: {gamma * epsilon}");
        }

        private static (int gamma, int epsilon) CalculatePower(string input)
        {
            // Split and clean the input.
            var (gamma, epsilon) = input.Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                
                // Handy rotation snippet from: https://stackoverflow.com/questions/39484996/rotate-transposing-a-listliststring-using-linq-c-sharp 
                .SelectMany(s => s.Select((digit, index) => (digit, index)))
                .GroupBy(tuple => tuple.index, tuple => tuple.digit)
                .Select(digits => digits.ToList())
                
                //Sum gammas and epsilons, note that results are unexpected if zeroes == ones.
                .Aggregate((gamma: new StringBuilder(), epsilon: new StringBuilder()), (power, column) =>
                {
                    var (workingGamma, workingEpsilon) = power;
                    var zeroes = column.Count(c => c.Equals('0'));
                    var ones = column.Count(c => c.Equals('1'));
                    if (zeroes > ones)
                    {
                        return (workingGamma.Append('0'), workingEpsilon.Append('1'));
                    }
                    else
                    {
                        return (workingGamma.Append('1'), workingEpsilon.Append('0'));
                    }
                });

            return (Convert.ToInt32(gamma.ToString(), 2), Convert.ToInt32(epsilon.ToString(), 2));
        }
    }
}