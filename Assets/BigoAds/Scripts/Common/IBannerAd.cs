using BigoAds.Scripts.Api;
using BigoAds.Scripts.Api.Constant;

namespace BigoAds.Scripts.Common
{
    public interface IBannerAd : IBigoAd<BigoBannerRequest>
    {
        void SetPosition(BigoPosition position);
    }
}