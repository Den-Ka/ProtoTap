using System.Collections;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace Etern0nety.Clicker.UI
{
    public class LeaderboadUI : MonoBehaviour
    {
        [SerializeField] private LeaderboardEntryUI _entryPrefab;
        [Space]
        [SerializeField] private RectTransform _content;
        [Space]
        [SerializeField] private Player _player;

        private Dictionary<string, LeaderboardEntryUI.Tier> _tiers = new()
        {
            { "Wood", LeaderboardEntryUI.Tier.Wood },
            { "Copper", LeaderboardEntryUI.Tier.Copper },
            { "Iron", LeaderboardEntryUI.Tier.Iron },
            { "Gold", LeaderboardEntryUI.Tier.Gold },
            { "Ruby", LeaderboardEntryUI.Tier.Ruby },
            { "Diamond", LeaderboardEntryUI.Tier.Diamond },
        };
        
        public bool IsOpen => gameObject.activeSelf;

        public void Toggle()
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
        private void Open()
        {
            ClearEntries();
            
            gameObject.SetActive(true);
            
            LoadLeaderboard();
        }
        private void Close()
        {
            gameObject.SetActive(false);
        }

        private async void LoadLeaderboard()
        {
            if (Leaderboard.Initialized)
            {
                await Leaderboard.UpdateScore(_player.Score);
                var leaderboard = await Leaderboard.GetLeaderboard();
                FillLeaderboard(leaderboard);
            }
            else
            {
                 Debug.Log("Leaderboard not initialized");
            }
        }

        private void FillLeaderboard(LeaderboardScores leaderboard)
        {
            foreach (var entry in leaderboard.Results)
            {
                var entryObject = Instantiate(_entryPrefab, _content);

                var rank = entry.Rank + 1;
                var isOwner = entry.PlayerId.Equals(Leaderboard.PlayerID);
                
                entryObject.Initialize(rank, entry.PlayerName, (long)entry.Score, _tiers[entry.Tier], isOwner);
            }
        }

        private void ClearEntries()
        {
            for (int i = 0; i < _content.childCount; i++)
            {
                Destroy(_content.GetChild(i).gameObject);
            }
        }
    }
}