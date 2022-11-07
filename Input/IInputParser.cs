namespace AdventOfCode2022.Input
{
    public interface IInputParser<TParsedInput>
    {
        TParsedInput ParseInput(string input);
    }
}