using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day20 : AocDay<long[]>
    {
        public Day20(IInputParser<long[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(long[] input)
        {
            Console.WriteLine(DecryptCoordinates(input.ToList()));
        }

        protected override void Part2(long[] input)
        {
            Console.WriteLine(DecryptCoordinates(input.Select(i => i * 811589153).ToList(), 10));
        }

        private long DecryptCoordinates(List<long> numbers, int times = 1)
        {
            List<int> indexes = new();
            for (int i = 0; i < numbers.Count; i++) 
                indexes.Add(i);
            int len = numbers.Count - 1;

            for (int i = 0; i < times; i++)
            {
                for (int j = 0; j < numbers.Count; j++)
                {
                    int from = indexes.IndexOf(j);
                    indexes.RemoveAt(from);
                    int num = (int)((from + numbers[j]) % len);
                    int to = num < 0 ? num + len : num;
                    indexes.Insert(to, j);
                }
            }
            int position = indexes.IndexOf(numbers.IndexOf(0)) + 1000;
            return numbers[indexes[position % numbers.Count]] +
                numbers[indexes[(position + 1000) % numbers.Count]] +
                numbers[indexes[(position + 2000) % numbers.Count]];
        }
    }
}