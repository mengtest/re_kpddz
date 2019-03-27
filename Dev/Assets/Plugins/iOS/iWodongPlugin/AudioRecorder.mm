/*!
 
 @brief      音频处理
 
 @copyright  Copyright © 2017年 ruiqugames. All rights reserved.
 
 @author     WP.Chu
 @version    1.0.0
 @date       2017.9.10
 
 */

#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>
#include <string>
#include <vector>
#import "amrFileCodec.h"
#include "MainAppController.h"

#pragma mark - UnityAppController录音扩展

/*!
 @brief 实现音频录制和播放协议<AVAudioRecorderDelegate, AVAudioPlayerDelegate>
 */
@interface UnityAppController(AudioRecordAndPlay)<AVAudioRecorderDelegate, AVAudioPlayerDelegate>

@end

@implementation UnityAppController(AudioRecordAndPlay)

/// 录音完成
- (void)audioRecorderDidFinishRecording:(AVAudioRecorder *)recorder successfully:(BOOL)flag
{
}

/// 播放完成
- (void)audioPlayerDidFinishPlaying:(AVAudioPlayer *)player successfully:(BOOL)flag
{
    //player = nil;
}

@end


#pragma mark - 变量和宏定义部分

/// 集合类型
#define TAudioSets std::vector<std::string>

/// 初始化检测
#define init_test do{ if(!_audioInit) return;}while(0)

/// 临时文件
static const std::string s_tempCaf = "tmpaudio.caf";

/// 音频存储目录
static std::string s_audioStorageDirectory = "";

/// 最短录音时间
static float s_minimumSeconds = 0.5f;

/// 是否初始化
bool _audioInit = false;

/// 开始时间
static double s_startRecordTime = 0;
/// 结束时间
static double s_endRecordTime = 0;

/// 音频合集
TAudioSets _audios;

/// 录音机
AVAudioRecorder* _audioRecorder = nil;

/// 播放器
AVAudioPlayer* _audioPlayer = nil;

/// 目标文件名
static std::string s_targetAmrName = "";

#pragma mark - 功能函数


/**
 *  取得录音文件设置
 *
 *  @return 录音设置
 */
NSDictionary* getAudioSetting()
{
    NSMutableDictionary *dicM=[NSMutableDictionary dictionary];
    //设置录音格式
    [dicM setObject:@(kAudioFormatLinearPCM) forKey:AVFormatIDKey];
    //设置录音采样率，8000是电话采样率，对于一般录音已经够了
    [dicM setObject:@(16000.00) forKey:AVSampleRateKey];
    //设置通道,这里采用单声道
    [dicM setObject:@(1) forKey:AVNumberOfChannelsKey];
    //每个采样点位数,分为8、16、24、32
    [dicM setObject:@(16) forKey:AVLinearPCMBitDepthKey];
    //是否使用浮点数采样
    [dicM setObject:@(NO) forKey:AVLinearPCMIsFloatKey];
    
    [dicM setObject:@(NO) forKey:AVLinearPCMIsNonInterleaved];
    [dicM setObject:@(NO) forKey:AVLinearPCMIsBigEndianKey];
    //....其他设置等
    return dicM;
}



/// 停止录音
NSURL* stopAudioRecordImpl()
{
    if (_audioRecorder == nil)
    {
        return nil;
    }
    
    NSURL *url =[[NSURL alloc]initWithString:_audioRecorder.url.absoluteString];
    [_audioRecorder stop];
    
    return url;
}

/// 删除音频
bool deleteAudioImpl(std::string audioName)
{
    NSFileManager* mgr = [NSFileManager defaultManager];
    NSString* audioPath = [[NSString stringWithUTF8String:s_audioStorageDirectory.c_str()] stringByAppendingPathComponent:[NSString stringWithUTF8String:audioName.c_str()]];
    
    if ([mgr fileExistsAtPath:audioPath])
    {
        NSError* err;
        return [mgr removeItemAtPath:audioPath error:&err];
    }
    
    return false;
}

/// 设置音频会话参数
bool setAVAudioSession()
{
    AVAudioSession *audioSession=[AVAudioSession sharedInstance];
    //设置为播放和录音状态，以便可以在录制完之后播放录音
    [audioSession setCategory:AVAudioSessionCategoryPlayAndRecord error:nil];
    UInt32 audioRouteOverride = kAudioSessionOverrideAudioRoute_Speaker;
    AudioSessionSetProperty (kAudioSessionProperty_OverrideAudioRoute,
                             sizeof (audioRouteOverride),
                             &audioRouteOverride);
    //Activate the session
    return [audioSession setActive:YES error: nil];
}

#pragma mark - Audios导出接口

/**
 * @brief 初始化
 *
 * @param audiosTargetDir	语音文件目标文件目录
 */
extern "C" void initAudio(char* audiosTargetDir)
{
    s_audioStorageDirectory = audiosTargetDir;
    
    // 创建录音目录
    NSFileManager* mgr = [NSFileManager defaultManager];
    _audioInit = [mgr createDirectoryAtPath:[NSString stringWithUTF8String:audiosTargetDir] withIntermediateDirectories:true attributes:nil error:nil];
    
    if (_audioInit)
    {
        _audios.reserve(16);
    }
}

/**
 * @brief 设置最短录音时间
 *
 * @param minimumSeconds	最短录音时间
 */
