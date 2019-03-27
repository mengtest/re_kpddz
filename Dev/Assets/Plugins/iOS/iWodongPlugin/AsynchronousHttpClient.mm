/*!
 
 @brief      异步http客户端
 
 @copyright  Copyright © 2017年 ruiqugames. All rights reserved.
 
 @author     WP.Chu
 @version    1.0.0
 @date       2017.9.10
 
 */

#import <Foundation/Foundation.h>
#include <string>
#import <AFNetworking/AFNetworking.h>
#include "EasyJson.h"

extern void vibrateDevice();

#pragma mark - unity回调函数名字

/// HTTP任务开始
static const std::string __func_start__  __attribute__((deprecated("此为Android才有的回调函数名， iOS无该回调")))  = "onStart";
/// HTTP任务结束
static const std::string __func_finish__ __attribute__((deprecated("此为Android才有的回调函数名， iOS无该回调")))   = "onFinish";
/// HTTP任务进度
static const std::string __func_progress__  = "onProgress";
/// HTTP任务成功
static const std::string __func_success__   = "onSuccess";
//／ HTTP任务失败
static const std::string __func_failure__   = "onFailure";
//／ HTTP任务重试
static const std::string __func_retry__  __attribute__((deprecated("此为Android才有的回调函数名， iOS无该回调")))   = "onRetry";


#pragma mark - 变量

/// 默认下载目录(APP -> Caches)
static std::string s_defaultDownloadDirectory = "";

/// 基础URL
static std::string s_baseURL = "http://ksyx.update.iwodong.com";

/// unity回调处理对象名字
static std::string s_unityGameObjectName = "";

/// 创建默认下载目录
static void createDefaultDownloadDir()
{
    NSFileManager* mgr = [NSFileManager defaultManager];
    
    NSArray* cachesDirs = NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES);
    NSString* cachesDir = [cachesDirs objectAtIndex:0];
    NSString* downloadDir = [cachesDir stringByAppendingPathComponent:@"download"];
    bool ret = [mgr createDirectoryAtPath:downloadDir withIntermediateDirectories:true attributes:nil error:nil];
    if (ret)
        s_defaultDownloadDirectory = [downloadDir UTF8String];
    else
        s_defaultDownloadDirectory = [cachesDir UTF8String];
}


#pragma mark - 导出接口

/**
 @brief    初始化异步HTTP客户端
 
 @param baseURL  基础URL地址
 */
extern "C" void initAsyncClient(char* baseURL)
{
    std::string url = baseURL;
    if (!url.empty())
        s_baseURL = url;
    
    // 创建默认下载目录
    createDefaultDownloadDir();
}



/*!
 @brief  异步HTTP下载文件
 
 @param url             URL地址
 @param relativeUrl     是否相对地址，是则取 baseUrl+url
 @param destPath        目标文件路径
 @param relativePath    是否相对路径, 是则取 s_defaultDownloadDirectory+elativePath
 @param destFileName    目标文件名
 @param vibrator		下载完成震动时间，0不震动
 @param gameObjectName	unity回调对象名字
 
 @todo 暂时在objectc里做，后续全部统一用unity3D的接口实现
 */
