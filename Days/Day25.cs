using AdventOfCode2022.Input;
using System.Text;

namespace AdventOfCode2022.Days
{
    public class Day25 : AocDay<string[]>
    {
        public Day25(IInputParser<string[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(string[] input)
        {
            var sum = input.Select(ToDecimal).Sum();
            Console.WriteLine(FromDecimal(sum));
        }

        protected override void Part2(string[] input)
        {
            //Console.WriteLine("AOC 2022 done");
        }

        public static long ToDecimal(string number)
        {
            long w = Digit(number[0]);
            for(int i = 1; i < number.Length; i++)
            {
                w *= 5;
                w += Digit(number[i]);
            }
            return w;
        }

        public static string FromDecimal(long number)
        {
            var s = new Stack<char>();
            while(number != 0)
            {
                var d = number % 5;
                if (d > 2)
                    d -= 5;
                s.Push(FromDigit(d));
                number = number - d;
                number /= 5;
            }
            return new string(s.ToArray());
        }

        public static long Digit(char c)
        {
            return c switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2,
                _ => throw new NotImplementedException()
            };
        }

        public static char FromDigit(long d)
        {
            return d switch
            {
                -2 => '=',
                -1 => '-',
                0 => '0',
                1 => '1',
                2 => '2',
                _ => throw new NotImplementedException()
            };
        }
    }
}