extern "C" void setMinimumTime(float minimumSeconds)
{
    s_minimumSeconds = minimumSeconds;
}

/**
 * @brief 清空语音文件目录
 */
extern "C" void clear()
{
    init_test;
    for(auto iter=_audios.end(); iter!=_audios.end(); ++iter)
    {
        deleteAudioImpl((*iter).c_str());
    }
    
    _audios.clear();
}

/**
 * @brief 删除指定语音文件
 *
 * @param audioName 语音文件音名，带扩展名
 */
extern "C" void deleteAudio (char* audioName)
{
    init_test;
    bool rlt = deleteAudioImpl(audioName);
    if (!rlt)
    {
        return;
    }
    
    auto iter = std::find(_audios.begin(), _audios.end(), std::string(audioName));
    if (iter != _audios.end())
    {
        _audios.erase(iter);
    }
}

/**
 * @brief 添加音频名称
 *
 * @param audioName 语音文件音名(带扩展名)
 */
extern "C" void addAudionName (const char* audioName)
{
    init_test;
    std::string name = audioName;
    auto iter = std::find(_audios.begin(), _audios.end(), std::string(name));
    if (iter == _audios.end())
    {
        _audios.push_back(name);
    }
    
}

/**
 * @brief 开始录音
 *
 * @param audioName 语音文件音名(带扩展名)
 */
extern "C" void startRecord (char* audioName)
{
    init_test;
    
    if (!setAVAudioSession())
    {
        return;
    }
        
    [_audioRecorder stop];
    [[NSFileManager defaultManager] removeItemAtURL:_audioRecorder.url error:nil];
    _audioRecorder = nil;
    

    //if (_audioRecorder == nil)
    {
        //NSString* cafFile = [NSString stringWithFormat:@"tempcaf%d.caf", i++];
        NSString* cafFile = [NSString stringWithUTF8String:s_tempCaf.c_str()];
        NSString* audioPath = [[NSString stringWithUTF8String:s_audioStorageDirectory.c_str()] stringByAppendingPathComponent:cafFile];
        NSURL *url= [NSURL fileURLWithPath:audioPath];
        
        //创建录音格式设置
        NSDictionary *setting= getAudioSetting();
        //创建录音机
        NSError *error = nil;
        _audioRecorder = [[AVAudioRecorder alloc] initWithURL:url settings:setting error:&error];
        if (error)
        {
            NSLog(@"创建录音机对象时发生错误，错误信息：%@",error.localizedDescription);
            return;
        }
        _audioRecorder.delegate = GetAppController();
        _audioRecorder.meteringEnabled=YES;
        bool ready = [_audioRecorder prepareToRecord];
        if (ready)
            [_audioRecorder record];
        
        // 记录开始时间
        s_startRecordTime = [NSDate timeIntervalSinceReferenceDate];
        
        // 保存目标文件
        s_targetAmrName = audioName;
        
    }
}

/**
 * @brief 停止录音
 */
extern "C" void stopRecord ()
{
    init_test;
    NSURL* url = stopAudioRecordImpl();
    
    AVAudioSession *session = [AVAudioSession sharedInstance];
    [session setCategory:AVAudioSessionCategoryPlayback error:nil];  //此处需要恢复设置回放标志，否则会导致其它播放声音也会变小
    
    // 记录结束时间
    s_endRecordTime = [NSDate timeIntervalSinceReferenceDate];
    if ((s_endRecordTime-s_startRecordTime) < s_minimumSeconds)
    {
        return;
    }
    
    if (url != nil)
    {
        NSData* waveData = [NSData dataWithContentsOfURL:url];
        
        NSData* amrData = EncodeWAVEToAMR(waveData,1,16);
        if (amrData) {
            NSString* audioPath = [[NSString stringWithUTF8String:s_audioStorageDirectory.c_str()] stringByAppendingPathComponent:[NSString stringWithUTF8String:s_targetAmrName.c_str()]];
            NSFileManager* mgr = [NSFileManager defaultManager];
            [mgr createFileAtPath:audioPath contents:amrData attributes:nil];
            
            // 添加到管理列表
            addAudionName(s_targetAmrName.c_str());
        }
    }
}

/**
 * @brief 播放音频
 *
 * @param audioName 录音名字(带扩展名)
 */
extern "C" void playAudio (char* audioName)
{
    init_test;
    
    NSString* audioPath = [[NSString stringWithUTF8String:s_audioStorageDirectory.c_str()] stringByAppendingPathComponent:[NSString stringWithUTF8String:audioName]];
    NSURL* url = [NSURL fileURLWithPath:audioPath];
    
    // 读取amr音频数据
    NSData* amrData = [NSData dataWithContentsOfURL:url];
    if (amrData == nil)
    {
        return;
    }
    
    NSLog(@"amr: %@", url);
    
    NSData* waveData = DecodeAMRToWAVE(amrData);
    NSError* error = nil;
    _audioPlayer = [[AVAudioPlayer alloc] initWithData:waveData error:&error];
    if (error)
    {
        NSLog(@"创建播放对象时发生错误，错误信息：%@",error.localizedDescription);
        _audioPlayer = nil;
        return;
    }
    
    _audioPlayer.delegate = GetAppController();
    _audioPlayer.numberOfLoops=0;
    //[audioPlayer setVolume:1.0];
    bool ready = [_audioPlayer prepareToPlay];
    if (ready)
    {
        [_audioPlayer play];
    }
}
