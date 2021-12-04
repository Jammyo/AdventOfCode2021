using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Bingo;

namespace Puzzle8
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(4);

            var bingoGame = BingoGame.ParseInput(input);

            var score = FindScoreOfLosingBoard(bingoGame);
            
            Console.WriteLine($"Score: {score}.");
        }

        private static int FindScoreOfLosingBoard(BingoGame bingoGame)
        {
            var discoveredWinningBoards = new HashSet<Board>();
            foreach (var calledNumber in bingoGame.CalledNumbers)
            {
                bingoGame.CallNumber(calledNumber);

                var winningBoards = bingoGame.GetWinningBoards();
                if (winningBoards.Count == bingoGame.Boards.Count)
                {
                    //This iteration created the last winning board; the losing board.
                    var losingBoards = winningBoards.Except(discoveredWinningBoards).ToList();
                    if (losingBoards.Count != 1)
                    {
                        throw new Exception($"Expected exactly one losing board, but there were {losingBoards.Count}.");
                    }

                    return losingBoards.First().CalculateScore(calledNumber);
                }
                foreach (var winningBoard in winningBoards)
                {
                    discoveredWinningBoards.Add(winningBoard);
                }
            }

            throw new Exception("Failed to find a winning board..");
        }
    }
}