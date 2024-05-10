#if UNITY_ANDROID
using System;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    class AndroidRewardedAd : IRewardedAd
    {
        private const string RewardedAdLoaderClassName = AndroidPlatformTool.ClassPackage + ".api.RewardVideoAdLoader$Builder";
        private const string RewardedAdRequestClassName = AndroidPlatformTool.ClassPackage + ".api.RewardVideoAdRequest$Builder";
        private const string RewardedAdBuildMethod = "build";
        private const string RewardedAdLoaderBuildMethod = "withAdLoadListener";
        private const string RewardedAdLoaderExtMethod = "withExt";

        private AndroidJavaObject RewardedAd;

        public event Action OnLoad;
        public event Action<int, string> OnLoadFailed;
        public event Action OnAdShowed;
        public event Action OnAdClicked;
        public event Action OnAdDismissed;
        public event Action<int, string> OnAdError;
        public event Action OnUserEarnedReward;

        public AndroidRewardedAd()
        {
            OnAdLoad += ((ad) => 
            {
                RewardedAd = ad;
                OnLoad?.Invoke();
            });
        }

        private event Action<AndroidJavaObject> OnAdLoad;

        public void Load(string slotId, BigoRewardedRequest request)
        {
            if (request == null)
            {
                return;
            }
            var rewardedAdLoaderBuilder = new AndroidJavaObject(RewardedAdLoaderClassName);
            rewardedAdLoaderBuilder?.Call<AndroidJavaObject>(RewardedAdLoaderExtMethod, request.ExtraInfoJson);
            rewardedAdLoaderBuilder?.Call<AndroidJavaObject>(RewardedAdLoaderBuildMethod, new AdLoadCallback(OnAdLoad, OnLoadFailed));
            var rewardedAdLoader = rewardedAdLoaderBuilder?.Call<AndroidJavaObject>(RewardedAdBuildMethod);
        
            var rewardedAdRequestBuilder = new AndroidJavaObject(RewardedAdRequestClassName);
            rewardedAdRequestBuilder?.Call<AndroidJavaObject>("withSlotId", slotId);
            rewardedAdRequestBuilder?.Call<AndroidJavaObject>("withAge", request.Age);
            rewardedAdRequestBuilder?.Call<AndroidJavaObject>("withGender", (int)(request.Gender));
            rewardedAdRequestBuilder?.Call<AndroidJavaObject>("withActivatedTime", request.ActivatedTime);

            var RewardedAdRequest = rewardedAdRequestBuilder?.Call<AndroidJavaObject>(RewardedAdBuildMethod);

            rewardedAdLoader?.Call("loadAd", RewardedAdRequest);
        }

        public bool IsLoaded()
        {
            return RewardedAd != null;
        }

        public void Show()
        {
            RewardedAd?.Call("setAdInteractionListener", new RewardedAdInteractionCallback(OnAdShowed, OnAdClicked, OnAdDismissed, OnAdError, OnUserEarnedReward));
            AndroidPlatformTool.CallMethodOnMainThread(() => 
            {
                RewardedAd?.Call("show");
            });
            
        }

        public void Destroy()
        {
            //post to main 
            AdHelper.DestroyAd(RewardedAd);
        }

        public bool IsExpired()
        {
            return RewardedAd != null && RewardedAd.Call<bool>("isExpired");
        }

        public bool IsClientBidding()
        {
            if (RewardedAd == null) return false;
            AndroidJavaObject bid = RewardedAd.Call<AndroidJavaObject>("getBid");
            return bid != null;
        }

        /// get price
        public double getPrice()
        {
            if (RewardedAd == null) return 0;
            AndroidJavaObject bid = RewardedAd.Call<AndroidJavaObject>("getBid");
            return bid == null ? 0 : bid.Call<double>("getPrice");
        }

        ///notify win
        public void notifyWin(double secPrice, string secBidder)
        {
            if (RewardedAd == null) return;
            var secPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", secPrice);
            RewardedAd.Call<AndroidJavaObject>("getBid")?.Call("notifyWin", secPriceDouble, secBidder);
        }

        ///notify loss
        public void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason)
        {
            if (RewardedAd == null) return;
            var firstPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", firstPrice);
            RewardedAd.Call<AndroidJavaObject>("getBid")?.Call("notifyLoss", firstPriceDouble, firstBidder, (int)lossReason);
        }
    }
}
#endif