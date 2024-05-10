#import "BigoUnityBaseAdHandler.h"

extern "C" {

    //load Native ad
    void BigoIOS_loadNativeAdData(UnityAd unityAd,
                                    const char *slotId,
                                    const char *requestJson,
                                    AdDidLoadCallback successCallback,
                                    AdLoadFailCallBack failCallback,
                                    AdDidShowCallback showCallback,
                                    AdDidClickCallback clickCallback,
                                    AdDidDismissCallback dismissCallback,
                                    AdDidErrorCallback adErrorCallback
                                    );

    void BigoIOS_SetNativeAdPosition(UnityAd unityAd,int x, int y);
    void BigoIOS_showNativeAd(UnityAd unityAd);
    void BigoIOS_removeNativeView(UnityAd unityAd);

}
