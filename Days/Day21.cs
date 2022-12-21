using AdventOfCode2022.Input;

namespace AdventOfCode2022.Days
{
    public class Day21 : AocDay<string[]>
    {
        public Day21(IInputParser<string[]> inputParser) : base(inputParser)
        {
        }

        protected override void Part1(string[] input)
        {
            var knownValues = new Dictionary<string, long>();
            while (!knownValues.ContainsKey("root"))
                Calculate(input, knownValues);
            Console.WriteLine(knownValues["root"]);
        }

        protected override void Part2(string[] input)
        {
            var knownValues = new Dictionary<string, long>();
            input = input.Where(l => !l.StartsWith("humn")).ToArray();
            while (!knownValues.ContainsKey("root"))
            {
                foreach(var line in input)
                {
                    var s = line.Split(": ");
                    var variable = s[0];
                    if (knownValues.ContainsKey(variable))
                        continue;
                    if (int.TryParse(s[1], out int val))
                        knownValues.Add(variable, val);
                    else
                    {
                        var r = s[1].Split();
                        if (variable == "root")
                        {
                            r[1] = "=";
                            if (knownValues.ContainsKey(r[0]))
                                knownValues.Add(r[2], knownValues[r[0]]);
                            if (knownValues.ContainsKey(r[2]))
                                knownValues.Add(r[0], knownValues[r[2]]);
                        }
                        if (knownValues.ContainsKey(r[0]) && knownValues.ContainsKey(r[2]))
                            knownValues.Add(variable, Operation(r[1][0])(knownValues[r[0]], knownValues[r[2]]));
                    }
                }
            }
            while (!knownValues.ContainsKey("humn"))
            {
                foreach (var line in input)
                {
                    var s = line.Split(": ");
                    var variable = s[0];
                    if (knownValues.ContainsKey(variable) && !int.TryParse(s[1], out _))
                    {
                        var r = s[1].Split();
                        if (knownValues.ContainsKey(r[0]) && knownValues.ContainsKey(r[2]))
                            continue;
                        if (knownValues.ContainsKey(r[0]) && !knownValues.ContainsKey(r[2]))
                            knownValues.Add(r[2], OperationInverse(r[1][0], true)(knownValues[variable], knownValues[r[0]]));
                        if (!knownValues.ContainsKey(r[0]) && knownValues.ContainsKey(r[2]))
                            knownValues.Add(r[0], OperationInverse(r[1][0], false)(knownValues[variable], knownValues[r[2]]));
                    }
                }
            }
            Console.WriteLine(knownValues["humn"]);
        }

        private static void Calculate(string[] input, Dictionary<string, long> knownVariables)
        {
            foreach(var line in input)
            {
                var s = line.Split(": ");
                var variable = s[0];
                if (knownVariables.ContainsKey(variable))
                    continue;
                if (int.TryParse(s[1], out int val))
                    knownVariables.Add(variable, val);
                else
                {
                    var r = s[1].Split();
                    if (knownVariables.ContainsKey(r[0]) && knownVariables.ContainsKey(r[2]))
                        knownVariables.Add(variable, Operation(r[1][0])(knownVariables[r[0]], knownVariables[r[2]]));
                }
            }
        }

        private static Func<long, long, long> Operation(char c)
        {
            return c switch
            {
                '+' => (a, b) => a + b,
                '-' => (a, b) => a - b,
                '*' => (a, b) => a * b,
                '/' => (a, b) => a / b,
                '=' => (a, b) => a == b ? 1L : 0L, 
                _ => throw new Exception()
            };
        }

        private static Func<long, long, long> OperationInverse(char c, bool firstKnown)
        {
            return c switch
            {
                '+' => (res, b) => res - b,
                '-' => (res, b) => firstKnown ? b - res : res + b,
                '*' => (res, b) => res / b,
                '/' => (res, b) => firstKnown ? b / res : res * b,
                _ => throw new Exception()
            };
        }
    }
}