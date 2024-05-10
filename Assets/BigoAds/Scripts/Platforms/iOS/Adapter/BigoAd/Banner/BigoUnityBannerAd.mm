#import <BigoADS/BigoADS.h>
#import "UnityAppController.h"
#import "BigoUnityBaseAdHandler.h"
#import "BigoUnityAdapterTools.h"
#import "BigoUnityAdHandlerManager.h"

extern "C"{


int BigoIOS_getScreenWidth () {
    __block int width = 0;
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        width = (int)vc.view.frame.size.width;
    });
    return width;
}

int BigoIOS_getScreenHeight () {
    __block int height = 0;
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        height = (int)vc.view.frame.size.height;
    });
    return height;
}

int BigoIOS_getScreenSafeBottom() {
    __block int bottom = 0;
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        bottom = (int)vc.view.safeAreaInsets.bottom;
    });
    return bottom;
}

int BigoIOS_getScreenSafeTop() {
    __block int top = 0;
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        top = (int)vc.view.safeAreaInsets.top;
    });
    return top;
}

//load Banner ad
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
                                ) {    
     BigoIOS_dispatchSyncMainQueue(^{
            BigoAdSize *size = BigoAdSize.BANNER;
            if (width == 300 && height == 250) {
                size = BigoAdSize.MEDIUM_RECTANGLE;
            }
            NSDictionary *requestDict = BigoIOS_requestJsonObjectFromJsonString(requestJson);
            BigoUnityBannerAdHandler *adHandler = [[BigoUnityBannerAdHandler alloc] init];
            BigoBannerAdRequest *request = [[BigoBannerAdRequest alloc] initWithSlotId:BigoIOS_transformNSStringForm(slotId) adSizes:@[size]];
            request.age = [requestDict[@"age"] intValue];
            request.gender = (BigoAdGender)[requestDict[@"gender"] intValue];
            request.activatedTime = [requestDict[@"activatedTime"] longLongValue];
            BigoBannerAdLoader *adLoader = [[BigoBannerAdLoader alloc] initWithBannerAdLoaderDelegate:adHandler];
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

BigoUnityBannerAdHandler* BigoIOS_getBannerAdHandler(UnityAd unityAd) {
    if (!unityAd) return nil;
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityBannerAdHandler *handler = (BigoUnityBannerAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    if (!handler || ![handler isMemberOfClass:[BigoUnityBannerAdHandler class]]) {
        return nil;
    }
    return handler;
}

void BigoIOS_showBannerAd(UnityAd unityAd) {
    BigoUnityBannerAdHandler *handler = BigoIOS_getBannerAdHandler(unityAd);
    if (!handler) return;
    BigoIOS_dispatchSyncMainQueue(^{
        UIViewController *vc = GetAppController().rootViewController;
        BigoBannerAd *ad = (BigoBannerAd *)handler.ad;
        [vc.view addSubview:ad.adView];
    });
}

void BigoIOS_SetBannerAdPosition(UnityAd unityAd,int x, int y) {
    BigoUnityBannerAdHandler *handler = BigoIOS_getBannerAdHandler(unityAd);
    if (!handler) return;
    
    BigoIOS_dispatchSyncMainQueue(^{
        BigoBannerAd *ad = (BigoBannerAd *)handler.ad;
        CGRect frame = ad.adView.frame;
        frame.origin.x = x;
        frame.origin.y = y;
        ad.adView.frame = frame;
    });
}

void BigoIOS_removeBannerView(UnityAd unityAd) {
    BigoUnityBannerAdHandler *handler = BigoIOS_getBannerAdHandler(unityAd);
    if (!handler) return;
    
    BigoIOS_dispatchSyncMainQueue(^{
        BigoBannerAd *ad = (BigoBannerAd *)handler.ad;
        [ad.adView removeFromSuperview];
    });
}

}

