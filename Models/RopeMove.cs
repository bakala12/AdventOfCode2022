namespace AdventOfCode2022.Models
{
    public enum RopeMoveDirection
    {
        Left, Right, Up, Down
    }

    public record struct RopeMove(RopeMoveDirection Direction, int Count);
}