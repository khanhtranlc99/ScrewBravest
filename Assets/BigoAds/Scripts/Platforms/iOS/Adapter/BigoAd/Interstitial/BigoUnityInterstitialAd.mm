#import <BigoADS/BigoADS.h>
#import "UnityAppController.h"
#import "BigoUnityBaseAdHandler.h"
#import "BigoUnityAdapterTools.h"
#import "BigoUnityAdHandlerManager.h"

extern "C"{

//load interstitial ad
void BigoIOS_loadInterstitialAdData(UnityAd unityAd,
                                      const char *slotId,
                                      const char *requestJson,
                                      AdDidLoadCallback successCallback,
                                      AdLoadFailCallBack failCallback,
                                      AdDidShowCallback showCallback,
                                      AdDidClickCallback clickCallback,
                                      AdDidDismissCallback dismissCallback,
                                      AdDidErrorCallback adErrorCallback
                                      ) {
     BigoIOS_dispatchSyncMainQueue(^{

            BigoUnityInterstitialAdHandler *adHandler = [[BigoUnityInterstitialAdHandler alloc] init];
            BigoInterstitialAdRequest *request = [[BigoInterstitialAdRequest alloc] initWithSlotId:BigoIOS_transformNSStringForm(slotId)];
            NSDictionary *requestDict = BigoIOS_requestJsonObjectFromJsonString(requestJson);
            request.age = [requestDict[@"age"] intValue];
            request.gender = (BigoAdGender)[requestDict[@"gender"] intValue];
            request.activatedTime = [requestDict[@"activatedTime"] longLongValue];
            BigoInterstitialAdLoader *adLoader = [[BigoInterstitialAdLoader alloc] initWithInterstitialAdLoaderDelegate:adHandler];
            adLoader.ext = requestDict[@"extraInfo"];
            adHandler.adLoader = adLoader;
            adHandler.unityAd = unityAd;
            adHandler.showCallback = showCallback;
            adHandler.clickCallback = clickCallback;
            adHandler.dismissCallback = dismissCallback;
            adHandler.adErrorCallback = adErrorCallback;
            adHandler.successCallback = successCallback;
            adHandler.failCallback = failCallback;
            NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
            [BigoUnityAdHandlerManager saveAdHandler:adHandler withKey:unityAdKey];
            [adLoader loadAd:request];
    });

}

BigoUnityInterstitialAdHandler* BigoIOS_getInterstitialAdHandler(UnityAd unityAd) {
    if (!unityAd) return nil;
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityInterstitialAdHandler *handler = (BigoUnityInterstitialAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    if (!handler || ![handler isMemberOfClass:[BigoUnityInterstitialAdHandler class]]) {
        return nil;
    }
    return handler;
}

void BigoIOS_showInterstitialAd(UnityAd unityAd) {
    BigoUnityInterstitialAdHandler *handler = BigoIOS_getInterstitialAdHandler(unityAd);
    if (!handler) return;
    
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        BigoInterstitialAd *ad = (BigoInterstitialAd *)handler.ad;
        [ad show:vc];
    });
}

}

