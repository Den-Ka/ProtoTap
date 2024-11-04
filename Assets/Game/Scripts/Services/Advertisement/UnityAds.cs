using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Etern0nety.Clicker.Ads
{
    public class UnityAds : IAdvertisementService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private readonly bool _testMode;
        
        private const string _androidId = "5720513";
        private const string _iosId = "5720512";
                
        private const string _rewardedAndroid = "Rewarded_Android";
        private const string _rewardedIOS = "Rewarded_iOS";
        
        private string _actualId;
        private string _rewardedId;

        public event Action AdvertisementLoaded;
        public event Action AdvertisementLoadFailed;
        public event Action AdvertisementShown;
        public event Action AdvertisementClosed;
        public event Action AdvertisementRewarded;
        
        private Action<bool> _loadCallback;
        private Action<bool> _rewardedCallback;

        private bool _initializing = false;
            
        #region Initialization
        
        public UnityAds(bool testMode)
        {
            _testMode = testMode;
        }
        
        private void Initialize()
        {
#if UNITY_IOS
            _actualId = _iosId;
            _rewardedId = _rewardedIOS;
#else
            _actualId = _androidId;
            _rewardedId = _rewardedAndroid;
#endif
            
            if (!Advertisement.isSupported)
            {
                Debug.Log("Unity Ads is not supported.");
                return;
            }

            if (Advertisement.isInitialized) return;
            
            _initializing = true;
            
            Debug.Log("Unity Ads initialization started.");
            Advertisement.Initialize(_androidId, _testMode, this);
        }
        
        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialized.");
            _initializing = false;
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log("Unity Ads initialization failed: " + message);
            _initializing = false;
        }

        #endregion

        #region Load Advertisement

        public async void LoadAdvertisement(Action<bool> callbackLoaded)
        {

            _loadCallback = callbackLoaded;

            if (!Advertisement.isInitialized)
            {
                if (!_initializing) Initialize();
                while (_initializing)
                {
                    await Awaitable.EndOfFrameAsync();
                }
            }
            
            Debug.Log("Unity Ads loading.");
            Advertisement.Load(_rewardedId, this);
        }
        public void OnUnityAdsAdLoaded(string placementId)
        {
            _loadCallback?.Invoke(true);
            AdvertisementLoaded?.Invoke();
        }
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            _loadCallback?.Invoke(false);
            AdvertisementLoadFailed?.Invoke();
            
            Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        #endregion

        #region Show Advertisement

        public void ShowAdvertisement(Action<bool> callbackRewarded)
        {
            _rewardedCallback = callbackRewarded;
            
            Advertisement.Show(_rewardedId, this);
        }
        public void OnUnityAdsShowStart(string placementId)
        {
            AdvertisementShown?.Invoke();
        }
        public void OnUnityAdsShowClick(string placementId) { }
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                AdvertisementRewarded?.Invoke();
                _rewardedCallback?.Invoke(true);
            }
            else
            {
                AdvertisementClosed?.Invoke();
                _rewardedCallback?.Invoke(false);
            }
        }
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            AdvertisementClosed?.Invoke();
            _rewardedCallback?.Invoke(false);
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        #endregion
    }
}
