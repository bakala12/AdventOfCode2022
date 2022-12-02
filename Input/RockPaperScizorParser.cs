using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class RockPaperScizorParser : IInputParser<RockPaperScizor[]>
    {
        public RockPaperScizor[] ParseInput(string input)
        {
            return input.Split(Environment.NewLine)
                .Select(s => s.Split())
                .Select(r => new RockPaperScizor(r[0][0] - 'A', r[1][0] - 'X'))
                .ToArray();
        }
    }
}