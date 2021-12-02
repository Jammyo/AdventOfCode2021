using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class CommandBuilder
    {
        public static IReadOnlyList<Command> ParseInput(string input)
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
    }
}