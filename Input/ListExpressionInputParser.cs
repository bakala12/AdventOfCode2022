using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class ListExpressionInputParser : IInputParser<(IExpression, IExpression)[]>
    {
        public (IExpression, IExpression)[] ParseInput(string input)
        {
            var list = new List<(IExpression, IExpression)>();
            var lines = input.Split(Environment.NewLine);
            for(int i = 0; i < lines.Length; i += 3)
            {
                var first = ParseExpression(lines[i]);
                var second = ParseExpression(lines[i + 1]);
                list.Add((first, second));
            }
            return list.ToArray();
        }

        private static IExpression ParseExpression(string line)
        {
            int pos = 0;
            return ParseExpression(line, ref pos);
        }

        private static ListExpression ParseList(string line, ref int pos)
        {
            var args = new List<IExpression>();
            while (true)
            {
                if (line[pos] == ']')
                {
                    pos++;
                    return new ListExpression(args.ToArray());
                }
                var arg = ParseExpression(line, ref pos);
                args.Add(arg);
                if (line[pos] == ',')
                    pos++;
            }
        }

        private static IExpression ParseExpression(string line, ref int pos)
        {
            var numbers = line.Skip(pos).TakeWhile(char.IsDigit).ToArray();
            if(numbers.Length > 0)
            {
                var val = int.Parse(new string(numbers));
                var ex = new IntegerExpression(val);
                pos += numbers.Length;
                return ex;
            }
            pos++;
            return ParseList(line, ref pos);
        }
    }
}