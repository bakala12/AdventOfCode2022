using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class LeftRightParser : IInputParser<LeftRight[]>
    {
        public LeftRight[] ParseInput(string input)
        {
            return input.Select(s => s == '<' ? LeftRight.Left : LeftRight.Right).ToArray();
        }
    }
}