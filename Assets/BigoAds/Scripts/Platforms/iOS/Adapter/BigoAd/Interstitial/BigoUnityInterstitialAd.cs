#if UNITY_IOS
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Common;

namespace BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd
{

    class BigoUnityinterstitialAd : BigoIOSBaseAd, IInterstitialAd
    {
        public void Load(string slotId, BigoInterstitialRequest request)
        {
            adType = 3;
            LoadAdData(slotId,request);
        }
    }
}
#endif
