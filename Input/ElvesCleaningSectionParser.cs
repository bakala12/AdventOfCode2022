using AdventOfCode2022.Models;
using Range = AdventOfCode2022.Models.Range;

namespace AdventOfCode2022.Input
{
    public class ElvesCleaningSectionParser : IInputParser<ElvesCleaningSections[]>
    {
        public ElvesCleaningSections[] ParseInput(string input)
        {
            return input.Split(Environment.NewLine).Select(ParseSection).ToArray();
        }

        private ElvesCleaningSections ParseSection(string line)
        {
            var ranges = line.Split(",");
            var f = ranges[0].Split("-");
            var s = ranges[1].Split("-");
            var e1 = new Range(int.Parse(f[0]), int.Parse(f[1]));
            var e2 = new Range(int.Parse(s[0]), int.Parse(s[1]));
            return new ElvesCleaningSections(e1, e2);
        }
    }
}