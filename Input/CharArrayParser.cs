namespace AdventOfCode2022.Input
{
    public class CharArrayParser : IInputParser<char[,]>
    {
        public char[,] ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var array = new char[lines.Length, lines[0].Length];
            for(int i = 0; i < lines.Length; i++)
                for(int j = 0; j < lines[0].Length; j++)
                    array[i,j] = lines[i][j];
            return array;
        }
    }
}