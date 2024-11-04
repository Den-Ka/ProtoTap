using System.Numerics;
using TMPro;
using UnityEngine;

namespace Etern0nety.Clicker.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Counter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public BigInteger Value
        {
            set => _text.text = value.ToString("N0");
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}