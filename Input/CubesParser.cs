namespace AdventOfCode2022.Input
{
    public class CubesParser : IInputParser<(int, int, int)[]>
    {
        public (int, int, int)[] ParseInput(string input)
        {
            return input.Split(Environment.NewLine)
                .Select(s =>
                {
                    var x = s.Split(',').Select(int.Parse).ToArray();
                    return (x[0], x[1], x[2]);
                }).ToArray();
        }
    }
}