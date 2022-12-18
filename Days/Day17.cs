using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day17 : AocDay<LeftRight[]>
    {
        public Day17(IInputParser<LeftRight[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(LeftRight[] input)
        {
            var well = new List<bool[]>();

            Console.WriteLine(well.Count);
        }

        protected override void Part2(LeftRight[] input)
        {
        }

        private List<List<bool[]>> Blocks = new List<List<bool[]>>()
        {

        };
    }
}