namespace Shared.Bingo
{
    public class Cell
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