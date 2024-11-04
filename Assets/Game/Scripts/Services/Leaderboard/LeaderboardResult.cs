namespace Etern0nety.Clicker.Leaderboard
{
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
}