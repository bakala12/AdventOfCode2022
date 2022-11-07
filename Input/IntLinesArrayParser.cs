namespace AdventOfCode2022.Input
{
    public class IntLinesArrayParser : ArrayInputParser<int>
    {
        public IntLinesArrayParser() : base(int.Parse)
        {
        }
    }
}