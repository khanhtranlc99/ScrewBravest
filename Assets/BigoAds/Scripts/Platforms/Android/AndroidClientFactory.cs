#if UNITY_ANDROID

using BigoAds.Scripts.Common;

namespace BigoAds.Scripts.Platforms.Android
{
    class AndroidClientFactory : IClientFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISDK BuildSDKClient()
        {
            return new BigoSdkClient();
        }

        public IBannerAd BuildBannerAdClient()
        {
            return new AndroidBannerAd();
        }

        public INativeAd BuildNativeAdClient()
        {
            return new AndroidNativeAd();
        }

        public IInterstitialAd BuildInterstitialAdClient()
        {
            return new AndroidInterstitialAd();
        }

        public ISplashAd BuildSplashAdClient()
        {
            return new AndroidSplashAd();
        }

        public IRewardedAd BuildRewardedAdClient()
        {
            return new AndroidRewardedAd();
        }
    }
}
#endif