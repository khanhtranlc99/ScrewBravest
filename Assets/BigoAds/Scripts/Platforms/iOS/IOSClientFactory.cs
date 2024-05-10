#if UNITY_IOS
using BigoAds.Scripts.Common;
using BigoAds.Scripts.Platforms.iOS;
using BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd;

namespace BigoAds.Scripts.Platforms.iOS
{
    class IOSClientFactory : IClientFactory
    {

        public ISDK BuildSDKClient()
        {
            return new BigoUnitySdk();
        }

        public IBannerAd BuildBannerAdClient()
        {
            return new BigoUnityBannerAd();
        }

        public INativeAd BuildNativeAdClient()
        {
            return new BigoUnityNativeAd();
        }

        public IInterstitialAd BuildInterstitialAdClient()
        {
            return new BigoUnityinterstitialAd();
        }

        public ISplashAd BuildSplashAdClient()
        {
            return new BigoUnitySplashAd();
        }
        
        public IRewardedAd BuildRewardedAdClient()
        {
            return new BigoUnityRewardedAd();
        }
    }
}
#endif