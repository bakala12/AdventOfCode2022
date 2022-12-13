using AdventOfCode2022.Input;
using AdventOfCode2022.Models;

namespace AdventOfCode2022.Days
{
    public class Day13 : AocDay<(IExpression, IExpression)[]>
    {
        public Day13(IInputParser<(IExpression, IExpression)[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1((IExpression, IExpression)[] input)
        {
            int sum = 0;
            for(int i = 0; i < input.Length; i++)
            {
                if (input[i].Item1.GoesBefore(input[i].Item2) >= 0)
                    sum += i + 1;
            }
            Console.WriteLine(sum);
        }

        protected override void Part2((IExpression, IExpression)[] input)
        {
            var list = input.SelectMany(x => new IExpression[] { x.Item1, x.Item2 }).ToList();
            var first = new ListExpression(new ListExpression(new IntegerExpression(2)));
            var second = new ListExpression(new ListExpression(new IntegerExpression(6)));
            list.Add(first);
            list.Add(second);
            list.Sort((e1, e2) => e2.GoesBefore(e1));
            var fi = list.IndexOf(first) + 1;
            var si = list.IndexOf(second) + 1;
            Console.WriteLine(fi * si);
        }
    }
}