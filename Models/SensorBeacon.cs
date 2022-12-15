namespace AdventOfCode2022.Models
{
    public readonly record struct Position(int X, int Y);

    public readonly record struct SensorBeacon(Position Sensor, Position Beacon);
}