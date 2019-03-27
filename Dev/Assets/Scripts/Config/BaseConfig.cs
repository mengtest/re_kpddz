
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BaseConfig
{
    static public void ChangeGameConfigUrl(int chnSdkId, int chnId, int subChnId, string clientVer, string resVer)
    {
        GameConfigUrl = GameConfigUrl + "search?sdk_id=" + chnSdkId + "&major_channel=" + chnId + "&sub_channel=" + subChnId + "&client=" + clientVer + "&version=" + resVer;
    }

#if UMIP_OUT
    static public string UpdateRoot = "http://192.168.4.166:1234/";
    static public string UpdateAnnouncementUrl = "http://192.168.1.213/UpdateNotice.xml";
    static public string ServerListUrl = "http://www.yxzr.com/serverlist/ServerList.xml";
    //static public string ServerListUrl = "http://uc-dota-update.umlife.net:32311/ServerList.xml";
    static public string GameConfigUrl = "http://192.168.4.166/dtll/index.php/version/";
	//static public string GameConfigUrl = "http://uc-dota-update.umlife.net:10889/dtll/index.php/vers
#elif UMIP_IN
    	static public string UpdateRoot = "http://192.168.4.166:1234/";
    static public string UpdateAnnouncementUrl = "http://192.168.1.213/UpdateNotice.xml";
    static public string ServerListUrl = "http://www.yxzr.com/serverlist/ServerList.xml";
    //static public string ServerListUrl = "http://uc-dota-update.umlife.net:32311/ServerList.xml";
    static public string GameConfigUrl = "http://192.168.4.166/dtll/index.php/version/";
	//static public string GameConfigUrl = "http://uc-dota-update.umlife.net:10889/dtll/index.php/vers
#elif UMIP_UMI
    	static public string UpdateRoot = "http://192.168.4.166:1234/";
    static public string UpdateAnnouncementUrl = "http://192.168.1.213/UpdateNotice.xml";
    static public string ServerListUrl = "http://www.yxzr.com/serverlist/ServerList.xml";
    //static public string ServerListUrl = "http://uc-dota-update.umlife.net:32311/ServerList.xml";
    static public string GameConfigUrl = "http://192.168.4.166/dtll/index.php/version/";
	//static public string GameConfigUrl = "http://uc-dota-update.umlife.net:10889/dtll/index.php/vers
#elif ANDROID_OUT
    	static public string UpdateRoot = "http://192.168.4.166:1234/";
    static public string UpdateAnnouncementUrl = "http://192.168.1.213/UpdateNotice.xml";
    static public string ServerListUrl = "http://www.yxzr.com/serverlist/ServerList.xml";
    //static public string ServerListUrl = "http://uc-dota-update.umlife.net:32311/ServerList.xml";
    static public string GameConfigUrl = "http://192.168.4.166/dtll/index.php/version/";
	//static public string GameConfigUrl = "http://uc-dota-update.umlife.net:10889/dtll/index.php/vers
#elif ANDROID_IN
    	static public string UpdateRoot = "http://192.168.4.166:1234/";
    static public string UpdateAnnouncementUrl = "http://192.168.1.213/UpdateNotice.xml";
    static public string ServerListUrl = "http://www.yxzr.com/serverlist/ServerList.xml";
    //static public string ServerListUrl = "http://uc-dota-update.umlife.net:32311/ServerList.xml";
    static public string GameConfigUrl = "http://192.168.4.166/dtll/index.php/version/";
	//static public string GameConfigUrl = "http://uc-dota-update.umlife.net:10889/dtll/index.php/vers
#elif ANDROID_Complex
    	static public string UpdateRoot = "http://192.168.4.166:1234/";
    static public string UpdateAnnouncementUrl = "http://192.168.1.213/UpdateNotice.xml";
    static public string ServerListUrl = "http://www.yxzr.com/serverlist/ServerList.xml";
    //static public string ServerListUrl = "http://uc-dota-update.umlife.net:32311/ServerList.xml";
    static public string GameConfigUrl = "http://192.168.4.166/dtll/index.php/version/";
	//static public string GameConfigUrl = "http://uc-dota-update.umlife.net:10889/dtll/index.php/vers
#else
    static public string UpdateRoot = "http://192.168.4.166:1234/";
    static public string UpdateAnnouncementUrl = "http://192.168.1.213/UpdateNotice.xml";
    static public string GameConfigUrl = "http://192.168.4.166/dtll/index.php/version/";
#endif


//    static public string IP = "192.168.1.148";
    static public string IP = "192.168.1.51";
    static public int port = 8991;

    //static public int HttpPort = 9899;
    static public int HttpPort = 9001;
    static public int ServerState = 1;//[1:良好][2:拥挤][3:爆满][4:维护]
	static public string RegistrationUrl = "http://uc-dota-01.umlife.net:9003/cgi-bin/account:register";
    /// <summary>
    /// 头像上传地址
    /// </summary>
    static public string HeadUpImgUrl = "http://106.15.39.88:9800/user/uploadface";
    /// <summary>
    /// 头像存放地址
    /// </summary>
    static public string HeadSaveImgUrl = "http://106.15.39.88:9800/Uploads/faces/";

    //static public string HeadUpImgUrl = "http://123.57.214.206:9800/user/uploadface";
    ///// <summary>
    ///// 头像存放地址
    ///// </summary>
    //static public string HeadSaveImgUrl = "http://123.57.214.206:9800/Uploads/faces/";
    /// <summary>
    /// 账号登陆地址（平台）
    /// </summary>
    //static public string LoginURL = "http://120.25.0.203:6101/index.php/user/login";

    /// <summary>
    /// 账号注册地址（平台）
    /// </summary>
    //static public string RegistURL = "http://120.25.0.203:6101/index.php/user/register";



}
