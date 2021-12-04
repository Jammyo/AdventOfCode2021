using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.Bingo;

namespace Puzzle7
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(4);

            var bingoGame = BingoGame.ParseInput(input);

            var score = FindScoreOfWinningBoard(bingoGame);
            
            Console.WriteLine($"Score: {score}.");
        }

        private static int FindScoreOfWinningBoard(BingoGame bingoGame)
        {
            foreach (var calledNumber in bingoGame.CalledNumbers)
            {
                bingoGame.CallNumber(calledNumber);

                var winningBoards = bingoGame.GetWinningBoards();
                var winningBoard = winningBoards.FirstOrDefault();
                if (winningBoard != null)
                {
                    return winningBoard.CalculateScore(calledNumber);
                }
            }

            throw new Exception("Failed to find a winning board..");
        }
    }
}