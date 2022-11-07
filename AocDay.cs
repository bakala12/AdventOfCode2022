using AdventOfCode2022.Input;

namespace AdventOfCode2022
{
    public abstract class AocDay<TParsedInput> : IDay
    {
        private IInputParser<TParsedInput> _inputParser;

        protected AocDay(IInputParser<TParsedInput> inputParser)
        {
            _inputParser = inputParser;
        }

        protected virtual string InputFilePath => $"InputFiles/{GetType().Name}.txt";

        protected abstract void Part1(TParsedInput input);
        protected abstract void Part2(TParsedInput input);

        public void Solve(string[] args)
        {
            var fileContent = File.ReadAllText(args.FirstOrDefault() ?? InputFilePath);
            var input = _inputParser.ParseInput(fileContent);
            Part1(input);
            Part2(input);
        }
    }
}