using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day19 : AocDay<BlueprintInfo[]>
    {
        public Day19(IInputParser<BlueprintInfo[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(BlueprintInfo[] input)
        {
            Console.WriteLine(input.Sum(i => i.Number * TestBlueprint(i)));
        }

        protected override void Part2(BlueprintInfo[] input)
        {
            Console.WriteLine(input.Take(3).Aggregate(1, (a, i) => a * TestBlueprint(i, 32)));
        }

        private static int TestBlueprint(BlueprintInfo info, int time = 24)
        {
            int best = 0;
            var limits = GetRobotCountLimits(info);
            var transpositions = new Dictionary<(int, Resources, (int, int, int, int)), int>();
            var res = TestBlueprint(info, time, (1, 0, 0, 0), new Resources(), limits, ref best, transpositions);
            return res;
        }

        private static int TestBlueprint(BlueprintInfo info, int remainingTime, (int, int, int, int) robotCounts, Resources resources, (int,int,int) robotCountLimits, ref int bestEver,
            Dictionary<(int, Resources, (int, int, int, int)), int> transpositions)
        {
            if (remainingTime == 1) //shortcut - reduce recursive calls
            {
                if (bestEver < resources.Geode + robotCounts.Item4)
                    bestEver = resources.Geode + robotCounts.Item4;
                return resources.Geode + robotCounts.Item4;
            }
            //optimization: If limit on robot counts is reached, then each next minute you can create new geode robot - so you can count all geodes easily
            if (robotCounts.Item1 == robotCountLimits.Item1 && robotCounts.Item2 == robotCountLimits.Item2 && robotCounts.Item3 == robotCountLimits.Item3)
                return resources.Geode + robotCounts.Item4 * remainingTime + (remainingTime - 1) * (remainingTime) / 2;
            //optimization: If upper bound on geode is worse than what has been achieved at some point - do not continue, solution will be not as good
            if (bestEver >= resources.Geode * remainingTime + robotCounts.Item4 + (remainingTime - 1) * (remainingTime) / 2)
                return 0;
            //optimization: If transposition is already stored - return it to save time
            if (transpositions.TryGetValue((remainingTime, resources, robotCounts), out int result))
                return result;
            var collected = Collect(robotCounts);
            int best = int.MinValue;
            var (oe, ce, obe, ge) = robotCounts;
            foreach (var (o, c, ob, g, res) in BuildNewRobots(info, resources, remainingTime, robotCounts, robotCountLimits))
                best = Math.Max(best, TestBlueprint(info, remainingTime - 1, (o + oe, c + ce, ob + obe, g + ge), res + collected, robotCountLimits, ref bestEver, transpositions));
            transpositions.Add((remainingTime, resources, robotCounts), best);
            return best;
        }

        private static Resources Collect((int, int, int, int) robotCounts)
        {
            return new Resources(robotCounts.Item1, robotCounts.Item2, robotCounts.Item3, robotCounts.Item4);
        }

        private static IEnumerable<(int, int, int, int, Resources)> BuildNewRobots(BlueprintInfo info, Resources res, int time, (int,int,int,int) robotCounts, (int,int,int) robotCountLimits)
        {
            Resources remaining;
            if (res.TryBuildRobot(info.GeodeRobot.Cost, out remaining))
                yield return (0, 0, 0, 1, remaining);
            if (time > 2) //optimization: Last build robot can be only geode robot if it will increase result.
            {
                //optimization: Do not even consider building robots over the limit.
                if (robotCounts.Item3 < robotCountLimits.Item3 && res.TryBuildRobot(info.ObsidianRobot.Cost, out remaining))
                    yield return (0, 0, 1, 0, remaining);
                if (robotCounts.Item2 < robotCountLimits.Item2 && res.TryBuildRobot(info.ClayRobot.Cost, out remaining))
                    yield return (0, 1, 0, 0, remaining);
                if (robotCounts.Item1 < robotCountLimits.Item1 && res.TryBuildRobot(info.OreRobot.Cost, out remaining))
                    yield return (1, 0, 0, 0, remaining);
            }
            yield return (0, 0, 0, 0, res);
        }

        private static (int,int,int) GetRobotCountLimits(BlueprintInfo info)
        {
            var ore = Math.Max(Math.Max(info.OreRobot.Cost.Ore, info.ClayRobot.Cost.Ore), Math.Max(info.ObsidianRobot.Cost.Ore, info.GeodeRobot.Cost.Ore));
            var clay = Math.Max(Math.Max(info.OreRobot.Cost.Clay, info.ClayRobot.Cost.Clay), Math.Max(info.ObsidianRobot.Cost.Clay, info.GeodeRobot.Cost.Clay));
            var obsidian = Math.Max(Math.Max(info.OreRobot.Cost.Obsidian, info.ClayRobot.Cost.Obsidian), Math.Max(info.ObsidianRobot.Cost.Obsidian, info.GeodeRobot.Cost.Obsidian));
            return (ore, clay, obsidian);
        }

        private readonly record struct Resources(int Ore, int Clay, int Obsidian, int Geode)
        {
            public static Resources operator +(Resources r1, Resources r2)
            {
                return new Resources(r1.Ore + r2.Ore, r1.Clay + r2.Clay, r1.Obsidian + r2.Obsidian, r1.Geode + r2.Geode);
            }

            public bool TryBuildRobot(RobotCost cost, out Resources remainingResources)
            {
                remainingResources = new Resources(Ore - cost.Ore, Clay - cost.Clay, Obsidian - cost.Obsidian, Geode);
                return remainingResources.Ore >= 0 && remainingResources.Clay >= 0 && remainingResources.Obsidian >= 0;
            }
        }
    }
}