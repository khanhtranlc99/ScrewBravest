#if UNITY_ANDROID
using System;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    public class SplashAdInteractionCallback : AdInteractionCallback
    {       
        private const string ListenerName = AndroidPlatformTool.ClassPackage + ".api.SplashAdInteractionListener";

        public SplashAdInteractionCallback(Action onAdShowed, Action onAdClicked, Action onAdDismissed, Action<int, string> onAdError) : base(
            onAdShowed, onAdClicked, onAdDismissed, onAdError, ListenerName)
        {
            
        }

        public void onAdSkipped()
        {
            
        }

        public void onAdFinished()
        {
            
        }
    }
}
#endif