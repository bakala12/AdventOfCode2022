using AdventOfCode2022.Input;
using AdventOfCode2022.Models;
using Range = AdventOfCode2022.Models.Range;

namespace AdventOfCode2022.Days
{
    public class Day4 : AocDay<ElvesCleaningSections[]>
    {
        public Day4(IInputParser<ElvesCleaningSections[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(ElvesCleaningSections[] input)
        {
            int overlaps = 0;
            foreach(var sections in input)
            {
                if (IsOverlapping(sections.Elf1, sections.Elf2))
                    overlaps++;
            }
            Console.WriteLine(overlaps);
        }

        protected override void Part2(ElvesCleaningSections[] input)
        {
            int intersecting = 0;
            foreach (var sections in input)
            {
                if (IsIntersecting(sections.Elf1, sections.Elf2))
                    intersecting++;
            }
            Console.WriteLine(intersecting);
        }

        private static bool IsOverlapping(Range first, Range second)
        {
            return (first.From <= second.From && second.To <= first.To) ||
                (second.From <= first.From && first.To <= second.To);
        }

        private static bool IsIntersecting(Range first, Range second)
        {
            return (first.From >= second.From && first.From <= second.To) ||
                (second.From >= first.From && second.From <= first.To);
        }
    }
}