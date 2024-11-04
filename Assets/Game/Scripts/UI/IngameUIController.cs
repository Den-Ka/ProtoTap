using Etern0nety.Clicker.Leaderboard;
using Etern0nety.Clicker.UI;
using Etern0nety.DI;
using UnityEngine;

namespace Etern0nety.Clicker
{
    public class IngameUIController : MonoBehaviour
    {
        [SerializeField] private IngameUI _ui;
        
        private Player _player;
        private TapBuster _buster;

        private ILeaderboardService _leaderboard;
        private IAdvertisementService _advertisement;

        private bool _advertisementIsLoaded = false;
        private bool _busterIsActive = false;
        
        public void Initialize(DIContainer container)
        {
            _player = container.Resolve<Player>();
            _buster = container.Resolve<TapBuster>();
            
            _leaderboard = container.Resolve<ILeaderboardService>();
            _advertisement = container.Resolve<IAdvertisementService>();

            //==========================================================//
            
            _ui.TapButton.OnClick.AddListener(OnTapButton);
            
            _player.ScoreChanged += _ui.SetScore;
            _ui.SetScore(_player.Score);
            
            _buster.Finished += OnBusterFinished;
            _buster.ScoreAdded += OnBusterScoreAdded;

            _ui.LeaderboardUI.Opened += OnLeaderboardOpened;
            
            _ui.AdvertisementButton.interactable = false;
            _ui.AdvertisementButton.onClick.AddListener(ShowAdvertisement);
            
            _advertisement.AdvertisementLoaded += OnAdvertisementLoaded;
            _advertisement.AdvertisementRewarded += OnAdvertisementRewarded;
            
            _advertisement.LoadAdvertisement();

        }

        private async void OnLeaderboardOpened()
        {
            await _leaderboard.UpdatePlayerScore(_player.Score);
            var result = await _leaderboard.LoadScores();

            if (string.IsNullOrEmpty(result.ErrorMessage))
            {
                _ui.LeaderboardUI.ShowLeaderboard(result);
            }
            else
            {
                _ui.LeaderboardUI.ShowMessage(result.ErrorMessage);
            }
        }

        private void OnTapButton()
        {
            var scoreToAdd = _player.ScoreForTap;
            var tapPosition = Input.mousePosition;
            
            _player.AddScore(scoreToAdd);

            _ui.DisplayTap();
            _ui.DisplayAddedScore(scoreToAdd, tapPosition);
        }

        private void OnBusterScoreAdded(int score)
        {
            var tapPosition = _ui.GetRandomTapButtonPoint();
            _ui.DisplayTap();
            _ui.DisplayAddedScore(score, tapPosition);
        }


        private void OnAdvertisementLoaded()
        {
            _advertisementIsLoaded = true;
            ValidateAdvertisementButton();
        }
        private void OnAdvertisementRewarded()
        {
            _buster.Launch();
            _ui.StartNewTimer(_buster);
            _busterIsActive = true;
        }

        private void ShowAdvertisement()
        {
            _advertisement.ShowAdvertisement();
            _advertisementIsLoaded = false;
            _ui.AdvertisementButton.interactable = false;
            
            _advertisement.LoadAdvertisement(); //Prepare next advertisement
        }
        
        private void OnBusterFinished()
        {
            _busterIsActive = false;
            ValidateAdvertisementButton();
        }

        private void ValidateAdvertisementButton()
        {
            _ui.AdvertisementButton.interactable = _advertisementIsLoaded && !_busterIsActive;
        }
    }
}