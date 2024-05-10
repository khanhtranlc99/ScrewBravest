#import <BigoADS/BigoADS.h>
#import "UnityAppController.h"
#import "BigoUnityBaseAdHandler.h"
#import "BigoUnityAdapterTools.h"
#import "BigoUnityAdHandlerManager.h"
#import "BigoUnityNativeCustomView.h"

extern "C"{

//load interstitial ad
void BigoIOS_loadNativeAdData(UnityAd unityAd,
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
            
            UIViewController *vc = GetAppController().rootViewController;
            BigoUnityNativeCustomView *nativeView = [[BigoUnityNativeCustomView alloc] initWithFrame:CGRectMake(0, 0, vc.view.frame.size.width, 300)];
            NSDictionary *requestDict = BigoIOS_requestJsonObjectFromJsonString(requestJson);
            BigoUnityNativeAdHandler *adHandler = [[BigoUnityNativeAdHandler alloc] init];
            BigoNativeAdRequest *request = [[BigoNativeAdRequest alloc] initWithSlotId:BigoIOS_transformNSStringForm(slotId)];
            request.age = [requestDict[@"age"] intValue];
            request.gender = (BigoAdGender)[requestDict[@"gender"] intValue];
            request.activatedTime = [requestDict[@"activatedTime"] longLongValue];
            BigoNativeAdLoader *adLoader = [[BigoNativeAdLoader alloc] initWithNativeAdLoaderDelegate:adHandler];
            adLoader.ext = requestDict[@"extraInfo"];
            adHandler.nativeView = nativeView;
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

BigoUnityNativeAdHandler* BigoIOS_getNativeAdHandler(UnityAd unityAd) {
    if (!unityAd) return nil;
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityNativeAdHandler *handler = (BigoUnityNativeAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    if (!handler || ![handler isMemberOfClass:[BigoUnityNativeAdHandler class]]) {
        return nil;
    }
    return handler;
}

void BigoIOS_showNativeAd(UnityAd unityAd) {
    BigoUnityNativeAdHandler *handler = BigoIOS_getNativeAdHandler(unityAd);
    if (!handler) return;
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        BigoNativeAd *ad = (BigoNativeAd *)handler.ad;
        //render
        BigoUnityNativeCustomView *nativeView = (BigoUnityNativeCustomView *)handler.nativeView;
        [ad registerViewForInteraction:nativeView mediaView:nativeView.mediaView adIconView:nativeView.adIconView adOptionsView:nativeView.adOptionsView clickableViews:nativeView.clickAbleViews];
        [nativeView refreshNativeView:ad];

        CGFloat screenHeight = [UIScreen mainScreen].bounds.size.height;
        CGRect frame = nativeView.frame;
        frame.origin.y = screenHeight - frame.size.height;
        nativeView.frame = frame;

        [vc.view addSubview:handler.nativeView];
        
    });
}

void BigoIOS_SetNativeAdPosition(UnityAd unityAd,int x, int y) {
    BigoUnityNativeAdHandler *handler = BigoIOS_getNativeAdHandler(unityAd);
    if (!handler) return;
    
    BigoIOS_dispatchSyncMainQueue(^{
        CGRect frame = handler.nativeView.frame;
        frame.origin.x = x;
        frame.origin.y = y;
        handler.nativeView.frame = frame;
    });
}

void BigoIOS_removeNativeView(UnityAd unityAd) {
    BigoUnityNativeAdHandler *handler = BigoIOS_getNativeAdHandler(unityAd);
    if (!handler) return;
    
    BigoIOS_dispatchSyncMainQueue(^{
        [handler.nativeView removeFromSuperview];
    });
}

}

