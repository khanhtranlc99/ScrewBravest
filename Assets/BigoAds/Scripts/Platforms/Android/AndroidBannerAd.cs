#if UNITY_ANDROID
using System;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    class AndroidBannerAd : IBannerAd
    {
        private const string BannerAdLoaderClassName = AndroidPlatformTool.ClassPackage + ".api.BannerAdLoader$Builder";
        private const string BannerAdRequestClassName = AndroidPlatformTool.ClassPackage + ".api.BannerAdRequest$Builder";
        private const string BannerSizeClassName = AndroidPlatformTool.ClassPackage + ".api.AdSize";
        private const string BannerBuildMethod = "build";
        private const string BannerAdLoaderBuildMethod = "withAdLoadListener";
        private const string BannerAdLoaderExtMethod = "withExt";


        private AndroidJavaObject BannerAd;

        public event Action OnLoad;
        public event Action<int, string> OnLoadFailed;
        public event Action OnAdShowed;
        public event Action OnAdClicked;
        public event Action OnAdDismissed;
        public event Action<int, string> OnAdError;

        public AndroidBannerAd()
        {
            OnAdLoad += ((ad) => 
            {
                BannerAd = ad;
                OnLoad?.Invoke();
            });
        }

        private event Action<AndroidJavaObject> OnAdLoad;

        public void Load(string slotId, BigoBannerRequest request)
        {
            if (request == null)
            {
                return;
            }
            var bannerLoaderBuilder = new AndroidJavaObject(BannerAdLoaderClassName);
            bannerLoaderBuilder?.Call<AndroidJavaObject>(BannerAdLoaderExtMethod, request.ExtraInfoJson);
            bannerLoaderBuilder?.Call<AndroidJavaObject>(BannerAdLoaderBuildMethod, new AdLoadCallback(OnAdLoad, OnLoadFailed));
            var bannerLoader = bannerLoaderBuilder?.Call<AndroidJavaObject>(BannerBuildMethod);
        
            var bannerRequestBuilder = new AndroidJavaObject(BannerAdRequestClassName);
            bannerRequestBuilder?.Call<AndroidJavaObject>("withSlotId", slotId);
            bannerRequestBuilder?.Call<AndroidJavaObject>("withAge", request.Age);
            bannerRequestBuilder?.Call<AndroidJavaObject>("withGender", (int)(request.Gender));
            bannerRequestBuilder?.Call<AndroidJavaObject>("withActivatedTime", request.ActivatedTime);
            
            var bannerSize = new AndroidJavaClass(BannerSizeClassName).GetStatic<AndroidJavaObject>("BANNER");
            int width = request.Size.Width;
            int height = request.Size.Height;
            if (width == 300 && height == 250) {
                bannerSize = new AndroidJavaClass(BannerSizeClassName).GetStatic<AndroidJavaObject>("MEDIUM_RECTANGLE");
            }
            AndroidJavaClass arrayClass = new AndroidJavaClass("java.lang.reflect.Array");
            AndroidJavaObject arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass(BannerSizeClassName), 1);
            arrayClass.CallStatic("set", arrayObject, 0, bannerSize);

            bannerRequestBuilder?.Call<AndroidJavaObject>("withAdSizes", arrayObject);

            var bannerRequest = bannerRequestBuilder?.Call<AndroidJavaObject>(BannerBuildMethod);

            bannerLoader?.Call("loadAd", bannerRequest);
        }

        public bool IsLoaded()
        {
            return BannerAd != null;
        }

        public void Show()
        {
            BannerAd?.Call("setAdInteractionListener", new AdInteractionCallback(OnAdShowed, OnAdClicked, OnAdDismissed, OnAdError));
            AndroidPlatformTool.CallMethodOnMainThread(() => 
            {
                AdHelper.ShowBannerAd(BannerAd);
            });
        }

        public void Destroy()
        {
            //post to main 
            AdHelper.DestroyAd(BannerAd);
        }

        public bool IsExpired()
        {

            return BannerAd != null && BannerAd.Call<bool>("isExpired");
        }

        public void SetPosition(BigoPosition position)
        {
            AndroidPlatformTool.CallMethodOnMainThread(() => 
            {
                AdHelper.ShowBannerAd(BannerAd, position);
            });
        }

        public bool IsClientBidding()
        {
            if (BannerAd == null) return false;
            AndroidJavaObject bid = BannerAd.Call<AndroidJavaObject>("getBid");
            return bid != null;
        }

        /// get price
        public double getPrice()
        {
            if (BannerAd == null) return 0;
            AndroidJavaObject bid = BannerAd.Call<AndroidJavaObject>("getBid");
            return bid == null ? 0 : bid.Call<double>("getPrice");
        }

        ///notify win
        public void notifyWin(double secPrice, string secBidder)
        {
            if (BannerAd == null) return;
            var secPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", secPrice);
            BannerAd.Call<AndroidJavaObject>("getBid")?.Call("notifyWin", secPriceDouble, secBidder);
        }

        ///notify loss
        public void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason)
        {
            if (BannerAd == null) return;
            var firstPriceDouble = new AndroidJavaClass("java.lang.Double").CallStatic<AndroidJavaObject> ("valueOf", firstPrice);
            BannerAd.Call<AndroidJavaObject>("getBid")?.Call("notifyLoss", firstPriceDouble, firstBidder, (int)lossReason);
        }

    }
}
#endif