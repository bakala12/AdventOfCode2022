using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day12 : AocDay<char[,]>
    {
        public Day12(IInputParser<char[,]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(char[,] input)
        {
            Console.WriteLine(FindPath(input, 'S'));
        }

        protected override void Part2(char[,] input)
        {
            Console.WriteLine(FindPath(input, 'a'));
        }

        private static int FindPath(char[,] input, char start)
        {
            var end = FindPosition(input, 'E');
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<((int, int), int)>();
            queue.Enqueue((end, 0));
            visited.Add(end);
            var minDist = int.MaxValue;
            var next = new Dictionary<(int, int), (int, int)>();
            while (queue.Count > 0)
            {
                var (pos, dis) = queue.Dequeue();
                var (r, c) = pos;
                if (input[r, c] == start)
                {
                    minDist = Math.Min(minDist, dis);
                    continue;
                }
                foreach (var from in GetNeighboirs(input, pos))
                {
                    var h = GetHeight(input, pos);
                    var fh = GetHeight(input, from);
                    if (!visited.Contains(from) && h - fh <= 1)
                    {

                        next.Add(from, pos);
                        queue.Enqueue((from, dis + 1));
                        visited.Add(from);
                    }
                }
            }
            return minDist;
        }

        private static (int, int) FindPosition(char[,] array, char c)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    if (array[i, j] == c)
                        return (i, j);
            return (-1, -1);
        }

        private static IEnumerable<(int, int)> GetNeighboirs(char[,] array, (int, int) pos)
        {
            var (r, c) = pos;
            if (r > 0)
                yield return (r - 1, c);
            if (c > 0)
                yield return (r, c - 1);
            if (r < array.GetLength(0) - 1)
                yield return (r + 1, c);

            if (c < array.GetLength(1) - 1)
                yield return (r, c + 1);
        }

        private static int GetHeight(char[,] input, (int, int) pos) =>
            input[pos.Item1, pos.Item2] switch
            {
                'S' => 0,
                'E' => 'z' - 'a',
                _ => input[pos.Item1, pos.Item2] - 'a'
            };
    }
}