using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day8 : AocDay<int[,]>
    {
        public Day8(IInputParser<int[,]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(int[,] input)
        {
            bool[,] visible = new bool[input.GetLength(0), input.GetLength(1)];
            for(int i = 0; i < input.GetLength(0); i++)
            {
                int h = -1;
                for(int j = 0; j < input.GetLength(1); j++)
                {
                    if (input[i,j] > h)
                    {
                        visible[i, j] = true;
                        h = input[i,j];
                    }
                }
                h = -1;
                for(int j = input.GetLength(1)-1; j >= 0; j--)
                {
                    if (input[i, j] > h)
                    {
                        visible[i, j] = true;
                        h = input[i, j];
                    }
                }
            }
            for (int j = 0; j < input.GetLength(1); j++)
            {
                int h = -1;
                for(int i = 0; i < input.GetLength(0); i++)
                {
                    if (input[i, j] > h)
                    {
                        visible[i, j] = true;
                        h = input[i, j];
                    }
                }
                h = -1;
                for(int i = input.GetLength(0) - 1; i>=0; i--)
                {
                    if (input[i, j] > h)
                    {
                        visible[i, j] = true;
                        h = input[i, j];
                    }
                }
            }
            int c = 0;
            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    if (visible[i, j])
                        c++;
            Console.WriteLine(c);
        }

        protected override void Part2(int[,] input)
        {
            int c = -1;
            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    c = Math.Max(c, CountTrees(input, i, j));
            Console.WriteLine(c);
        }

        private static int CountTrees(int[,] input, int i, int j)
        {
            int a = 0, b = 0, c = 0, d = 0;
            for (int ii = i-1; ii >= 0; ii--)
            {
                a++;
                if (input[ii, j] >= input[i,j])
                    break;
            }
            for (int ii = i + 1; ii < input.GetLength(0); ii++)
            {
                b++;
                if (input[ii, j] >= input[i, j])
                    break;
            }
            for(int jj = j - 1; jj >= 0; jj--)
            {
                c++;
                if (input[i, jj] >= input[i, j])
                    break;
            }
            for (int jj = j + 1; jj < input.GetLength(1); jj++)
            {
                d++;
                if (input[i, jj] >= input[i, j])
                    break;
            }
            return a * b * c * d;
        }
    }
}