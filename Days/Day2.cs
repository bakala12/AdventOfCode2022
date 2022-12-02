using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day2 : AocDay<RockPaperScizor[]>
    {
        public Day2(IInputParser<RockPaperScizor[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(RockPaperScizor[] input)
        {
            Console.WriteLine(input.Sum(r => r.RoundScore()));
        }

        protected override void Part2(RockPaperScizor[] input)
        {
            Console.WriteLine(input.Sum(r => r.RoundScore2()));
        }
    }
}