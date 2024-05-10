using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Common
{
    public interface ISDK
    {
        ///
        /// Starts the Bigo SDK
        /// 
        void Init(BigoAdConfig config, BigoAdSdk.InitResultDelegate initResultDelegate);

        ////
        /// The SDK initialization state
        ////
        bool IsInitSuccess();

        ///////
        /// Bigo SDK version
        /// ////
        string GetSDKVersion();


        ///////
        /// Bigo SDK version name
        /// ////
        string GetSDKVersionName();

        ///////
        /// Bigo SDK set user consent
        /// ////
        void SetUserConsent(ConsentOptions option, bool consent);

        ///////
        /// Only works on Android
        /// Bigo SDK set user consent
        /// ////
        void AddExtraHost(string country, string host);
    }
}