#if UNITY_IOS
using System;
using System.Threading;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd
{
    using System;
    using BigoAds.Scripts.Api;
    using BigoAds.Scripts.Common;
    using BigoAds.Scripts.Platforms.iOS;
    using System.Runtime.InteropServices;
    public class BigoUnityNativeAd : BigoIOSBaseAd, INativeAd
    {
        public void Load(string slotId, BigoNativeRequest request)
        {
            adType = 1;
            LoadAdData(slotId,request);
        }
        
        private int CalculatePositionY(BigoPosition position, int nativeHeight)
        {
            int screenHeight = BigoIOS_getScreenHeight();
            float y = BigoIOS_getScreenSafeTop();
            if (position == BigoPosition.Middle)
            {
                y = (float)((screenHeight - nativeHeight) * 0.5);
            }
            else if (position == BigoPosition.Bottom)
            {
                y = (float)(screenHeight - nativeHeight - BigoIOS_getScreenSafeBottom());
            }
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"position:{position},screenHeight:{screenHeight}-y:{y}");
            return (int)y;
        }

        private int CalculateMiddleX(int nativeWidth)
        {
            int screenWidth = BigoIOS_getScreenWidth();
            int x = (int)((screenWidth - nativeWidth) * 0.5);
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"screenWidth:{screenWidth}-x:{x}");
            return x;
        }

        public void SetPosition(BigoPosition position)
        {
            int x = 0;
            int y = CalculatePositionY(position, 300); //dafault 300
            BigoIOS_SetNativeAdPosition(unityAdPtr,x,y);
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"position-{position}");
        }
        //MARK: c# - oc
        [DllImport("__Internal")]
        static extern void BigoIOS_loadNativeAdData(IntPtr unityAdPtr,
            string slotId,
            string requestJson,
        int x,
        int y,
        int width,
        int height,
            adDidLoadCallback_delegate successCallback,
            adLoadFailCallBack_delegate failCallback,
            adDidShowCallback_delegate showCallback,
            adDidClickCallback_delegate clickCallback,
            adDidDismissCallback_delegate dismissCallback
        );

        [DllImport("__Internal")]
        static extern void BigoIOS_SetNativeAdPosition(IntPtr unityAdPtr, int x, int y);

        [DllImport("__Internal")]
        static extern int BigoIOS_getScreenWidth();
        [DllImport("__Internal")]
        static extern int BigoIOS_getScreenHeight();
        [DllImport("__Internal")]
        static extern int BigoIOS_getScreenSafeTop();
        [DllImport("__Internal")]
        static extern int BigoIOS_getScreenSafeBottom();
    }
}
#endif