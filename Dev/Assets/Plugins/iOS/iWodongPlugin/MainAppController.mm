//
//  MainAppController.m
//  Unity-iPhone
//
//  Created by WoDongMac1 on 2017/9/9.
//
//
#define SYSTEM_VERSION_LESS_THAN(v)                 ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] == NSOrderedAscending)

#import "MainAppController.h"
#include "EasyJson.h"
#include <string>
//#import <AlipaySDK/AlipaySDK.h>
#import <CommonCrypto/CommonDigest.h>
//#import <SecurityGuardSDK/Open/OpenSecurityGuardManager.h>
//#import "SGSDK.h"
#import "OpenInstallSDK.h"
#import "ShareItem.h"

NSString* wxLoginAppId = @"wxca7116033db16bdf";
NSString* wxPayAppId = @"wxca7116033db16bdf";
NSString* wxPartnerId = @"1487039872";
BOOL wxIsRegister = NO;

@implementation MainAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    //initWXSDK(wxLoginAppId);
    //向微信注册
    [WXApi registerApp:wxLoginAppId enableMTA:YES];
    //向微信注册支持的文件类型
    UInt64 typeFlag = MMAPP_SUPPORT_TEXT | MMAPP_SUPPORT_PICTURE | MMAPP_SUPPORT_LOCATION | MMAPP_SUPPORT_VIDEO |MMAPP_SUPPORT_AUDIO | MMAPP_SUPPORT_WEBPAGE | MMAPP_SUPPORT_DOC | MMAPP_SUPPORT_DOCX | MMAPP_SUPPORT_PPT | MMAPP_SUPPORT_PPTX | MMAPP_SUPPORT_XLS | MMAPP_SUPPORT_XLSX | MMAPP_SUPPORT_PDF;
    
    [WXApi registerAppSupportContentFlag:typeFlag];
    
    //[[SGSDK shareClass] initSecGuard];
    [OpenInstallSDK setAppKey:@"lu5t1m" withDelegate:self];
    
    return YES;
}


- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    
    if ([super respondsToSelector:@selector(application:handleOpenURL:)])
        [super application:application handleOpenURL: url];
    
    if (wxIsRegister)
        [WXApi handleOpenURL:url delegate:self];
    
    
    return  YES;
}

- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
{
    [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
    
    if (wxIsRegister)
        [WXApi handleOpenURL:url delegate:self];
    
    if ([url.host isEqualToString:@"safepay"]) {
        // 支付跳转支付宝钱包进行支付，处理支付结果
//        [[AlipaySDK defaultService] processOrderWithPaymentResult:url standbyCallback:^(NSDictionary *resultDic) {
//            NSLog(@"result = %@",resultDic);
//        }];
    }
    return YES;
}

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options
{
    if (wxIsRegister)
        [WXApi handleOpenURL:url delegate:self];
    
    if ([url.host isEqualToString:@"safepay"]) {
        // 支付跳转支付宝钱包进行支付，处理支付结果
//        [[AlipaySDK defaultService] processOrderWithPaymentResult:url standbyCallback:^(NSDictionary *resultDic) {
//            NSLog(@"result = %@",resultDic);
//        }];
    }
    
    return YES;
}

#pragma mark - 微信回调

//static std::string s_wxCallbackGameObject;

static NSString *s_wxCallbackGameObject;

static NSString *s_openInstallParam;


-(void)onReq:(BaseReq *)req
{
    
}

/// 微信回调
-(void)onResp:(BaseResp *)resp
{
    if ([resp isKindOfClass:[SendAuthResp class]])  // 登陆返回
    {
        SendAuthResp *authResp = (SendAuthResp *)resp;
        
        @autoreleasepool {
            EasyJson* jsObj = [[EasyJson alloc] init];
            [jsObj addKey:@"errCode" int:authResp.errCode];
            [jsObj addKey:@"wxCode" string:authResp.code];
            NSString* jstr = [jsObj toStr];
            UnitySendMessage([s_wxCallbackGameObject UTF8String], "onWXLogin", [jstr UTF8String]);
        }
    }
    else if ([resp isKindOfClass:[PayResp class]])  // 支付返回
    {
        PayResp* response = (PayResp*)resp;
        NSLog(@"微信支付结果： %d", response.errCode);
    }
    else if ([resp isKindOfClass:[SendMessageToWXResp class]])  // 分享返回
    {
        SendMessageToWXResp *authResp = (SendMessageToWXResp *)resp;
        
        @autoreleasepool {
            EasyJson* jsObj = [[EasyJson alloc] init];
            [jsObj addKey:@"errCode" int:authResp.errCode];
            NSString* jstr = [jsObj toStr];
            UnitySendMessage([s_wxCallbackGameObject UTF8String], "onWXShare", [jstr UTF8String]);
        }
    }
}

- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray * _Nullable))restorationHandler{
    //判断是否通过OpenInstall Universal Link 唤起App
    if ([OpenInstallSDK continueUserActivity:userActivity]){//如果使用了Universal link ，此方法必写
        return YES;
    }else{
        //其他第三方回调；
        return YES;
    }
}

