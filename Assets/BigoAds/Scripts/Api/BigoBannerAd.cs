using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Api
{
    public class BigoBannerAd : BigoBaseAd<BigoBannerRequest>
    {
        private readonly IBannerAd _bannerAdClient;


        /// <summary>
        /// create a banner ad
        /// </summary>
        /// <param name="slotId"></param>
        public BigoBannerAd(string slotId) : base(slotId, BigoAdSdk.GetClientFactory().BuildBannerAdClient())
        {
            _bannerAdClient = (IBannerAd) ADClient;
        }

        /// <summary>
        /// set position for banner
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(BigoPosition position)
        {
            _bannerAdClient?.SetPosition(position);
        }
    }
}