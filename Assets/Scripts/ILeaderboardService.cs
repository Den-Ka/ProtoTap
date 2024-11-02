using System.Threading.Tasks;

namespace Etern0nety.Clicker
{
    public interface ILeaderboardService
    {
        Task UpdatePlayerScore(long playerScore);
        Task<LeaderboardResult> LoadScores();
    }

    public struct LeaderboardResult
    {
        public readonly LeaderboardEntry[] Entries;
        public readonly string ErrorMessage;
        
        public readonly int PlayerEntryIndex;
        
        public LeaderboardResult(LeaderboardEntry[] entries, int playerEntryIndex)
        {
            Entries = entries;
            PlayerEntryIndex = playerEntryIndex;
            ErrorMessage = null;
        }

        public LeaderboardResult(string errorMessage)
        {
            Entries = null;
            PlayerEntryIndex = -1;
            ErrorMessage = errorMessage;
        }
    }

    public struct LeaderboardEntry
    {
        public readonly string Nickname;
        public readonly long Score;
        public readonly int Rank;
        public readonly string Tier;

        public LeaderboardEntry(string nickname, long score, int rank, string tier)
        {
            Nickname = nickname;
            Score = score;
            Rank = rank;
            Tier = tier;
        }
    }
}