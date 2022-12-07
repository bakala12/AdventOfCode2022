using AdventOfCode2022.Input;
using Directory = AdventOfCode2022.Models.Directory;

namespace AdventOfCode2022.Days
{
    public class Day7 : AocDay<Directory>
    {
        public Day7(IInputParser<Directory> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(Directory input)
        {
            var sizes = new Dictionary<string, long>();
            CalculateSizes(input, sizes);
            Console.WriteLine(sizes.Where(s => s.Value <= 100000).Sum(s => s.Value));
        }

        protected override void Part2(Directory input)
        {
            var sizes = new Dictionary<string, long>();
            CalculateSizes(input, sizes);
            var total = sizes["/"];
            var unusedNow = 70000000 - total;
            Console.WriteLine(sizes.Values.Where(s => unusedNow + s >= 30000000).Min());
        }

        private static long CalculateSizes(Directory dir, Dictionary<string, long> sizes)
        {
            long size = dir.Files.Sum(f => f.Size) + dir.Directories.Sum(d => CalculateSizes(d, sizes));
            sizes.Add(dir.Path, size);
            return size;
        }
    }
}