extern "C" void downloadFile (char* url, bool relativeUrl, char* destPath, bool relativePath, char* destFileName, int vibrator, char* gameObjectName)
{
    
    s_unityGameObjectName = gameObjectName;
    
    // 生成下载路径
    NSURL* dwnUrl = nil;
    if (relativeUrl)
    {
        dwnUrl = [NSURL URLWithString: [NSString stringWithUTF8String:s_baseURL.c_str()]];
        dwnUrl = [dwnUrl URLByAppendingPathComponent:[NSString stringWithUTF8String:url]];
    }
    else
    {
        dwnUrl = [NSURL URLWithString: [NSString stringWithUTF8String:url]];
    }
    
    // 生成目标路径
    NSString* targetPath = nil;
    if (relativePath)
    {
        targetPath = [NSString stringWithUTF8String:s_defaultDownloadDirectory.c_str()];
        targetPath = [targetPath stringByAppendingPathComponent:[NSString stringWithUTF8String:destPath]];
    }
    else
    {
        targetPath = [NSString stringWithUTF8String:destPath];
    }
    targetPath = [targetPath stringByAppendingPathComponent:[NSString stringWithUTF8String:destFileName]];
    
    // 执行下载
    AFHTTPSessionManager *mgr = [AFHTTPSessionManager manager];
    mgr.responseSerializer = [AFHTTPResponseSerializer serializer]; // http响应序列化器， 默认为json
    NSURLSessionDataTask* session = [mgr GET: dwnUrl.absoluteString parameters:nil
                                    // 任务进度
                                    progress:^(NSProgress * _Nonnull downloadProgress)
                                     {
                                         NSLog(@"progress: %lld%/%lld", downloadProgress.completedUnitCount, downloadProgress.totalUnitCount);
                                         
                                         EasyJson* jsObj = [[EasyJson alloc] init];
                                         [jsObj addKey:@"currentSize" int:(int)downloadProgress.completedUnitCount];
                                         [jsObj addKey:@"totalSize" int:(int)downloadProgress.totalUnitCount];
                                         NSString* jstr = [jsObj toStr];
                                         UnitySendMessage(s_unityGameObjectName.c_str(), __func_progress__.c_str(), [jstr UTF8String]);
                                     }
                                     
                                     // 成功
                                     success:^(NSURLSessionDataTask * _Nonnull task, id  _Nullable responseObject)
                                     {
                                         NSLog(@"-------success %@", task.response);
                                         NSData* data = (NSData*)responseObject;
                                         
                                         // 保存文件
                                         NSFileManager* fmgr = [NSFileManager defaultManager];
                                         bool rlt = [fmgr createFileAtPath:targetPath contents:data attributes:nil];
                                         if (rlt)
                                         {
                                             EasyJson* jsObj = [[EasyJson alloc] init];
                                             [jsObj addKey:@"statusCode" int: 0];
                                             [jsObj addKey:@"filePath" string:targetPath];
                                             [jsObj addKey:@"msg" string:@""];
                                             NSString* jstr = [jsObj toStr];
                                             UnitySendMessage(s_unityGameObjectName.c_str(), __func_success__.c_str(), [jstr UTF8String]);

                                         }
                                         else
                                         {
                                             EasyJson* jsObj = [[EasyJson alloc] init];
                                             [jsObj addKey:@"statusCode" int: 1];
                                             [jsObj addKey:@"msg" string: @"iOS - Create file failure."];
                                             NSString* jstr = [jsObj toStr];
                                             UnitySendMessage(s_unityGameObjectName.c_str(), __func_failure__.c_str(), [jstr UTF8String]);
                                         }
                                     }
                                     
                                     // 失败
                                     failure:^(NSURLSessionDataTask * _Nullable task, NSError * _Nonnull error)
                                     {
                                         NSLog(@"-------failure %@", error);
                                         
                                         EasyJson* jsObj = [[EasyJson alloc] init];
                                         [jsObj addKey:@"statusCode" int: (int)[error code]];
                                         [jsObj addKey:@"msg" string: [error localizedDescription]];
                                         NSString* jstr = [jsObj toStr];
                                         UnitySendMessage(s_unityGameObjectName.c_str(), __func_failure__.c_str(), [jstr UTF8String]);

                                     }];
    
}


/*!
 @brief  异步HTTP下载文件
 
 @param url             URL地址
 @param relativeUrl     是否相对地址，是则取 baseUrl+url
 @param destPath        目标文件路径
 @param relativePath    是否相对路径, 是则取 s_defaultDownloadDirectory+elativePath
 @param destFileName    目标文件名
 @param vibrator		下载完成震动时间，0不震动
 @param gameObjectName	unity回调对象名字
 
 @todo 暂时在objectc里做，后续全部统一用unity3D的接口实现
 */
extern "C" void downloadFileDefault (char* url, bool relativeUrl, char* destPath, char* destFileName, int vibrator, char* gameObjectName)
{
    bool relativePath = true;
    NSString* fileName = [[NSString alloc] initWithUTF8String:destFileName];
    NSString* ext = [fileName pathExtension];
    if ([ext isEqualToString:@"amr"])
    {
        relativePath = false;
    }
    
    
    downloadFile(url, relativeUrl, destPath, relativePath, destFileName, vibrator, gameObjectName);
}



