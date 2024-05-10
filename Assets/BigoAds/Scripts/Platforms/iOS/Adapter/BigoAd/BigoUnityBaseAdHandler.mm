
#import "BigoUnityBaseAdHandler.h"
#import "BigoUnityAdHandlerManager.h"

@implementation BigoUnityBaseAdHandler

//广告异常
- (void)onAd:(BigoAd *)ad error:(BigoAdError *)error {

}

//广告展示
- (void)onAdImpression:(BigoAd *)ad {
    if (self.showCallback) {
        self.showCallback(self.unityAd);
    }
}

//广告点击
- (void)onAdClicked:(BigoAd *)ad {
    if (self.clickCallback) {
        self.clickCallback(self.unityAd);
    }
}

//广告打开
- (void)onAdOpened:(BigoAd *)ad {

}

//广告关闭
- (void)onAdClosed:(BigoAd *)ad {
    if (self.dismissCallback) {
        self.dismissCallback(self.unityAd);
    }
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:self.unityAd];
    [BigoUnityAdHandlerManager removeAdHandlerWithKey:unityAdKey];
}

- (void)dealloc {
    NSLog(@"BigoAd-iOS-[BigoUnityBaseAdHandler]-[dealloc]");
}

@end

@implementation BigoUnityBannerAdHandler


#pragma mark - BigoBannerAdLoaderDelegate
- (void)onBannerAdLoaded:(BigoBannerAd *)ad {
    [ad setAdInteractionDelegate:self];
    self.ad = ad;
    if (self.successCallback) {
        self.successCallback(self.unityAd);
    }
}


- (void)onBannerAdLoadError:(BigoAdError *)error {
    if (self.failCallback) {
        self.failCallback(self.unityAd, error.errorCode, error.errorMsg.UTF8String);
    }
}

@end

@implementation BigoUnityInterstitialAdHandler

#pragma mark - BigoInterstitialAdLoaderDelegate
- (void)onInterstitialAdLoaded:(BigoInterstitialAd *)ad {
    [ad setAdInteractionDelegate:self];
    self.ad = ad;
    if (self.successCallback) {
        self.successCallback(self.unityAd);
    }
}

- (void)onInterstitialAdLoadError:(BigoAdError *)error {
    if (self.failCallback) {
        self.failCallback(self.unityAd, error.errorCode, error.errorMsg.UTF8String);
    }
}

@end

@implementation BigoUnityRewardedAdHandler

#pragma mark - BigoRewardVideoAdLoaderDelegate
- (void)onRewardVideoAdLoaded:(BigoRewardVideoAd *)ad {
    [ad setRewardVideoAdInteractionDelegate:self];
    self.ad = ad;
    if (self.successCallback) {
        self.successCallback(self.unityAd);
    }
}


- (void)onRewardVideoAdLoadError:(BigoAdError *)error {
    if (self.failCallback) {
        self.failCallback(self.unityAd, error.errorCode, error.errorMsg.UTF8String);
    }
}

#pragma mark - BigoRewardVideoAdInteractionDelegate
//激励视频激励回调
- (void)onAdRewarded:(BigoRewardVideoAd *)ad {
    if (self.earnCallback) {
        self.earnCallback(self.unityAd);
    }
}
@end

@implementation BigoUnitySplashAdHandler

#pragma mark - BigoSplashAdLoaderDelegate
- (void)onSplashAdLoaded:(BigoSplashAd *)ad {
    [ad setSplashAdInteractionDelegate:self];
    self.ad = ad;
    if (self.successCallback) {
        self.successCallback(self.unityAd);
    }
}


- (void)onSplashAdLoadError:(BigoAdError *)error {
    if (self.failCallback) {
        self.failCallback(self.unityAd, error.errorCode, error.errorMsg.UTF8String);
    }
}

#pragma mark - BigoSplashAdInteractionDelegate

/**
 * 广告可跳过回调，通常是由用户点击了右上角 SKIP 按钮所触发
 */
- (void)onAdSkipped:(BigoAd *)ad {
    if (self.dismissCallback) {
        self.dismissCallback(self.unityAd);
    }
}

/**
 * 广告倒计时结束回调
 */
- (void)onAdFinished:(BigoAd *)ad {
    if (self.dismissCallback) {
        self.dismissCallback(self.unityAd);
    }
}

@end

@implementation BigoUnityNativeAdHandler

#pragma mark - BigoNativeAdLoaderDelegate
- (void)onNativeAdLoaded:(BigoNativeAd *)ad {
    [ad setAdInteractionDelegate:self];
    self.ad = ad;
    if (self.successCallback) {
        self.successCallback(self.unityAd);
    }
}

- (void)onNativeAdLoadError:(BigoAdError *)error {
    if (self.failCallback) {
        self.failCallback(self.unityAd, error.errorCode, error.errorMsg.UTF8String);
    }
}

 @end