//通过OpenInstall获取自定义参数，数据为空时也会回调此方法。渠道统计返回参数名称为openinstallChannelCode
- (void)getInstallParamsFromOpenInstall:(NSDictionary *) params withError: (NSError *) error {
    NSLog(@"OpenInstall enter %@", params);
    if (error)
    {
        NSLog(@"OpenInstall error %@", error);
    }
    else{
        if (params) {
            NSLog(@"OpenInstall 自定义数据：%@", [params description]);
            NSString *paramsStr = [NSString stringWithFormat:@"%@",params];
            /*          UIAlertController *alert = [UIAlertController alertControllerWithTitle:@"来自网页的安装参数，请根据需求，将参数计入统计数据或是根据App的业务流程处理（如免填邀请码建立邀请关系、自动加好友、自动进入某个群组或房间等）" message:paramsStr preferredStyle:  UIAlertControllerStyleAlert];
             [alert addAction:[UIAlertAction actionWithTitle:@"确定" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
             }]];
             //弹出提示框(便于调试，调试完成后删除此代码)
             [self.window.rootViewController presentViewController:alert animated:true completion:nil];
             */
            //获取到参数后可保存到本地，等到需要使用时再从本地获取。
            NSUserDefaults *openinstallData = [NSUserDefaults standardUserDefaults];
            [openinstallData setObject:params forKey:@"openinstallParams"];
            
            @autoreleasepool {
                EasyJson* jsParam = [[EasyJson alloc] init];
                for (NSString *str in params)
                {
                    NSString *strValue = [params objectForKey:str];
                    [jsParam addKey:str string:strValue];
                }
                NSString* paramJsStr = [jsParam toStr];
                
                EasyJson* jsObj = [[EasyJson alloc] init];
                [jsObj addKey:@"errCode" string:@""];
                [jsObj addKey:@"bindData" string:paramJsStr];
                [jsObj addKey:@"channelCode" string:@"0"];
                NSString* jstr = [jsObj toStr];
                if (s_wxCallbackGameObject == nil || s_wxCallbackGameObject.length <= 0)
                {
                    s_openInstallParam = jstr;
                }
                else
                {
                    UnitySendMessage([s_wxCallbackGameObject UTF8String], "OnGetShareParam", [jstr UTF8String]);
                }
            }
        }
    }
}

@end


#pragma mark - 微信支付和阿里支付接口

/// 阿里支付
void aliPay(const char* orderInfo)
{
    
//    [[AlipaySDK defaultService] payOrder:[NSString stringWithUTF8String:orderInfo] fromScheme:@"alipayruiqugamesniuniu" callback:^(NSDictionary *resultDic) {
//        NSLog(@"reslut = %@",resultDic);
//    }];

}

