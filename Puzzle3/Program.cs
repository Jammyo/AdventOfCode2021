using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Puzzle3
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(2);
            var commands = ParseInput(input);
            var (position, depth) = CalculateCourse(commands);
            
            Console.WriteLine($"Position: {position}, Depth: {depth}");
            Console.WriteLine($"Position * Depth = {position * depth}");
        }

        static IReadOnlyList<Command> ParseInput(string input)
        {
            //Clean and convert input.
            return input.Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Split(' '))
                .Select(s => new Command(ParseDirection(s[0]), ParseRange(s[1])))
                .ToList();
        }

        static Direction ParseDirection(string direction)
        {
            switch (direction)
            {
                case "forward":
                    return Direction.Forward;
                case "down":
                    return Direction.Down;
                case "up":
                    return Direction.Up;
                default:
                    throw new Exception($"Failed to parse direction {direction}.");
            }
        }

        static int ParseRange(string range)
        {
            if (int.TryParse(range, out var result))
            {
                return result;
            }
            throw new Exception($"Failed to parse range {range}");
        }
        
        static (int position, int depth) CalculateCourse(IReadOnlyList<Command> commands)
        {
            var position = 0;
            var depth = 0;
            
            foreach (var command in commands)
            {
                switch (command.Direction)
                {
                    case Direction.Forward:
                        position += command.Range;
                        break;
                    case Direction.Up:
                        depth -= command.Range;
                        break;
                    case Direction.Down:
                        depth += command.Range;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(command), $"{command.Direction}");
                }
            }

            return (position, depth);
        }

        class Command
        {
            public Command(Direction direction, int range)
            {
                Direction = direction;
                Range = range;
            }

            public Direction Direction { get; }
            public int Range { get; }
        }

        enum Direction
        {
            Forward,
            Up,
            Down
        }
    }
}