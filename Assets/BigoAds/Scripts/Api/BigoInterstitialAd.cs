using BigoAds.Scripts.Common;

namespace BigoAds.Scripts.Api
{
    public class BigoInterstitialAd : BigoBaseAd<BigoInterstitialRequest>
    {
        public BigoInterstitialAd(string slotId) : base(slotId, BigoAdSdk.GetClientFactory().BuildInterstitialAdClient())
        {
        }
    }
}