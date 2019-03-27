/***************************************************************


 *
 *
 * Filename:  	ClientDefine.cs	
 * Summary: 	客户端全局宏定义
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/01/20 18:09
 ***************************************************************/


using System.Collections.Generic;

public class ClientDefine
{
    public const int PLAT_FORM_TYPE = 2; //机器类型 1表示iphone，2表示android
    //本地程序版本号 
    public const string LOCAL_PROGRAM_VERSION = "2.0.1";
    public const string LOCAL_SOURCE_VERSION = "1002";
    //游戏开发中
    public static int GAME_IN_DEVELOPING = 0;

    //接入第三方SDK
    public static bool THIRD_PARTY_SDK = false;

    //脚本模块
    public static bool LUA_MODULE = true;

    //人物声音存放
    public static Dictionary<uint, uint> heroSoundDic = new Dictionary<uint, uint>();
    public static bool _NoVip = false;
    /// <summary>
    /// 生成的Lua脚本的作者
    /// </summary>
    public static string EditorAuthor = "EQ";
    /// <summary>
    ///  平台标识：1开发服 2外网测试服
    /// </summary>
    public static uint PLATFORM_FLAG = 1;
    /// <summary>
    /// 使用捕鱼的登陆时用默认的IP及Port
    /// </summary>
    public static bool isUseBuYuNet = false;

#if UNITY_EDITOR
    public static int _OpenGuideFlag = 0;//0：不干涉引导       1：强制开启       2：强制关闭
    public static int _ShowGuideSwitch = 1;//0:隐藏    1:显示
    public static int _OpenVipPay = 1;//1：开启  2:不开启
    public static int _OpenNotice = 1;//2不弹出
    public static bool _DoNotCheckVersion = true;//是否不检查更新
    public static bool isWXLogin = false;//是否微信登录
    
#else
	public static int _OpenGuideFlag = 1;//0：不干涉引导       1：强制开启       2：强制关闭
	public static int _ShowGuideSwitch = 0;//0:隐藏    1:显示
    public static int _OpenVipPay = 2;//1：开启
    public static int _OpenNotice = 1;//1弹出
    public static bool _DoNotCheckVersion = false;//是否不检查更新
    public static bool isWXLogin = true;//是否微信登录
#endif
}


 