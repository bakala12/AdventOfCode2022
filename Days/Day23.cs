using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day23 : AocDay<char[,]>
    {
        public Day23(IInputParser<char[,]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(char[,] input)
        {
            var elves = new HashSet<(int,int)>();
            for (int y = 0; y < input.GetLength(0); y++)
                for (int x = 0; x < input.GetLength(1); x++)
                    if (input[y, x] == '#')
                        elves.Add((y, x));
            var next = new HashSet<(int,int)>();
            int firstConsideredDirection = 0;
            for (int i = 0; i < 10; i++)
            {
                Round(elves, next, firstConsideredDirection);
                var temp = elves;
                elves = next;
                next = temp;
                next.Clear();
                firstConsideredDirection++;
                firstConsideredDirection %= 4;
            }
            var minY = elves.Min(i => i.Item1);
            var maxY = elves.Max(i => i.Item1);
            var minX = elves.Min(i => i.Item2);
            var maxX = elves.Max(i => i.Item2);
            Console.WriteLine((maxX - minX + 1) * (maxY - minY + 1) - elves.Count);
        }

        protected override void Part2(char[,] input)
        {
            var elves = new HashSet<(int, int)>();
            for (int y = 0; y < input.GetLength(0); y++)
                for (int x = 0; x < input.GetLength(1); x++)
                    if (input[y, x] == '#')
                        elves.Add((y, x));
            var next = new HashSet<(int, int)>();
            int firstConsideredDirection = 0;
            int round = 1;
            while(true)
            {
                if(!Round(elves, next, firstConsideredDirection))
                    break;
                var temp = elves;
                elves = next;
                next = temp;
                next.Clear();
                firstConsideredDirection++;
                firstConsideredDirection %= 4;
                round++;
            }
            Console.WriteLine(round);
        }

        private static bool Round(HashSet<(int,int)> elves, HashSet<(int,int)> next, int firstConsideredDirection)
        {
            bool moves = false;
            foreach (var grouping in elves.Select(e => (e, NextPosition(e, elves, firstConsideredDirection))).GroupBy(e => e.Item2))
            {
                if (grouping.Count() > 1)
                    foreach (var newPos in grouping)
                        next.Add(newPos.Item1);
                else
                {
                    var move = grouping.Single();
                    next.Add(move.Item2);
                    moves|= move.Item2 != move.Item1;
                }
            }
            return moves;

        }

        private static (int,int) NextPosition((int,int) elf, HashSet<(int, int)> elves, int firstConsideredDirection)
        {
            var (y, x) = elf;
            var n = !elves.Contains((y - 1, x));
            var nw = !elves.Contains((y - 1, x - 1));
            var ne = !elves.Contains((y - 1, x + 1));
            var w = !elves.Contains((y, x - 1));
            var sw = !elves.Contains((y + 1, x - 1));
            var s = !elves.Contains((y + 1, x));
            var se = !elves.Contains((y + 1, x + 1));
            var e = !elves.Contains((y, x + 1));
            if (n && nw && ne && s && se && sw && w && e)
                return elf;
            var dirs = new (bool, (int, int))[]
            {
                (n && nw && ne, (y-1,x)),
                (s && se && sw, (y+1,x)),
                (w && nw && sw, (y, x-1)),
                (e && ne && se, (y, x+1))
            };
            int dir = firstConsideredDirection;
            for (int i = 0; i < 4; i++)
                if (dirs[(dir + i) % 4].Item1)
                    return dirs[(dir + i) % 4].Item2;
            return elf;
        }
    }
}