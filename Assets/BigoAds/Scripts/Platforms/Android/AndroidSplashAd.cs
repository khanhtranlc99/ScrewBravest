#if UNITY_ANDROID
using System;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    class AndroidSplashAd : ISplashAd
    {
        private const string SplashAdLoaderClassName = AndroidPlatformTool.ClassPackage + ".api.SplashAdLoader$Builder";
        private const string SplashAdRequestClassName = AndroidPlatformTool.ClassPackage + ".api.SplashAdRequest$Builder";
        private const string SplashBuildMethod = "build";
        private const string SplashAdLoaderBuildMethod = "withAdLoadListener";
        private const string SplashAdLoaderExtMethod = "withExt";

        private AndroidJavaObject SplashAd;

        public event Action OnLoad;
        public event Action<int, string> OnLoadFailed;
        public event Action OnAdShowed;
        public event Action OnAdClicked;
        public event Action OnAdDismissed;
        public event Action<int, string> OnAdError;

        public AndroidSplashAd()
        {
            OnAdLoad += ((ad) => 
            {
                SplashAd = ad;
                OnLoad?.Invoke();
            });
        }

        private event Action<AndroidJavaObject> OnAdLoad;

        public void Load(string slotId, BigoSplashRequest request)
        {
            if (request == null)
            {
                return;
            }
            var SplashLoaderBuilder = new AndroidJavaObject(SplashAdLoaderClassName);
            SplashLoaderBuilder?.Call<AndroidJavaObject>(SplashAdLoaderExtMethod, request.ExtraInfoJson);
            SplashLoaderBuilder?.Call<AndroidJavaObject>(SplashAdLoaderBuildMethod, new AdLoadCallback(OnAdLoad, OnLoadFailed));
            var SplashLoader = SplashLoaderBuilder?.Call<AndroidJavaObject>(SplashBuildMethod);
        
            var SplashRequestBuilder = new AndroidJavaObject(SplashAdRequestClassName);
            SplashRequestBuilder?.Call<AndroidJavaObject>("withSlotId", slotId);
            SplashRequestBuilder?.Call<AndroidJavaObject>("withAge", request.Age);
            SplashRequestBuilder?.Call<AndroidJavaObject>("withGender", (int)(request.Gender));
            SplashRequestBuilder?.Call<AndroidJavaObject>("withActivatedTime", request.ActivatedTime);

            var SplashRequest = SplashRequestBuilder?.Call<AndroidJavaObject>(SplashBuildMethod);

            SplashLoader?.Call("loadAd", SplashRequest);
        }

        public bool IsLoaded()
        {
            return SplashAd != null;
        }

        public void Show()
        {
            SplashAd?.Call("setAdInteractionListener", new SplashAdInteractionCallback(OnAdShowed, OnAdClicked, OnAdDismissed, OnAdError));
            AndroidPlatformTool.CallMethodOnMainThread(() => 
            {
                SplashAd?.Call("show");
            });
            
        }

        public void Destroy()
        {
            //post to main 
            AdHelper.DestroyAd(SplashAd);
        }

        public bool IsExpired()
        {
            return SplashAd != null && SplashAd.Call<bool>("isExpired");
        }

        public bool IsClientBidding()
        {
            if (SplashAd == null) return false;
            AndroidJavaObject bid = SplashAd.Call<AndroidJavaObject>("getBid");
            return bid != null;
        }

        /// get price
        public double getPrice()
        {
            if (SplashAd == null) return 0;
            AndroidJavaObject bid = SplashAd.Call<AndroidJavaObject>("getBid");
            return bid == null ? 0 : bid.Call<double>("getPrice");
        }

        ///notify win
        public void notifyWin(double secPrice, string secBidder)
        {
            if (SplashAd == null) return;
            var secPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", secPrice);
            SplashAd.Call<AndroidJavaObject>("getBid")?.Call("notifyWin", secPriceDouble, secBidder);
        }

        ///notify loss
        public void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason)
        {
            if (SplashAd == null) return;
            var firstPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", firstPrice);
            SplashAd.Call<AndroidJavaObject>("getBid")?.Call("notifyLoss", firstPriceDouble, firstBidder, (int)lossReason);
        }
    }
}
#endif