/// <summary>
/// 上传文件（流）到服务器
/// </summary>
/// <param name="url">URL地址.</param>
/// <param name="relativeUrl">是否相对地址.</param>
/// <param name="destFileName">目标文件名.</param>
/// <param name="fileAbsolutePath">文件绝对路径.</param>
/// <param name="gameObjectName">unity回调对象名称</param>
extern "C" void uploadFile (const char* url, bool relativeUrl, const char* destFileName, const char* fileAbsolutePath, const char* gameObjectName)
{
    s_unityGameObjectName = gameObjectName;
    
    // 生成上传路径
    NSURL* uploadUrl = nil;
    if (relativeUrl)
    {
        uploadUrl = [NSURL URLWithString: [NSString stringWithUTF8String:s_baseURL.c_str()]];
        uploadUrl = [uploadUrl URLByAppendingPathComponent:[NSString stringWithUTF8String:url]];
    }
    else
    {
        uploadUrl = [NSURL URLWithString: [NSString stringWithUTF8String:url]];
    }

    NSString* remoteFileName = [NSString stringWithUTF8String:destFileName];
    NSString* filePath = [NSString stringWithUTF8String:fileAbsolutePath];
    NSLog(@"uploadFile: %@",filePath);
    AFHTTPSessionManager *mgr = [AFHTTPSessionManager manager];
    mgr.responseSerializer = [AFHTTPResponseSerializer serializer]; // http响应序列化器， 默认为json
    NSURLSessionDataTask* session = [mgr POST:uploadUrl.absoluteString parameters:@{@"fileName" : remoteFileName}
                                // 构造http body
                                constructingBodyWithBlock:^(id<AFMultipartFormData> formData)
                                {
                                    [formData appendPartWithFileURL:[NSURL fileURLWithPath: filePath] name:@"file" fileName: remoteFileName mimeType:@"image/png" error:nil];
                                }
                                
                                // 上传进度
                                progress:^(NSProgress *uploadProgress)
                                {
                                    //NSLog(@"progress: %lld%/%lld", uploadProgress.completedUnitCount, uploadProgress.totalUnitCount);
                                    NSLog(@"progress: %lld%/%lld", uploadProgress.completedUnitCount, uploadProgress.totalUnitCount);
                                
                                    EasyJson* jsObj = [[EasyJson alloc] init];
                                    [jsObj addKey:@"currentSize" int:(int)uploadProgress.completedUnitCount];
                                    [jsObj addKey:@"totalSize" int:(int)uploadProgress.totalUnitCount];
                                    NSString* jstr = [jsObj toStr];
                                    UnitySendMessage(s_unityGameObjectName.c_str(), __func_progress__.c_str(), [jstr UTF8String]);
                                }
                                
                                // 上传成功
                                success:^(NSURLSessionDataTask *task, id responseObject)
                                {
                                    NSLog(@"-------success %@", task.response);
                                    NSData* data = (NSData*)responseObject;
                                    NSString* ret = [[NSString alloc] initWithData:data  encoding:NSUTF8StringEncoding];
                                    NSLog(@"-------responseObject %@", ret);
                                    
                                    EasyJson* jsObj = [[EasyJson alloc] init];
                                    [jsObj addKey:@"statusCode" int: 0];
                                    [jsObj addKey:@"filePath" string:ret];
                                    [jsObj addKey:@"msg" string:ret];
                                    NSString* jstr = [jsObj toStr];
                                    UnitySendMessage(s_unityGameObjectName.c_str(), __func_success__.c_str(), [jstr UTF8String]);
                                }
                                
                                // 上传失败
                                failure:^(NSURLSessionDataTask *task, NSError *error) {
                                    NSLog(@"-------failure %@", error);
                                    EasyJson* jsObj = [[EasyJson alloc] init];
                                    [jsObj addKey:@"statusCode" int: (int)[error code]];
                                    [jsObj addKey:@"msg" string: [error localizedDescription]];
                                    NSString* jstr = [jsObj toStr];
                                    UnitySendMessage(s_unityGameObjectName.c_str(), __func_failure__.c_str(), [jstr UTF8String]);

                                }];
    
}
