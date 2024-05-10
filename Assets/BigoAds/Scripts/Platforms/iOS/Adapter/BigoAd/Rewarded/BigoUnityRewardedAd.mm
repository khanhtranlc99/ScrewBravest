#import <BigoADS/BigoADS.h>
#import "UnityAppController.h"
#import "BigoUnityBaseAdHandler.h"
#import "BigoUnityAdapterTools.h"
#import "BigoUnityAdHandlerManager.h"

extern "C"{

//load rewarded ad
void BigoIOS_loadRewardedAdData(UnityAd unityAd,
                                      const char *slotId,
                                      const char *requestJson,
                                      AdDidLoadCallback successCallback,
                                      AdLoadFailCallBack failCallback,
                                      AdDidShowCallback showCallback,
                                      AdDidClickCallback clickCallback,
                                      AdDidDismissCallback dismissCallback,
                                      AdDidErrorCallback adErrorCallback,
                                      AdDidEarnRewardCallback earnCallback
                                      ) {
     BigoIOS_dispatchSyncMainQueue(^{

            BigoUnityRewardedAdHandler *adHandler = [[BigoUnityRewardedAdHandler alloc] init];
            BigoRewardVideoAdRequest *request = [[BigoRewardVideoAdRequest alloc] initWithSlotId:BigoIOS_transformNSStringForm(slotId)];
            NSDictionary *requestDict = BigoIOS_requestJsonObjectFromJsonString(requestJson);
            request.age = [requestDict[@"age"] intValue];
            request.gender = (BigoAdGender)[requestDict[@"gender"] intValue];
            request.activatedTime = [requestDict[@"activatedTime"] longLongValue];
            BigoRewardVideoAdLoader *adLoader = [[BigoRewardVideoAdLoader alloc] initWithRewardVideoAdLoaderDelegate:adHandler];
            adLoader.ext = requestDict[@"extraInfo"];
            adHandler.adLoader = adLoader;
            adHandler.unityAd = unityAd;
            adHandler.showCallback = showCallback;
            adHandler.clickCallback = clickCallback;
            adHandler.dismissCallback = dismissCallback;
            adHandler.adErrorCallback = adErrorCallback;
            adHandler.successCallback = successCallback;
            adHandler.failCallback = failCallback;
            adHandler.earnCallback = earnCallback;
            NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
            [BigoUnityAdHandlerManager saveAdHandler:adHandler withKey:unityAdKey];
            [adLoader loadAd:request];
    });

}

BigoUnityRewardedAdHandler* BigoIOS_getRewardedAdHandler(UnityAd unityAd) {
    if (!unityAd) return nil;
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityRewardedAdHandler *handler = (BigoUnityRewardedAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    if (!handler || ![handler isMemberOfClass:[BigoUnityRewardedAdHandler class]]) {
        return nil;
    }
    return handler;
}

void BigoIOS_showRewardedAd(UnityAd unityAd) {
    BigoUnityRewardedAdHandler *handler = BigoIOS_getRewardedAdHandler(unityAd);
    if (!handler) return;
    
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        BigoRewardVideoAd *ad = (BigoRewardVideoAd *)handler.ad;
        [ad show:vc];
    });
}

}

