//
//  BGSplashAdViewController.m
//  demo
//
//  Created by cai on 2022/5/23.
//

#import "BGSplashAdViewController.h"

@interface BGSplashAdViewController () <BigoSplashAdInteractionDelegate>

@end

@implementation BGSplashAdViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    self.view.backgroundColor = [UIColor whiteColor];
    
    [self.ad setSplashAdInteractionDelegate:self];
    [self.ad showInAdContainer:self.view];
}

- (void)dismissSplashController:(BigoAd *)ad {
    [self.adHandle onAdClosed:ad];
    [[UIApplication sharedApplication].keyWindow setRootViewController:self.rootVC];
}

/**
 * 广告可跳过回调，通常是由用户点击了右上角 SKIP 按钮所触发
 */
- (void)onAdSkipped:(BigoAd *)ad {
    [self dismissSplashController:ad];
}

/**
 * 广告倒计时结束回调
 */
- (void)onAdFinished:(BigoAd *)ad {

}

- (void)onAd:(BigoAd *)ad error:(BigoAdError *)error {
    [self dismissSplashController:ad];
}

//广告展示
- (void)onAdImpression:(BigoAd *)ad {
    [self.adHandle onAdImpression:ad];
}

//广告点击
- (void)onAdClicked:(BigoAd *)ad {
    [self.adHandle onAdClicked:ad];
}

- (void)viewWillAppear:(BOOL)animated {
    [super viewWillAppear:animated];
    [self.navigationController setNavigationBarHidden:YES animated:animated];
}

- (void)viewWillDisappear:(BOOL)animated {
    [super viewWillDisappear:animated];
     [self.navigationController setNavigationBarHidden:NO animated:animated];
}

- (BOOL)prefersStatusBarHidden {
    return YES;
}

@end
