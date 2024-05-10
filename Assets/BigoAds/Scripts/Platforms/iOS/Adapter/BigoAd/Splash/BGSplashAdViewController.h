//
//  BGSplashAdViewController.h
//  demo
//
//  Created by cai on 2022/5/23.
//

#import <UIKit/UIKit.h>
#import <BigoADS/BigoSplashAd.h>
#import "BigoUnityBaseAdHandler.h"

NS_ASSUME_NONNULL_BEGIN

@interface BGSplashAdViewController : UIViewController

@property (nonatomic, strong) UIViewController *rootVC;

@property (nonatomic, strong) BigoSplashAd *ad;

@property (nonatomic, weak) BigoUnitySplashAdHandler *adHandle;

@end

NS_ASSUME_NONNULL_END
