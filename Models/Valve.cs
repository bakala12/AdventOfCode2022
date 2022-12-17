namespace AdventOfCode2022.Models
{
    public readonly record struct Valve(string Name, int Rate, string[] TunnelsTo);
}