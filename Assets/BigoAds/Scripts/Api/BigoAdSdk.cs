using BigoAds.Scripts.Common;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Api
{
    public static class BigoAdSdk
    {
        private static IClientFactory _clientFactory;

        private static ISDK _sdk;

        internal static ISDK SDK
        {
            get
            {
                if (_sdk == null)
                {
                    _sdk = GetClientFactory().BuildSDKClient();
                }

                return _sdk;
            }
        }

        internal static IClientFactory GetClientFactory()
        {
            if (_clientFactory != null)
            {
                return _clientFactory;
            }

            _clientFactory =
#if UNITY_ANDROID
                new BigoAds.Scripts.Platforms.Android.AndroidClientFactory();
#elif UNITY_IOS
                new BigoAds.Scripts.Platforms.iOS.IOSClientFactory();
#else
                null;
            throw new PlatformNotSupportedException();
#endif
            return _clientFactory;
        }

        public delegate void InitResultDelegate();

        public static event InitResultDelegate OnInitFinish;

                /// Starts the Bigo SDK
        /// @warning Call this method as early as possible to reduce  ad request fail.
        /// @param config SDK configuration
        /// @param callback Callback for starting the Bigo SDK
        /// ////
        public static void Initialize(BigoAdConfig config)
        {
            if (IsInitSuccess())
            {
                OnInitFinish?.Invoke();
                return;
            }

            SDK.Init(config, (() => { OnInitFinish?.Invoke(); }));
        }

        ////
        /// The SDK initialization state
        ////
        public static bool IsInitSuccess()
        {
            return SDK.IsInitSuccess();
        }

        ///////
        /// Bigo SDK version
        /// ////
        public static string GetSDKVersion()
        {
            return SDK.GetSDKVersion();
        }

        ///////
        /// Bigo SDK version name
        /// ////
        public static string GetSDKVersionName()
        {
            return SDK.GetSDKVersionName();
        }

        ///////
        /// Bigo SDK set user consent
        /// ////
        public static void SetUserConsent(ConsentOptions option, bool consent)
        {
            SDK.SetUserConsent(option, consent);
        }

        ///////
        /// Only works on Android
        /// Bigo SDK set user consent
        /// ////
        public static void AddExtraHost(string country, string host)
        {
            SDK.AddExtraHost(country, host);
        }
        
    }
}
