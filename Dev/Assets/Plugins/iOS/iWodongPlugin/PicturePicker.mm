/*!
 
 @brief      处理从相册或者相机拾取图片
 
 @copyright  Copyright © 2017年 ruiqugames. All rights reserved.

 @author     WP.Chu
 @version    1.0.0
 @date       2017.9.8
 
 */

#import <Foundation/Foundation.h>
#import <MobileCoreServices/MobileCoreServices.h>
#include "MainAppController.h"
#include <string>
#include "EasyJson.h"


/// unity图片回调处理对象名字
static std::string s_unityGameObjectName;

/// 当前图片格式
static std::string s_fmt;


#pragma mark - 功能函数

/// 创建图片目录
static bool createTemporaryImgDir()
{
    NSFileManager* mgr = [NSFileManager defaultManager];
    
    NSArray* cachesDirs = NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES);
    NSString* cachesDir = [cachesDirs objectAtIndex:0];
    NSString* imgDir = [cachesDir stringByAppendingPathComponent: @"img"];
    return [mgr createDirectoryAtPath:imgDir withIntermediateDirectories:true attributes:nil error:nil];
}


/// 获取临时图片路径
static NSString* getTemporaryPngPath()
{
    NSArray* cachesDirs = NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES);
    NSString* cachesDir = [cachesDirs objectAtIndex:0];
    NSString* imgCachesDir = [cachesDir stringByAppendingPathComponent: @"img"];
    return [imgCachesDir stringByAppendingPathComponent:@"temp.png"];
}

/// 发送成功信息到unity3d
static void sendSuccess2Unity(NSString* imgPath)
{
    @autoreleasepool {
        EasyJson* jsObj = [[EasyJson alloc] init];
        [jsObj addKey:@"result" string:@"success"];
        [jsObj addKey:@"filePath" string:imgPath];
        NSString* jstr = [jsObj toStr];
        UnitySendMessage(s_unityGameObjectName.c_str(), "onSuccess", [jstr UTF8String]);
    }
}

/// 发送失败信息导unity3d
static void sendFailure2Unity(int error, NSString* msg)
{
    @autoreleasepool {
        EasyJson* jsObj = [[EasyJson alloc] init];
        [jsObj addKey:@"error" int:error];
        [jsObj addKey:@"msg" string:msg];
        NSString* jstr = [jsObj toStr];
        UnitySendMessage(s_unityGameObjectName.c_str(), "onFailure", [jstr UTF8String]);
    }
}

#pragma mark - UnityAppController图片拾取扩展


/*!
 @brief 实现图片拾取协议<UIImagePickerControllerDelegate,UINavigationControllerDelegate>
 
 @discussion 处理object-C和unity层交互部分
 */
@interface UnityAppController(ImagePicker)<UIImagePickerControllerDelegate, UINavigationControllerDelegate>

@end


@implementation UnityAppController(ImagePicker)

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<NSString *,id> *)info
{
    //todo: 下面两句注释打开回出现一个link error, 因为只处理图片，所以屏蔽if判断也不会影响功能，后续再查
    //NSString *mediaType=[info objectForKey:UIImagePickerControllerMediaType];
    //if ([mediaType isEqualToString:(NSString *)kUTTypeImage])
    {
    
        // 创建图片目录
        bool ret = createTemporaryImgDir();
        if (!ret)
        {
            sendFailure2Unity(1, @"iOS - Create image temporary directory failure");
            return;
        }
        
        // 压缩图片
        NSData *fileData = UIImagePNGRepresentation(info[UIImagePickerControllerEditedImage]);
        if (fileData == nil)
        {
            sendFailure2Unity(2, @"iOS - Image data is nil");
            return;
        }
        
        NSString* imgPath = getTemporaryPngPath();
        NSFileManager* mgr = [NSFileManager defaultManager];
        bool success = [mgr createFileAtPath:imgPath contents:fileData attributes:nil];
        if (!success)
        {
            sendFailure2Unity(3, @"iOS - Create temporary image file failure");
            return;
        }
        
        if (s_unityGameObjectName.empty())
        {
            sendFailure2Unity(4, @"iOS - Unity game object's name is null");
            return;
        }
        
        // 通知unity
        sendSuccess2Unity(imgPath);
    }
    
    // 关闭图片选择view
    [GetAppController().rootViewController dismissViewControllerAnimated:YES completion:nil];
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
    sendFailure2Unity(4, @"iOS - User cancel");

    // 关闭图片选择view
    [GetAppController().rootViewController dismissViewControllerAnimated:YES completion:nil];
    
    
    // EasyJson 测试
    /*
     
    NSString* jsArray = [[NSString alloc] initWithString:@"[123234234234, asdfasdfa, 987654325454545]"];
    EasyJson* jobj = [[EasyJson alloc] initWithString:jsArray];
    
    NSLog(@"Json array:  %@, %@, %@", [jobj objectAtIndex:0], [jobj objectAtIndex:1], [jobj objectAtIndex:2]);
    
    
    NSString* jsDict = [[NSString alloc] initWithString:@"{\"a\":2, \"b\":\"fuck\", \"c\":\"you\", \"d\":1.268}"];
    EasyJson* jobjDict = [[EasyJson alloc] initWithString:jsDict];
    NSLog(@"Json dict:  %@, %@, %@, %@", [jobjDict objectForKey:@"a"], [jobjDict objectForKey:@"b"], [jobjDict objectForKey:@"c"], [jobjDict objectForKey:@"d"]);
    */
}

@end


#pragma mark - 图片拾取导出接口

/*!
 @brief 从相册拾取图片
 
 @param compressFormat	图片压缩格式
 @param crop            是否裁剪
 @param outputX         裁剪宽(无效)
 @param outputY			裁剪高（无效）
 @param gameObjectName	unity回调对象名字
 
 @todo 实现拾取后裁剪
 */
extern "C" void pickPictureFromAlbum (char* compressFormat, bool crop, int outputX, int outputY, char* gameObjectName)
{
    s_fmt = compressFormat;
    s_unityGameObjectName = gameObjectName;
    
    @autoreleasepool {
        UIImagePickerController *imagePickerController= [[UIImagePickerController alloc] init];
        imagePickerController.delegate = GetAppController();
        imagePickerController.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;
        imagePickerController.allowsEditing = YES;
        imagePickerController.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
        [GetAppController().rootViewController presentViewController:imagePickerController animated:YES completion:nil];
    }
}


extern "C" void pickPictureFromCamera (char* compressFormat, bool crop, int outputX, int outputY, char* gameObjectName)
{
    s_fmt = compressFormat;
    s_unityGameObjectName = gameObjectName;
    
    @autoreleasepool {
        UIImagePickerController *imagePickerController= [[UIImagePickerController alloc] init];
        imagePickerController.delegate = GetAppController();
        imagePickerController.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;
        imagePickerController.allowsEditing = YES;
        imagePickerController.sourceType = UIImagePickerControllerSourceTypeCamera;
        [GetAppController().rootViewController presentViewController:imagePickerController animated:YES completion:nil];
    }

}




