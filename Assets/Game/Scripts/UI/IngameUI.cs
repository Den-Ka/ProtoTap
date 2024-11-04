using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Etern0nety.Clicker.UI
{
    public class IngameUI : MonoBehaviour
    {
        [SerializeField] private TapButton _tapButton;
        [SerializeField] private Counter _score;
        [SerializeField] private AddedScoreDisplay _addedScoreDisplay;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private LeaderboardUI _leaderboard;
        [SerializeField] private Button _advertisementButton;
        [SerializeField] private TimerIcon _timerIcon;

        public TapButton TapButton => _tapButton;
        public LeaderboardUI LeaderboardUI => _leaderboard;
        public Button AdvertisementButton => _advertisementButton;


        public void Awake()
        {
            _leaderboardButton.onClick.AddListener(_leaderboard.Toggle);
        }

        private void OnDestroy()
        {
            _leaderboardButton.onClick.RemoveListener(_leaderboard.Toggle);
        }

        public void SetScore(BigInteger score) => _score.Value = score;

        public void DisplayAddedScore(int score, Vector3 screenPosition) =>
            _addedScoreDisplay.DisplayAddedScore(score, screenPosition);

        public void DisplayTap() => _tapButton.Animate();

        public void StartNewTimer(IProgression progress) => _timerIcon.Initialize(progress);

        public Vector3 GetRandomTapButtonPoint()
        {
            var buttonTransform = _tapButton.transform as RectTransform;
            var buttonRect = buttonTransform.rect;

            var randomX = Random.Range(buttonRect.xMin, buttonRect.xMax);
            var randomY = Random.Range(buttonRect.yMin, buttonRect.yMax);

            return new Vector3(randomX, randomY) + buttonTransform.position;
        }
    }
}