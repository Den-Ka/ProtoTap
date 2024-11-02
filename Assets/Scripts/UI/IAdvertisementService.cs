using System;
using System.Threading.Tasks;

namespace Etern0nety.Clicker
{
    internal interface IAdvertisementService
    {
        event Action AdvertisementLoaded;
        event Action AdvertisementLoadFailed;
        event Action AdvertisementShown;
        event Action AdvertisementClosed;
        event Action AdvertisementRewarded;
        
        void LoadAdvertisement(Action<bool> callbackLoaded = null);
        void ShowAdvertisement(Action<bool> callbackRewarded = null);
    }
}