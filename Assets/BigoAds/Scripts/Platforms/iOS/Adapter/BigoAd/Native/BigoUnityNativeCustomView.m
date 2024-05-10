
#import "BigoUnityNativeCustomView.h"

@implementation BigoUnityNativeCustomView

- (instancetype)initWithFrame:(CGRect)frame {
    
    if (self = [super initWithFrame:frame]) {
        
        self.backgroundColor = UIColor.whiteColor;
        
        self.mediaView = [BigoAdMediaView new];
        self.mediaView.bigoNativeAdViewTag = BigoNativeAdViewTagMedia;
        self.mediaView.frame = CGRectMake(0, 50, CGRectGetWidth(self.bounds) , CGRectGetHeight(self.bounds) - 100);

        self.adIconView = [UIImageView new];
        self.adIconView.bigoNativeAdViewTag = BigoNativeAdViewTagIcon;
        self.adIconView.frame = CGRectMake(0, 0, 50, 50);

        self.cta = [UIButton new];
        self.cta.bigoNativeAdViewTag = BigoNativeAdViewTagCallToAction;
        self.cta.titleLabel.textColor = [UIColor colorWithRed:0 green:0 blue:255 alpha:255];
        self.cta.bigoNativeAdViewTag = BigoNativeAdViewTagCallToAction;
        [self.cta setBackgroundColor:[UIColor grayColor]];

        self.adOptionsView = [BigoAdOptionsView new];
        self.adOptionsView.bigoNativeAdViewTag = BigoNativeAdViewTagOption;
        self.adOptionsView.frame = CGRectMake(CGRectGetWidth(self.bounds) - 20, 0, 20, 20);

        [self addSubview:self.mediaView];
        [self addSubview:self.adOptionsView];
        [self addSubview:self.adIconView];
        [self addSubview:self.cta];
        
        self.titleLabel = UILabel.new;
        self.titleLabel.textColor = UIColor.blackColor;
        self.titleLabel.bigoNativeAdViewTag = BigoNativeAdViewTagTitle;
        self.titleLabel.font = [UIFont systemFontOfSize:18];
        [self addSubview:self.titleLabel];
        
        self.detailLabel = UILabel.new;
        self.detailLabel.textColor = UIColor.lightGrayColor;
        self.detailLabel.bigoNativeAdViewTag = BigoNativeAdViewTagDescription;
        self.detailLabel.font = [UIFont systemFontOfSize:18];
        [self addSubview:self.detailLabel];

        self.titleLabel.frame = CGRectMake(65, 5, CGRectGetWidth(self.bounds)-95, 20);
        self.detailLabel.frame = CGRectMake(65, 30, CGRectGetWidth(self.bounds)-95, 20);

        self.cta.frame = CGRectMake(CGRectGetWidth(self.bounds)-100, 250, 100, 50);


    }
    return self;
}

- (NSArray<UIView *> *)clickAbleViews {
    return @[self.adIconView, self.titleLabel, self.detailLabel, self.cta];
}

- (void)refreshNativeView:(BigoNativeAd *)nativeAd {
    self.titleLabel.text = nativeAd.title;
    self.detailLabel.text = nativeAd.adDescription;
    NSString *ctaTitle = nativeAd.callToAction.length > 0 ? nativeAd.callToAction : @"Install";
    [self.cta setTitle:ctaTitle forState:UIControlStateNormal];
    //设置按钮选择状态下的标题
    [self.cta setTitle:ctaTitle forState:UIControlStateSelected];
    //设置按钮高亮状态下的标题
    [self.cta setTitle:ctaTitle forState:UIControlStateHighlighted];
}

@end
