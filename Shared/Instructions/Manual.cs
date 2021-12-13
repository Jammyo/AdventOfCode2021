using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Instructions
{
    public class Manual
    {
        public static (IReadOnlyList<(int x, int y)> page, IReadOnlyList<(bool foldX, int position)> instructions) ParseInput(string input)
        {
            var manual = input.Split("\n\n");
            var page = manual[0]
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split(',').Select(int.Parse).ToList())
                .Select(ints => (x: ints[0], y: ints[1]))
                .ToList();
            var instructions = manual[1]
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Replace("fold along ", ""))
                .Select(s => s.Split('='))
                .Select(strings => (foldX: strings[0].Equals("x"), position: int.Parse(strings[1])))
                .ToList();

            return (page, instructions);
        }
        
        public static IReadOnlyList<(int x, int y)> FoldPage(IReadOnlyList<(int x, int y)> page, (bool foldX, int position) instruction)
        {
            var foldedPage = new List<(int x, int y)>();
            
            // Fold the page.
            foreach (var (x, y) in page)
            {
                if (instruction.foldX)
                {
                    foldedPage.Add((instruction.position - Math.Abs(x - instruction.position), y));
                }
                else
                {
                    foldedPage.Add((x, instruction.position - Math.Abs(y - instruction.position)));
                }
            }
            
            // Collapse the results.
            return foldedPage.Distinct().Reverse().ToList();
        }
    }
}