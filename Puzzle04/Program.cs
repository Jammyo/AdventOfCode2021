using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared;

namespace Puzzle04
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(2);
            var commands = CommandBuilder.ParseInput(input);
            var (position, depth) = CalculateCourse(commands);
            
            Console.WriteLine($"Position: {position}, Depth: {depth}");
            Console.WriteLine($"Position * Depth = {position * depth}");
        }

        static (int position, int depth) CalculateCourse(IReadOnlyList<Command> commands)
        {
            var position = 0;
            var depth = 0;
            var aim = 0;
            
            foreach (var command in commands)
            {
                switch (command.Direction)
                {
                    case Direction.Forward:
                        position += command.Value;
                        depth += aim * command.Value;
                        break;
                    case Direction.Up:
                        aim -= command.Value;
                        break;
                    case Direction.Down:
                        aim += command.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(command), $"{command}");
                }
            }
            
            return (position, depth);
        }
    }
}