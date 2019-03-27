//
//  cexternAppPurchase.mm
//  InAppPurchasesExample
//
//  Created by iWodong on 13-9-5.
//
//

#include "stdio.h"
#import "cexternIOSTools.h"
#import "UnityInterface.h"
#import <AdSupport/AdSupport.h>
#import "DeviceUUID.h"
#import <sys/socket.h>
#import <netdb.h>
#import <arpa/inet.h>
#import <err.h>
#import "InAppWebView.h"


//所有回调函数绑定的对象
static bool _bBindGameobject = false;
static NSString *_sGameObjectName;

#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

#ifdef __cplusplus
    extern "C" {
#endif
        
void CallUnityFunction(NSString *callbackFun, NSString *args)
{
    if (_bBindGameobject)
    {
        UnitySendMessage([_sGameObjectName UTF8String], [callbackFun UTF8String], [args UTF8String]);
    }
    else
    {
        NSLog(@"--------------not bind gameobject----------------------");
    }
}
        
//注册回调函数的对象
void registerCallbackGameObject(const char*  name)
{
    _bBindGameobject = true;
    _sGameObjectName = [NSString stringWithUTF8String:name];
}

//打开网页
void openURL(char* identifier)
{
	 NSString *nsIdentifier = [NSString stringWithUTF8String: identifier];
	[[UIApplication sharedApplication] openURL:[NSURL URLWithString:nsIdentifier]];
}
//Copy到粘贴面板
void copyToPasteBoard(char* identifier)
{
	NSString *nsIdentifier = [NSString stringWithUTF8String: identifier];
	UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
	pasteboard.string = nsIdentifier;
}
//取设备唯一ID
void getUUID()
{
	NSString *result_uuid = [DeviceUUID getUUID];
	NSString *callbackFun = @"OnGetDeviceUUID";
	NSLog(result_uuid);
	NSLog(_sGameObjectName);
    CallUnityFunction(callbackFun, result_uuid);
}
//取广告标识符
void getIDFA()
{
	NSString *idfa = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
	NSString *callbackFun = @"OnGetIDFA";
    CallUnityFunction(callbackFun, idfa);
}
//处理IPV6地址
const char* getIPv6(const char *mHost,const char *mPort)
{
    if( nil == mHost )
        return NULL;
    const char *newChar = "No";
    const char *cause = NULL;
    struct addrinfo* res0;
    struct addrinfo hints;
    struct addrinfo* res;
    int n, s;
    
    memset(&hints, 0, sizeof(hints));
    
    hints.ai_flags = AI_DEFAULT;
    hints.ai_family = PF_UNSPEC;
    hints.ai_socktype = SOCK_STREAM;
    //此处是IOS的关键函数
    if((n=getaddrinfo(mHost, "http", &hints, &res0))!=0)
    {
        printf("getaddrinfo error: %s\n",gai_strerror(n));
        return NULL;
    }
    
    struct sockaddr_in6* addr6;
    struct sockaddr_in* addr;
    NSString * NewStr = NULL;
    char ipbuf[32];
    s = -1;
    for(res = res0; res; res = res->ai_next)
    {
        if (res->ai_family == AF_INET6)
        {
            addr6 =( struct sockaddr_in6*)res->ai_addr;
            newChar = inet_ntop(AF_INET6, &addr6->sin6_addr, ipbuf, sizeof(ipbuf));
            NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar
                                                        encoding:NSASCIIStringEncoding];
            NSString * TempB = [NSString stringWithUTF8String:"&&ipv6"];
            
            NewStr = [TempA stringByAppendingString: TempB];
            printf("%s\n", newChar);
        }
        else
        {
            addr =( struct sockaddr_in*)res->ai_addr;
            newChar = inet_ntop(AF_INET, &addr->sin_addr, ipbuf, sizeof(ipbuf));
            NSString * TempA = [[NSString alloc] initWithCString:(const char*)newChar
                                                        encoding:NSASCIIStringEncoding];
            NSString * TempB = [NSString stringWithUTF8String:"&&ipv4"];
            
            NewStr = [TempA stringByAppendingString: TempB];
            printf("%s\n", newChar);
        }
        break;
    }
    
    
    freeaddrinfo(res0);
    
    printf("getaddrinfo OK");
    
    NSString * mIPaddr = NewStr;
    return MakeStringCopy(mIPaddr);
}
//打开内嵌网页
void OpenWebView(char* c_url)
{
    InAppWebView* _WKWebView = [InAppWebView shareClass];
    NSURL* url = [NSURL URLWithString:[NSString stringWithUTF8String:c_url]];
    [_WKWebView showTitleBar];
    [_WKWebView showWeb:url];
}
    
#ifdef __cplusplus
}
#endif









