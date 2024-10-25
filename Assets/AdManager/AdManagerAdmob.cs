using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using Firebase;
using Firebase.Crashlytics;
using Firebase.Analytics;
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Ump.Common;

public class AdManagerAdmob : MonoBehaviour
{
    public string appID;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewarded;
    public bool _isbannershown;
    //AdUnit Ids
    [Header("Change Admob Ids Here")]
    public string bannerId = "ca-app-pub-3940256099942544/6300978111";
    public string interstitialId = "ca-app-pub-3940256099942544/1033173712";
    public string rewardedId = "ca-app-pub-3940256099942544/5224354917";
    public bool _isRewarded, interstitialAdShown;
    [HideInInspector]
    public bool IsRewarded = false;
  
    public static AdManagerAdmob instance;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    [HideInInspector]
    public bool firebaseInitialized = false;
    [HideInInspector]

    private Action _OnRewardedFailed;

    private Action _OnRewarded;
    private int rewardedShownCount = 0;
    private int RewardedPerSession = 100;
    private bool RemoveAds = false;
    [HideInInspector]
    public bool IsInitialized = false;
    [HideInInspector]
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void Start()
    {
        _isbannershown = false;
        _isRewarded = false;
        IsInitialized = true;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

       // RequestBanner();
        LoadAdInterestitial();
        ConsentFormInit();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                var app = FirebaseApp.DefaultInstance;
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void RequestBanner()
    {
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);

        AdRequest request = new AdRequest();

        bannerView.LoadAd(request);

        bannerView.Show();
        _isbannershown = true;
    }

    public void DestroyBanner()
    {
        bannerView.Hide();
        _isbannershown = false;
    }
    #region Interestitial
    public void LoadAdInterestitial()
    {
        // Clean up the old ad before loading a new one.
        if (interstitial != null)
        {
            //  DestroyAd();
        }

        Debug.Log("Loading interstitial ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();
        // Send the request to load the ad.
        
        InterstitialAd.Load(interstitialId, adRequest, (InterstitialAd adinterestitial, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (adinterestitial == null)
            {
                Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.

            interstitial = adinterestitial;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(interstitial);

            // Inform the UI that the ad is ready.
       
        });
       
    }



    public void ShowInterstitialAd()
    {
        LoadAdInterestitial();
        if (interstitial != null && interstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitial.Show();
            interstitialAdShown = true;
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }

        // Inform the UI that the ad is not ready.

    }

    public void DestroyAdInterestital()
    {
        if (interstitial != null)
        {
            Debug.Log("Destroying interstitial ad.");
            interstitial.Destroy();
            interstitial = null;
        }

        // Inform the UI that the ad is not ready.

    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error : "
                + error);
        };
    }

    #endregion
    #region Rewarded
    public void ShowRewarded(Action OnFailed, Action OnRewarded)
    {
        // Debug.Log("Show rewarded2");
        _OnRewardedFailed = OnFailed;
        _OnRewarded = OnRewarded;
        if (rewardedShownCount >= RewardedPerSession)
        {
            //  HideLoadingScreen();
            _OnRewardedFailed?.Invoke();
            Debug.Log("reward failed2");
            return;
        }

        if (RemoveAds || Application.internetReachability == NetworkReachability.NotReachable)
        {
            // HideLoadingScreen();
            _OnRewardedFailed?.Invoke();
            Debug.Log("reward failed3");
            return;
        }

        if (!IsInitialized)
        {
            // HideLoadingScreen();
            _OnRewardedFailed?.Invoke();
            Debug.Log("reward failed4");
            return;
        }

        IsRewarded = false;
        if (rewarded != null)
        {
            rewarded.Destroy();
            rewarded = null;
        }

        // ShowLoadingScreen();
        AdRequest request = new AdRequest();
        RewardedAd.Load(rewardedVideoId, request, delegate (RewardedAd ad, LoadAdError error)
        {
            if (error != null || ad == null)
            {
                // HideLoadingScreen();
                _OnRewardedFailed?.Invoke();
                Debug.Log("reward failed1");
            }
            else
            {
                rewarded = ad;
                RegisterEventHandlers(ad);
                StartCoroutine(ShowRewardedAsync());
                IsRewarded = true;
                Debug.Log("Show rewarded");
            }
        });
    }
    public string rewardedVideoId
    {
        get
        {
            return rewardedId;
        }
    }
    private IEnumerator ShowRewardedAsync()
    {
        Debug.Log("Show rewarded1");

        yield return new WaitForEndOfFrame();
        ShowRewardedAdInternal();
    }

    private void ShowRewardedAdInternal()
    {
       
            if (rewarded != null && rewarded.CanShowAd())
            {
                Debug.Log("Rewarded ad can be shown");
                rewardedShownCount++;
                rewarded.Show(delegate (Reward reward)
                {
                    IsRewarded = reward.Amount > 0.0;
                    if (IsRewarded)
                    {
                        Debug.Log("Rewarded ad succeeded");
                        _OnRewarded?.Invoke();
                    }
                    else
                    {
                        Debug.Log("Rewarded ad failed");
                        _OnRewardedFailed?.Invoke();
                    }
                });
            }
            else
            {
                Debug.LogError("Rewarded ad cannot be shown or is null");
                _OnRewardedFailed?.Invoke();
                if (rewarded != null)
                {
                    rewarded.Destroy();
                }
            }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentOpened += delegate
        {

        };
        ad.OnAdFullScreenContentClosed += delegate
        {
            if (rewarded != null)
            {
                rewarded.Destroy();
                IsRewarded = false;
            }
        };
        ad.OnAdFullScreenContentFailed += delegate
        {
            _OnRewardedFailed?.Invoke();
            if (rewarded != null)
            {
                rewarded.Destroy();
                IsRewarded = false;
            }
        };

    }
    public bool CanShowRewardedAd()
    {
        /* if (this.rewarded.CanShowAd())
         {

             return true;
         }
         else
         {

             return false;
         }*/
        return true;
    }
    #endregion
    #region Firebase
    void InitializeFirebase()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        Debug.LogError("Firebase crashlytics initilized succssfully");
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(
          FirebaseAnalytics.UserPropertySignUpMethod,
          "Google");
        // Set the user ID.
        FirebaseAnalytics.SetUserId("uber_user_510");
        // Set default session duration values.
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
        //showFireBaseInitialization.text = "Firebase is initialized";
        Debug.Log("Firebase initialize");
    }
    public void CustomLogEvents(string customEventName)
    {
        FirebaseAnalytics.LogEvent(customEventName);
    }
    #endregion
    #region
    ConsentForm _consentForm;
    void ConsentFormInit()
    {
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            DebugGeography = DebugGeography.EEA,
        };
        // Here false means users are not under age.
        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings,
        };
        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }
        if (ConsentInformation.IsConsentFormAvailable())
        {
            LoadConsentForm();
        }
        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
    }
    void LoadConsentForm()
    {
        // Loads a consent form.
        ConsentForm.Load(OnLoadConsentForm);
        Screen.orientation = ScreenOrientation.LandscapeLeft;

    }
    void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }
        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = consentForm;
        // You are now ready to show the form.
        if (GoogleMobileAds.Ump.Api.ConsentInformation.ConsentStatus == GoogleMobileAds.Ump.Api.ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }

    }
    void OnShowForm(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }
        // Handle dismissal by reloading form.
        LoadConsentForm();
    }

    // public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    // {
    //     UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    // }
    // public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    // {
    //     UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    // }
    #endregion
}



