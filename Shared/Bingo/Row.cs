using System.Collections.Generic;

namespace Shared.Bingo
{
    public class Row
    {
        public Row(IReadOnlyList<Cell> cells)
        {
            Cells = cells;
        }

        public IReadOnlyList<Cell> Cells { get; }
    }
}