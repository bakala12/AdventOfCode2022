using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day9 : AocDay<RopeMove[]>
    {
        public Day9(IInputParser<RopeMove[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(RopeMove[] input)
        {
            var tailLocations = new HashSet<(int, int)>();
            var head = (0, 0);
            var tail = (0, 0);
            tailLocations.Add(tail);
            foreach (var m in UngroupMove(input))
            {
                (head, tail) = Move(head, tail, m);
                if (!tailLocations.Contains(tail))
                    tailLocations.Add(tail);
            }
            Console.WriteLine(tailLocations.Count);
        }

        protected override void Part2(RopeMove[] input)
        {
            var tailLocations = new HashSet<(int, int)>();
            var knots = new (int, int)[10];
            for (int i = 0; i < 10; i++)
                knots[i] = (0, 0);
            tailLocations.Add(knots.Last());

            foreach (var m in UngroupMove(input))
            {
                (knots[0], knots[1]) = Move(knots[0], knots[1], m);
                for (int i = 2; i < 10; i++)
                {
                    knots[i] = FollowTail(knots[i - 1], knots[i]);
                }
                if (!tailLocations.Contains(knots.Last()))
                    tailLocations.Add(knots.Last());
            }
            Console.WriteLine(tailLocations.Count);
        }

        private static IEnumerable<RopeMoveDirection> UngroupMove(RopeMove[] moves)
        {
            foreach (var m in moves)
                for (int i = 0; i < m.Count; i++)
                    yield return m.Direction;
        }

        private static ((int, int), (int, int)) Move((int, int) head, (int, int) tail, RopeMoveDirection move)
        {
            var (hx, hy) = head;
            head = move switch
            {
                RopeMoveDirection.Left => (hx - 1, hy),
                RopeMoveDirection.Right => (hx + 1, hy),
                RopeMoveDirection.Up => (hx, hy - 1),
                RopeMoveDirection.Down => (hx, hy + 1),
                _ => throw new ArgumentException()
            };
            return (head, FollowTail(head, tail));
        }

        private static (int, int) FollowTail((int, int) head, (int, int) tail)
        {
            if (AreNeighbours(head, tail))
                return tail;
            return GetNeighbours(head).First(hn => AreNeighbours(hn, tail));
        }

        private static IEnumerable<(int, int)> GetNeighbours((int, int) s)
        {
            var (x, y) = s;
            yield return (x - 1, y);
            yield return (x, y - 1);
            yield return (x, y + 1);
            yield return (x + 1, y);
            yield return (x - 1, y - 1);
            yield return (x - 1, y + 1);
            yield return (x + 1, y - 1);
            yield return (x + 1, y + 1);
        }

        private static bool AreNeighbours((int, int) s1, (int, int) s2)
        {
            var (x1, y1) = s1;
            var (x2, y2) = s2;
            return Math.Abs(y1 - y2) <= 1 && Math.Abs(x1 - x2) <= 1;
        }
    }
}