#if UNITY_IOS
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

    class BigoUnitySdk : ISDK 
    {
        private BigoAdSdk.InitResultDelegate resultDelegate;

        public void Init(BigoAdConfig config, BigoAdSdk.InitResultDelegate initResultDelegate) 
        {
            BigoUnityConfig unityConfig = new BigoUnityConfig();
            unityConfig.Init(config);
            resultDelegate = initResultDelegate;
            IntPtr ptr = (IntPtr)GCHandle.Alloc(this);
            BigoIOS_initSDK(ptr, unityConfig.getInternalConfig(), cs_successCallback);
        }

        public bool IsInitSuccess()
        {
            return BigoIOS_sdkInitializationState();
        }

        public string GetSDKVersion()
        {
            return BigoIOS_sdkVersion();
        }

        public string GetSDKVersionName()
        {
            return BigoIOS_sdkVersionName();
        }

        public void SetUserConsent(ConsentOptions option, bool consent)
        {
            if (option == ConsentOptions.GDPR) {
                BigoIOS_setConsentGDPR(consent);
            } else if (option == ConsentOptions.CCPA) {
                BigoIOS_setConsentCCPA(consent);
            }
        }

        public void AddExtraHost(string country, string host)
        {
            BigoIOS_addExtraHost(country, host);
        }

        [DllImport("__Internal")]
        static extern void BigoIOS_initSDK(IntPtr unitySDK, IntPtr config, SuccessCallbackDelegate successCallback);

        [DllImport("__Internal")]
        static extern bool BigoIOS_sdkInitializationState();
        
        [DllImport("__Internal")]
        static extern string BigoIOS_sdkVersion();

        [DllImport("__Internal")]
        static extern string BigoIOS_sdkVersionName();

        [DllImport("__Internal")]
        static extern void BigoIOS_setConsentGDPR(bool consent);

        [DllImport("__Internal")]
        static extern void BigoIOS_setConsentCCPA(bool consent);

        [DllImport("__Internal")]
        static extern void BigoIOS_addExtraHost(string country, string host);

        private delegate void SuccessCallbackDelegate(IntPtr unitySDK);

        [MonoPInvokeCallback(typeof(SuccessCallbackDelegate))]
        private static void cs_successCallback(IntPtr unitySDK)
        {
            BigoUnitySdk sdk = GetUnitySdk(unitySDK);
            sdk.resultDelegate();
        }

        private static BigoUnitySdk GetUnitySdk(IntPtr unitySdkPtr)
        {
            GCHandle handle = (GCHandle) unitySdkPtr;
            BigoUnitySdk unitysdk = handle.Target as BigoUnitySdk;
            return unitysdk;
        }
    }
}

#endif