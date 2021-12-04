using System.Collections.Generic;
using System.Linq;

namespace Shared.Bingo
{
    public class Board
    {
        public Board(IReadOnlyList<Row> rows)
        {
            Rows = rows;
        }

        public IReadOnlyList<Row> Rows { get; }

        public int CalculateScore(int calledNumber)
        {
            return Rows.Sum(row => row.Cells.Where(cell => !cell.IsMarked).Sum(cell => cell.Number)) * calledNumber;
        }
    }
}