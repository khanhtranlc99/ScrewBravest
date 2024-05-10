#import <UIKit/UIKit.h>
#import <BigoADS/BigoADS.h>

@interface BigoUnityNativeCustomView : UIView

@property (nonatomic, strong) BigoAdMediaView *mediaView;
@property (nonatomic, strong) UIImageView *adIconView;
@property (nonatomic, strong) BigoAdOptionsView *adOptionsView;
@property (nonatomic, strong) UILabel *titleLabel;
@property (nonatomic, strong) UILabel *detailLabel;
@property (nonatomic, strong) UIButton *cta;

- (NSArray<UIView *> *)clickAbleViews;

- (void)refreshNativeView:(BigoNativeAd *)nativeAd;

@end
