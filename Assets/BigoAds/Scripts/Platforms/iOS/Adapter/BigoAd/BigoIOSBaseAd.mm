#import <BigoADS/BigoADS.h>
#import "BigoUnityBaseAdHandler.h"
#import "BigoUnityAdHandlerManager.h"
#import "BigoUnityBannerAd.h"
#import "BigoUnityInterstitialAd.h"
#import "BigoUnityRewardedAd.h"
#import "BigoUnitySplashAd.h"
#import "BigoUnityNativeAd.h"
#import "BigoUnityAdapterTools.h"

extern "C" {
enum BigoIOSAdType {
    native = 1, banner = 2, interstitial = 3, rewarded = 4, splash = 5
};
void BigoIOS_loadAdData(int adType,
                          UnityAd unityAd,
                          const char *slotId,
                          const char *requestJson,
                          AdDidLoadCallback successCallback,
                          AdLoadFailCallBack failCallback,
                          AdDidShowCallback showCallback,
                          AdDidClickCallback clickCallback,
                          AdDidDismissCallback dismissCallback,
                          AdDidErrorCallback adErrorCallback) {
    if (adType == rewarded || adType == banner) {//custom load
        return;
    }
    
    if (adType == interstitial) {
        BigoIOS_loadInterstitialAdData(unityAd, slotId, requestJson, successCallback, failCallback, showCallback, clickCallback, dismissCallback, adErrorCallback);
    }
    else if (adType == splash) {
        BigoIOS_loadSplashAdData(unityAd, slotId, requestJson, successCallback, failCallback, showCallback, clickCallback, dismissCallback, adErrorCallback);
    }
    else if (adType == native) {
        BigoIOS_loadNativeAdData(unityAd, slotId, requestJson, successCallback, failCallback, showCallback, clickCallback, dismissCallback, adErrorCallback);
    }
    
}
void BigoIOS_showAd(int adType, UnityAd unityAd) {
    if (adType == interstitial) {
        BigoIOS_showInterstitialAd(unityAd);
    }
    else if (adType == splash) {
        BigoIOS_showSplashAd(unityAd);
    }
    else if (adType == rewarded) {
        BigoIOS_showRewardedAd(unityAd);
    }
    else if (adType == banner) {
        BigoIOS_showBannerAd(unityAd);
    }
    else if (adType == native) {
        BigoIOS_showNativeAd(unityAd);
    }
}

void BigoIOS_destroyAd(int adType, UnityAd unityAd) {
    if (adType == banner) {
        BigoIOS_removeBannerView(unityAd);
    }
    else if (adType == native) {
        BigoIOS_removeNativeView(unityAd);
    }
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    [BigoUnityAdHandlerManager removeAdHandlerWithKey:unityAdKey];
}

BOOL BigoIOS_IsExpired(UnityAd unityAd) {
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityBaseAdHandler *handler = (BigoUnityBaseAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    BigoAd *ad = (BigoAd *)(handler.ad);
    return ad.isExpired;
}

BOOL BigoIOS_IsClientBidding(UnityAd unityAd) {
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityBaseAdHandler *handler = (BigoUnityBaseAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    BigoAd *ad = (BigoAd *)(handler.ad);
    return ad.getBid != nil;
}

CGFloat BigoIOS_GetPrice(UnityAd unityAd) {
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityBaseAdHandler *handler = (BigoUnityBaseAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    BigoAd *ad = (BigoAd *)(handler.ad);
    return ad.getBid.getPrice;
}

void BigoIOS_NotifyWin(UnityAd unityAd, CGFloat secPrice, const char* secBidder) {
    NSString *secBidderString = BigoIOS_transformNSStringForm(secBidder);
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityBaseAdHandler *handler = (BigoUnityBaseAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    BigoAd *ad = (BigoAd *)(handler.ad);
    [ad.getBid notifyWinWithSecPrice:secPrice secBidder:secBidderString];
}

void BigoIOS_NotifyLoss(UnityAd unityAd, CGFloat firstPrice , const char* firstBidder, int lossReason) {
    NSString *firstBidderString = BigoIOS_transformNSStringForm(firstBidder);
    NSString *unityAdKey = [BigoUnityAdHandlerManager createKeyUnityAd:unityAd];
    BigoUnityBaseAdHandler *handler = (BigoUnityBaseAdHandler*)[BigoUnityAdHandlerManager handlerWithKey:unityAdKey];
    BigoAd *ad = (BigoAd *)(handler.ad);
    [ad.getBid notifyLossWithFirstPrice:firstPrice firstBidder:firstBidderString lossReason:(BGAdLossReasonType)lossReason];
}

}
