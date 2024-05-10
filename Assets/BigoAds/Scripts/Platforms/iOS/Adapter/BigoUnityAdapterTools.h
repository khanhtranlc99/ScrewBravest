extern "C" {
    extern NSString * const BigoIOSRequest_exterInfo = @"extraInfo";
    extern NSString * const BigoIOSRequest_adString = @"adString";
    extern NSString * const BigoIOSRequest_timeout = @"timeout";
    NSString* BigoIOS_transformNSStringForm(const char * string);
    void BigoIOS_dispatchSyncMainQueue(void (^block)(void));
    NSDictionary* BigoIOS_jsonObjectFromJsonString(NSString *jsonString);
    NSDictionary* BigoIOS_requestJsonObjectFromJsonString(const char * json);
} 
