using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Etern0nety.Clicker.UI
{
    public class TapButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private float _buttonAnimationTime = 0.1f;
        [SerializeField] private AnimationCurve _buttonAnimationCurve;

        public Button.ButtonClickedEvent OnClick => _button.onClick;
        
        private Coroutine _tapCoroutine;

        public void Animate()
        {
            if (_tapCoroutine != null) StopCoroutine(_tapCoroutine);
            _tapCoroutine = StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            var timer = new Timer(_buttonAnimationTime);
            while (timer.IsRunning)
            {
                float progress = _buttonAnimationCurve.Evaluate(timer.Progress);

                transform.localScale = Vector3.one * progress;
                yield return null;
            }

            transform.localScale = Vector3.one;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _button = GetComponent<Button>();
        }
#endif
    }
}