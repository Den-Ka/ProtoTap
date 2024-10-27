using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Etern0nety.Clicker.Ads
{
    public class UnityAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private Button _rewardedAdsButton;
        [SerializeField] private TapAccelerator _tapAccelerator;
        [SerializeField] private bool _testMode;
        [Space]
        [SerializeField] private string _androidId;
        [SerializeField] private string _iosId;
        
        private const string _rewardedAndroid = "Rewarded_Android";
        private const string _rewardedIOS = "Rewarded_iOS";
        
        private string _actualId;
        private string _rewardedId;
        private bool _adLoaded = false;
        private bool _bonusInProgress = false;

        #region Initialization

        private void Awake()
        {
            InitializeAds();

            _rewardedAdsButton.interactable = false;
            _rewardedAdsButton.onClick.AddListener(ShowAd);
            _tapAccelerator.Finished += OnBonusFinished;
        }

        private void OnDestroy()
        {
            _tapAccelerator.Finished -= OnBonusFinished;
        }

        private void InitializeAds()
        {
#if UNITY_IOS
            _actualId = _iosId;
            _rewardedId = _rewardedIOS;
#else
            _actualId = _androidId;
            _rewardedId = _rewardedAndroid;
#endif

            Debug.Log("Unity Ads initialization.");
            if (!Advertisement.isSupported)
            {
                Debug.Log("Unity Ads is not supported.");
                return;
            }
            if (!Advertisement.isInitialized)
            {
                Debug.Log("Unity Ads initialization started.");
                Advertisement.Initialize(_androidId, _testMode, this);
            }
            else
            {
                Debug.Log("Unity Ads initialized.");
                LoadAd();                
            }
        }
        
        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialized.");
            LoadAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log("Unity Ads initialization failed: " + message);
        }

        #endregion

        private void OnBonusFinished()
        {
            _bonusInProgress = false;
            ValidateRewardButton();
        }

        private void ValidateRewardButton()
        {
            _rewardedAdsButton.interactable = !_bonusInProgress && _adLoaded;
        }

        #region Ad Loading
        
        private void LoadAd()
        {
            Debug.Log("Unity Ads loading.");
            Advertisement.Load(_rewardedId, this);
        }
        public void OnUnityAdsAdLoaded(string placementId)
        {
            _adLoaded = true;
            ValidateRewardButton();
        }
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        }
        
        #endregion

        #region Show ad

        private void ShowAd()
        {
            Advertisement.Show(_rewardedId, this);
            _rewardedAdsButton.interactable = false;
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                _tapAccelerator.Launch();
            }
        }

        public void OnUnityAdsShowStart(string placementId) { }
        
        public void OnUnityAdsShowClick(string placementId) { }
        
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }
        
        #endregion
    }
}
