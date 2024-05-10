using System;
using BigoAds.Scripts.Api;

namespace BigoAds.Scripts.Common
{
    public interface IRewardedAd : IBigoAd<BigoRewardedRequest>
    {
        event Action OnUserEarnedReward;
    }
}