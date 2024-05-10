#import "BigoUnityBaseAdHandler.h"

extern "C" {

    //load banner Ad
    void BigoIOS_loadBannerAdData(UnityAd unityAd,
                                    const char *slotId,
                                    const char *requestJson,
                                    int x,
                                    int y,
                                    int width,
                                    int height,
                                    AdDidLoadCallback successCallback,
                                    AdLoadFailCallBack failCallback,
                                    AdDidShowCallback showCallback,
                                    AdDidClickCallback clickCallback,
                                    AdDidDismissCallback dismissCallback,
                                    AdDidErrorCallback adErrorCallback
                                    );
    //show banner ad
    void BigoIOS_showBannerAd(UnityAd unityAd);
    void BigoIOS_SetBannerAdPosition(UnityAd unityAd,int x, int y);
    void BigoIOS_removeBannerView(UnityAd unityAd);
}
