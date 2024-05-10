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
    public class BigoUnityBannerAd: BigoIOSBaseAd, IBannerAd
    {
        private BigoBannerRequest bannerRequest;
        //TODO:线程问题需要考虑下
        public void Load(string slotId, BigoBannerRequest request)
        {
            bannerRequest = request;
            adType = 2;
            this.unityAdPtr = (IntPtr)GCHandle.Alloc (this);
            IntPtr ptr = this.unityAdPtr;

            int width = request.Size.Width;
            int height = request.Size.Height;
            
            int x = CalculateMiddleX(width);
            int y = CalculatePositionY(request.Position,height);
            
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"calculate-origin:{x},{y}-size:{width},{height}");
            BigoIOS_loadBannerAdData(ptr, 
                slotId,
                request.ToJson(),
                x,
                y,
                width,
                height,
                cs_adDidLoadCallback, 
                cs_adLoadFailCallBack,
                cs_adDidShowCallback,
                cs_adDidClickCallback,
                cs_adDidDismissCallback,
                cs_adDidErrorCallBack
                );
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"request.position:{request.Position},slotId:{slotId}");
        }

        private int CalculatePositionY(BigoPosition position, int bannerHeight)
        {
            int screenHeight = BigoIOS_getScreenHeight();
            float y = BigoIOS_getScreenSafeTop();
            if (position == BigoPosition.Middle)
            {
                y = (float)((screenHeight - bannerHeight) * 0.5);
            }
            else if (position == BigoPosition.Bottom)
            {
                y = (float)(screenHeight - bannerHeight - BigoIOS_getScreenSafeBottom());
            }
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"position:{position},screenHeight:{screenHeight}-y:{y}");
            return (int)y;
        }

        private int CalculateMiddleX(int bannerWidth)
        {
            int screenWidth = BigoIOS_getScreenWidth();
            int x = (int)((screenWidth - bannerWidth) * 0.5);
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"screenWidth:{screenWidth}-x:{x}");
            return x;
        }

        public void SetPosition(BigoPosition position)
        {
            int x = CalculateMiddleX(bannerRequest.Size.Width);
            int y = CalculatePositionY(position, bannerRequest.Size.Height);
            BigoIOS_SetBannerAdPosition(unityAdPtr,x,y);
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"position-{position}");
        }
        //MARK: c# - oc
        [DllImport("__Internal")]
        static extern void BigoIOS_loadBannerAdData(IntPtr unityAdPtr,
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
            adDidDismissCallback_delegate dismissCallback,
            adDidErrorCallback_delegate adErrorCallback
        );

        [DllImport("__Internal")]
        static extern void BigoIOS_SetBannerAdPosition(IntPtr unityAdPtr, int x, int y);

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