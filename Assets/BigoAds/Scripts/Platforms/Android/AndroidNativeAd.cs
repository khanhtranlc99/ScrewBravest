#if UNITY_ANDROID
using System;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    class AndroidNativeAd : INativeAd
    {
        private const string NativeAdLoaderClassName = AndroidPlatformTool.ClassPackage + ".api.NativeAdLoader$Builder";
        private const string NativeAdRequestClassName = AndroidPlatformTool.ClassPackage + ".api.NativeAdRequest$Builder";
        private const string NativeBuildMethod = "build";
        private const string NativeAdLoaderBuildMethod = "withAdLoadListener";
        private const string NativeAdLoaderExtMethod = "withExt";

        private AndroidJavaObject NativeAd;

        public event Action OnLoad;
        public event Action<int, string> OnLoadFailed;
        public event Action OnAdShowed;
        public event Action OnAdClicked;
        public event Action OnAdDismissed;
        public event Action<int, string> OnAdError;

        public AndroidNativeAd()
        {
            OnAdLoad += ((ad) => 
            {
                NativeAd = ad;
                OnLoad?.Invoke();
            });
        }

        private event Action<AndroidJavaObject> OnAdLoad;

        public void Load(string slotId, BigoNativeRequest request)
        {
            if (request == null)
            {
                return;
            }
            var nativeLoaderBuilder = new AndroidJavaObject(NativeAdLoaderClassName);
            nativeLoaderBuilder?.Call<AndroidJavaObject>(NativeAdLoaderExtMethod, request.ExtraInfoJson);
            nativeLoaderBuilder?.Call<AndroidJavaObject>(NativeAdLoaderBuildMethod, new AdLoadCallback(OnAdLoad, OnLoadFailed));
            var nativeLoader = nativeLoaderBuilder?.Call<AndroidJavaObject>(NativeBuildMethod);
        
            var nativeRequestBuilder = new AndroidJavaObject(NativeAdRequestClassName);
            nativeRequestBuilder?.Call<AndroidJavaObject>("withSlotId", slotId);
            nativeRequestBuilder?.Call<AndroidJavaObject>("withAge", request.Age);
            nativeRequestBuilder?.Call<AndroidJavaObject>("withGender", (int)(request.Gender));
            nativeRequestBuilder?.Call<AndroidJavaObject>("withActivatedTime", request.ActivatedTime);

            var nativeRequest = nativeRequestBuilder?.Call<AndroidJavaObject>(NativeBuildMethod);

            nativeLoader?.Call("loadAd", nativeRequest);
        }

        public bool IsLoaded()
        {
            return NativeAd != null;
        }

        public void Show()
        {
            NativeAd?.Call("setAdInteractionListener", new AdInteractionCallback(OnAdShowed, OnAdClicked, OnAdDismissed, OnAdError));

            AndroidPlatformTool.CallMethodOnMainThread(() => 
            {
                AdHelper.ShowNativeAd(NativeAd);
            });
            
        }

        public void Destroy()
        {
            AdHelper.DestroyAd(NativeAd);
        }

        public bool IsExpired()
        {

            return NativeAd != null && NativeAd.Call<bool>("isExpired");
        }

        public void SetPosition(BigoPosition position)
        {
            AndroidPlatformTool.CallMethodOnMainThread(() => 
            {
                AdHelper.ShowNativeAd(NativeAd, position);
            });
        }

        public bool IsClientBidding()
        {
            if (NativeAd == null) return false;
            AndroidJavaObject bid = NativeAd.Call<AndroidJavaObject>("getBid");
            return bid != null;
        }

        /// get price
        public double getPrice()
        {
            if (NativeAd == null) return 0;
            AndroidJavaObject bid = NativeAd.Call<AndroidJavaObject>("getBid");
            return bid == null ? 0 : bid.Call<double>("getPrice");
        }

        ///notify win
        public void notifyWin(double secPrice, string secBidder)
        {
            if (NativeAd == null) return;
            var secPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", secPrice);
            NativeAd.Call<AndroidJavaObject>("getBid")?.Call("notifyWin", secPriceDouble, secBidder);
        }

        ///notify loss
        public void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason)
        {
            if (NativeAd == null) return;
            var firstPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", firstPrice);
            NativeAd.Call<AndroidJavaObject>("getBid")?.Call("notifyLoss", firstPriceDouble, firstBidder, (int)lossReason);
        }
    }
}
#endif