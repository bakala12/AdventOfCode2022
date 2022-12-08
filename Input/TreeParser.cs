namespace AdventOfCode2022.Input
{
    public class TreeParser : IInputParser<int[,]>
    {
        public int[,] ParseInput(string input)
        {
            var s = input.Split(Environment.NewLine);
            var a = new int[s.Length, s[0].Length];
            for (int i = 0; i < s.Length; i++)
                for (int j = 0; j < s[0].Length; j++)
                    a[i, j] = s[i][j] - '0';
            return a;
        }
    }
}