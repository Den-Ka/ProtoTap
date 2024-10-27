using System.Collections;
using TMPro;
using UnityEngine;
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
        [SerializeField] private LeaderboadUI _leaderboard;
        [Header("Tap Acceleration")]
        [SerializeField] private Image _accelerationTimerIcon;
        [SerializeField] private TapAccelerator _tapAccelerator;
        [Space]
        [SerializeField] private Player _player;

        private void Awake()
        {
            _tapButton.onClick.AddListener(TapButton);
            _leaderboardButton.onClick.AddListener(_leaderboard.Toggle);

            _player.ScoreChanged += UpdateScoreText;
            
            _tapAccelerator.Started += OnAccelerationStarted;
            _tapAccelerator.ScoreAdded += OnAccelerationScoreAdded;
            
            UpdateScoreText(_player.Score);
        }
        private void OnDestroy()
        {
            _tapButton.onClick.RemoveListener(TapButton);
            _leaderboardButton.onClick.RemoveListener(_leaderboard.Toggle);

            _player.ScoreChanged -= UpdateScoreText;
            
            _tapAccelerator.Started -= OnAccelerationStarted;
            _tapAccelerator.ScoreAdded -= OnAccelerationScoreAdded;
        }

        private void UpdateScoreText(long score)
        {
            _scoreText.text = score.ToString("N0");
        }

        private void TapButton()
        {
            var score = _player.ScoreForTap;

            _player.AddScore(score);

            Vector2 position = Input.mousePosition;
            DisplayTap(score, position);
        }
        
        private void DisplayTap(int score, Vector2 position)
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

        private void OnAccelerationStarted()
        {
            StartCoroutine(AccelerationTimerAnimation());
        }
        private void OnAccelerationScoreAdded(int score)
        {
            var buttonTransform = _tapButton.transform as RectTransform;
            var buttonRect = buttonTransform.rect;

            var randomX = Random.Range(buttonRect.xMin, buttonRect.xMax);
            var randomY = Random.Range(buttonRect.yMin, buttonRect.yMax);
            
            var position = new Vector2(randomX, randomY) + (Vector2)buttonTransform.position;

            DisplayTap(score, position);
        }
        private IEnumerator AccelerationTimerAnimation()
        {
        
            _accelerationTimerIcon.fillAmount = 1f;
            _accelerationTimerIcon.gameObject.SetActive(true);
            
            var timer = new Timer(_tapAccelerator.WorkingTime);
            while (timer.IsRunning)
            {
                _accelerationTimerIcon.fillAmount = 1f - timer.Progress;
                yield return null;
            }

            _accelerationTimerIcon.fillAmount = 0f;
            _accelerationTimerIcon.gameObject.SetActive(false);
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