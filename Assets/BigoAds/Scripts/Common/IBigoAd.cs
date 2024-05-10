using System;
using BigoAds.Scripts.Api;

namespace BigoAds.Scripts.Common
{
    public interface IBigoAd<in T> : IClientBidding where T : BigoRequest
    {
        event Action OnLoad;
        event Action<int, string> OnLoadFailed;
        event Action OnAdShowed;
        event Action OnAdClicked;
        event Action OnAdDismissed;
        event Action<int, string> OnAdError;
        void Load(string slotId, T request);
        bool IsLoaded();
        bool IsExpired();
        void Show();
        void Destroy();
        bool IsClientBidding();
    }
}