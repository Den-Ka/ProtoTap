using System.Collections;
using TMPro;
using UnityEngine;

namespace Etern0nety.Clicker.UI
{
    public class AddedScoreDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform _container;
        [SerializeField] TextMeshProUGUI _addedScorePrefab;
        [Space] [SerializeField] private float _animationTime = 1.5f;
        [SerializeField] private Vector2 _animationVector = Vector2.up;
        [SerializeField] private float _positionRandomOffsetRadius = 20f;


        public void DisplayAddedScore(int score, Vector3 screenPosition)
        {
            var prefabInstance = Instantiate(_addedScorePrefab, screenPosition, Quaternion.identity, _container);
            prefabInstance.text = $"+{score:N0}";

            StartCoroutine(Animation(prefabInstance, screenPosition, screenPosition + (Vector3)_animationVector));
        }

        private IEnumerator Animation(TextMeshProUGUI text, Vector3 startPosition, Vector3 endPosition)
        {
            text.gameObject.SetActive(true);

            Color startColor = text.color;
            Color endColor = startColor;
            endColor.a = 0f;

            var timer = new Timer(_animationTime);
            while (timer.IsRunning)
            {
                var progress = timer.Progress;

                text.rectTransform.position = Vector3.LerpUnclamped(startPosition, endPosition, progress);
                text.color = Color.Lerp(startColor, endColor, progress);
                yield return null;
            }

            Destroy(text.gameObject);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _container = GetComponent<RectTransform>();
        }
#endif
    }
}