/// 生成随机字符串
NSString* getRandomStringByLength(int length) {
    NSString* base = @"abcdefghijklmnopqrstuvwxyz0123456789";
    srand((unsigned)time(NULL));
    NSString* rstr = [[NSString alloc] init];
    for (int i = 0; i < length; i++) {
        int number = rand() % length;
        rstr = [rstr stringByAppendingString: [base substringWithRange:{static_cast<NSUInteger>(number), 1}]];
    }
    return rstr;
}


/// md5加密
NSString* md5(NSString* str)
{
    const char *cStr = [str UTF8String];
    unsigned char digest[CC_MD5_DIGEST_LENGTH];
    CC_MD5( cStr, strlen(cStr), digest );
    
    NSMutableString *output = [NSMutableString stringWithCapacity:CC_MD5_DIGEST_LENGTH * 2];
    
    for(int i = 0; i < CC_MD5_DIGEST_LENGTH; i++)
        [output appendFormat:@"%02x", digest[i]];
    
    return [output uppercaseString];
}

/// 获取签名
NSString* getSign(NSDictionary* dictKV)
{
    NSArray* keys = [dictKV allKeys];
    NSArray* sortKeys = [keys sortedArrayUsingSelector:@selector(compare:)];
    
    NSString* sign = @"";
    for (int i=0; i<sortKeys.count; ++i)
    {
        NSString* key = sortKeys[i];
        NSString* value = [dictKV objectForKey:key];
        sign = [[[[sign stringByAppendingString:key] stringByAppendingString:@"="]stringByAppendingString:value] stringByAppendingString:@"&"];
    }
    sign = [sign stringByAppendingString:@"key=mqNG9x1N6g5kI0NmxF8nu7aLu6YeQBxi"];
    
    return md5(sign);
}




/// 获取10位的时间戳
uint64_t getTimeStamp()
{
    return [[NSDate date] timeIntervalSince1970];
}


/// 微信支付
void weChatPay(const char* orderInfo)
{
    @autoreleasepool {
        
        EasyJson* jsObj = [[EasyJson alloc] initWithString: [NSString stringWithUTF8String:orderInfo]];
        NSString* prepayId = [jsObj objectAtIndex:0];
        NSString* svrTimeStamp = [jsObj objectAtIndex:1];
        NSString* svrRandomStr = [jsObj objectAtIndex:2];
        NSString* svrSign = [jsObj objectAtIndex:3];
        
        //NSString* randomStr = getRandomStringByLength(32);
        //uint64_t timeStamp = getTimeStamp();
        
        PayReq *request = [[PayReq alloc] init];
        request.partnerId = wxPartnerId;
        request.prepayId = prepayId;
        request.package = @"Sign=WXPay";
        request.nonceStr = svrRandomStr;
        request.timeStamp = [svrTimeStamp intValue];
        
        
//        NSDictionary* dict = [[NSDictionary alloc] initWithObjectsAndKeys:
//                              wxAppId, @"appid",
//                              @"1487039872", @"partnerid",
//                              prepayId, @"prepayid",
//                              @"Sign=WXPay", @"package",
//                              randomStr, @"noncestr",
//                              [NSString stringWithFormat:@"%lld", timeStamp], @"timestamp",
//                              nil];
        
        request.sign = svrSign; //getSign(dict);
        if (wxIsRegister)
            [WXApi sendReq: request];
    }
}

void initWXSDK(NSString* appId)
{
    //向微信注册
    [WXApi registerApp:appId enableMTA:YES];
    //向微信注册支持的文件类型
    UInt64 typeFlag = MMAPP_SUPPORT_TEXT | MMAPP_SUPPORT_PICTURE | MMAPP_SUPPORT_LOCATION | MMAPP_SUPPORT_VIDEO |MMAPP_SUPPORT_AUDIO | MMAPP_SUPPORT_WEBPAGE | MMAPP_SUPPORT_DOC | MMAPP_SUPPORT_DOCX | MMAPP_SUPPORT_PPT | MMAPP_SUPPORT_PPTX | MMAPP_SUPPORT_XLS | MMAPP_SUPPORT_XLSX | MMAPP_SUPPORT_PDF;
    
    [WXApi registerAppSupportContentFlag:typeFlag];
    
    
    wxIsRegister = YES;
}


