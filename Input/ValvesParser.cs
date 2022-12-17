using AdventOfCode2022.Models;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Input
{
    public class ValvesParser : IInputParser<Valve[]>
    {
        private static readonly Regex ParseRegex = new Regex(@"Valve (\w+) has flow rate=(\d+); tunnel(s?) lead(s?) to valve(s?) (((\w+)(, )?)+)", RegexOptions.Compiled);

        public Valve[] ParseInput(string input)
        {
            return input.Split(Environment.NewLine).Select(ParseValve).ToArray();
        }

        private static Valve ParseValve(string line)
        {
            var m = ParseRegex.Match(line);
            return new Valve(m.Groups[1].Value, int.Parse(m.Groups[2].Value), m.Groups[6].Value.Split(", "));
        }
    }
}