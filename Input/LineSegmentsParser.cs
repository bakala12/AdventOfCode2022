namespace AdventOfCode2022.Input
{
    public class LineSegmentsParser : IInputParser<(int, int)[]>
    {
        public (int, int)[] ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            List<(int, int)> coords = new();
            foreach(var l in lines)
            {
                var seg = l.Split(" -> ").Select(a => (int.Parse(a.Split(",")[0]), int.Parse(a.Split(",")[1]))).ToArray();
                for(int i = 1; i < seg.Length; i++)
                {
                    if (seg[i-1].Item1 == seg[i].Item1)
                    {
                        int x = seg[i - 1].Item1;
                        for (int y = Math.Min(seg[i - 1].Item2, seg[i].Item2); y <= Math.Max(seg[i - 1].Item2, seg[i].Item2); y++)
                            coords.Add((x, y));
                    }
                    else
                    {
                        int y = seg[i - 1].Item2;
                        for (int x = Math.Min(seg[i - 1].Item1, seg[i].Item1); x <= Math.Max(seg[i - 1].Item1, seg[i].Item1); x++)
                            coords.Add((x, y));
                    }
                }
            }
            return coords.Distinct().ToArray();
        }
    }
}