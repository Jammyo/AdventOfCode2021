namespace Shared
{
    public class Command
    {
        public Command(Direction direction, int value)
        {
            Direction = direction;
            Value = value;
        }

        public Direction Direction { get; }
        public int Value { get; }

        public override string ToString()
        {
            return $"{nameof(Direction)}: {Direction}, {nameof(Value)}: {Value}";
        }
    }
}