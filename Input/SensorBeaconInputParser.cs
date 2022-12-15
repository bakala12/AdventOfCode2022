using AdventOfCode2022.Models;

namespace AdventOfCode2022.Input
{
    public class SensorBeaconInputParser : IInputParser<SensorBeacon[]>
    {
        public SensorBeacon[] ParseInput(string input)
        {
            return input.Split(Environment.NewLine).Select(Parse).ToArray();
        }

        private static SensorBeacon Parse(string line)
        {
            var s = line.Split(new char[] { ' ', ',', '=', ':' }, StringSplitOptions.RemoveEmptyEntries);
            return new SensorBeacon(new Position(int.Parse(s[3]), int.Parse(s[5])), new Position(int.Parse(s[11]), int.Parse(s[13])));
        }
    }
}