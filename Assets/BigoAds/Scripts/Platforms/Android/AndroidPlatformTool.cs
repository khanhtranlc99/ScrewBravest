#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using System.Text;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    internal static class AndroidPlatformTool
    {
        public const string ClassPackage = "sg.bigo.ads";
        private const string UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
        private const string CurrentActivityMethod = "currentActivity";
        private const string ResourceUtilClassName = ClassPackage + ".ResourceUtil";
        private const string AdConfigClassName = ClassPackage + ".api.AdConfig$Builder";

        public static AndroidJavaObject GetGameActivity()
        {
            return new AndroidJavaClass(UnityPlayerClassName).GetStatic<AndroidJavaObject>(CurrentActivityMethod);
        }

        public static AndroidJavaObject GetBigoConfig(BigoAdConfig config)
        {
            var builder = new AndroidJavaObject(AdConfigClassName);
            if (config != null)
            {
                void CallNativeFunction<T>(string methodName, T arg)
                {
                    builder.Call<AndroidJavaObject>(methodName, arg);
                }

                if (!string.IsNullOrEmpty(config.AppId))
                {
                    CallNativeFunction("setAppId", config.AppId);
                }

                if (config.DebugLog) //default value is false
                {
                    Debug.Log("set debug true");
                    CallNativeFunction("setDebug", true);
                }

                if (!string.IsNullOrEmpty(config.Channel))
                {
                    CallNativeFunction("setChannel", config.Channel);
                }

                if (config.Age > 0)
                {
                    CallNativeFunction("setAge", config.Age);
                }

                if (config.Gender > 0)
                {
                    CallNativeFunction("setGender", config.Gender);
                }

                if (config.ActivatedTime > 0)
                {
                    CallNativeFunction("setActivatedTime", config.ActivatedTime);
                }
                
                foreach (KeyValuePair<string, string> item in config.ExtraDictionary)
                {
                    Debug.Log($"bigo sdk config extra:" + item.Key + "=" + item.Value);
                    builder.Call<AndroidJavaObject>("addExtra", item.Key, item.Value);
                }
            }

            return builder.Call<AndroidJavaObject>("build");
        }

        public static AndroidJavaObject GetAdRequest(BigoRequest request)
        {
            switch (request)
            {
                case BigoBannerRequest _:
                    break;
                case BigoNativeRequest _:
                    break;
                case BigoSplashRequest _:
                    break;
                case BigoInterstitialRequest _:
                    break;
                case BigoRewardedRequest _:
                    break;
            }

            return null;
        }

        public static void CallMethodOnMainThread(Action task)
        {
            GetGameActivity()?.Call("runOnUiThread", new AndroidJavaRunnable(task));
        }
    }
}
#endif