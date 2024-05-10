#import "BigoUnityBaseAdHandler.h"

extern "C" {

    //load Rewarded Ad
    void BigoIOS_loadRewardedAdData(UnityAd unityAd,
                                     const char *slotId,
                                     const char *requestJson,
                                     AdDidLoadCallback successCallback,
                                     AdLoadFailCallBack failCallback,
                                     AdDidShowCallback showCallback,
                                     AdDidClickCallback clickCallback,
                                     AdDidDismissCallback dismissCallback,
                                     AdDidErrorCallback adErrorCallback,
                                     AdDidEarnRewardCallback earnCallback);
    //show open ad
    void BigoIOS_showRewardedAd(UnityAd unityAd);
}
