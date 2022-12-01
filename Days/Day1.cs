using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day1 : AocDay<ElvesCalories[]>
    {
        public Day1(IInputParser<ElvesCalories[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(ElvesCalories[] input)
        {
            Console.WriteLine(input.Max(x => x.Calories.Sum()));
        }

        protected override void Part2(ElvesCalories[] input)
        {
            Console.WriteLine(input.OrderByDescending(x => x.Calories.Sum()).Take(3).Sum(x => x.Calories.Sum()));
        }
    }
}