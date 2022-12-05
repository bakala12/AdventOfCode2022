using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class CratesStacksParser : IInputParser<CratesStacks>
    {
        public CratesStacks ParseInput(string input)
        {
            var s = input.Split($"{Environment.NewLine}{Environment.NewLine}");
            var stacks = ParseStacks(s[0]);
            var moves = ParseMoves(s[1]).ToArray();
            return new CratesStacks(stacks, moves);
        }

        private static CrateStack[] ParseStacks(string input)
        {
            var stacks = new List<List<char>>();
            foreach(var line in input.Split(Environment.NewLine))
            {
                if (line.StartsWith(" 1")) break;
                int sc = 0;
                foreach(var p in line.Chunk(4))
                {
                    var s = p[1];
                    if (sc == stacks.Count)
                        stacks.Add(new List<char>());
                    if (s != ' ')
                    {
                        var stack = stacks[sc];
                        stack.Add(s);
                    }
                    sc++;
                }
            }
            return stacks.Select(c => new CrateStack(c.ToArray())).ToArray();
        }

        private static IEnumerable<StackMove> ParseMoves(string input)
        {
            foreach (var move in input.Split(Environment.NewLine))
            {
                var s = move.Split(" ");
                yield return new StackMove(int.Parse(s[1]), int.Parse(s[3]), int.Parse(s[5]));
            }
        }
    }
}