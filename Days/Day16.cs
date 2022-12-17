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
                int t = time - u;
                if (t > 0)
                {
                    t--;
                    int pressure = currentPressure + valves[achievable].Rate * t;
                    opened.Add(achievable);
                    best = Math.Max(best, FindBest(valves, graph, achievable, t, pressure, opened));
                    opened.Remove(achievable);
                }

            }
            return best;
        }
    }
}