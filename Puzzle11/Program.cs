﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Shared.LanternFish;

namespace Puzzle11
{
    class Program
    {
        static async Task Main()
        {
            var input = await AdventOfCode.GetInput(6);

            var initialFish = LanternFish.ParseInput(input);

            var finalFish = LanternFish.CountFishAfterDays(initialFish, 80);
            
            Console.WriteLine($"Lantern fish: {finalFish}.");
        }
    }
}