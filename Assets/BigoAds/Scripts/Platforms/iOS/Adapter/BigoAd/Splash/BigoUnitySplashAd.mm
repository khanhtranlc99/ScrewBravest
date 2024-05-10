#import <BigoADS/BigoADS.h>
#import "UnityAppController.h"
#import "BigoUnityBaseAdHandler.h"
#import "BigoUnityAdapterTools.h"
#import "BigoUnityAdHandlerManager.h"
#import "BGSplashAdViewController.h"

extern "C"{

//load splash ad
void BigoIOS_loadSplashAdData(UnityAd unityAd,
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

            BigoUnitySplashAdHandler *adHandler = [[BigoUnitySplashAdHandler alloc] init];
            BigoSplashAdRequest *request = [[BigoSplashAdRequest alloc] initWithSlotId:BigoIOS_transformNSStringForm(slotId)];
            NSDictionary *requestDict = BigoIOS_requestJsonObjectFromJsonString(requestJson);
            request.age = [requestDict[@"age"] intValue];
            request.gender = (BigoAdGender)[requestDict[@"gender"] intValue];
            request.activatedTime = [requestDict[@"activatedTime"] longLongValue];
            BigoSplashAdLoader *adLoader = [[BigoSplashAdLoader alloc] initWithSplashAdLoaderDelegate:adHandler];
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

BigoUnitySplashAdHandler* BigoIOS_getSplashAdHandler(UnityAd unityAd) {
    if (!unityAd) return nil;
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnitySplashAdHandler *handler = (BigoUnitySplashAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    if (!handler || ![handler isMemberOfClass:[BigoUnitySplashAdHandler class]]) {
        return nil;
    }
    return handler;
}

void BigoIOS_showSplashAd(UnityAd unityAd) {
    BigoUnitySplashAdHandler *handler = BigoIOS_getSplashAdHandler(unityAd);
    if (!handler) return;
    
    BigoIOS_dispatchSyncMainQueue(^{
        BGSplashAdViewController *vc = [BGSplashAdViewController new];
        BigoSplashAd *ad = (BigoSplashAd *)handler.ad;
        vc.ad = ad;
        vc.adHandle = handler;
        vc.rootVC = [UIApplication sharedApplication].keyWindow.rootViewController;
        [[UIApplication sharedApplication].keyWindow setRootViewController:vc];
    });
}

}

