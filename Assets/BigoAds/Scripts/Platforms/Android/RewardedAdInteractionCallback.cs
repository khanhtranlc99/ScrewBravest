#if UNITY_ANDROID
using System;
using BigoAds.Scripts.Common;
using UnityEngine;

namespace BigoAds.Scripts.Platforms.Android
{
    public class RewardedAdInteractionCallback : AdInteractionCallback
    {
        private event Action OnUserEarnedReward;
        
        private const string ListenerName = AndroidPlatformTool.ClassPackage + ".api.RewardAdInteractionListener";

        public RewardedAdInteractionCallback(Action onAdShowed, Action onAdClicked, Action onAdDismissed, Action<int, string> onAdError,
            Action userEarnedReward) : base(
            onAdShowed, onAdClicked, onAdDismissed, onAdError, ListenerName)
        {
            OnUserEarnedReward = userEarnedReward;
        }

        public void onAdRewarded()
        {
            OnUserEarnedReward?.Invoke();
        }
    }
}
#endif