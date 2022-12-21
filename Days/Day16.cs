using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day16 : AocDay<Valve[]>
    {
        public Day16(IInputParser<Valve[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(Valve[] input)
        {
            var dictionary = input.ToDictionary(i => i.Name, i => i);
            var shortest = ShortestPathsFull(dictionary);
            var reduced = shortest.Where(k => IsRelevant(dictionary, "AA", k.Key)).GroupBy(k => k.Key.Item1)
                .ToDictionary(k => k.Key, k => k.Select(x => (x.Key.Item2, x.Value)).ToArray());
            Console.WriteLine(FindBest(dictionary, reduced, "AA", 30, 0, new List<string>()));
        }

        protected override void Part2(Valve[] input)
        {
            var dictionary = input.ToDictionary(i => i.Name, i => i);
            var shortest = ShortestPathsFull(dictionary);
            var reduced = shortest.Where(k => IsRelevant(dictionary, "AA", k.Key)).GroupBy(k => k.Key.Item1)
                .ToDictionary(k => k.Key, k => k.ToDictionary(e => e.Key.Item2, e => e.Value));
            var notOpened = input.Where(v => v.Rate > 0).Select(x => x.Name).ToList();
            Console.WriteLine(FindBestWithElephant(dictionary, reduced, ("AA", "AA"), (26, 26), 0, notOpened));
        }

        private static Dictionary<(string, string), int> ShortestPathsFull(Dictionary<string, Valve> valves)
        {
            var paths = new Dictionary<(string, string), int>();
            foreach (var v in valves.Keys)
                foreach (var v2 in valves.Keys)
                    paths[(v, v2)] = v == v2 ? 0 : 1000000;
            foreach (var v in valves.Values)
                foreach (var v2 in v.TunnelsTo)
                    paths[(v.Name, v2)] = 1;
            foreach (var through in valves.Keys)
                foreach (var from in valves.Keys)
                    foreach (var to in valves.Keys)
                        if (paths[(from, through)] + paths[(through, to)] < paths[(from, to)])
                            paths[(from, to)] = paths[(from, through)] + paths[(through, to)];
            return paths;
        }

        private static bool IsRelevant(Dictionary<string, Valve> valves, string start, (string, string) key)
        {
            return (key.Item1 == start || valves[key.Item1].Rate > 0) &&
                (key.Item2 == start || valves[key.Item2].Rate > 0) && key.Item1 != key.Item2;
        }

        private static int FindBest(Dictionary<string, Valve> valves, Dictionary<string, (string, int)[]> graph,
            string currentValve, int time, int currentPressure, List<string> opened)
        {
            if (opened.Count + 1 == graph.Count)
                return currentPressure;
            if (time <= 0)
                return currentPressure;
            int best = currentPressure;
            foreach (var (achievable, u) in graph[currentValve])
            {
                if (opened.Contains(achievable))
                    continue;
                int t = time - u - 1;
                if (t > 0)
                {
                    int pressure = currentPressure + valves[achievable].Rate * t;
                    opened.Add(achievable);
                    best = Math.Max(best, FindBest(valves, graph, achievable, t, pressure, opened));
                    opened.Remove(achievable);
                }
            }
            return best;
        }

        private static int FindBestWithElephant(Dictionary<string, Valve> valves, Dictionary<string, Dictionary<string, int>> graph,
            (string, string) currentValve, (int, int) time, int currentPressure, List<string> notopened)
        {
            if (notopened.Count == 0)
                return currentPressure;
            if (time.Item1 <= 0 && time.Item2 <= 0)
                return currentPressure;
            var (me, elephant) = currentValve;
            var (myTime, elephantTime) = time;
            var candidates = notopened.Select(n => GetRouteDetails(currentValve, n, time, graph));
            var myCandidates = candidates.Where(rd => rd.RemainingMyTime > 0 && rd.RemainingMyTime >= rd.RemainingElephantTime).ToArray();
            var elephantCandidates = candidates.Where(rd => rd.RemainingElephantTime > 0 && rd.RemainingElephantTime >= rd.RemainingMyTime).ToArray();

            int best = currentPressure;
            if (myCandidates.Length == 0 && elephantCandidates.Length > 0)
            {
                foreach (var ec in elephantCandidates)
                {
                    notopened.Remove(ec.Valve);
                    int pressure = currentPressure + valves[ec.Valve].Rate * ec.RemainingElephantTime;
                    best = Math.Max(best, FindBestWithElephant(valves, graph, (currentValve.Item1, ec.Valve), (time.Item1, ec.RemainingElephantTime), pressure, notopened));
                    notopened.Add(ec.Valve);
                }
            }
            else if (elephantCandidates.Length == 0 && myCandidates.Length > 0)
            {
                foreach (var mc in myCandidates)
                {
                    notopened.Remove(mc.Valve);
                    int pressure = currentPressure + valves[mc.Valve].Rate * mc.RemainingMyTime;
                    best = Math.Max(best, FindBestWithElephant(valves, graph, (mc.Valve, currentValve.Item2), (mc.RemainingMyTime, time.Item2), pressure, notopened));
                    notopened.Add(mc.Valve);
                }
            }
            else
            {
                foreach (var mc in myCandidates)
                    foreach (var ec in elephantCandidates)
                        if (mc.Valve != ec.Valve)
                        {
                            notopened.Remove(mc.Valve);
                            notopened.Remove(ec.Valve);
                            int pressure = currentPressure + valves[mc.Valve].Rate * mc.RemainingMyTime + valves[ec.Valve].Rate * ec.RemainingElephantTime;
                            best = Math.Max(best, FindBestWithElephant(valves, graph, (mc.Valve, ec.Valve), (mc.RemainingMyTime, ec.RemainingElephantTime), pressure, notopened));
                            notopened.Add(mc.Valve);
                            notopened.Add(ec.Valve);
                        }
            }
            return best;
        }

        private static (string Valve, int RemainingMyTime, int RemainingElephantTime) GetRouteDetails((string, string) location, string valve,
            (int, int) time, Dictionary<string, Dictionary<string, int>> graph)
        {
            var myTime = time.Item1 - graph[location.Item1][valve] - 1;
            var elephantTime = time.Item2 - graph[location.Item2][valve] - 1;
            return (valve, myTime, elephantTime);
        }
    }
}