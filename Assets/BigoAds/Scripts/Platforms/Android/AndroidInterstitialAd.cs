#if UNITY_ANDROID
using System;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    class AndroidInterstitialAd : IInterstitialAd
    {
        private const string InterstitialAdLoaderClassName = AndroidPlatformTool.ClassPackage + ".api.InterstitialAdLoader$Builder";
        private const string InterstitialAdRequestClassName = AndroidPlatformTool.ClassPackage + ".api.InterstitialAdRequest$Builder";
        private const string InterstitialBuildMethod = "build";
        private const string InterstitialAdLoaderBuildMethod = "withAdLoadListener";
        private const string InterstitialAdLoaderExtMethod = "withExt";

        private AndroidJavaObject InterstitialAd;

        public event Action OnLoad;
        public event Action<int, string> OnLoadFailed;
        public event Action OnAdShowed;
        public event Action OnAdClicked;
        public event Action OnAdDismissed;
        public event Action<int, string> OnAdError;

        public AndroidInterstitialAd()
        {
            OnAdLoad += ((ad) => 
            {
                InterstitialAd = ad;
                OnLoad?.Invoke();
            });
        }

        private event Action<AndroidJavaObject> OnAdLoad;

        public void Load(string slotId, BigoInterstitialRequest request)
        {
            if (request == null)
            {
                return;
            }
            var InterstitialLoaderBuilder = new AndroidJavaObject(InterstitialAdLoaderClassName);
            InterstitialLoaderBuilder?.Call<AndroidJavaObject>(InterstitialAdLoaderExtMethod, request.ExtraInfoJson);
            InterstitialLoaderBuilder?.Call<AndroidJavaObject>(InterstitialAdLoaderBuildMethod, new AdLoadCallback(OnAdLoad, OnLoadFailed));
            var InterstitialLoader = InterstitialLoaderBuilder?.Call<AndroidJavaObject>(InterstitialBuildMethod);
        
            var InterstitialRequestBuilder = new AndroidJavaObject(InterstitialAdRequestClassName);
            InterstitialRequestBuilder?.Call<AndroidJavaObject>("withSlotId", slotId);
            InterstitialRequestBuilder?.Call<AndroidJavaObject>("withAge", request.Age);
            InterstitialRequestBuilder?.Call<AndroidJavaObject>("withGender", (int)(request.Gender));
            InterstitialRequestBuilder?.Call<AndroidJavaObject>("withActivatedTime", request.ActivatedTime);

            var InterstitialRequest = InterstitialRequestBuilder?.Call<AndroidJavaObject>(InterstitialBuildMethod);

            InterstitialLoader?.Call("loadAd", InterstitialRequest);
        }

        public bool IsLoaded()
        {
            return InterstitialAd != null;
        }

        public void Show()
        {
            InterstitialAd?.Call("setAdInteractionListener", new AdInteractionCallback(OnAdShowed, OnAdClicked, OnAdDismissed, OnAdError));
            AndroidPlatformTool.CallMethodOnMainThread(() => 
            {
                InterstitialAd?.Call("show");
            });
            
        }

        public void Destroy()
        {
            //post to main 
            AdHelper.DestroyAd(InterstitialAd);
        }

        public bool IsExpired()
        {
            return InterstitialAd != null && InterstitialAd.Call<bool>("isExpired");
        }

        public bool IsClientBidding()
        {
            if (InterstitialAd == null) return false;
            AndroidJavaObject bid = InterstitialAd.Call<AndroidJavaObject>("getBid");
            return bid != null;
        }

        /// get price
        public double getPrice()
        {
            if (InterstitialAd == null) return 0;
            AndroidJavaObject bid = InterstitialAd.Call<AndroidJavaObject>("getBid");
            return bid == null ? 0 : bid.Call<double>("getPrice");
        }

        ///notify win
        public void notifyWin(double secPrice, string secBidder)
        {
            if (InterstitialAd == null) return;
            var secPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", secPrice);
            InterstitialAd.Call<AndroidJavaObject>("getBid")?.Call("notifyWin", secPriceDouble, secBidder);
        }

        ///notify loss
        public void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason)
        {
            if (InterstitialAd == null) return;
            var firstPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", firstPrice);
            InterstitialAd.Call<AndroidJavaObject>("getBid")?.Call("notifyLoss", firstPriceDouble, firstBidder, (int)lossReason);
        }
    }
}
#endif