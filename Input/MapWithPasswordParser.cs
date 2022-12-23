using AdventOfCode2022.Models;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Input
{
    public class MapWithPasswordParser : IInputParser<MapWithPassword>
    {
        public MapWithPassword ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var width = lines.Take(lines.Length - 2).Max(l => l.Length);
            var map = new char[lines.Length - 2, width];
            for (int i = 0; i < lines.Length - 2; i++)
                for (int j = 0; j < width; j++)
                    map[i, j] = ' ';
            for(int i = 0; i < lines.Length - 2; i++)
                for(int j = 0; j < lines[i].Length; j++)
                    map[i,j] = lines[i][j];
            var passwordLine = lines.Last();
            var first = int.Parse(Regex.Match(passwordLine, @"\d+").Value);
            var all = Regex.Matches(passwordLine, @"([LR])(\d+)");
            return new MapWithPassword(map, first, all.Select(Convert).ToArray());
        }

        private static TurnInfo Convert(Match match)
        {
            return new TurnInfo(match.Groups[1].Value == "L" ? TurnType.Left : TurnType.Right, int.Parse(match.Groups[2].Value));
        }
    }
}