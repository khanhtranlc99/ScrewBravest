#import "BigoUnityAdHandlerManager.h"

@interface BigoUnityAdHandlerManager()

@property (nonatomic, strong) dispatch_semaphore_t semaphore;
@property (nonatomic, strong) NSMutableDictionary *dictionary;

@end

@implementation BigoUnityAdHandlerManager

+ (instancetype)sharedInstance {
    static BigoUnityAdHandlerManager *manager = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        manager = [[BigoUnityAdHandlerManager alloc] init];
    });
    return manager;
}

- (instancetype)init
{
    self = [super init];
    if (self) {
        self.dictionary = [NSMutableDictionary new];
        self.semaphore = dispatch_semaphore_create(1);
    }
    return self;
}

+ (void)saveAdHandler:(BigoUnityBaseAdHandler *)adhandler withKey:(NSString *)key {
    if (!adhandler || key.length == 0) {
        return;
    }
    BigoUnityAdHandlerManager *manager = [BigoUnityAdHandlerManager sharedInstance];
    dispatch_semaphore_wait(manager.semaphore, DISPATCH_TIME_FOREVER);
    [manager.dictionary setObject:adhandler forKey:key];
    dispatch_semaphore_signal(manager.semaphore);
}

+ (void)removeAdHandlerWithKey:(NSString *)key {
    if (key.length == 0) {
        return;
    }
    BigoUnityAdHandlerManager *manager = [BigoUnityAdHandlerManager sharedInstance];
    dispatch_semaphore_wait(manager.semaphore, DISPATCH_TIME_FOREVER);
    [manager.dictionary removeObjectForKey:key];
    dispatch_semaphore_signal(manager.semaphore);
}

+ (NSString *)createKeyUnityAd:(UnityAd)unityAd {
    return [NSString stringWithFormat:@"%p",unityAd];
}

+ (nullable BigoUnityBaseAdHandler *)handlerWithKey:(NSString *)key {
    if (key.length == 0) {
        return nil;
    }
    BigoUnityAdHandlerManager *manager = [BigoUnityAdHandlerManager sharedInstance];
    dispatch_semaphore_wait(manager.semaphore, DISPATCH_TIME_FOREVER);
    BigoUnityBaseAdHandler *handler = [manager.dictionary objectForKey:key];
    dispatch_semaphore_signal(manager.semaphore);
    return handler;
}

+ (void)printList {
    NSLog(@"bigo-ios-list:%@",[BigoUnityAdHandlerManager sharedInstance].dictionary);
}

@end