#pragma mark - 牛牛项目独有的导出接口放在这里

#ifdef __cplusplus
extern "C"
{
#endif

/// 设置回调对象名字
extern "C" void InitPaySdk(char* loginAppId, char* payAppId, char* partnerId)
{
    wxLoginAppId = [NSString stringWithUTF8String:loginAppId];
    wxPayAppId = [NSString stringWithUTF8String:payAppId];
    wxPartnerId = [NSString stringWithUTF8String:partnerId];
    
    initWXSDK(wxLoginAppId);
}

/// 设置回调对象名字
extern "C" void setUnityGameObjectName(char* gameObjectName)
{
    s_wxCallbackGameObject = [NSString stringWithUTF8String:gameObjectName];
}
    
/// 微信登陆
extern "C" void requestWXLogin()
{
    @autoreleasepool {
        initWXSDK(wxLoginAppId);
        //构造SendAuthReq结构体
        SendAuthReq* req =[[SendAuthReq alloc ] init];
        req.scope = @"snsapi_userinfo" ;
        req.state = @"123" ;
        //第三方向微信终端发送一个SendAuthReq消息结构
        [WXApi sendReq:req];
    }
}

/// 请求支付
extern "C" void requestPay(char* goodsID, char* goodsName, char* goodsDesc, char* quantifier,
                        char* cpOrderID, char* callbackUrl, char* extrasParams, char* price, char* amount, char* count,
                        char* serverName, char* serverID, char* gameRoleName, char* gameRoleID, char* gameRoleBalance,
                        char* vipLevel, char* gameRoleLevel, char* partyName)
{
    if (*extrasParams == '1')
    {
        aliPay(cpOrderID);
    }
    else if (*extrasParams == '2')
    {
        initWXSDK(wxPayAppId);
        weChatPay(cpOrderID);
    }
}
    
    /// 微信分享Web
void requestWXWebShare(int scene, char* shareTitle, char* shareText, char* iconPath, char* webUrl)
{
    @autoreleasepool {
        //构造SendAuthReq结构体
        WXMediaMessage* message =[WXMediaMessage message];
        message.title = [NSString stringWithCString:shareTitle encoding:NSUTF8StringEncoding];
        message.description = [NSString stringWithCString:shareText encoding:NSUTF8StringEncoding];
        [message setThumbImage:[UIImage imageNamed:[NSString stringWithCString:iconPath encoding:NSUTF8StringEncoding]]];
        
        WXWebpageObject *webpadgeObject = [WXWebpageObject object];
        webpadgeObject.webpageUrl = [NSString stringWithCString:webUrl encoding:NSUTF8StringEncoding];
        message.mediaObject = webpadgeObject;
        
        SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
        req.bText = NO;
        req.message = message;
        if(scene == 1)
        {
            req.scene = WXSceneSession;
        }
        else if (scene == 2)
        {
            req.scene = WXSceneTimeline;
        }
        //第三方向微信终端发送一个SendAuthReq消息结构
        [WXApi sendReq:req];
    }
}

/// 微信分享图片
void requestWXImageShare(int scene, char* iconPath, char* imagePath)
{
    @autoreleasepool {
        //构造SendAuthReq结构体
        WXMediaMessage* message =[WXMediaMessage message];
        
        //大图
        WXImageObject *imageObject = [WXImageObject object];
        NSString *filepath = [NSString stringWithCString:imagePath encoding:NSUTF8StringEncoding];
        imageObject.imageData = [NSData dataWithContentsOfFile:filepath];
        message.mediaObject = imageObject;
        
        //缩小图
        CGFloat compression = 0.7;
        NSUInteger maxLength = 32*1024;
        UIImage *bigImage = [UIImage imageWithData:imageObject.imageData];
        UIImage *smallImage = bigImage;
        NSData *data = UIImageJPEGRepresentation(bigImage, compression);
        if (data.length < maxLength)
        {
            smallImage = bigImage;
        }
        else{
            CGFloat max = 1;
            CGFloat min = 0;
            // Compress by quatity
            for (int i = 0; i < 6; ++i) {
                compression = (max + min) / 2;
                data = UIImageJPEGRepresentation(bigImage, compression);
                if (data.length < maxLength * 0.9) {
                    min = compression;
                } else if (data.length > maxLength) {
                    max = compression;
                } else {
                    break;
                }
            }
            smallImage = [UIImage imageWithData:data];
            if (data.length < maxLength)
            {
                //smallImage = resultImage;
            }
            else
            {
                // Compress by size
                NSUInteger lastDataLength = 0;
                while (data.length > maxLength && data.length != lastDataLength) {
                    lastDataLength = data.length;
                    CGFloat ratio = (CGFloat)maxLength / data.length;
                    CGSize size = CGSizeMake((NSUInteger)(smallImage.size.width * sqrtf(ratio)),
                                             (NSUInteger)(smallImage.size.height * sqrtf(ratio))); // Use NSUInteger to prevent white blank
                    UIGraphicsBeginImageContext(size);
                    [smallImage drawInRect:CGRectMake(0, 0, size.width, size.height)];
                    smallImage = UIGraphicsGetImageFromCurrentImageContext();
                    UIGraphicsEndImageContext();
                    data = UIImageJPEGRepresentation(smallImage, compression);
                }
            }
        }
        
        [message setThumbImage:smallImage];
        
        
        SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
        req.bText = NO;
        req.message = message;
        if(scene == 1)
        {
            req.scene = WXSceneSession;
        }
        else if (scene == 2)
        {
            req.scene = WXSceneTimeline;
        }
        //第三方向微信终端发送一个SendAuthReq消息结构
        [WXApi sendReq:req];
    }
}

/// 设置回调对象名字
extern "C" void avmSign(char* request_core, int signType)
{
//    NSString *ns_request_core = [NSString stringWithCString:request_core encoding:NSUTF8StringEncoding];
//    NSString *result = [[SGSDK shareClass] avmSign:ns_request_core];
    
//    @autoreleasepool {
//        EasyJson* jsObj = [[EasyJson alloc] init];
//        [jsObj addKey:@"input" string:ns_request_core];
//        [jsObj addKey:@"signResult" string:result];
//        [jsObj addKey:@"type" string:[NSString stringWithFormat:@"%d", signType]];
//        NSString* jstr = [jsObj toStr];
//        UnitySendMessage([s_wxCallbackGameObject UTF8String], "onGetAvmpSign", [jstr UTF8String]);
//    }
}


    /// 微信分享图片
    extern "C" void ShareMutilPic(int flag, char* desc, char* img1, char* img2, char* img3, char* img4, char* img5, char* img6)
    {
        @autoreleasepool {
            NSString *filepath1 = [NSString stringWithCString:img1 encoding:NSUTF8StringEncoding];
            NSString *filepath2 = [NSString stringWithCString:img2 encoding:NSUTF8StringEncoding];
            NSString *filepath3 = [NSString stringWithCString:img3 encoding:NSUTF8StringEncoding];
            NSString *filepath4 = [NSString stringWithCString:img4 encoding:NSUTF8StringEncoding];
            NSString *filepath5 = [NSString stringWithCString:img5 encoding:NSUTF8StringEncoding];
            NSString *filepath6 = [NSString stringWithCString:img6 encoding:NSUTF8StringEncoding];
            
            NSArray *filePathArray = @[filepath1,filepath2,filepath3,filepath4,filepath5,filepath6];
            NSArray *filePaths = [NSArray array];
            for (int i=0; i< filePathArray.count; i++)
            {
                NSString *path = filePathArray[i];
                if ( path == nil or path.length <= 0)
                {}
                else
                {
                    filePaths = [filePaths arrayByAddingObject:path];
                }
            }
            /*NSArray *activityItems = @[imgUrl1,imgUrl2,randomUrl];
             UIActivityViewController *activityVC = [[UIActivityViewController alloc]initWithActivityItems:activityItems applicationActivities:nil];
             
             activityVC.excludedActivityTypes = @[UIActivityTypePostToFacebook,UIActivityTypePostToTwitter, UIActivityTypePostToWeibo,UIActivityTypeMessage,UIActivityTypeMail,UIActivityTypePrint,UIActivityTypeCopyToPasteboard,UIActivityTypeAssignToContact,UIActivityTypeSaveToCameraRoll,UIActivityTypeAddToReadingList,UIActivityTypePostToFlickr,UIActivityTypePostToVimeo,UIActivityTypePostToTencentWeibo,UIActivityTypeAirDrop,UIActivityTypeOpenInIBooks];
             //[self presentViewController:activityVC animated:TRUE completion:nil];
             [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:activityVC animated:TRUE completion:nil];
             */
            
            NSMutableArray *array = [[NSMutableArray alloc]init];
            for (int i = 0; i<filePaths.count; i++) {
                NSString *imagePath = filePaths[i];
                NSLog(@"imagePath:%@",imagePath);
                NSData * data = [NSData dataWithContentsOfFile:imagePath];
                
                UIImage *imagerang = [UIImage imageWithData:data];
                NSURL *shareobj = [NSURL fileURLWithPath:imagePath];
                /** 这里做个解释 imagerang : UIimage 对象 shareobj:NSURL 对象 这个方法的实际作用就是 在调起微信的分享的时候 传递给他 UIimage对象,在分享的时候 实际传递的是 NSURL对象 达到我们分享九宫格的目的 */
                SharedItem *item = [[SharedItem alloc] initWithData:imagerang andFile:shareobj];
                [array addObject:item];
            }
            UIActivityViewController *activityViewController =[[UIActivityViewController alloc] initWithActivityItems:array applicationActivities:nil];
            if (SYSTEM_VERSION_LESS_THAN(@"9.0")) {
                UIAlertView *mBoxView = [[UIAlertView alloc]
                                         initWithTitle:@"提示"
                                         message:@"您的系统版本太低，无法进行朋友圈分享。"
                                         delegate:nil
                                         cancelButtonTitle:@"取消"
                                         otherButtonTitles:@"确定", nil];
                [mBoxView show];
            }
            else
            {
                //尽量不显示其他分享的选项内容
                activityViewController.excludedActivityTypes = @[ UIActivityTypePostToFacebook,
                                                                  UIActivityTypePostToTwitter,
                                                                  UIActivityTypePostToWeibo,
                                                                  UIActivityTypeMessage,
                                                                  UIActivityTypeMail,
                                                                  UIActivityTypePrint,
                                                                  UIActivityTypeCopyToPasteboard,
                                                                  UIActivityTypeAssignToContact,
                                                                  UIActivityTypeSaveToCameraRoll,
                                                                  UIActivityTypeAddToReadingList,
                                                                  UIActivityTypePostToFlickr,
                                                                  UIActivityTypePostToVimeo,
                                                                  UIActivityTypePostToTencentWeibo,
                                                                  UIActivityTypeAirDrop
                                                                  ];//UIActivityTypeOpenInIBooks
                if (activityViewController) {
                    UIViewController * sourceView = [UIApplication sharedApplication].keyWindow.rootViewController;
                    if (sourceView != nil)
                    {
                        [sourceView presentViewController:activityViewController animated:TRUE completion:nil];
                    }
                }
            }
        }
    }
#ifdef __cplusplus
}
#endif


