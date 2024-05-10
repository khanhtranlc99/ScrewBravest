#import <BigoADS/BigoADS.h>
#import <UIKit/UIKit.h>
#import "BigoUnityAdapterTools.h"

extern "C" {

BigoAdConfig* BigoIOS_config(const char* appID) {
    NSString *appId = BigoIOS_transformNSStringForm(appID);
    return [[BigoAdConfig alloc] initWithAppId:appId];
}
BigoAdConfig* BigoIOS_GetConfig(void* __nullable config) {
    BigoAdConfig *bigoConfig = config ? (__bridge BigoAdConfig*)config : nil;
    return bigoConfig;
}

void BigoIOS_configSetExtra(void* __nullable config, const char* key, const char* extra) {
    BigoAdConfig *bigoConfig = BigoIOS_GetConfig(config);
    NSString *keyString =  BigoIOS_transformNSStringForm(key);
    NSString *extraString =  BigoIOS_transformNSStringForm(extra);
    [bigoConfig addExtraWithKey:keyString extra:extraString];
}

void BigoIOS_configSetInfo(void* config, bool debugLog, int age, int gender, long activatedTime) {
    BigoIOS_GetConfig(config).testMode = debugLog;
    BigoIOS_GetConfig(config).age = age;
    BigoIOS_GetConfig(config).gender = (BigoAdGender)gender;
    BigoIOS_GetConfig(config).activatedTime = activatedTime;
}

}
