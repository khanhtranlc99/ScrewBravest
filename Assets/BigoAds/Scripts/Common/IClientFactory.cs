namespace BigoAds.Scripts.Common
{
    public interface IClientFactory
    {
        ISDK BuildSDKClient();
        IBannerAd BuildBannerAdClient();
        INativeAd BuildNativeAdClient();
        IInterstitialAd BuildInterstitialAdClient();
        ISplashAd BuildSplashAdClient();
        IRewardedAd BuildRewardedAdClient();
    }
}