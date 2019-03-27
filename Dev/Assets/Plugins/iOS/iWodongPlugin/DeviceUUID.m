
//#import "KeychainItemWrappera.h"
#import "WDSSKeychain.h"

@implementation DeviceUUID
+(NSString*) getUUID{
    
    NSString *APP_SERVER = @"WodongSanguoServerice";
    NSString *APP_ACCOUNT = @"WodongSanguoAccount";
    NSError *error = nil;
    NSString *result_uuid = [WDSSKeychain passwordForService:APP_SERVER account:APP_ACCOUNT error:&error];
    
    if ([error code] == SSKeychainErrorNotFound) {
        NSLog(@"Password not found");
        CFUUIDRef puuid = CFUUIDCreate( nil );
        CFStringRef uuidString = CFUUIDCreateString( nil, puuid );
        result_uuid = (NSString *)CFBridgingRelease(CFStringCreateCopy( NULL, uuidString));
        CFRelease(puuid);
        CFRelease(uuidString);
        
        [WDSSKeychain setPassword:result_uuid forService:APP_SERVER account:APP_ACCOUNT];
    }
    NSLog(result_uuid);
    return result_uuid;
}

@end
