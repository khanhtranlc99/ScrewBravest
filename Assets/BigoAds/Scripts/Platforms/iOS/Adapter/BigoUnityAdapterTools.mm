#import <Foundation/Foundation.h>

extern "C" {
//字符串转化工具
NSString* BigoIOS_transformNSStringForm(const char * string)
{
    if (!string)
    {
        string = "";
    }
    return [NSString stringWithUTF8String:string];
}

void BigoIOS_dispatchSyncMainQueue(void (^block)(void)) {
    if (!block) return;
    if ([[NSThread currentThread] isMainThread]){
        block();
        return;
    }
    dispatch_sync(dispatch_get_main_queue(), ^{ block(); });
}

NSDictionary* BigoIOS_jsonObjectFromJsonString(NSString *jsonString) {
    if (jsonString.length == 0) {
        return [[NSDictionary alloc] init];
    }
    NSData *data = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSError *error = nil;
    id obj = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&error];
    if (error || ![obj isKindOfClass:[NSDictionary class]]) {
        return [[NSDictionary alloc] init];
    }
    return obj;
}

NSDictionary* BigoIOS_requestJsonObjectFromJsonString(const char * json) {
    NSString *string = BigoIOS_transformNSStringForm(json);
    return BigoIOS_jsonObjectFromJsonString(string);
}



}
