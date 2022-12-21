namespace AdventOfCode2022.Input
{
    public class LongLinesArrayParser : ArrayInputParser<long>
    {
        public LongLinesArrayParser() : base(long.Parse)
        {
        }
    }
}