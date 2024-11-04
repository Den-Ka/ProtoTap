namespace Etern0nety.Clicker.Leaderboard
{
    public struct LeaderboardEntry
    {
        public readonly string Nickname;
        public readonly long Score;
        public readonly int Rank;
        public readonly Tier Tier;

        public LeaderboardEntry(string nickname, long score, int rank, Tier tier)
        {
            Nickname = nickname;
            Score = score;
            Rank = rank;
            Tier = tier;
        }
    }
}