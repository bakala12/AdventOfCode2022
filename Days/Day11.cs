using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day11 : AocDay<MonkeyItems[]>
    {
        public Day11(IInputParser<MonkeyItems[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(MonkeyItems[] input)
        {
            var items = new List<long>[input.Length];
            var toAdd = new List<long>[input.Length];
            int[] inspections = new int[input.Length];
            for(int i = 0; i < items.Length; i++)
            {
                items[i] = input[i].ItemsWorryLevels.ToList();
                toAdd[i] = new List<long>();
            }
            for(int round = 1; round <= 20; round++)
            {
                for(int i = 0; i < input.Length; i++)
                {
                    foreach(var item in items[i])
                    {
                        var @new = input[i].Operation(item) / 3;
                        if (@new % input[i].TestDivisibleBy == 0)
                            toAdd[input[i].ThrowIfTrueMonkey].Add(@new);
                        else
                            toAdd[input[i].ThrowIfFalsoMonkey].Add(@new);
                        inspections[i]++;
                    }
                    items[i].Clear();
                    for(int k = 0; k < input.Length; k++)
                    {
                        items[k].AddRange(toAdd[k]);
                        toAdd[k].Clear();
                    }
                }
            }
            Console.WriteLine(inspections.OrderByDescending(i => i).Take(2).Aggregate(1, (a, i) => a * i));
        }

        protected override void Part2(MonkeyItems[] input)
        {
            var m = input.Aggregate(1L, (a, m) => a * m.TestDivisibleBy);
            var items = new List<long>[input.Length];
            var toAdd = new List<long>[input.Length];
            int[] inspections = new int[input.Length];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = input[i].ItemsWorryLevels.ToList();
                toAdd[i] = new List<long>();
            }
            for (int round = 1; round <= 10000; round++)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    foreach (var item in items[i])
                    {
                        var @new = input[i].Operation(item) % m;
                        if (@new % input[i].TestDivisibleBy == 0)
                            toAdd[input[i].ThrowIfTrueMonkey].Add(@new);
                        else
                            toAdd[input[i].ThrowIfFalsoMonkey].Add(@new);
                        inspections[i]++;
                    }
                    items[i].Clear();
                    for (int k = 0; k < input.Length; k++)
                    {
                        items[k].AddRange(toAdd[k]);
                        toAdd[k].Clear();
                    }
                }
            }
            Console.WriteLine(inspections.OrderByDescending(i => i).Take(2).Aggregate(1L, (a, i) => a * i));
        }
    }
}