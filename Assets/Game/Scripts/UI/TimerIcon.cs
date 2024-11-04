using System;
using UnityEngine;
using UnityEngine.UI;

namespace Etern0nety.Clicker.UI
{
    [RequireComponent(typeof(Image))]
    public class TimerIcon : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private IProgression _progression;

        public void Initialize(IProgression progression)
        {
            if (progression == null)
            {
                Debug.LogError($"Can't launch {nameof(TimerIcon)}: {nameof(progression)} is null");
                return;
            }

            _progression = progression;
            gameObject.SetActive(true);

            Progress();
        }

        private void Update()
        {
            Progress();
        }

        private void Progress()
        {
            var currentProgress = _progression.Progress;
            
            _image.fillAmount = 1f - currentProgress;
            if (currentProgress >= 1)
            {
                gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _image = GetComponent<Image>();
            _image.type = Image.Type.Filled;
            _image.fillMethod = Image.FillMethod.Radial360;
        }
#endif
    }
}