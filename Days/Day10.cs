using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day10 : AocDay<string[]>
    {
        public Day10(IInputParser<string[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(string[] input)
        {
            int x = 1;
            int cycle = 1;
            var signal = 0;
            foreach(var instr in input)
            {
                if (cycle > 220)
                    break;
                if (cycle % 40 == 20)
                    signal += cycle * x;
                if (instr == "noop")
                    cycle++;
                else
                {
                    var s = int.Parse(instr.Substring(5));
                    cycle++;
                    if (cycle % 40 == 20)
                        signal += cycle * x;
                    x += s;
                    cycle++;
                }
            }
            Console.WriteLine(signal);
        }

        protected override void Part2(string[] input)
        {
            int x = 1;
            int cycle = 1;
            var screen = new bool[6, 40];
            foreach (var instr in input)
            {
                if (cycle > 240)
                    break;
                Draw(screen, cycle, x);
                if (instr == "noop")
                    cycle++;
                else
                {
                    var s = int.Parse(instr.Substring(5));
                    cycle++;
                    Draw(screen, cycle, x);
                    x += s;
                    cycle++;
                }
            }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                    Console.Write(screen[i, j] ? "#" : ".");
                Console.WriteLine();
            }
        }

        private static void Draw(bool[,] screen, int cycle, int x)
        {
            var row = (cycle - 1) / 40;
            var column = (cycle - 1) % 40;
            screen[row, column] = Math.Abs(x - column) <= 1;
        }
    }
}