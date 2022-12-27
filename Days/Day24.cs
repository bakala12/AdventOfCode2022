using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day24 : AocDay<BlizzardsValley>
    {
        public Day24(IInputParser<BlizzardsValley> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(BlizzardsValley input)
        {
            var blizzardsOverTime = new Dictionary<int, (int, int)[]>();
            var blizardsOverTimePositionsSet = new Dictionary<int, HashSet<(int, int)>>();
            blizzardsOverTime.Add(0, input.Blizzards.Select(b => b.Position).ToArray());
            var set = new HashSet<(int, int)>(input.Blizzards.Select(b => b.Position).Distinct());
            blizardsOverTimePositionsSet.Add(0, set);
            var queue = new Queue<((int, int), int)>();
            var seen = new HashSet<((int, int), int)>();
            queue.Enqueue((input.Start, 0));
            seen.Add((input.Start, 0));
            while (queue.Count > 0)
            {
                var (pos, time) = queue.Dequeue();
                if(pos == input.Finish)
                {
                    Console.WriteLine(time);
                    return;
                }
                var nextBlizzards = GetBlizzardPositionsByTime(time + 1, blizzardsOverTime, blizardsOverTimePositionsSet, input.Blizzards, input.Width, input.Height);
                foreach(var move in GetLegalMoves(pos, input.Start, input.Finish, input.Width, input.Height).Where(n => !nextBlizzards.Contains(n)))
                {
                    if (!seen.Contains((move, time+1)))
                    {
                        queue.Enqueue((move, time+1));
                        seen.Add((move, time+1));
                    }
                }
            }
        }

        protected override void Part2(BlizzardsValley input)
        {
            var blizzardsOverTime = new Dictionary<int, (int, int)[]>();
            var blizardsOverTimePositionsSet = new Dictionary<int, HashSet<(int, int)>>();
            blizzardsOverTime.Add(0, input.Blizzards.Select(b => b.Position).ToArray());
            var set = new HashSet<(int, int)>(input.Blizzards.Select(b => b.Position).Distinct());
            blizardsOverTimePositionsSet.Add(0, set);
            var queue = new Queue<((int, int), int)>();
            var seen = new HashSet<((int, int), int)>();
            queue.Enqueue((input.Start, 0));
            seen.Add((input.Start, 0));
            var minTime = 0;
            while (queue.Count > 0)
            {
                var (pos, time) = queue.Dequeue();
                if (pos == input.Finish)
                {
                    minTime = time;
                    break;
                }
                var nextBlizzards = GetBlizzardPositionsByTime(time + 1, blizzardsOverTime, blizardsOverTimePositionsSet, input.Blizzards, input.Width, input.Height);
                foreach (var move in GetLegalMoves(pos, input.Start, input.Finish, input.Width, input.Height).Where(n => !nextBlizzards.Contains(n)))
                {
                    if (!seen.Contains((move, time + 1)))
                    {
                        queue.Enqueue((move, time + 1));
                        seen.Add((move, time + 1));
                    }
                }
            }
            queue.Clear();
            seen.Clear();
            queue.Enqueue((input.Finish, minTime));
            seen.Add((input.Finish, minTime));
            while (queue.Count > 0)
            {
                var (pos, time) = queue.Dequeue();
                if (pos == input.Start)
                {
                    minTime = time;
                    break;
                }
                var nextBlizzards = GetBlizzardPositionsByTime(time + 1, blizzardsOverTime, blizardsOverTimePositionsSet, input.Blizzards, input.Width, input.Height);
                foreach (var move in GetLegalMoves(pos, input.Start, input.Finish, input.Width, input.Height).Where(n => !nextBlizzards.Contains(n)))
                {
                    if (!seen.Contains((move, time + 1)))
                    {
                        queue.Enqueue((move, time + 1));
                        seen.Add((move, time + 1));
                    }
                }
            }
            queue.Clear();
            seen.Clear();
            queue.Enqueue((input.Start, minTime));
            seen.Add((input.Start, minTime));
            while (queue.Count > 0)
            {
                var (pos, time) = queue.Dequeue();
                if (pos == input.Finish)
                {
                    minTime = time;
                    break;
                }
                var nextBlizzards = GetBlizzardPositionsByTime(time + 1, blizzardsOverTime, blizardsOverTimePositionsSet, input.Blizzards, input.Width, input.Height);
                foreach (var move in GetLegalMoves(pos, input.Start, input.Finish, input.Width, input.Height).Where(n => !nextBlizzards.Contains(n)))
                {
                    if (!seen.Contains((move, time + 1)))
                    {
                        queue.Enqueue((move, time + 1));
                        seen.Add((move, time + 1));
                    }
                }
            }
            Console.WriteLine(minTime);
        }

        private static IEnumerable<(int,int)> GetLegalMoves((int, int) position, (int,int) start, (int,int) finish, int width, int height)
        {
            foreach(var newPos in GetAvailableMoveCandidates(position))
            {
                if (newPos == start || newPos == finish)
                    yield return newPos;
                var (y, x) = newPos;
                if (y < 1 || x < 1 || y >= height - 1 || x >= width - 1)
                    continue;
                yield return newPos;
            }
        }

        private static IEnumerable<(int,int)> GetAvailableMoveCandidates((int,int) position)
        {
            var (y, x) = position;
            yield return ((y - 1, x));
            yield return ((y + 1, x));
            yield return ((y, x - 1));
            yield return ((y, x + 1));
            yield return (position);
        }

        private static HashSet<(int,int)> GetBlizzardPositionsByTime(int moment, Dictionary<int, (int, int)[]> blizzardsOverTime, 
            Dictionary<int, HashSet<(int, int)>> blizzardsOverTimeSet,
            Blizzard[] blizzards, int valleyWidth, int valleyHeight)
        {
            if (blizzardsOverTimeSet.TryGetValue(moment, out var value))
                return value;
            var prev = blizzardsOverTime[moment - 1];
            var nextPos = prev.Select((b, ind) => MoveBlizzard(b, blizzards[ind].Direction, valleyWidth, valleyHeight)).ToArray();
            var nextSet = new HashSet<(int, int)>();
            foreach (var item in nextPos)
                if (!nextSet.Contains(item))
                    nextSet.Add(item);
            blizzardsOverTime.Add(moment, nextPos);
            blizzardsOverTimeSet.Add(moment, nextSet);
            return nextSet;
        }

        private static (int,int) MoveBlizzard((int,int) position, BlizzardDirection direction, int valleyWidth, int valleyHeight)
        {
            var (y, x) = position;
            var (ny,nx) = direction switch
            {
                BlizzardDirection.Up => (y-1,x),
                BlizzardDirection.Down => (y+1,x),
                BlizzardDirection.Left => (y, x - 1),
                BlizzardDirection.Right => (y, x+1),
                _ => throw new Exception()
            };
            if (ny >= valleyHeight - 1)
                ny = 1;
            if(ny <= 0)
                ny = valleyHeight - 2;
            if (nx >= valleyWidth - 1)
                nx = 1;
            if(nx <= 0)
                nx = valleyWidth - 2;
            return (ny,nx);
        }
    }
}