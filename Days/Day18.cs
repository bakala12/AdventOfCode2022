using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day18 : AocDay<(int, int, int)[]>
    {
        public Day18(IInputParser<(int, int, int)[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1((int, int, int)[] input)
        {
            var connected = 0;
            for(int i = 0; i < input.Length; i++)
            {
                connected += GetNeighbours(input[i]).Count(c => input.Contains(c));
            }
            Console.WriteLine(6 * input.Length - connected);
        }

        protected override void Part2((int,int,int)[] input)
        {
            var minX = input.Min(c => c.Item1) - 2;
            var minY = input.Min(c => c.Item2) - 2;
            var minZ = input.Min(c => c.Item3) - 2;
            var maxX = input.Max(c => c.Item1) + 2;
            var maxY = input.Max(c => c.Item2) + 2;
            var maxZ = input.Max(c => c.Item3) + 2;
            var queue = new Queue<((int, int, int), ReachedFrom)>();
            var reached = input.ToDictionary(i => i, i => ReachedFrom.None);
            var seen = new HashSet<((int, int, int), ReachedFrom)>();
            var min = (minX, minY, minZ);
            queue.Enqueue((min, ReachedFrom.None));
            seen.Add((min, ReachedFrom.Up));
            seen.Add((min, ReachedFrom.Down));
            seen.Add((min, ReachedFrom.Left));
            seen.Add((min, ReachedFrom.Right));
            seen.Add((min, ReachedFrom.Front));
            seen.Add((min, ReachedFrom.Behind));
            while (queue.Count > 0)
            {
                var (c, from) = queue.Dequeue();
                if (input.Contains(c))
                    reached[c] |= from;
                else 
                {
                    foreach (var (next, nextFrom) in GetNeighboursWithReachedFrom(c))
                    {
                        var (x, y, z) = next;
                        if(x >= minX && x <= maxX && y >= minY && y <= maxY && z >= minZ && z <= maxZ)
                        {
                            if(!seen.Contains((next, nextFrom)))
                            {
                                seen.Add((next, nextFrom));
                                queue.Enqueue((next, nextFrom));
                            }
                        }
                    }
                }
            }
            var count = reached.Values.Sum(CountFlags);
            Console.WriteLine(count);
        }

        private static IEnumerable<(int,int,int)> GetNeighbours((int,int, int) c)
        {
            var (x, y, z) = c;
            yield return (x - 1, y, z);
            yield return (x + 1, y, z);
            yield return (x, y - 1, z);
            yield return (x, y + 1, z);
            yield return (x, y, z - 1);
            yield return (x, y, z + 1);
        }

        private static IEnumerable<((int, int, int), ReachedFrom)> GetNeighboursWithReachedFrom((int, int, int) c)
        {
            var (x, y, z) = c;
            yield return ((x - 1, y, z), ReachedFrom.Right);
            yield return ((x + 1, y, z), ReachedFrom.Left);
            yield return ((x, y - 1, z), ReachedFrom.Down);
            yield return ((x, y + 1, z), ReachedFrom.Up);
            yield return ((x, y, z - 1), ReachedFrom.Behind);
            yield return ((x, y, z + 1), ReachedFrom.Front);
        }

        [Flags]
        private enum ReachedFrom
        {
            None = 0,
            Up = 1, 
            Down = 2, 
            Left = 4, 
            Right = 8, 
            Front = 16, 
            Behind = 32,
            All = 63
        }

        private static int CountFlags(ReachedFrom e)
        {
            int s = 0;
            if (e.HasFlag(ReachedFrom.Left)) s++;
            if (e.HasFlag(ReachedFrom.Right)) s++;
            if (e.HasFlag(ReachedFrom.Up)) s++;
            if (e.HasFlag(ReachedFrom.Down)) s++;
            if (e.HasFlag(ReachedFrom.Front)) s++;
            if (e.HasFlag(ReachedFrom.Behind)) s++;
            return s;
        }
    }
}