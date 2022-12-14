using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day14 : AocDay<(int, int)[]>
    {
        public Day14(IInputParser<(int, int)[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1((int, int)[] input)
        {
            var arr = BuildArray(input);
            (int, int)? s;
            int i = 0;
            while((s = MoveSand(arr)) != null)
            {
                var (x, y) = s.Value;
                arr[y, x] = true;
                i++;
            }
            Console.WriteLine(i);
        }

        protected override void Part2((int, int)[] input)
        {
            var arr = BuildArrayWithFloor(input);
            (int, int)? s;
            int i = 0;
            while ((s = MoveSand(arr)) != null)
            {
                var (x, y) = s.Value;
                if (arr[y, x])
                    break;
                arr[y, x] = true;
                i++;
            }
            Console.WriteLine(i);
        }

        private static bool[,] BuildArray((int, int)[] input)
        {
            var arr = new bool[input.Max(x => x.Item2)+1, 1000];
            foreach(var (x,y) in input)
                arr[y,x] = true;
            return arr;
        }

        private static bool[,] BuildArrayWithFloor((int, int)[] input)
        {
            var arr = new bool[input.Max(x => x.Item2) + 3, 1000];
            foreach (var (x, y) in input)
                arr[y, x] = true;
            for (int i = 0; i < 1000; i++)
                arr[arr.GetLength(0) - 1, i] = true;
            return arr;
        }

        private static (int,int)? MoveSand(bool[,] arr)
        {
            var s = (500, 0);
            while (s.Item2 < arr.GetLength(0)-1)
            {
                var (x, y) = s;
                if (!arr[y + 1, x])
                    s = (x, y + 1);
                else if (!arr[y + 1, x - 1])
                    s = (x - 1, y + 1);
                else if (!arr[y + 1, x + 1])
                    s = (x + 1, y + 1);
                else 
                    return s;
            }
            return null;
        }
    }
}