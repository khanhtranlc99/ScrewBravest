#if UNITY_IOS

using System.Collections.Generic;
using AOT;
using UnityEngine;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Platforms.iOS.Adapter.BigoAd
{
    using System;
    using BigoAds.Scripts.Api;
    using BigoAds.Scripts.Common;
    using BigoAds.Scripts.Platforms.iOS;
    using System.Runtime.InteropServices;
    public class BigoIOSBaseAd : IBigoAd<BigoRequest>
    {
        public event Action OnLoad;
        public event Action<int, string> OnLoadFailed;
        public event Action OnAdShowed;
        public event Action OnAdClicked;
        public event Action OnAdDismissed;
        public event Action<int, string> OnAdError;
        public void Load(string slotId, BigoRequest request)
        {
            
        }
        
        protected void LoadAdData(string slotId, BigoRequest request) {
            this.unityAdPtr = (IntPtr)GCHandle.Alloc (this);
            IntPtr ptr = this.unityAdPtr;
            
            BigoIOS_loadAdData(adType, 
                ptr, 
                slotId,
                request.ToJson(), 
                cs_adDidLoadCallback, 
                cs_adLoadFailCallBack,
                cs_adDidShowCallback,
                cs_adDidClickCallback,
                cs_adDidDismissCallback,
                cs_adDidErrorCallBack
            );
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"slotId:{slotId},request{request}");
        }

        public bool IsLoaded()
        {
            return bigoAdLoaded;
        }

        public virtual void Show()
        {
            LOGWithMessage("Show");
            BigoIOS_showAd(adType,this.unityAdPtr);
        }

        public virtual bool IsExpired()
        {
            return BigoIOS_IsExpired(unityAdPtr);
        } 

        public void Destroy()
        {
            BigoIOS_destroyAd(adType,unityAdPtr);
            if (unityAdPtr != IntPtr.Zero)
            {
                GCHandle unityAdhandle = (GCHandle)this.unityAdPtr;
                unityAdhandle.Free();
            }
            unityAdPtr = IntPtr.Zero;
            LOGWithMessage(System.Reflection.MethodBase.GetCurrentMethod()?.Name,$"unityPtr:{unityAdPtr}");
        }

        public bool IsClientBidding()
        {
            return BigoIOS_IsClientBidding(unityAdPtr);
        }

        /// get price
        public double getPrice()
        {
            return BigoIOS_GetPrice(unityAdPtr);
        }

        ///notify win
        public void notifyWin(double secPrice, string secBidder)
        {
            BigoIOS_NotifyWin(unityAdPtr, secPrice, secBidder);
        }

        ///notify loss
        public void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason)
        {
            BigoIOS_NotifyLoss(unityAdPtr, firstPrice, firstBidder, (int)lossReason);
        }

        protected void LOGWithMessage(string method)
        {
            LOGWithMessage(method,string.Empty);
        }
        protected void LOGWithMessage(string method,string msg)
        {
            //native = 1, banner = 2, interstitial = 3, rewarded = 4, appOpen = 5
            int adtype = adType;
            string adTypeString = "unknown";
            if (adtype == 1)
            {
                adTypeString = "native";
            }
            else if (adtype == 2)
            {
                adTypeString = "banner";
            }
            else if (adtype == 3)
            {
                adTypeString = "interstitial";
            }
            else if (adtype == 4)
            {
                adTypeString = "rewarded";
            }
            else if (adtype == 5)
            {
                adTypeString = "appOpen";
            }

            //System.Reflection.MethodBase.GetCurrentMethod()?.Name
            Type classType = this.GetType();
            string className = $"{adTypeString}-{classType.Name}";
            BigoUnityTools.LOGWithMessage(className,method,msg);
        }

        protected static BigoIOSBaseAd GetUnityAd(IntPtr unityAdPtr)
        {
            GCHandle handle = (GCHandle) unityAdPtr;
            BigoIOSBaseAd unityAd = handle.Target as BigoIOSBaseAd;
            // handle.Free ();
            return unityAd;
        }

        protected int adType = 0;
        private bool bigoAdLoaded = false;
        protected IntPtr unityAdPtr = IntPtr.Zero;
        //MARK:  c --> oc
        [DllImport("__Internal")]
        static extern void BigoIOS_loadAdData(int adType, 
            IntPtr unityAdPtr, 
            string slotId,
            string requestJson,
        adDidLoadCallback_delegate successCallback,
            adLoadFailCallBack_delegate failCallback,
        adDidShowCallback_delegate showCallback,
            adDidClickCallback_delegate clickCallback,
            adDidDismissCallback_delegate dismissCallback,
            adDidErrorCallback_delegate adErrorCallback);
        [DllImport("__Internal")]
        static extern void BigoIOS_showAd(int adType,IntPtr unityAdPtr);

        [DllImport("__Internal")]
        static extern void BigoIOS_destroyAd(int adType, IntPtr unityAdPtr);
        
        [DllImport("__Internal")]
        static extern bool BigoIOS_IsExpired(IntPtr unityAdPtr);

        [DllImport("__Internal")]
        static extern bool BigoIOS_IsClientBidding(IntPtr unityAdPtr);

        [DllImport("__Internal")]
        static extern double BigoIOS_GetPrice(IntPtr unityAdPtr);

        [DllImport("__Internal")]
        static extern void BigoIOS_NotifyWin(IntPtr unityAdPtr, double secPrice, string secBidder);
        
        [DllImport("__Internal")]
        static extern void BigoIOS_NotifyLoss(IntPtr unityAdPtr, double firstPrice, string firstBidder, int lossReason);

        //callback method 
        protected delegate void adDidLoadCallback_delegate(IntPtr unityAdPtr);
        protected delegate void adLoadFailCallBack_delegate(IntPtr unityAdPtr, int code, string msg);
        
        protected delegate void adDidShowCallback_delegate(IntPtr unityAdPtr);
        protected delegate void adDidClickCallback_delegate(IntPtr unityAdPtr);
        protected delegate void adDidDismissCallback_delegate(IntPtr unityAdPtr);
        protected delegate void adDidErrorCallback_delegate(IntPtr unityAdPtr, int code, string msg);

        [MonoPInvokeCallback(typeof(adDidLoadCallback_delegate))]
        protected static void cs_adDidLoadCallback(IntPtr unityAdPtr)
        {
            BigoIOSBaseAd unityAd = BigoIOSBaseAd.GetUnityAd(unityAdPtr);
            if (unityAd == null)
            {
                return;
            }
            unityAd.bigoAdLoaded = true;
            unityAd.OnLoad?.Invoke();
            unityAd.LOGWithMessage("cs_adDidLoad");
        }

        [MonoPInvokeCallback(typeof(adLoadFailCallBack_delegate))]
        protected static void cs_adLoadFailCallBack(IntPtr unityAdPtr, int code, string msg)
        {
         
            BigoIOSBaseAd unityAd = BigoIOSBaseAd.GetUnityAd(unityAdPtr);
            if (unityAd.OnLoadFailed != null)
            {
                unityAd.OnLoadFailed(code, msg);
            }
            unityAd.LOGWithMessage("cs_adLoadFail",$"code:{code},msg:{msg}");
        }

        [MonoPInvokeCallback(typeof(adDidShowCallback_delegate))]
        protected static void cs_adDidShowCallback(IntPtr unityAdPtr)
        {
            BigoIOSBaseAd unityAd = BigoIOSBaseAd.GetUnityAd(unityAdPtr);
             if (unityAd == null)
             {
                 return;
             }
             unityAd.OnAdShowed?.Invoke();
             unityAd.LOGWithMessage($"cs_show");
        }

        [MonoPInvokeCallback(typeof(adDidClickCallback_delegate))]
        protected static void cs_adDidClickCallback(IntPtr unityAdPtr)
        {
            BigoIOSBaseAd unityAd = BigoIOSBaseAd.GetUnityAd(unityAdPtr);
            if (unityAd == null)
            {
                return;
            }
            unityAd.OnAdClicked?.Invoke();
            unityAd.LOGWithMessage("cs_click");
        }

        [MonoPInvokeCallback(typeof(adDidDismissCallback_delegate))]
        protected static void cs_adDidDismissCallback(IntPtr unityAdPtr)
        {
            BigoIOSBaseAd unityAd = BigoIOSBaseAd.GetUnityAd(unityAdPtr);
            if (unityAd == null)
            {
                return;
            }
            unityAd.OnAdDismissed?.Invoke();
            unityAd.LOGWithMessage("cs_dismiss");   
        }

        [MonoPInvokeCallback(typeof(adDidErrorCallback_delegate))]
        protected static void cs_adDidErrorCallBack(IntPtr unityAdPtr, int code, string msg)
        {
            BigoIOSBaseAd unityAd = BigoIOSBaseAd.GetUnityAd(unityAdPtr);
            if (unityAd.OnAdError != null)
            {
                unityAd.OnAdError(code, msg);
            }
            unityAd.LOGWithMessage("cs_aderror",$"code:{code},msg:{msg}");
        }
    }
}
#endif