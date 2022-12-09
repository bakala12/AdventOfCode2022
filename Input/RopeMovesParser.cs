using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class RopeMovesParser : IInputParser<RopeMove[]>
    {
        public RopeMove[] ParseInput(string input)
        {
            return input.Split(Environment.NewLine).Select(ParseMove).ToArray();
        }

        private static RopeMove ParseMove(string line)
        {
            var dir = line[0] switch
            {
                'L' => RopeMoveDirection.Left,
                'R' => RopeMoveDirection.Right,
                'U' => RopeMoveDirection.Up,
                'D' => RopeMoveDirection.Down,
                _ => throw new ArgumentException()
            };
            return new RopeMove(dir, int.Parse(line.Substring(2)));
        }
    }
}