namespace AdventOfCode2022.Input
{
    public abstract class ArrayInputParser<TArrayItem> : IInputParser<TArrayItem[]>
    {
        private readonly Func<string, TArrayItem> _parserDelegate;
        private readonly string _delimeter;

        protected ArrayInputParser(Func<string, TArrayItem> parserDelegate, string? delimeter = null)
        {
            _parserDelegate = parserDelegate;
            _delimeter = delimeter ?? Environment.NewLine;
        }

        public TArrayItem[] ParseInput(string input)
        {
            return input.Split(_delimeter).Select(_parserDelegate).ToArray();
        }
    }
}