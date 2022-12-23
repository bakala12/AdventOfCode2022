namespace AdventOfCode2022.Models
{
    public enum TurnType
    {
        Left, Right
    }

    public readonly record struct TurnInfo(TurnType Turn, int Move);

    public readonly record struct MapWithPassword(char[,] Map, int InitialMove, TurnInfo[] Moves);
}