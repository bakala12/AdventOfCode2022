namespace AdventOfCode2022.Models
{
    public record class MonkeyItems(long[] ItemsWorryLevels, Func<long,long> Operation, int TestDivisibleBy, int ThrowIfTrueMonkey, int ThrowIfFalsoMonkey);
}