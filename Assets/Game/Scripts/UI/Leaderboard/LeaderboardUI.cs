using System;
using System.Collections.Generic;
using Etern0nety.Clicker.Leaderboard;
using Etern0nety.DI;
using TMPro;
using UnityEngine;

namespace Etern0nety.Clicker.UI
{
    public class LeaderboardUI : MonoBehaviour
    {
        public event Action Opened;
        
        [SerializeField] private LeaderboardEntryUI _entryPrefab;
        [Space]
        [SerializeField] private GameObject _view;
        [SerializeField] private RectTransform _content;
        [SerializeField] private TextMeshProUGUI _message;

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
            
            ShowMessage("Loading...");
            
            gameObject.SetActive(true);
            Opened?.Invoke();
        }
        private void Close()
        {
            gameObject.SetActive(false);
        }

        public void ShowLeaderboard(LeaderboardResult leaderboard)
        {
            if(!IsOpen) return;
            
            _message.gameObject.SetActive(false);
            _view.SetActive(true);
            
            var entriesCount = leaderboard.Entries.Length;

            for (int i = 0; i < entriesCount; i++)
            {
                var entryObject = Instantiate(_entryPrefab, _content);
                
                var nickname = leaderboard.Entries[i].Nickname;
                var score = leaderboard.Entries[i].Score;
                var rank = leaderboard.Entries[i].Rank;
                var tier = leaderboard.Entries[i].Tier;
                
                var isOwner = leaderboard.PlayerEntryIndex == i;
                
                entryObject.Initialize(rank, nickname, score, tier, isOwner);
            }

            if (entriesCount == 0)
            {
                ShowMessage("No entries.");
            }
        }
        
        private void ClearEntries()
        {
            for (int i = 0; i < _content.childCount; i++)
            {
                Destroy(_content.GetChild(i).gameObject);
            }
        }

        public void ShowMessage(string message)
        {
            _view.SetActive(false);
            _message.text = message;
            _message.gameObject.SetActive(true);
        }
    }
}