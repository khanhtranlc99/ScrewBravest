using System;
using BigoAds.Scripts.Common;

namespace BigoAds.Scripts.Api
{
    public class BigoRewardedAd : BigoBaseAd<BigoRewardedRequest>
    {
        public event Action OnUserEarnedReward;

        public BigoRewardedAd(string slotId) : base(slotId, BigoAdSdk.GetClientFactory().BuildRewardedAdClient())
        {
            var rewardedAdClient = (IRewardedAd) ADClient;
            rewardedAdClient.OnUserEarnedReward += InvokeOnUserEarnedReward;
        }


        private void InvokeOnUserEarnedReward()
        {
            if (CallbackOnMainThread)
            {
                BigoDispatcher.PostTask((() => { OnUserEarnedReward?.Invoke(); }));
            }
            else
            {
                OnUserEarnedReward?.Invoke();
            }
        }
    }
}