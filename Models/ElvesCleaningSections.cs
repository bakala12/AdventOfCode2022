namespace AdventOfCode2022.Models
{
    public readonly record struct Range(int From, int To);

    public readonly record struct ElvesCleaningSections(Range Elf1, Range Elf2);
}