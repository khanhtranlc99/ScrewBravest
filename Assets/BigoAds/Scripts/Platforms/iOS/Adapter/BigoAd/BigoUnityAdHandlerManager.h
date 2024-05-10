
#import <Foundation/Foundation.h>
#import "BigoUnityBaseAdHandler.h"

NS_ASSUME_NONNULL_BEGIN

@interface BigoUnityAdHandlerManager : NSObject

+ (void)saveAdHandler:(BigoUnityBaseAdHandler *)adhandler withKey:(NSString *)key;

+ (void)removeAdHandlerWithKey:(NSString *)key;

+ (NSString *)createKeyUnityAd:(UnityAd)unityAd;

+ (nullable BigoUnityBaseAdHandler *)handlerWithKey:(NSString *)key;

+ (void)printList;


@end

NS_ASSUME_NONNULL_END
