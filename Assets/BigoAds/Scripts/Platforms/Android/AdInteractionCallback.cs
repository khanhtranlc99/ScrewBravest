#if UNITY_ANDROID

using System;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    public class AdInteractionCallback : AndroidJavaProxy
    {
        private readonly Action OnAdShowed;
        protected readonly Action OnAdDismissed;
        private readonly Action OnAdClicked;
        private readonly Action<int, string> OnAdError;
        private const string ListenerName = AndroidPlatformTool.ClassPackage + ".api.AdInteractionListener";

        public AdInteractionCallback(Action onAdShowed, Action onAdClicked, Action onAdDismissed,  Action<int, string> onAdError,
            string listenerName = ListenerName) : base(listenerName)
        {
            OnAdShowed = onAdShowed;
            OnAdDismissed = onAdDismissed;
            OnAdClicked = onAdClicked;
            OnAdError = onAdError;
        }

        public void onAdImpression()
        {
            OnAdShowed?.Invoke();
        }

        public void onAdClosed()
        {
            OnAdDismissed?.Invoke();
        }

        public void onAdClicked()
        {
            OnAdClicked?.Invoke();
        }

        public void onAdError(AndroidJavaObject error)
        {
            var code = error.Call<int>("getCode");
            var message = error.Call<string>("getMessage");
            OnAdError?.Invoke(code, message);
        }

        public void onAdOpened()
        {
            
        }
    }
}
#endif