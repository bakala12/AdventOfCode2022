using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day15 : AocDay<SensorBeacon[]>
    {
        public Day15(IInputParser<SensorBeacon[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(SensorBeacon[] input)
        {
            int y = 2000000;
            var ranges = input.Select(sb => RangeAt(sb.Sensor, Radius(sb), y))
                .Where(r => r != null)
                .Select(r => r.Value)
                .OrderBy(r => r.From)
                .ToArray();
            int from = ranges[0].From;
            int to = ranges[0].To;
            int c = ranges[0].Length;
            for(int i = 1; i < ranges.Length; i++) 
            {
                if (ranges[i].From > to)
                {
                    from = ranges[i].From;
                    to = ranges[i].To;
                    c += ranges[i].Length;
                }
                else
                {
                    if(to < ranges[i].To)
                    {
                        c += ranges[i].To - to;
                        to = ranges[i].To;
                    }
                }
            }
            Console.WriteLine(c - input.Where(sb => sb.Beacon.Y == y).Select(sb => sb.Beacon.X).Distinct().Count());
        }

        protected override void Part2(SensorBeacon[] input)
        {
            var (xFrom, xTo, yFrom, yTo) = (0,4000000, 0, 4000000);
            for(int y = yFrom; y <= yTo; y++)
            {
                var x = FindInRow(input, y, xFrom, xTo);
                if(x != null)
                {
                    Console.WriteLine(4000000*(long)x+y);
                    return;
                }
            }
        }

        private static int? FindInRow(SensorBeacon[] input, int y, int xMin, int xMax)
        {
            var ranges = input.Select(sb => RangeAt(sb.Sensor, Radius(sb), y))
                .Where(r => r != null)
                .Select(r => r.Value)
                .Where(r => r.To >= xMin)
                .OrderBy(r => r.From).ThenByDescending(r => r.Length)
                .ToArray();
            int from = xMin;
            int to = ranges[0].To;
            for (int i = 1; i < ranges.Length; i++)
            {
                if (to >= xMax)
                    return null;
                if (ranges[i].From > to+1)
                {
                    return to + 1;
                }
                to = Math.Max(to, ranges[i].To);
            }
            return null;
        }

        private static int Radius(SensorBeacon s)
        {
            return Manhattan(s.Sensor, s.Beacon);
        }

        private static int Manhattan(Position p, Position q)
        {
            return Math.Abs(p.X - q.X) + Math.Abs(p.Y - q.Y);
        }

        private readonly record struct Range(int From, int To)
        {
            public int Length => To - From + 1;
        }

        private static Range? RangeAt(Position middle, int radius, int y)
        {
            int o = Manhattan(middle, new Position(middle.X, y));
            if (Manhattan(middle, new Position(middle.X, y)) > radius)
                return null;
            int d = radius - o;
            return new Range(middle.X - d, middle.X + d);
        }

        private static Range? Intercept(Range a, Range b)
        {
            if (a.From > b.To || a.To < b.From)
                return null;
            return new Range(Math.Max(a.From, b.From), Math.Min(a.To, b.To));
        }
    }
}