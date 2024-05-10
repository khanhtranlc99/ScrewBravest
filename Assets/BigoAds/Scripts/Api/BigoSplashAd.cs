using BigoAds.Scripts.Common;

namespace BigoAds.Scripts.Api
{
    public class BigoSplashAd : BigoBaseAd<BigoSplashRequest>
    {
        public BigoSplashAd(string slotId) : base(slotId, BigoAdSdk.GetClientFactory().BuildSplashAdClient())
        {
        }
    }
}