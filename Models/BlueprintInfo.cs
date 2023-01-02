namespace AdventOfCode2022.Models
{
    public enum RobotType
    {
        Ore,
        Clay,
        Obsidian,
        Geode
    }

    public readonly record struct RobotCost(int Ore, int Clay, int Obsidian);

    public readonly record struct Robot(RobotType Type, RobotCost Cost);
    
    public readonly record struct BlueprintInfo(int Number, Robot OreRobot, Robot ClayRobot, Robot ObsidianRobot, Robot GeodeRobot);
}