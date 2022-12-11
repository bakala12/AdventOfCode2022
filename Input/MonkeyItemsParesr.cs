using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    internal class MonkeyItemsParesr : IInputParser<MonkeyItems[]>
    {
        public MonkeyItems[] ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var monkeys = new List<MonkeyItems>();
            int i = 0;
            while(i < lines.Length)
            {
                var items = lines[i + 1].Substring(17).Split(",").Select(long.Parse).ToArray();
                Func<long, long> operation = ParseOperation(lines[i + 2].Substring(20));
                var test = int.Parse(lines[i+3].Split().Last());
                var trueThrow = int.Parse(lines[i+4].Split().Last());
                var falseThrow = int.Parse(lines[i+5].Split().Last());
                monkeys.Add(new MonkeyItems(items, operation, test, trueThrow, falseThrow));
                i += 7;
            }
            return monkeys.ToArray();
        }

        private static Func<long,long> ParseOperation(string line)
        {
            var s = line.Split();
            if (long.TryParse(s[2], out long op))
            {
                if (s[1] == "+")
                    return x => x + op;
                else
                    return x => x * op;
            }
            else
            {
                if (s[1] == "+")
                    return x => x + x;
                else
                    return x => x * x;
            }
        }
    }
}