using System;
using System.Linq;
using System.Numerics;

namespace Shared.Probe
{
    public class Probe
    {
        public static TargetArea ParseInput(string input)
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
        
        private static (int highestYPosition, int validInitialVelocities) RunSimulation(TargetArea targetArea)
        {
            var lowestXVelocity = CalculateLowestXVelocity(targetArea) - 1;
            var highestXVelocity = targetArea.Right + 1;
            var lowestYVelocity = targetArea.Bottom - 1;
            var highestYVelocity = -targetArea.Bottom + 1;
            var totalHighestYPosition = 0;
            var validInitialVelocities = 0;
            
            foreach (var xVelocity in Enumerable.Range(lowestXVelocity, highestXVelocity - lowestXVelocity))
            {
                foreach (var yVelocity in Enumerable.Range(lowestYVelocity, highestYVelocity - lowestYVelocity))
                {
                    var (hitTarget, highestYPosition) = Simulate(new Vector2(xVelocity, yVelocity), targetArea);
                    if(hitTarget)
                    {
                        // See if this marks our highest point, then try going even higher!
                        ++validInitialVelocities; 
                        if (totalHighestYPosition < highestYPosition)
                        {
                            totalHighestYPosition = highestYPosition;
                        }
                    }
                }
            }

            return (totalHighestYPosition, validInitialVelocities);
        }

        private static (bool hitTarget, int highestYPosition) Simulate(Vector2 velocity, TargetArea targetArea)
        {
            var position = new Vector2(0, 0);
            var highestY = 0;
            
            while (position.X <= targetArea.Right && position.Y >= targetArea.Bottom)
            {
                position += velocity;
                velocity -= new Vector2(Math.Sign(velocity.X), 1);
                if (highestY < position.Y)
                {
                    highestY = (int)position.Y;
                }

                if (targetArea.Contains((int)position.X, (int)position.Y))
                {
                    return (true, highestY);
                }
            }

            return (false, highestY);
        }

        public static int CalculateHighestYPosition(TargetArea targetArea)
        {
            return RunSimulation(targetArea).highestYPosition;
        }

        public static int CountValidInitialVelocities(TargetArea targetArea)
        {
            return RunSimulation(targetArea).validInitialVelocities;
        }
    }
}