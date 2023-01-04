using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day22 : AocDay<MapWithPassword>
    {
        public Day22(IInputParser<MapWithPassword> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(MapWithPassword input)
        {
            var start = FindStart(input.Map);
            var (pos, facing) = Move(input.Map, start, Facing.Up, new TurnInfo(TurnType.Right, input.InitialMove));
            foreach (var m in input.Moves)
                (pos, facing) = Move(input.Map, pos, facing, m);
            var pass = 1000 * (pos.Item1 + 1) + 4 * (pos.Item2 + 1) + (int)facing;
            Console.WriteLine(pass);
        }

        protected override void Part2(MapWithPassword input)
        {
            var wallMap = BuildCube(input.Map);
            var start = FindStart(input.Map);
            var (pos, facing) = Move2(input.Map, start, Facing.Up, new TurnInfo(TurnType.Right, input.InitialMove), wallMap);
            foreach (var m in input.Moves)
                (pos, facing) = Move2(input.Map, pos, facing, m, wallMap);
            var pass = 1000 * (pos.Item1 + 1) + 4 * (pos.Item2 + 1) + (int)facing;
            Console.WriteLine(pass);
        }

        private static (int, int) FindStart(char[,] map)
        {
            int x = 0;
            for (int i = 0; i < map.GetLength(1); i++)
                if (map[0, i] == '.')
                {
                    x = i;
                    break;
                }
            return (0, x);
        }

        public enum Facing
        {
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3
        }

        private static Facing Turn(Facing facing, TurnType turn)
        {
            return facing switch
            {
                Facing.Left => turn == TurnType.Left ? Facing.Down : Facing.Up,
                Facing.Right => turn == TurnType.Left ? Facing.Up : Facing.Down,
                Facing.Up => turn == TurnType.Left ? Facing.Left : Facing.Right,
                Facing.Down => turn == TurnType.Left ? Facing.Right : Facing.Left,
                _ => throw new Exception("Invalid facing")
            };
        }

        private static (int, int) NextPosition((int, int) pos, Facing facing)
        {
            var (y, x) = pos;
            return facing switch
            {
                Facing.Right => (y, x + 1),
                Facing.Left => (y, x - 1),
                Facing.Up => (y - 1, x),
                Facing.Down => (y + 1, x),
                _ => throw new Exception("Invalid move")
            };
        }

        private static (int, int) CoerceIndexes((int, int) pos, char[,] map)
        {
            var (y, x) = pos;
            if (x < 0) x = map.GetLength(1) - 1;
            if (x >= map.GetLength(1)) x = 0;
            if (y < 0) y = map.GetLength(0) - 1;
            if (y >= map.GetLength(0)) y = 0;
            return (y, x);
        }

        private static (int, int) SingleMove((int, int) pos, Facing facing, char[,] map)
        {
            var (y, x) = CoerceIndexes(NextPosition(pos, facing), map);
            while (map[y, x] == ' ')
                (y, x) = CoerceIndexes(NextPosition((y, x), facing), map);
            if (map[y, x] == '#')
                return pos;
            return (y, x);
        }

        private static ((int, int), Facing) Move(char[,] map, (int, int) pos, Facing facing, TurnInfo turnInfo)
        {
            facing = Turn(facing, turnInfo.Turn);
            for (int step = 0; step < turnInfo.Move; step++)
                pos = SingleMove(pos, facing, map);
            return (pos, facing);
        }

        private static ((int, int), Facing) Move2(char[,] map, (int, int) pos, Facing facing, TurnInfo turnInfo, Dictionary<CubeWall, Dictionary<Facing, NextWall>> wallMap)
        {
            facing = Turn(facing, turnInfo.Turn);
            for (int step = 0; step < turnInfo.Move; step++)
                (pos, facing) = SingleMove2(pos, facing, map, wallMap);
            return (pos, facing);
        }

        private static ((int, int), Facing) SingleMove2((int, int) pos, Facing facing, char[,] map, Dictionary<CubeWall, Dictionary<Facing, NextWall>> wallMap)
        {
            var currentWall = wallMap.First(w => w.Key.Contains(pos)).Key;
            var nPos = NextPosition(pos, facing);
            Facing newFacing = facing;
            if (!currentWall.Contains(nPos))
            {
                var nextWall = wallMap[currentWall][facing];
                nPos = CoercePositionOnCube(nPos, facing, currentWall, nextWall.Wall, nextWall.FacingTo);
                currentWall = nextWall.Wall;
                newFacing = nextWall.FacingTo;
            }
            var (ny, nx) = nPos;
            if (map[ny, nx] == '#')
                return (pos, facing);
            return (nPos, newFacing);
        }

        private static (int,int) CoercePositionOnCube((int,int) pos, Facing currentFacing, CubeWall currentWall, CubeWall nextWall, Facing newFacing)
        {
            var p = GetRelativePositionOnEdge(pos, currentFacing, currentWall);
            return GetInitialPosition(nextWall, newFacing, p, IsToCoercePositionOnEdge(currentFacing, newFacing));
        }

        private static bool IsToCoercePositionOnEdge(Facing currentFacing, Facing newFacing)
        {
            return (currentFacing == Facing.Up && newFacing == Facing.Left) ||
                (currentFacing == Facing.Down && newFacing == Facing.Right) ||
                (currentFacing == Facing.Down && newFacing == Facing.Up) ||
                (currentFacing == Facing.Up && newFacing == Facing.Down) ||
                (currentFacing == Facing.Left && newFacing == Facing.Up) || 
                (currentFacing == Facing.Left && newFacing == Facing.Right) ||
                (currentFacing == Facing.Right && newFacing == Facing.Left) ||
                (currentFacing == Facing.Right && newFacing == Facing.Down);
        }

        private static int GetRelativePositionOnEdge((int,int) pos, Facing f, CubeWall wall)
        {
            var (y, x) = pos;
            return f switch
            {
                Facing.Up => x - wall.MinX,
                Facing.Down => x - wall.MinX, 
                Facing.Left => y - wall.MinY, 
                Facing.Right => y - wall.MinY, 
                _ => throw new InvalidOperationException(),
            };
        }

        private static (int, int) GetInitialPosition(CubeWall wall, Facing facing, int p, bool reverse)
        {
            return facing switch
            {
                Facing.Up => (wall.MaxY, Between(wall.MinX, wall.MaxX, p, reverse)),
                Facing.Down => (wall.MinY, Between(wall.MinX, wall.MaxX, p, reverse)),
                Facing.Left => (Between(wall.MinY, wall.MaxY, p, reverse), wall.MaxX),
                Facing.Right => (Between(wall.MinY, wall.MaxY, p, reverse), wall.MinX),
                _ => throw new InvalidOperationException(),
            };
        }

        private static int Between(int min, int max, int p, bool reverse)
        {
            if (reverse)
                return Math.Max(min, max - p);
            return Math.Min(min + p, max); 
        }

        private static Dictionary<CubeWall, Dictionary<Facing, NextWall>> BuildCube(char[,] map)
        {
            var list = new List<CubeWall>();
            int num = 1;
            var edgeLength = (int)Math.Sqrt(map.Cast<char>().Count(c => c != ' ') / 6);
            for (int i = 0; i < map.GetLength(0); i += edgeLength)
                for (int j = 0; j < map.GetLength(1); j += edgeLength)
                    if (map[i, j] != ' ')
                        list.Add(new CubeWall(num++, i / edgeLength, j / edgeLength, j, i, j + edgeLength - 1, i + edgeLength - 1));
            var dictionary = list.ToDictionary(l => (l.I, l.J), l => l);
            return CreateWallMap(dictionary);
        }

        private static Dictionary<CubeWall, Dictionary<Facing, NextWall>> CreateWallMap(Dictionary<(int, int), CubeWall> dictionary)
        {
            var wallMap = dictionary.ToDictionary(d => d.Value, d => new Dictionary<Facing, NextWall>());
            foreach (var ((i, j), wall) in dictionary) //Adding neighbours straight or diagonally connected
            {
                if (dictionary.TryGetValue((i - 1, j), out var up))
                    wallMap[wall].Add(Facing.Up, new NextWall(up, Facing.Up));
                if (dictionary.TryGetValue((i + 1, j), out var down))
                    wallMap[wall].Add(Facing.Down, new NextWall(down, Facing.Down));
                if (dictionary.TryGetValue((i, j - 1), out var left))
                    wallMap[wall].Add(Facing.Left, new NextWall(left, Facing.Left));
                if (dictionary.TryGetValue((i, j + 1), out var right))
                    wallMap[wall].Add(Facing.Right, new NextWall(right, Facing.Right));
                if (up != null && dictionary.TryGetValue((i - 1, j - 1), out var left2))
                    wallMap[wall].Add(Facing.Left, new NextWall(left2, Facing.Up));
                if (up != null && dictionary.TryGetValue((i - 1, j + 1), out var right2))
                    wallMap[wall].Add(Facing.Right, new NextWall(right2, Facing.Up));
                if (down != null && dictionary.TryGetValue((i + 1, j - 1), out var left3))
                    wallMap[wall].Add(Facing.Left, new NextWall(left3, Facing.Down));
                if (down != null && dictionary.TryGetValue((i + 1, j + 1), out var right3))
                    wallMap[wall].Add(Facing.Right, new NextWall(right3, Facing.Down));
                if (left != null && dictionary.TryGetValue((i - 1, j - 1), out var up2))
                    wallMap[wall].Add(Facing.Up, new NextWall(up2, Facing.Left));
                if (left != null && dictionary.TryGetValue((i + 1, j - 1), out var down2))
                    wallMap[wall].Add(Facing.Down, new NextWall(down2, Facing.Left));
                if (right != null && dictionary.TryGetValue((i - 1, j + 1), out var up3))
                    wallMap[wall].Add(Facing.Up, new NextWall(up3, Facing.Right));
                if (right != null && dictionary.TryGetValue((i + 1, j + 1), out var down3))
                    wallMap[wall].Add(Facing.Down, new NextWall(down3, Facing.Right));
            }
            var oppositeWalls = wallMap.ToDictionary(w => w.Key, w => FindOppositeWall(wallMap, w.Key)); //error
            List<(CubeWall, (Facing, NextWall))> toAdd = new();
            foreach (var wall in dictionary.Values)
                foreach (var (f, next) in wallMap[wall])
                {
                    var next1 = wallMap[wall][f];
                    if (wallMap[next1.Wall].TryGetValue(next1.FacingTo, out var next2) && wallMap[next2.Wall].TryGetValue(next2.FacingTo, out var next3) &&
                        !wallMap[next3.Wall].TryGetValue(next3.FacingTo, out var _))
                        toAdd.Add((wall, (OppositeFacing(f), new NextWall(next3.Wall, OppositeFacing(next3.FacingTo)))));
                }
            foreach (var l in toAdd) //adding neighbours by traversing 4 wall cycles
                wallMap[l.Item1].Add(l.Item2.Item1, l.Item2.Item2);
            var facingCandidates = FindPossibleFacingTo(wallMap).GroupBy(w => w.Item1).ToDictionary(v => v.Key, v => v.Select(x => x.Item2).ToList());
            while (wallMap.Any(v => v.Value.Count < 4)) //add missing things
            {
                foreach (var wall in dictionary.Values)
                {
                    var n = GetMissingNeighbours(wallMap, wall, oppositeWalls[wall]).ToList();
                    if (n.Count == 1)
                    {
                        var nn = n.First();
                        var f = FindFacing(wall, nn, facingCandidates[nn]);
                        var dir = Enum.GetValues<Facing>().First(d => !wallMap[wall].ContainsKey(d));
                        wallMap[wall].Add(dir, new NextWall(nn, f));
                        wallMap[nn].Add(OppositeFacing(f), new NextWall(wall, OppositeFacing(dir)));
                    }
                }
            }
            return wallMap;
        }

        private record class CubeWall(int Number, int I, int J, int MinX, int MinY, int MaxX, int MaxY)
        {
            public bool Contains((int, int) pos)
            {
                var (y, x) = pos;
                return MinX <= x && x <= MaxX && MinY <= y && y <= MaxY;
            }
        }

        private record struct NextWall(CubeWall Wall, Facing FacingTo);

        private static CubeWall FindOppositeWall(Dictionary<CubeWall, Dictionary<Facing, NextWall>> wallMap, CubeWall wall)
        {
            foreach (var (f, n) in wallMap[wall])
                if (wallMap[n.Wall].TryGetValue(n.FacingTo, out var w))
                    return w.Wall;
            throw new InvalidOperationException();
        }

        private static IEnumerable<(CubeWall, Facing)> FindPossibleFacingTo(Dictionary<CubeWall, Dictionary<Facing, NextWall>> wallMap)
        {
            foreach (var grouping in wallMap.SelectMany(kv => kv.Value.Values).GroupBy(v => v.Wall))
                foreach (var facing in Enum.GetValues<Facing>())
                    if (!grouping.Any(nw => nw.FacingTo == facing))
                        yield return (grouping.Key, facing);
        }

        private static IEnumerable<CubeWall> GetMissingNeighbours(Dictionary<CubeWall, Dictionary<Facing, NextWall>> wallMap, CubeWall wall, CubeWall opposite)
        {
            return wallMap.Keys.Where(w => w != wall && w != opposite && !wallMap[wall].Values.Any(a => a.Wall == w));
        }

        private static Facing FindFacing(CubeWall wall, CubeWall neighbour, List<Facing> facings)
        {
            if (facings.Count == 1)
                return facings[0];
            return facings.First(f => FacingInDirection(wall, f, neighbour));
        }

        private static bool FacingInDirection(CubeWall wall, Facing f, CubeWall neighbour)
        {
            var iDiff = wall.I - neighbour.I;
            var jDiff = wall.J - neighbour.J;
            return f switch
            {
                Facing.Down => iDiff < 0,
                Facing.Up => iDiff > 0,
                Facing.Left => jDiff < 0,
                Facing.Right => jDiff > 0,
                _ => throw new InvalidOperationException()
            };
        }

        private static Facing OppositeFacing(Facing f)
        {
            return (Facing)(((int)f + 2) % 4);
        }
    }
}