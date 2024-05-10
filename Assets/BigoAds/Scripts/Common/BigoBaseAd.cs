using System;
using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Common
{
    public class BigoBaseAd<T>  where T : BigoRequest
    {
        /// <summary>
        /// event for load ad success
        /// </summary>
        public event Action OnLoad;

        /// <summary>
        /// load ad failed with error code and error message
        /// </summary>
        public event Action<int, string> OnLoadFailed;

        /// <summary>
        /// event for ad impression
        /// </summary>
        public event Action OnAdShowed;

        /// <summary>
        /// event for ad be clicked
        /// </summary>
        public event Action OnAdClicked;

        /// <summary>
        /// event for ad be closed
        /// </summary>
        public event Action OnAdDismissed;

        /// <summary>
        /// event for ad error
        /// </summary>
        public event Action<int, string> OnAdError;

        private readonly string _slotId;

        private bool _isAdLoaded;

        protected readonly IBigoAd<T> ADClient;

        public bool CallbackOnMainThread { get; set; } = true;

        private BigoAdBid _bid;

        protected BigoBaseAd(string id, IBigoAd<T> adClient)
        {
            _slotId = id;
            ADClient = adClient;
            InitEvent(ADClient);
        }

        private void InitEvent(IBigoAd<T> adClient)
        {
            adClient.OnLoad += InvokeOnLoad;
            adClient.OnLoadFailed += InvokeOnLoadFailed;

            adClient.OnAdShowed += InvokeOnAdShowed;
            adClient.OnAdClicked += InvokeOnAdClicked;
            adClient.OnAdDismissed += InvokeOnAdDismissed;
            adClient.OnAdError += InvokeOnAdError;
        }

        public void Load(T request)
        {
            if (_isAdLoaded)
            {
                InvokeOnLoad();
            }
            if (string.IsNullOrEmpty(_slotId))
            {
                InvokeOnLoadFailed(-1, "slotId must be not null");
                return;
            }

            if (!BigoAdSdk.IsInitSuccess())
            {
                InvokeOnLoadFailed(-1, "sdk has not init");
                return;
            }
            ADClient?.Load(_slotId, request);
        }

        public virtual void Show()
        {
            _isAdLoaded = false;
            ADClient?.Show();
        }

        public void DestroyAd()
        {
            ADClient?.Destroy();
        }

        public bool IsLoaded()
        {
            return _isAdLoaded;
        }

        public bool IsExpired()
        {
            return ADClient == null ? false : ADClient.IsExpired();
        }

        private void InvokeOnLoad()
        {
            OnLoad?.Invoke();
            _isAdLoaded = true;
        }

        protected void InvokeOnLoadFailed(int errorCode, string errorMessage)
        {
            if (CallbackOnMainThread)
            {
                BigoDispatcher.PostTask((() => { OnLoadFailed?.Invoke(errorCode, errorMessage); }));
            }
            else
            {
                OnLoadFailed?.Invoke(errorCode, errorMessage);
            }
        }


        private void InvokeOnAdShowed()
        {
            if (CallbackOnMainThread)
            {
                BigoDispatcher.PostTask((() => { OnAdShowed?.Invoke(); }));
            }
            else
            {
                OnAdShowed?.Invoke();
            }
        }

        private void InvokeOnAdClicked()
        {
            if (CallbackOnMainThread)
            {
                BigoDispatcher.PostTask((() => { OnAdClicked?.Invoke(); }));
            }
            else
            {
                OnAdClicked?.Invoke();
            }
        }

        private void InvokeOnAdDismissed()
        {
            if (CallbackOnMainThread)
            {
                BigoDispatcher.PostTask((() => { OnAdDismissed?.Invoke(); }));
            }
            else
            {
                OnAdDismissed?.Invoke();
            }
        }

        private void InvokeOnAdError(int errorCode, string errorMessage)
        {
            if (CallbackOnMainThread)
            {
                BigoDispatcher.PostTask((() => { OnAdError?.Invoke(errorCode, errorMessage); }));
            }
            else
            {
                OnAdError?.Invoke(errorCode, errorMessage);
            }
        }

        public BigoAdBid GetBid()
        {
            if (ADClient == null) return null;
            if (!ADClient.IsClientBidding()) return null;
            if (_bid == null)
            {
                _bid = new BigoAdBid(ADClient);
            }
            return _bid;
        }

        public class BigoAdBid : IClientBidding
        {
            protected readonly IBigoAd<T> _ADClient;

            public BigoAdBid(IBigoAd<T> ADClient)
            {
                _ADClient = ADClient;
            }
            /// get price
            public double getPrice()
            {
                return _ADClient == null ? 0 : _ADClient.getPrice();
            }

            ///notify win
            public void notifyWin(double secPrice, string secBidder)
            {
                _ADClient?.notifyWin(secPrice, secBidder);
            }

            ///notify loss
            public void notifyLoss(double firstPrice, string firstBidder, BGAdLossReason lossReason)
            {
                _ADClient?.notifyLoss(firstPrice, firstBidder, lossReason);
            }
        }
    }
}