using System.Collections;
using Etern0nety.DI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Etern0nety.Clicker.UI
{
    public class IngameUI : MonoBehaviour
    {
        [Header("Tap Button")]
        [SerializeField] private Button _tapButton;
        [Space]
        [SerializeField] private float _buttonAnimationTime = 0.1f;
        [SerializeField] private AnimationCurve _buttonAnimationCurve;
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Color _scoreTextColor = Color.white;
        [Space]
        [SerializeField] private TextMeshProUGUI _addedScorePrefab;
        [SerializeField] private RectTransform _addedScoreContainer;
        [Space]
        [SerializeField] private float _addedScoreAnimationTime = 1.5f;
        [SerializeField] private Vector2 _addedScoreAnimationVector = Vector2.up;
        [SerializeField] private float _addedScoreRandomRadius = 20f;
        [Header("Leaderboard")]
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private LeaderboardUI _leaderboard;
        [FormerlySerializedAs("_accelerationTimerIcon")]
        [Header("Tap Acceleration")]
        [SerializeField] private Image _timerIcon;
        [Header("Advertisement")]
        [SerializeField] private Button _advertisementButton;
        
        public Button AdvertisementButton => _advertisementButton;
        public Button TapButton => _tapButton;
        public LeaderboardUI LeaderboardUI => _leaderboard;

        public void Awake()
        {
            _leaderboardButton.onClick.AddListener(_leaderboard.Toggle);
        }

        private void OnDestroy()
        {
            _leaderboardButton.onClick.RemoveListener(_leaderboard.Toggle);
        }

        public void SetScore(long score)
        {
            _scoreText.text = score.ToString("N0");
        }

        public void DisplayTap(int score, Vector2 position)
        {
            if (_addedScorePrefab == null) return;
            
            StartCoroutine(ButtonTapAnimation());
            
            var addedScoreText = Instantiate(_addedScorePrefab, position, Quaternion.identity, _addedScoreContainer);
            addedScoreText.text = $"+{score:N0}";
            addedScoreText.color = _scoreTextColor;
            addedScoreText.gameObject.SetActive(true);
            StartCoroutine(ScoreAdditionAnimation(addedScoreText));
        }
        
        private IEnumerator ButtonTapAnimation()
        {
            var timer = new Timer(_buttonAnimationTime);
            while (timer.IsRunning)
            {
                float progress = _buttonAnimationCurve.Evaluate(timer.Progress);

                _tapButton.transform.localScale = Vector3.one * progress;
                yield return null;
            }

            _tapButton.transform.localScale = Vector3.one;
        }

        private IEnumerator ScoreAdditionAnimation(TextMeshProUGUI text)
        {
            Vector2 RandomizedPosition() => Random.insideUnitCircle * _addedScoreRandomRadius;

            Vector2 initialPosition = (Vector2)text.rectTransform.position + RandomizedPosition();
            Vector2 destinationPosition = initialPosition + _addedScoreAnimationVector;

            Color initialColor = text.color;
            Color destinationColor = initialColor;
            destinationColor.a = 0f;

            var timer = new Timer(_addedScoreAnimationTime);
            while(timer.IsRunning)
            {
                float progress = timer.Progress;

                text.rectTransform.position = Vector2.LerpUnclamped(initialPosition, destinationPosition, progress);
                text.color = Color.Lerp(initialColor, destinationColor, progress);
                yield return null;
            }

            text.rectTransform.position = destinationPosition;

            Destroy(text.gameObject);
        }
        
        #region Acceleration bonus
        

        private Coroutine _busterTimerCoroutine;
        
        public void StartBusterTimer(float time)
        {
            if(_busterTimerCoroutine != null) StopCoroutine(_busterTimerCoroutine);
            
            _busterTimerCoroutine = StartCoroutine(BusterTimerAnimation(time));
        }
        
        public Vector2 GetRandomTapButtonPoint()
        {
            var buttonTransform = _tapButton.transform as RectTransform;
            var buttonRect = buttonTransform.rect;

            var randomX = Random.Range(buttonRect.xMin, buttonRect.xMax);
            var randomY = Random.Range(buttonRect.yMin, buttonRect.yMax);
            
            return new Vector2(randomX, randomY) + (Vector2)buttonTransform.position;
        }

        private IEnumerator BusterTimerAnimation(float time)
        {
            _timerIcon.fillAmount = 1f;
            _timerIcon.gameObject.SetActive(true);
            
            var timer = new Timer(time);
            while (timer.IsRunning)
            {
                _timerIcon.fillAmount = 1f - timer.Progress;
                yield return null;
            }

            _timerIcon.fillAmount = 0f;
            _timerIcon.gameObject.SetActive(false);
        }

        #endregion
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_scoreText != null) _scoreText.color = _scoreTextColor;
        }
#endif
    }
}