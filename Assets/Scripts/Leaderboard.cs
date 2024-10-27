using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEditor;
using UnityEngine;

namespace Etern0nety.Clicker
{
    public static class Leaderboard
    {
        private const string _leaderboardID = "Test_Clicker_Leaderboard";
        
        public static bool Initialized;
        public static string PlayerID;

        [RuntimeInitializeOnLoadMethod]
        public static async void Initialize()
        {
            if(Initialized) return;
            
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            Debug.Log("Authentication service initialized.");
            
            var auth = AuthenticationService.Instance;
            PlayerID = auth.PlayerId;
            
            Initialized = true;
        }

        public static async Task UpdateScore(long score)
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardID, score);
        }

        public static async Task<LeaderboardScores> GetLeaderboard()
        {
            return await LeaderboardsService.Instance.GetPlayerRangeAsync(_leaderboardID);
        }
    }
}