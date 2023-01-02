using AdventOfCode2022.Models;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Input
{
    public class BlueprintInfoParser : IInputParser<BlueprintInfo[]>
    {
        private static readonly Regex _parseRegex = new Regex(
            @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.", 
            RegexOptions.Compiled);

        public BlueprintInfo[] ParseInput(string input)
        {
            return _parseRegex.Matches(input).Select(ConvertMatch).ToArray();
        }

        private static BlueprintInfo ConvertMatch(Match match)
        {
            var num = int.Parse(match.Groups[1].Value);
            var oreRobot = new Robot(RobotType.Ore, new RobotCost(int.Parse(match.Groups[2].Value), 0, 0));
            var clayRobot = new Robot(RobotType.Clay, new RobotCost(int.Parse(match.Groups[3].Value), 0, 0));
            var obsidianRobot = new Robot(RobotType.Obsidian, new RobotCost(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), 0));
            var geodeRobot = new Robot(RobotType.Ore, new RobotCost(int.Parse(match.Groups[6].Value), 0, int.Parse(match.Groups[7].Value)));
            return new BlueprintInfo(num, oreRobot, clayRobot, obsidianRobot, geodeRobot);
        }
    }
}