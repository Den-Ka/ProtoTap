using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEditor;
using UnityEngine;

namespace Etern0nety.Clicker
{
    public class UnityLeaderboard : ILeaderboardService
    {
        private const string _leaderboardID = "Test_Clicker_Leaderboard";
        
        public static bool Initialized;
        public static string PlayerID;

        public UnityLeaderboard()
        {
            Initialize();
        }

        public async void Initialize()
        {
            if(Initialized) return;
            
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            Debug.Log("Authentication service initialized.");
            
            var auth = AuthenticationService.Instance;
            PlayerID = auth.PlayerId;
            
            Initialized = true;
        }

        public async Task UpdatePlayerScore(long playerScore)
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardID, playerScore);
        }

        public async Task<LeaderboardResult> LoadScores()
        {
            try
            {
                var leaderboard = await LeaderboardsService.Instance.GetPlayerRangeAsync(_leaderboardID);

                var resultsCount = leaderboard.Results.Count;

                var entries = new LeaderboardEntry[resultsCount];
                var playerEntryIndex = -1;
                
                for (int i = 0; i < resultsCount; i++)
                {
                    var result = leaderboard.Results[i];
                    entries[i] = new LeaderboardEntry(result.PlayerName, (long)result.Score, result.Rank + 1, result.Tier);
                    if (result.PlayerId == PlayerID) playerEntryIndex = i;
                }
                
                return new LeaderboardResult(entries, playerEntryIndex);
            }
            catch(Exception e)
            {
                return new LeaderboardResult(e.Message);
            }
        }
    }
}