using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Common
{
    public interface INativeAd : IBigoAd<BigoNativeRequest>
    {
        void SetPosition(BigoPosition position);
    }
}