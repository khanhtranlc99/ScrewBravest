#if UNITY_IOS
using System.Collections.Generic;
using AOT;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd
{
    using System;
    using BigoAds.Scripts.Api;
    using BigoAds.Scripts.Common;
    using BigoAds.Scripts.Platforms.iOS;
    using System.Runtime.InteropServices;
    class BigoUnityRewardedAd : BigoIOSBaseAd, IRewardedAd
    {
        public event Action OnUserEarnedReward;

        public void Load(string slotId, BigoRewardedRequest request)
        {
            adType = 4;
            this.unityAdPtr = (IntPtr)GCHandle.Alloc (this);
            IntPtr ptr = this.unityAdPtr;
            
            BigoIOS_loadRewardedAdData(ptr,
                slotId,
                request.ToJson(), 
                cs_adDidLoadCallback, 
                cs_adLoadFailCallBack,
                cs_adDidShowCallback,
                cs_adDidClickCallback,
                cs_adDidDismissCallback, 
                cs_adDidErrorCallBack,
                cs_adDidEarnRewardCallback);
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"slotId:{slotId},request{request}");
        }

        // c-> oc
        [DllImport("__Internal")]
        static extern void BigoIOS_loadRewardedAdData(IntPtr unityAd,
        string slotId,
        string requestJson,
        adDidLoadCallback_delegate successCallback,
        adLoadFailCallBack_delegate failCallback,
        adDidShowCallback_delegate showCallback,
        adDidClickCallback_delegate clickCallback,
        adDidDismissCallback_delegate dismissCallback,
        adDidErrorCallback_delegate adErrorCallback,
        adDidEarnRewardCallback_delegate earnCallback);

        private delegate void adDidEarnRewardCallback_delegate(IntPtr unityAdPtr);

        [MonoPInvokeCallback(typeof(adDidEarnRewardCallback_delegate))]
        private static void cs_adDidEarnRewardCallback(IntPtr unityAdPtr)
        {
            BigoUnityRewardedAd unityAd = BigoIOSBaseAd.GetUnityAd(unityAdPtr) as BigoUnityRewardedAd;
            if (unityAd == null)
            {
                return;
            }
            unityAd.OnUserEarnedReward?.Invoke();
        }
    }
}
#endif
