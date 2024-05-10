#import "BigoUnityBaseAdHandler.h"

extern "C" {

    //load appOpen Ad
    void BigoIOS_loadSplashAdData(UnityAd unityAd,
                                     const char *slotId,
                                     const char *requestJson,
                                     AdDidLoadCallback successCallback,
                                     AdLoadFailCallBack failCallback,
                                     AdDidShowCallback showCallback,
                                     AdDidClickCallback clickCallback,
                                     AdDidDismissCallback dismissCallback,
                                     AdDidErrorCallback adErrorCallback);
    //show open ad
    void BigoIOS_showSplashAd(UnityAd unityAd);
}
