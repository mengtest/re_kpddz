/*!
 
 @brief      提供系统通用功能
 
 @copyright  Copyright © 2017年 ruiqugames. All rights reserved.
 
 @author     WP.Chu
 @version    2017.9.8
 
 */

#import <Foundation/Foundation.h>
#import <AudioToolbox/AudioToolbox.h>

#ifdef __cplusplus
extern "C"
{
#endif


/// 获取电池剩余电量（0.00～1.00）
float getBatteryRemainingPower()
{
    [UIDevice currentDevice].batteryMonitoringEnabled = true;
    return [UIDevice currentDevice].batteryLevel;
}

/// 震动设备
void vibrateDevice()
{
    AudioServicesPlaySystemSound(kSystemSoundID_Vibrate) ;
}



#ifdef __cplusplus
}
#endif
