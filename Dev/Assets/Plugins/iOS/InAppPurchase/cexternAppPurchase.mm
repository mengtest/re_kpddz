//
//  cexternAppPurchase.mm
//  InAppPurchasesExample
//
//  Created by iWodong on 13-9-5.
//
//

#include "stdio.h"
#import "MKStoreManager.h"
#import "cexternIOSTools.h"
#import "cexternAppPurchase.h"
#import "UnityInterface.h"
#import "DeviceUUID.h"
#import <sys/socket.h>
#import <netdb.h>
#import <arpa/inet.h>
#import <err.h>
#import <AdSupport/AdSupport.h>


//查询回调
static bool _bBindReqHandler = false;
static NSString *_sOnReqCallback;

//监听成功失败
static bool _bBindObserver = false;
static NSString *_sOnFailCallback;
static NSString *_sOnCompleteCallback;

#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

#ifdef __cplusplus
    extern "C" {
#endif

//绑定查询档位的c#回调函数
void registerScriptOnResponseRequest(const char*  callbackFunc)
{
    _bBindReqHandler = true;
    _sOnReqCallback = [NSString stringWithUTF8String:callbackFunc];
}

//绑定充值成功失败的c#回调函数
void registerScriptObserver( const char*  completeCallbackFunc, const char*  failedCallbackFunc)
{
    _bBindObserver = true;
    _sOnFailCallback = [NSString stringWithUTF8String:failedCallbackFunc];
    _sOnCompleteCallback = [NSString stringWithUTF8String:completeCallbackFunc];
}

//查询档位
void requestItemInfo(const char* strItemList)
{
    NSString* nstrItemList = [NSString stringWithUTF8String: strItemList];
    NSArray *arrayItem = [nstrItemList componentsSeparatedByString:@","];
    NSSet* setItem = [NSSet setWithArray: arrayItem];

    [[MKStoreManager sharedManager] requestProductData:setItem];
}
   
//查询档位返回
void onRequestItem(int nRlt)
{
	NSLog(@"请求充值产品结果: %d", nRlt);
    NSString *sRlt = [NSString stringWithFormat:@"%d", nRlt];
    CallUnityFunction(_sOnReqCallback, sRlt);
}

//充值请求
void buyItem(char* identifier)
{
    NSString* nstrId = [NSString stringWithUTF8String: identifier];
    [[MKStoreManager sharedManager] buyFeature:nstrId];
	NSLog(@"to buy identifier = %s", identifier);
}

//充值返回
void onBuyItem(int nRlt,const char* identifier,const char* receipt,const char* transactionID)
{
	//nRlt:0进行中 1成功 2失败 3重新提交 4排队中
	if (nRlt == 1) {
		NSLog(@"identifier = %s, receipt = %s", identifier, receipt);
		
		NSString *nsRlt = [NSString stringWithFormat:@"%d", nRlt];
		NSString *nsIdentifier = [NSString stringWithUTF8String: identifier];
		NSString *nsReceipt = [NSString stringWithUTF8String: receipt];
		NSString *nsTransactionNum = [NSString stringWithUTF8String: transactionID];
		
		NSDictionary *dic = [NSDictionary dictionaryWithObjectsAndKeys:nsRlt,@"nsRlt", nsIdentifier, @"nsIdentifier", nsReceipt, @"nsReceipt", nsTransactionNum, @"nsTransactionNum", nil, nil];
		NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:NSJSONWritingPrettyPrinted error:nil];
		NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
		CallUnityFunction(_sOnCompleteCallback, jsonString);
	}
	else{
		NSLog(@"充值失败");
		
		NSString *nsRlt = [NSString stringWithFormat:@"%d", nRlt];
		NSString *nsIdentifier = [NSString stringWithUTF8String: identifier];
		
		NSDictionary *dic = [NSDictionary dictionaryWithObjectsAndKeys:nsRlt,@"nsRlt", nsIdentifier, @"nsIdentifier",  nil, nil];
		NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:NSJSONWritingPrettyPrinted error:nil];
		NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
		CallUnityFunction(_sOnFailCallback, jsonString);
	}
}
    
        
        
        
#ifdef __cplusplus
}
#endif









