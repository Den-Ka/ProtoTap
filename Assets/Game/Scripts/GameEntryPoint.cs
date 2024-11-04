using Etern0nety.Clicker.Ads;
using Etern0nety.Clicker.Leaderboard;
using Etern0nety.DI;
using UnityEngine;

namespace Etern0nety.Clicker
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private TapBuster _tapBuster;
        [Header("UI")] [SerializeField] private IngameUIController _UIController;

        private void Awake()
        {
            IAdvertisementService advertisementService = new UnityAds(true);
            advertisementService.AdvertisementRewarded += _tapBuster.Launch;

            var container = new DIContainer();

            container.RegisterInstance(_player);
            container.RegisterInstance(_tapBuster);

            container.RegisterInstance<ILeaderboardService>(new UnityLeaderboard());
            container.RegisterInstance<IAdvertisementService>(advertisementService);

            _tapBuster.Initialize(container);

            _UIController.Initialize(container);
        }
    }
}