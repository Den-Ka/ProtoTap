using UnityEngine;

namespace Etern0nety.Clicker.Leaderboard
{
    public enum Tier
    {
        Wood,
        Copper,
        Iron,
        Gold,
        Ruby,
        Diamond
    }

    [CreateAssetMenu(menuName = "Etern0nety/Leaderboard/Tiers")]
    public class Tiers : ScriptableObject
    {
        [SerializeField] private Color _woodTierColor = new Color(0.4f, 0.2f, 0f);
        [SerializeField] private Color _copperTierColor = new Color(0.75f, 0.5f, 0.25f);
        [SerializeField] private Color _ironTierColor = Color.white;
        [SerializeField] private Color _goldTierColor = Color.yellow;
        [SerializeField] private Color _rubyTierColor = Color.red;
        [SerializeField] private Color _diamondTierColor = Color.cyan;

        public static Tier GetTierFromString(string tier)
        {
            return tier switch
            {
                "Wood" => Tier.Wood,
                "Copper" => Tier.Copper,
                "Iron" => Tier.Iron,
                "Gold" => Tier.Gold,
                "Ruby" => Tier.Ruby,
                "Diamond" => Tier.Diamond,
                _ => throw new System.ArgumentException("Invalid Tier: " + tier)
            };
        }

        public Color GetColorFromTier(Tier tier)
        {
            return tier switch
            {
                Tier.Wood => _woodTierColor,
                Tier.Copper => _copperTierColor,
                Tier.Iron => _ironTierColor,
                Tier.Gold => _goldTierColor,
                Tier.Ruby => _rubyTierColor,
                Tier.Diamond => _diamondTierColor,
                _ => Color.white
            };
        }
    }
}