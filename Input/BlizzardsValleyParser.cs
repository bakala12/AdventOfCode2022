using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class BlizzardsValleyParser : IInputParser<BlizzardsValley>
    {
        public BlizzardsValley ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var startColumn = FindColumnWhereNoWall(lines[0]);
            var finishColumn = FindColumnWhereNoWall(lines[lines.Length-1]);
            var blizzards = new List<Blizzard>();
            for (int y = 1; y < lines.Length - 1; y++)
                for (int x = 1; x < lines[y].Length - 1; x++)
                    if (TryParseBlizzardDirection(lines[y][x], out BlizzardDirection dir))
                        blizzards.Add(new Blizzard((y, x), dir));
            return new BlizzardsValley(lines[0].Length, lines.Length, (0, startColumn), (lines.Length-1, finishColumn), blizzards.ToArray());
        }

        private static int FindColumnWhereNoWall(string line)
        {
            for (int i = 0; i < line.Length; i++)
                if (line[i] == '.')
                    return i;
            return -1;
        }

        private static bool TryParseBlizzardDirection(char c, out BlizzardDirection dir)
        {
            dir = c switch
            {
                '<' => BlizzardDirection.Left,
                '>' => BlizzardDirection.Right,
                '^' => BlizzardDirection.Up,
                'v' => BlizzardDirection.Down,
                _ => (BlizzardDirection)(-1)
            };
            return (int)dir >= 0;
        }
    }
}