using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Bingo
{
    public class BingoGame
    {
        public BingoGame(IReadOnlyList<int> calledNumbers, IReadOnlyList<Board> boards)
        {
            CalledNumbers = calledNumbers;
            Boards = boards;
        }

        public IReadOnlyList<int> CalledNumbers { get; }
        public IReadOnlyList<Board> Boards { get; }
        
        public static BingoGame ParseInput(string input)
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

        public void CallNumber(int calledNumber)
        {
            // Mark new number
            foreach (var board in Boards)
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
        }

        public IReadOnlyList<Board> GetWinningBoards()
        {
            var winningBoards = new List<Board>();
            
            foreach (var board in Boards)
            {
                var columnSize = board.Rows[0].Cells.Count;
                
                // Check for any rows where all cells are marked.
                if (board.Rows.Any(row => row.Cells.All(cell => cell.IsMarked)))
                {
                    winningBoards.Add(board);
                }
                // Otherwise check for any columns where all the rows are marked.
                else if (Enumerable.Range(0, columnSize).Any(i => board.Rows.All(row => row.Cells[i].IsMarked)))
                {
                    winningBoards.Add(board);
                }
            }

            return winningBoards;
        }
    }
}