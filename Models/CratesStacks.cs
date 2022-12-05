namespace AdventOfCode2022.Models
{
    public readonly record struct CrateStack(char[] Items);

    public readonly record struct StackMove(int Quantity, int From, int To);

    public readonly record struct CratesStacks(CrateStack[] Stacks, StackMove[] Moves);
}