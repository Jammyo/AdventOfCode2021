using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Shared;

namespace Puzzle33
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(17);

            var targetArea = ParseInput(input);

            var highestYPosition = CalculateHighestYPosition(targetArea);
            
            Console.WriteLine($"Highest y position {highestYPosition}.");
        }

        private static TargetArea ParseInput(string input)
        {
            var inputs = input
                .Trim()
                .Replace("target area: ", "")
                .Replace("x=", "")
                .Replace("y=", "")
                .Split(',', StringSplitOptions.TrimEntries)
                .Select(s => s.Split("..", StringSplitOptions.TrimEntries).Select(int.Parse).ToList())
                .ToList();
            return new TargetArea(
                inputs[0][0],
                inputs[1][0],
                inputs[0][1],
                inputs[1][1]);
        }

        private static int CalculateHighestYPosition(TargetArea targetArea)
        {
            var lowestXVelocity = CalculateLowestXVelocity(targetArea);
            var highestXVelocity = targetArea.Right;
            var lowestYVelocity = 1;
            var highestYVelocity = targetArea.Bottom * -1;
            var totalHighestYPosition = 0;
            
            foreach (var xVelocity in Enumerable.Range(lowestXVelocity, highestXVelocity))
            {
                foreach (var yVelocity in Enumerable.Range(lowestYVelocity, highestYVelocity))
                {
                    var (simulationResult, highestYPosition) = Simulate(new Vector2(xVelocity, yVelocity), targetArea);
                    var shouldIncreaseXVelocity = false;
                    switch (simulationResult)
                    {
                        case SimulationResult.Undershot:
                            // More velocity needed, break out to the xVelocity loop since going higher will not help with this shortfall.
                            shouldIncreaseXVelocity = true;
                            break;
                        case SimulationResult.Overshot:
                            // Break out to the xVelocity loop since going higher will only make the overshoot worse.
                            shouldIncreaseXVelocity = true;
                            break;
                        case SimulationResult.Missed:
                            // Y can go higher until we overshoot.
                            break;
                        case SimulationResult.Hit:
                            // See if this marks our highest point, then try going even higher!
                            if (totalHighestYPosition < highestYPosition)
                            {
                                totalHighestYPosition = highestYPosition;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (shouldIncreaseXVelocity)
                    {
                        break;
                    }
                }
            }

            return totalHighestYPosition;
        }

        private static int CalculateLowestXVelocity(TargetArea targetArea)
        {
            var position = 0;
            // Reverse engineer the drag coefficient by slowly incrementing x and adding this to the position until we pass the target area. 
            foreach (var x in Enumerable.Range(1, targetArea.Right))
            {
                position += x;
                if (position >= targetArea.Left)
                {
                    return x; // x is the smallest velocity which can make it to the target area.
                }
            }

            throw new Exception("Failed to calculate the lowest x velocity.");
        }

        private static (SimulationResult simulationResult, int highestYPosition) Simulate(Vector2 velocity, TargetArea targetArea)
        {
            var position = new Vector2(0, 0);
            var highestY = 0;
            
            while (position.X <= targetArea.Right && position.Y > targetArea.Bottom)
            {
                position += velocity;
                velocity -= new Vector2(Math.Sign(velocity.X), 1);
                if (highestY < position.Y)
                {
                    highestY = (int)position.Y;
                }

                if (targetArea.Contains((int)position.X, (int)position.Y))
                {
                    return (SimulationResult.Hit, highestY);
                }
            }

            SimulationResult simulationResult;
            if (position.X < targetArea.Left)
            {
                simulationResult = SimulationResult.Undershot;
            }
            else if (position.Y > targetArea.Top)
            {
                simulationResult = SimulationResult.Overshot;
            }
            else
            {
                simulationResult = SimulationResult.Missed;
            }

            return (simulationResult, highestY);
        }
    }

    public enum SimulationResult
    {
        Undershot, // Speed ran out before we hit the target area.
        Overshot, // We were too high when we passed the target area.
        Missed, // We missed the target area as a result of travelling too fast.
        Hit // Success.
    }

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