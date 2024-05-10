
#import <BigoADS/BigoADS.h>
#import "BigoUnityAdapterTools.h"

char* MakeStringCopy(const char* string) {
    if (string == NULL) return NULL;
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C" {
    
    typedef void (*BigoSDKInitSuccessCallBack)(void* unitySDK);
    
    void BigoIOS_initSDK(void* unitySDK, void* config, BigoSDKInitSuccessCallBack successCallback) {
        BigoAdConfig *adConfig = config ? (__bridge BigoAdConfig*)config : nil;
        [[BigoAdSdk sharedInstance] initializeSdkWithAdConfig:adConfig completion:^{
            successCallback(unitySDK);
        }];   
    }
    
    BOOL BigoIOS_sdkInitializationState() {
        return [[BigoAdSdk sharedInstance] isInitialized];
    }
    
    const char* BigoIOS_sdkVersion() {
        NSString *version = [[BigoAdSdk sharedInstance] getSDKVersion];
        return MakeStringCopy([version UTF8String]);
    }

    const char* BigoIOS_sdkVersionName() {
        NSString *versionName = [[BigoAdSdk sharedInstance] getSDKVersionName];
        return MakeStringCopy([versionName UTF8String]);
    }

    void BigoIOS_setConsentGDPR(bool consent) {
        [BigoAdSdk setUserConsentWithOption:BigoConsentOptionsGDPR consent:consent];
    }

    void BigoIOS_setConsentCCPA(bool consent) {
        [BigoAdSdk setUserConsentWithOption:BigoConsentOptionsCCPA consent:consent];
    }

    void BigoIOS_addExtraHost(const char* country,  const char* host) {
        NSString *countryString = BigoIOS_transformNSStringForm(country);
        NSString *hostString = BigoIOS_transformNSStringForm(host);
        [BigoAdSdk addExtraHostWithCountry:countryString host:hostString];
    }
}
