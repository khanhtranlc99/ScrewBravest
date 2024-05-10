#if UNITY_ANDROID
using System;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    public class AdLoadCallback : AndroidJavaProxy
    {
        private readonly Action<AndroidJavaObject> onLoad;
        private readonly Action<int, string> onLoadFailed;

        public AdLoadCallback(Action<AndroidJavaObject> onLoad, Action<int, string> onLoadFailed) : base(
            AndroidPlatformTool.ClassPackage + ".api.AdLoadListener")
        {
            this.onLoad = onLoad;
            this.onLoadFailed = onLoadFailed;
        }

        public void onError(AndroidJavaObject error)
        {
            var code = error.Call<int>("getCode");
            var message = error.Call<string>("getMessage");
            onLoadFailed?.Invoke(code, message);
        }

        public void onAdLoaded(AndroidJavaObject ad)
        {
            onLoad?.Invoke(ad);
        }
    }

}
#endif