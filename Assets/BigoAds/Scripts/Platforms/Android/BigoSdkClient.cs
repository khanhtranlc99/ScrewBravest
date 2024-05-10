#if UNITY_ANDROID

using BigoAds.Scripts.Api;
using BigoAds.Scripts.Common;
using UnityEngine;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Platforms.Android
{
    class BigoSdkClient : ISDK
    {
        private const string SDKClientClassName = AndroidPlatformTool.ClassPackage + ".BigoAdSdk";
        private const string InitMethod = "initialize";
        private const string InitSuccessMethod = "isInitialized";
        private const string SDKVersionMethod = "getSDKVersion";
        private const string InitListenerInterfaceName = AndroidPlatformTool.ClassPackage + ".BigoAdSdk$InitListener";
        private const string ConsentOptionsClassName = AndroidPlatformTool.ClassPackage + ".ConsentOptions";


        public void Init(BigoAdConfig config, BigoAdSdk.InitResultDelegate initResultDelegate)
        {
            InvokeNativeMethod(InitMethod, AndroidPlatformTool.GetGameActivity(),
                AndroidPlatformTool.GetBigoConfig(config),
                new InitCallBack(initResultDelegate));
        }

        public bool IsInitSuccess()
        {
            return InvokeNativeMethod<bool>(InitSuccessMethod);
        }

        public string GetSDKVersion()
        {
            return InvokeNativeMethod<string>("getSDKVersion");
        }

        public string GetSDKVersionName()
        {
            return InvokeNativeMethod<string>("getSDKVersionName");
        }

        public void SetUserConsent(ConsentOptions option, bool consent)
        {
            var clazz = new AndroidJavaClass(ConsentOptionsClassName);
            AndroidJavaObject obj = null;
            switch (option)
            {
                case ConsentOptions.GDPR:
                    obj = clazz.GetStatic<AndroidJavaObject>("GDPR");
                    break;
                case ConsentOptions.CCPA:
                    obj = clazz.GetStatic<AndroidJavaObject>("CCPA");
                    break;
            }

            InvokeNativeMethod("setUserConsent", AndroidPlatformTool.GetGameActivity(), obj, consent);
        }

        public void AddExtraHost(string country, string host)
        {
            InvokeNativeMethod("addExtraHost", country, host);
        }


        private static void InvokeNativeMethod(string methodName, params object[] args)
        {
            new AndroidJavaClass(SDKClientClassName).CallStatic(methodName, args);
        }

        private static T InvokeNativeMethod<T>(string methodName, params object[] args)
        {
            return new AndroidJavaClass(SDKClientClassName).CallStatic<T>(methodName, args);
        }

        private class InitCallBack : AndroidJavaProxy
        {
            private event BigoAdSdk.InitResultDelegate InitListener;

            public InitCallBack(BigoAdSdk.InitResultDelegate initResultDelegate) : base(InitListenerInterfaceName)
            {
                this.InitListener = initResultDelegate;
            }

            public void onInitialized()
            {
                InitListener?.Invoke();
            }
        }
    }
}
#endif