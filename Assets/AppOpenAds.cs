using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using System.Collections;
using GoogleMobileAds.Ump;
using GoogleMobileAds.Ump.Api;

public class AppOpenAds : MonoBehaviour
{
    public static AppOpenAd instance;
    public float _loadingDuration;
    private AppOpenAd appOpenAd;
    private bool isAppOpenAdLoading;
    #region
    public string  AppOpenId;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        // Implement Singleton pattern
        if (instance == null)
        {
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        // Request and display a banner ad
        
        LoadAppOpenAd();
        Invoke(nameof(ShowAppOpenAd), _loadingDuration);
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!AdManagerAdmob.instance.IsRewarded && !AdManagerAdmob.instance.interstitialAdShown)
        {
            ShowAppOpenAd();
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (!AdManagerAdmob.instance.IsRewarded && !AdManagerAdmob.instance.interstitialAdShown)
        {
            //Invoke(nameof(ShowAppOpenAd), 1f);
            ShowAppOpenAd();
        }
    }
    #region App Open Ads
    public void LoadAppOpenAd()
    {
        if (isAppOpenAdLoading || appOpenAd != null)
        {
            return;
        }

        Debug.Log("Loading App Open ad.");

        isAppOpenAdLoading = true;

#if UNITY_ANDROID
        string adUnitId = AppOpenId; // Test Ad Unit ID for Android
#elif UNITY_IPHONE
    string adUnitId = "ca-app-pub-3940256099942544/5662855259"; // Test Ad Unit ID for iOS
#else
    string adUnitId = "unexpected_platform";
#endif

        AdRequest adRequest = new AdRequest();

        AppOpenAd.Load(adUnitId, adRequest, (AppOpenAd ad, LoadAdError error) =>
        {
            isAppOpenAdLoading = false;

            if (error != null)
            {
                Debug.LogError("App Open ad failed to load with error: " + error);
                return;
            }

            Debug.Log("App Open ad loaded with response: " + ad.GetResponseInfo());
            appOpenAd = ad;
            RegisterAppOpenAdEventHandlers(appOpenAd);
        });
    }

    public void ShowAppOpenAd()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            Debug.Log("Showing App Open ad.");
            appOpenAd.Show();
        }
        else
        {
          //  Debug.LogError("App Open ad is not ready yet.");
            LoadAppOpenAd();
        }
    }
    public bool IsAppOpenAdLoaded()
    {
        return appOpenAd != null && appOpenAd.CanShowAd();
    }

    private void RegisterAppOpenAdEventHandlers(AppOpenAd appOpenAd)
    {
        appOpenAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("App Open ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        appOpenAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App Open ad recorded an impression.");
        };
        appOpenAd.OnAdClicked += () =>
        {
            Debug.Log("App Open ad was clicked.");
        };
        appOpenAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App Open ad full screen content opened.");
        };
        appOpenAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App Open ad full screen content closed.");
            appOpenAd = null;
            LoadAppOpenAd(); // Reload the ad
        };
        appOpenAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App Open ad failed to open full screen content with error: " + error);
            appOpenAd = null;
            LoadAppOpenAd(); // Reload the ad
        };
    }
    #endregion
    
   

}