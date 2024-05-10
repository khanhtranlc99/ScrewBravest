#if UNITY_IOS
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Common;

namespace BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd
{

    class BigoUnitySplashAd : BigoIOSBaseAd, ISplashAd
    {
        public void Load(string slotId, BigoSplashRequest request)
        {
            adType = 5;
            LoadAdData(slotId,request);
        }
    }
}
#endif
