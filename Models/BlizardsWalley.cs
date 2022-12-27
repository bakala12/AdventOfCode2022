namespace AdventOfCode2022.Models
{
    public enum BlizzardDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public readonly record struct Blizzard((int,int) Position, BlizzardDirection Direction);

    public readonly record struct BlizzardsValley(int Width, int Height, (int,int) Start, (int,int) Finish, Blizzard[] Blizzards);
}