using System.Numerics;
using System.Threading.Tasks;

namespace Etern0nety.Clicker.Leaderboard
{
    public interface ILeaderboardService
    {
        Task UpdatePlayerScore(BigInteger playerScore);
        Task<LeaderboardResult> LoadScores();
    }
}