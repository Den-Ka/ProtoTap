using Etern0nety.Clicker.Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etern0nety.Clicker.UI
{
    public class LeaderboardEntryUI : MonoBehaviour
    {
        [SerializeField] private bool _isOwner;
        [SerializeField] private Tier _tier;
        [Space] [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [Space] [SerializeField] private Image _panel;
        [SerializeField] private Image _glow;
        [Space] [SerializeField] private Color _defaultGlowColor = Color.black;
        [SerializeField] private Color _ownerGlowColor = Color.yellow;
        [Space] [SerializeField] private Color _defaultTextColor = Color.white;
        [SerializeField] private Color _ownerTextColor = Color.cyan;
        [Space] [SerializeField] private Tiers _tiersConfig;

        public void Initialize(int rank, string nickname, long score, Tier tier, bool isOwner)
        {
            _rankText.SetText($"#{rank}");
            _nameText.SetText(nickname);
            _scoreText.SetText(score.ToString("N0"));

            SetOwner(isOwner);
            SetTier(tier);
        }

        private void SetOwner(bool isOwner)
        {
            _isOwner = isOwner;
            if (_rankText != null) _rankText.color = isOwner ? _ownerTextColor : _defaultTextColor;
            if (_scoreText != null) _scoreText.color = isOwner ? _ownerTextColor : _defaultTextColor;
            if (_nameText != null) _nameText.color = isOwner ? _ownerTextColor : _defaultTextColor;

            if (_glow != null) _glow.color = isOwner ? _ownerGlowColor : _defaultGlowColor;
        }

        private void SetTier(Tier tier)
        {
            _tier = tier;
            if (_panel != null) _panel.color = _tiersConfig.GetColorFromTier(tier);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetOwner(_isOwner);
            SetTier(_tier);
        }
#endif
    }
}