namespace Shared.Probe
{
    public class TargetArea
    {
        public int Left { get; }
        public int Bottom { get; }
        public int Right { get; }
        public int Top { get; }

        public TargetArea(int left, int bottom, int right, int top)
        {
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
        }

        public bool Contains(int x, int y)
        {
            return Left <= x && x <= Right && Bottom <= y && y <= Top;
        }
    }
}