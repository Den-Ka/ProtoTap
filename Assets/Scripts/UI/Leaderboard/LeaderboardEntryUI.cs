using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etern0nety.Clicker
{
    public class LeaderboardEntryUI : MonoBehaviour
    {
        public enum Tier { Wood, Copper, Iron, Gold, Ruby, Diamond }
        
        [SerializeField] private bool _isOwner;
        [SerializeField] private Tier _tier;
        [Space]
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [Space]
        [SerializeField] private Image _panel;
        [SerializeField] private Image _glow;
        [Space]
        [SerializeField] private Color _defaultGlowColor = Color.black;
        [SerializeField] private Color _ownerGlowColor = Color.yellow;
        [Space]
        [SerializeField] private Color _defaultTextColor = Color.white;
        [SerializeField] private Color _ownerTextColor = Color.cyan;
        [Space]
        [SerializeField] private Color _woodTierColor = new Color(0.4f, 0.2f, 0f);
        [SerializeField] private Color _copperTierColor = new Color(0.75f, 0.5f, 0.25f);
        [SerializeField] private Color _ironTierColor = Color.white;
        [SerializeField] private Color _goldTierColor = Color.yellow;
        [SerializeField] private Color _rubyTierColor = Color.red;
        [SerializeField] private Color _diamondTierColor = Color.cyan;

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
            if(_rankText != null) _rankText.color = isOwner ? _ownerTextColor : _defaultTextColor;
            if(_scoreText != null) _scoreText.color = isOwner ? _ownerTextColor : _defaultTextColor;
            if(_nameText != null) _nameText.color = isOwner ? _ownerTextColor : _defaultTextColor;
            
            if(_glow != null) _glow.color = isOwner ? _ownerGlowColor : _defaultGlowColor;
        }

        private void SetTier(Tier tier)
        {
            _tier = tier;
            
            if (_panel != null)
            {
                _panel.color = tier switch
                {
                    Tier.Wood => _woodTierColor,
                    Tier.Copper => _copperTierColor,
                    Tier.Iron => _ironTierColor,
                    Tier.Gold => _goldTierColor,
                    Tier.Ruby => _rubyTierColor,
                    Tier.Diamond => _diamondTierColor,
                    _ => _panel.color
                };
            }
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