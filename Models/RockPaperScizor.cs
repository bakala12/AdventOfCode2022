namespace AdventOfCode2022.Models
{
    public record class RockPaperScizor(int Opponent, int Player)
    {
        public int RoundScore()
        {
            return 3 * ((Player - Opponent + 4) % 3) + Player + 1;
        }

        public int RoundScore2()
        {
            int own = (Opponent + Player + 2) % 3;
            return new RockPaperScizor(Opponent, own).RoundScore();
        }
    }
}