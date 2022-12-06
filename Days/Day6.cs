using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day6 : AocDay<string>
    {
        public Day6(IInputParser<string> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(string input)
        {
            for(int i = 3; i < input.Length; i++)
            {
                if(AreDifferent(input, i, 4))
                {
                    Console.WriteLine(i+1);
                    break;
                }
            }
        }

        protected override void Part2(string input)
        {
            for (int i = 13; i < input.Length; i++)
            {
                if (AreDifferent(input, i, 14))
                {
                    Console.WriteLine(i + 1);
                    break;
                }
            }
        }

        private static bool AreDifferent(string input, int position, int count)
        {
            for(int i = position - count + 1; i < position; i++)
                for(int j = position; j > i; j--)
                    if (input[i] == input[j])
                        return false;
            return true;
        }
    }
}