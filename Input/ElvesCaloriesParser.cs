using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class ElvesCaloriesParser : IInputParser<ElvesCalories[]>
    {
        public ElvesCalories[] ParseInput(string input)
        {
            return input.Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(i => i.Split(Environment.NewLine).Select(int.Parse).ToArray())
                .Select(s => new ElvesCalories(s)).ToArray();
        }
    }
}