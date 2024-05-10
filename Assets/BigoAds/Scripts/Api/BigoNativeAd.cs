using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Api
{
    public class BigoNativeAd : BigoBaseAd<BigoNativeRequest>
    {
        private readonly INativeAd _NativeAdClient;

        public BigoNativeAd(string slotId) : base(slotId, BigoAdSdk.GetClientFactory().BuildNativeAdClient())
        {
            _NativeAdClient = (INativeAd) ADClient;
        }

        /// <summary>
        /// set position for native
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(BigoPosition position)
        {
            _NativeAdClient?.SetPosition(position);
        }

    }
}