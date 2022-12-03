using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day3 : AocDay<string[]>
    {
        public Day3(IInputParser<string[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(string[] input)
        {
            int priorities = 0;
            foreach (var line in input)
            {
                bool toBreak = false;
                for (int i = 0; i <= line.Length / 2 && !toBreak; i++) 
                    for (int j = line.Length - 1; j >= line.Length / 2; j--)
                        if (line[i] == line[j])
                        {
                            priorities += (char.IsUpper(line[i]) ? 27 + line[i] - 'A' : line[i] - 'a' + 1);
                            toBreak = true;
                            break;
                        }
            }
            Console.WriteLine(priorities);
        }

        protected override void Part2(string[] input)
        {
            int priorities = 0;
            foreach (var lines in input.Chunk(3))
            {
                foreach (var c in lines[0])
                    if (lines[1].Contains(c) && lines[2].Contains(c))
                    {
                        priorities += (char.IsUpper(c) ? 27 + c - 'A' : c - 'a' + 1);
                        break;
                    }
            }
            Console.WriteLine(priorities);
        }
    }
}