
#import <Foundation/Foundation.h>
#import <BigoADS/BigoADS.h>
#import <UIKit/UIKit.h>

typedef void* UnityAd;//unity ad
typedef void* BigoAdSdkAd;//bigo ad
//int

//callback of ad load
typedef void(*AdDidLoadCallback)(UnityAd unityAd);
typedef void (*AdLoadFailCallBack)(UnityAd unityAd, int code, const char *msg);

//callback of ad event
typedef void(*AdDidShowCallback)(UnityAd unityAd);
typedef void(*AdDidClickCallback)(UnityAd unityAd);
typedef void(*AdDidDismissCallback)(UnityAd unityAd);
typedef void (*AdDidErrorCallback)(UnityAd unityAd, int code, const char *msg);

typedef void (*AdDidEarnRewardCallback)(UnityAd unityA);

@interface BigoUnityBaseAdHandler : NSObject<BigoAdInteractionDelegate>

@property (nonatomic, assign) UnityAd unityAd;
@property (nonatomic, strong) BigoAd *ad;

@property (nonatomic, assign) AdDidShowCallback showCallback;
@property (nonatomic, assign) AdDidClickCallback clickCallback;
@property (nonatomic, assign) AdDidDismissCallback dismissCallback;
@property (nonatomic, assign) AdDidErrorCallback adErrorCallback;

//强持有AdLoader
@property (nonatomic, strong) BigoAdLoader *adLoader;
@property (nonatomic, assign) AdDidLoadCallback successCallback;
@property (nonatomic, assign) AdLoadFailCallBack failCallback;

@end

@interface BigoUnityBannerAdHandler :  BigoUnityBaseAdHandler<BigoBannerAdLoaderDelegate>

@end


@interface BigoUnityInterstitialAdHandler :  BigoUnityBaseAdHandler<BigoInterstitialAdLoaderDelegate>

@end

@interface BigoUnityRewardedAdHandler :  BigoUnityBaseAdHandler<BigoRewardVideoAdLoaderDelegate, BigoRewardVideoAdInteractionDelegate>

@property (nonatomic, assign) AdDidEarnRewardCallback earnCallback;

@end

@interface BigoUnitySplashAdHandler :  BigoUnityBaseAdHandler<BigoSplashAdLoaderDelegate, BigoSplashAdInteractionDelegate>

@end

@interface BigoUnityNativeAdHandler :  BigoUnityBaseAdHandler<BigoNativeAdLoaderDelegate>

@property (nonatomic, strong)  UIView *nativeView;

@end
