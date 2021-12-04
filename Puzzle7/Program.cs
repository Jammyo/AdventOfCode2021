using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle7
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(4);

            var bingoGame = ParseInput(input);

            var score = FindScoreOfWinningBoard(bingoGame);
            
            Console.WriteLine($"Score: {score}.");
        }

        private static BingoGame ParseInput(string input)
        {
            var numbersAndBoards = input.Split('\n', 2);
            var calledNumbers = numbersAndBoards[0].Trim()
                .Split(',')
                .Select(int.Parse)
                .ToList();
            var boards = numbersAndBoards[1].Split("\n\n") // Each double new line demarcates a new board.
                .Select(s => s.Trim().Split('\n')) // Each new line marks a new row.
                .Select(rows => rows.Select(row => row.Trim() // Trim each row
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries) // Each space marks a new cell.
                    .Select(s => new Cell(int.Parse(s))))) // Store the cell.
                .Select(rows => rows.Select(cells => new Row(cells.ToList()))) // Store the row.
                .Select(rows => new Board(rows.ToList())) // Store the board.
                .ToList();

            return new BingoGame(calledNumbers, boards);
        }

        private static int FindScoreOfWinningBoard(BingoGame bingoGame)
        {
            foreach (var calledNumber in bingoGame.CalledNumbers)
            {
                // Mark new number
                foreach (var board in bingoGame.Boards)
                {
                    foreach (var row in board.Rows)
                    {
                        foreach (var cell in row.Cells)
                        {
                            if (cell.Number.Equals(calledNumber))
                            {
                                cell.IsMarked = true;
                            }
                        }
                    }
                }
                
                // See if we have a winning board.
                foreach (var board in bingoGame.Boards)
                {
                    // Check for any rows where all cells are marked.
                    if (board.Rows.Any(row => row.Cells.All(cell => cell.IsMarked)))
                    {
                        return CalculateBoardScore(board, calledNumber);
                    }
                    
                    // Check for any columns where all the rows are marked.
                    var columnSize = board.Rows[0].Cells.Count;
                    if (Enumerable.Range(0, columnSize).Any(i => board.Rows.All(row => row.Cells[i].IsMarked)))
                    {
                        return CalculateBoardScore(board, calledNumber);
                    }
                }
            }

            throw new Exception("Failed to find a winning board..");
        }

        private static int CalculateBoardScore(Board board, int calledNumber)
        {
            return board.Rows.Sum(row => row.Cells.Where(cell => !cell.IsMarked).Sum(cell => cell.Number)) * calledNumber;
        }
    }

    class BingoGame
    {
        public BingoGame(IReadOnlyList<int> calledNumbers, IReadOnlyList<Board> boards)
        {
            CalledNumbers = calledNumbers;
            Boards = boards;
        }

        public IReadOnlyList<int> CalledNumbers { get; }
        public IReadOnlyList<Board> Boards { get; }
    }

    class Board
    {
        public Board(IReadOnlyList<Row> rows)
        {
            Rows = rows;
        }

        public IReadOnlyList<Row> Rows { get; }
    }

    class Row
    {
        public Row(IReadOnlyList<Cell> cells)
        {
            Cells = cells;
        }

        public IReadOnlyList<Cell> Cells { get; }
    }

    class Cell
    {
        public Cell(int number)
        {
            Number = number;
            IsMarked = false;
        }

        public int Number { get; }
        public bool IsMarked { get; set; }
    }
}