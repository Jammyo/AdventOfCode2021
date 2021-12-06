using System.Collections.Generic;
using System.Linq;

namespace Shared.LanternFish
{
    public class LanternFish
    {
        public static IReadOnlyDictionary<int, long> ParseInput(string input)
        {
            return input.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .GroupBy(timer => timer, (timer, fishWithTheSameTimer) => (timer, count: fishWithTheSameTimer.Count()))
                .ToDictionary(tuple => tuple.timer, tuple => (long)tuple.count);
        }

        public static long CountFishAfterDays(IReadOnlyDictionary<int, long> initialFish, int days)
        {
            //Setup our working dictionary ensuring that all valid day values are initialised to 0.
            var fish = initialFish.ToDictionary(pair => pair.Key, pair => pair.Value);
            for (var i = 0; i <= 8; i++)
            {
                if (!fish.ContainsKey(i))
                {
                    fish[i] = 0;
                }
            }
            
            for (var day = 0; day < days; day++)
            {
                var todaysSpawningFish = fish.ContainsKey(0) ? fish[0] : 0;
                // Move each group of fish with the same timer down by one day.
                for (var i = 1; i <= 8; ++i)
                {
                    if (fish.ContainsKey(i))
                    {
                        fish[i - 1] = fish[i];
                    }
                    else
                    {
                        fish[i - 1] = 0;
                    }
                }
                
                //Add new fish based on today's spawning fish, and move today's spawning fish to day 6.
                fish[8] = todaysSpawningFish;
                if (fish.ContainsKey(6))
                {
                    fish[6] += todaysSpawningFish;
                }
                else
                {
                    fish[6] = todaysSpawningFish;
                }
            }

            return fish.Values.Sum();
        }
    }
}