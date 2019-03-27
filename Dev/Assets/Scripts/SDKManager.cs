/***************************************************************


 *
 *
 * Filename:  	SDKManager.cs	
 * Summary: 	SDK管理
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/03/24 15:54
 ***************************************************************/

//以下单选
#define ___NO_SDK_WINDOW____    //无sdk
//#define ___NO_SDK_ANDROID____    //无sdk
//#define ___NO_SDK_ANDROID_DDZ____    //蛋蛋赚
//#define ___NO_SDK_ANDROID_DC____    //点财
//#define ___NO_SDK_ANDROID_DM____    //多盟
//#define ___NO_SDK_ANDROID_SYZ____    //手游赚
//#define ___NO_SDK_ANDROID_YM____      //有米
//#define ___NO_SDK_ANDROID_MZ____      //米赚
//#define ___NO_SDK_ANDROID_XW____       //闲玩
//#define ___NO_SDK_ANDROID_TG____       //推广包

//#define ___APP_STORE_IOS____    //苹果商店
//#define ___NO_SDK_IOS____    //无sdk
//#define ___QUICK_SDK_ANDROID____    //quickSDK
//#define ___QUICK_SDK_IOS____    //quickSDK
//#define ___APP_STORE_IOS_2____    //苹果商店
//#define ___APP_STORE_IOS_KMGCUQ_1____    //苹果商店
//#define ___49_SDK_ANDROID____ //49
//#define ___YIJIE_SDK_ANDROID____ //49
//#define ___QUICK_SDK_ANDROID_CPS____    //quickSDK_cps
//#define ___WuHanKW_SDK_ANDROID____  //武汉快玩
//#define ___YL_YDJH_SDK_ANDROID____  //悦动聚合

//以下多选
//#define ___REYUN_IMPORT____  //热云统计
//#define ___REYUN_TRACT____  //热云追踪
#define ___DATAEYE_IMPORT____

using UnityEngine;
using System.Collections;
using Utils;
//using quicksdk;
//using sdk.quick;
using sdk.local;
//using sdk.sj;
//using sdk.yijie;
using Scene;
//using dataEyeStatistics;
//using task;
using System.Xml.Linq;
using System.Text;
//using sdk.ydjh;

namespace sdk
{
	/// <summary>
	/// SDKs 管理器
	/// </summary>
	public class SDKManager : Singleton<SDKManager>
	{
        public delegate void delegateLoadComplete(bool result, int errorCode);
        public delegateLoadComplete loadCallback;

        static public bool bAppStore = false;
        static public int Q_ID = 0; //0:无
        static public int S_QID = 0;
        static public string AppKey = "5a545f60a40fa30f48000052";
        static public string CurSDK = "version_buyu_window";
        static public string APKName = "BuYu_Official";
        static public string url_paths = "http://update.game3336.com/VERSION_BUYU/version_buyu_window/SDKManager.xml"; //SDKManager.xml

        static public string FastLoginURL = "http://123.57.214.206:3800/index.php/user/openid";//快速登陆地址（平台）
        static public string SaveLastLoginServerURL = "http://123.57.214.206:6100/userdata.php?uid=";//保存上次登录地址（平台）
        static public string AccountLoginURL = "http://123.57.214.206:3800/user/login";//账号登陆地址（平台）
        static public string AccountRegistURL = "http://123.57.214.206:3800/user/register";//账号注册地址（平台）
        static public string AccountBindURL = "http://123.57.214.206:3800/index.php/user/binduser";//账号绑定地址（平台）
        static public string ResetPasswordURL = "http://123.57.214.206:3800/index.php/user/forgetpwd";//重置密码地址（平台）
        static public string VerificationCodeURL = "http://123.57.214.206:3800/user/getcode";//请求手机验证码（平台）
        static public string LuaHotScriptURL = "http://update.game3336.com/VERSION_BUYU/version_war3g_appStore/luaCode/";//lua热更地址
        static public string ShareWebURL = "http://106.15.39.88:9800/geturl?code="; //分享的连接网页
        static public string ServerListUrl = "";//服务器列表
        static public string NoticeUrl = "";//公告xml
        static public string VersionXMLUrl = "";//版本xml
        static public string VersionDownloadUrl = "";//版本下载地址LoadPackagePath
        static public string PackageDownloadUrl = "";//包下載地址(內部下載)
        static public string PackageDownloadWeb = "";//包下載地址（跳网页下载）
        static public string LoginUrl = "";//登录地址
        static public string WxLoginUrl = "http://123.57.214.206:6800/index.php/user/openid";//登录地址";
        static public string AutoLoginUrl = "http://123.57.214.206:6800/user/checklogin";//微信自动登录地址
        static public string PlatformPayUrl = "";//平台支付地址
        static public string SharePicUrl = "";//分享
        static public string LoginSignUrl = "";//分享

        static public string BaiduTokenUrl = "http://login.thirteen.25y.com/baidu/gettoken";//取百度语音token
        static public string AudioUpUrl = "http://login.thirteen.25y.com/user/uploadaudio";//上传语音地址 //"http://bystatic-cdn.76y.com/upfile/";//
        static public string AudioSaveUrl = "http://login.thirteen.25y.com/Uploads/audio";//下载语音地址

		static public string PlatformPayUrlForAlipayAndWX = "http://123.57.214.206:6800/pay?d={0}&m={1}&uname={2}&server={3}&id={4}&type={5}&saletype={6}";// iOS微信和支付宝平台支付地址
        static public string PayCallbackUrl = "";//支付回调地址
        static public bool _toolSDKInited = false;//确保sdk只初始化一次
        static public bool _commonSDKInited = false;//确保sdk只初始化一次
        static public string PayConfigUrl = "http://123.57.214.206:9800/config/pay";
        //static public string StatisticsIconClick = "http://123.57.214.206:6100/stat.php?t=2&sid={0}&acid={1}&uid={2}&point={3}";//统计icon点击
        //static public string StatisticsDeviceUUID = "http://123.57.214.206:6100/stat.php?t=3&uuid={0}";//统计设备UUID
        //static public string StatisticsNewbieguideStep = "http://123.57.214.206:6100/stat.php?t=4&uuid={0}&point={1}";//统计新手引导步骤
        //通用接口
        ISDKCommonInterfce _commonSDKInterface = null;

        // SDKManager配置是否初始化
        private bool _isSDKXMLInited = false;

        void Awake()
        {
            initSDKMgrXML();
        }

        public void LoadUrlXml()
        {
            Debug.Log("CurSDK-------------->" + CurSDK);
            StartCoroutine("GetUrlXml");
        }

        IEnumerator GetUrlXml()
        {
            string xml_path = new StringBuilder(url_paths).AppendFormat("?p={0}", UtilTools.GetClientTime()).ToString();
            //下载ServerList.xml
            WWW www = new WWW(xml_path);
            yield return www;

            if (www.error != null)
            {
                www.Dispose();
                yield return new WaitForSeconds(1f);
                //下载ServerList.xml
                www = new WWW(xml_path);
                yield return www;
            }

            if (www.error != null)
            {
                www.Dispose();
                yield return new WaitForSeconds(1f);
                //下载ServerList.xml
                www = new WWW(xml_path);
                yield return www;
            }


            if (www == null)
            {
                ExecuteLoadCallback(false, 1);//失败(下载www创建失败)
            }
            else if ( www.error != null)
            {
                ExecuteLoadCallback(false, 2);//失败(下载的XML失败)
                Utils.LogSys.LogError("Load SDKManager.xml Error:" + www.error);
            }
            else if (  www.text.Length == 0)
            {
                ExecuteLoadCallback(false, 3);//失败(下载的XML是空的)
            }
            else
            {
                //解释版本列表xml。
                ParseUrlXML(www.text);
            } 
            www.Dispose();
        }
        public void GetConfig()
        {
            StartCoroutine(GetPayConfig());
        }
        IEnumerator GetPayConfig()
        {
            yield return null;
            WWW www = new WWW(SDKManager.PayConfigUrl);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                yield return new WaitForSeconds(1f);
                www = new WWW(SDKManager.PayConfigUrl);
                yield return www;
            }
            if (string.IsNullOrEmpty(www.error))
            {
                if (www.isDone)
                {
                    if (!string.IsNullOrEmpty(www.text))
                    {
                        JSONObject arrStr = new JSONObject(www.text);
                        SDKManager.getInstance().CommonSDKInterface.InitPaySdk(arrStr[0].str, arrStr[1].str);
                    }
                }
            }
        }

        void ParseUrlXML_XElement(XElement item)
        {
            foreach (XElement param in item.Descendants("Item"))
            {
                string sName = param.Attribute("name").Value;
                string sUrl = param.Attribute("url").Value;

                if (!string.IsNullOrEmpty(sName) && sName == "FastLoginURL") { FastLoginURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "SaveLastLoginServerURL") { SaveLastLoginServerURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "AccountLoginURL") { AccountLoginURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "AccountRegistURL") { AccountRegistURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "AccountBindURL") { AccountBindURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "ResetPasswordURL") { ResetPasswordURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "AppKey") { AppKey = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "VerificationCodeURL") { VerificationCodeURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "LuaHotScriptURL") { LuaHotScriptURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "ShareWebURL") { ShareWebURL = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "PayConfigUrl")
                {
                    PayConfigUrl = sUrl; 
                }

                
                else if (!string.IsNullOrEmpty(sName) && sName == "BaiduTokenUrl") { BaiduTokenUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "AudioUpUrl") { AudioUpUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "AudioSaveUrl") { AudioSaveUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "LoginSignUrl") { LoginSignUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "ServerListUrl") { ServerListUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "NoticeUrl") { NoticeUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "VersionXMLUrl") { VersionXMLUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "VersionDownloadUrl") { VersionDownloadUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "AutoLoginUrl") { AutoLoginUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "WxLoginUrl") { WxLoginUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "PackageDownloadWeb") { PackageDownloadWeb = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "LoginUrl") { LoginUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "PlatformPayUrl") { PlatformPayUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "PlatformPayUrlForAlipayAndWX") { PlatformPayUrlForAlipayAndWX = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "PayCallbackUrl") { PayCallbackUrl = sUrl; }
                else if (!string.IsNullOrEmpty(sName) && sName == "SharePicUrl") { SharePicUrl = sUrl; }
                    
                else if (!string.IsNullOrEmpty(sName) && sName == "PackageDownloadUrl")
                {
                    if (sUrl.IndexOf(".apk") == -1)
                        PackageDownloadUrl = sUrl + APKName + ".apk";
                    else
                        PackageDownloadUrl = sUrl;
                }
                else if (!string.IsNullOrEmpty(sName) && sName == "Q_ID") { int.TryParse(sUrl, out Q_ID); }
                else if (!string.IsNullOrEmpty(sName) && sName == "S_QID") { int.TryParse(sUrl, out S_QID); }

                //else if (!string.IsNullOrEmpty(sName) && sName == "StatisticsIconClick") { StatisticsIconClick = sUrl; }
                //else if (!string.IsNullOrEmpty(sName) && sName == "StatisticsDeviceUUID") { StatisticsDeviceUUID = sUrl; }
                //else if (!string.IsNullOrEmpty(sName) && sName == "StatisticsNewbieguideStep") { StatisticsNewbieguideStep = sUrl; }
            }
        }
        void ParseUrlXML(string xmlFile)
        {
            XDocument docTemp = XDocument.Parse(xmlFile);

            if (docTemp == null)
            {
                ExecuteLoadCallback(false, 4);//失败(解释的XML失败)
                return;//下载版本信息文件失败
            }

            bool bFound = false;
            //取默认版本列表
            foreach (XElement item in docTemp.Root.Descendants("Platform"))
            {
                string SDKName = item.Attribute("SDKName").Value;
                if (!string.IsNullOrEmpty(SDKName) && SDKName.Equals("version_buyu_default"))
                {
                    ParseUrlXML_XElement(item);
                    bFound = true;
                    break;
                }
            }
            //取特殊修改的版本列表
            foreach (XElement item in docTemp.Root.Descendants("Platform"))
            {
                string SDKName = item.Attribute("SDKName").Value;
                if (!string.IsNullOrEmpty(SDKName) && SDKName == CurSDK)
                {
                    ParseUrlXML_XElement(item);
                    break;
                }
            }
            
            if (bFound)
            {
                GetConfig();
                ExecuteLoadCallback(true, 0);//成功
            }
            else
            {
                ExecuteLoadCallback(false, 5);//失败(没有找到当前平台的Url配置)
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="errorCode"></param>
        /// 1:失败(下载www创建失败)
        /// 2:失败(下载的XML失败)
        /// 3:失败(下载的XML是空的)
        /// 4:失败(解释的XML失败)
        /// 5:失败(没有找到不前平台的Url配置)
        void ExecuteLoadCallback(bool res, int errorCode)
        {
            if (loadCallback != null)
            {
                loadCallback(res, errorCode);
            }
        }

		/// <summary>
		/// 获取接口
		/// </summary>
		public ISDKCommonInterfce CommonSDKInterface
		{
			get
			{
				return _commonSDKInterface;
			}
		}


        //SDK初始化
        public void init()
        {
            //初始化CommonSDK
            //initCommonSDK();
            funcIconClick("0");
        }

        public void initToolSDK()
        {
            if (_toolSDKInited)
                return;

            _toolSDKInited = true;

            //Testin
            initTestin();

            bool needReYunGame = false;
            bool needReYunTrack = false;
#if ___REYUN_IMPORT____
			needReYunGame = true;
#endif
#if ___REYUN_TRACT____
			needReYunTrack = true;
#endif
            //热云初始化
            reyunInit(needReYunGame, needReYunTrack);
            Invoke("sendDeviceUniqueIdentifier", 2);
        }

        //////////////////////////////////////////////////////////////////////////////

        /**
		 * tesIn 接入
		 * 
		 * Android：TestinAgent Helper.Init ()
		 * iOS: CrashMasterHelper.Init ()
		 */

        void initTestin()
		{
#if UNITY_IOS
			//CrashMasterHelper.Init();
#elif UNITY_ANDROID
			//TestinAgentHelper.Init();
#endif
		}

		//////////////////////////////////////////////////////////////////////////////

		/**
		 * QUICKSDK接入
		 * 
		 * Android：TestinAgent Helper.Init ()
		 * iOS: CrashMasterHelper.Init ()
		 */
        public void initCommonSDK()
		{
			if (_commonSDKInited)
                return;

            _commonSDKInterface = new LocalSDKInterface(gameObject);
            //初始化
            if (_commonSDKInterface != null) 
				_commonSDKInterface.init();

            _commonSDKInited = true;
            BaiDuSDK.getInstance().Init();
        }

        /// <summary>
        /// 读取SDKManager.xml
        /// </summary>
        public void initSDKMgrXML()
        {
            if (_isSDKXMLInited)
                return;

            TextAsset text_asset = Resources.Load("SDKManager") as TextAsset;
            INIParser ini = new INIParser();
            ini.Open(text_asset);
            CurSDK = ini.ReadValue("platform", "name", "version_poker_window");
            if (!string.IsNullOrEmpty(JARUtilTools.Channel_Name))
            {
                CurSDK = JARUtilTools.Channel_Name;
                Debug.Log("JARUtilTools.CurChannel========>" + CurSDK);
            }
            else
            {
                Debug.Log("JARUtilTools.CurChannel is null    !!!!!!!!!!!!");
            }
            Q_ID = ini.ReadValue(CurSDK, "qid", 0);
            if (!string.IsNullOrEmpty(JARUtilTools.Channel_QID))
                int.TryParse(JARUtilTools.Channel_QID, out Q_ID);
            S_QID = ini.ReadValue(CurSDK, "sqid", 0);
            if (!string.IsNullOrEmpty(JARUtilTools.Channel_SQID))
                int.TryParse(JARUtilTools.Channel_SQID, out S_QID);
            bAppStore = ini.ReadValue(CurSDK, "appstore", false);
            string xmlFile = ini.ReadValue(CurSDK, "xmlfile", "SDKManager.xml");
            APKName = ini.ReadValue(CurSDK, "apkname", "BuYu_Official");
            if (!string.IsNullOrEmpty(JARUtilTools.Channel_ApkName))
                APKName = JARUtilTools.Channel_ApkName;
            if (CurSDK != "version_poker_android_out_test" && CurSDK != "version_poker_android_in_test")
            {
#if UNITY_IOS
                    url_paths = string.Format("http://update.game3336.com/VERSION_POKER/{0}/{1}", "version_poker_ios_appstore", xmlFile);
#else
                url_paths = string.Format("http://update.game3336.com/VERSION_POKER/{0}/{1}", "version_poker_android_official", xmlFile);
#endif
            }
            else
            {
                url_paths = string.Format("http://update.game3336.com/VERSION_POKER/{0}/{1}", CurSDK, xmlFile);
            }
            ini.Close();

            _isSDKXMLInited = true;
        }

        /// <summary>
        /// 热云初始化接口
        /// </summary>
		void reyunInit(bool needReYunGame, bool needReYunTrack)
		{
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="idIcon"></param>
        public void funcIconClick(string idIcon)
        {
        }
        

        /// <summary>
        /// 发送UDID
        /// </summary>
        public void sendDeviceUniqueIdentifier()
		{
        }

        /// <summary>
        /// 发送新手引导UDID
        /// </summary>
        public  void sendNewbieguideDeviceUniqueIdentifier(string point)
        {
        }

        

        /// <summary>
        /// 发送给本地统计服务器设备ID
        /// </summary>
        public IEnumerator send2LocalStatisticServerUDID(object value)
        {
            string strInfo = value as string;

            WWW www = new WWW(strInfo);
            yield return www;
        }

        //是否appStore版本
        public static bool isAppStoreVersion()
        {
            getInstance().initSDKMgrXML();
            return bAppStore;
        }

        //是否要等SDK初始化完成后再开始检测版本
        public static bool needWaitForSDKInitComplete_ToCheckVersion()
        {
            if (!ClientDefine.THIRD_PARTY_SDK)
            {
                return false;
            }
// 			if (GameDataMgr.PLAYER_DATA != null && MyPlayer._bLoginOut) {
// 				return false;
// 			}
            return false;
        }

        public static void ToCheckVersion()
        {
            if (needWaitForSDKInitComplete_ToCheckVersion())
            {
                GameObject sceneObj = GameObject.Find("VersionUpdate");
                if (sceneObj)
                {
                    SDKManager.getInstance().LoadUrlXml();
//                     VersionUpdate versionMono = sceneObj.GetComponent<VersionUpdate>();
//                     if (versionMono != null)
//                     {
//                         versionMono.Init();
//                     }
                }
            }
        }

        //是否走官方支付
        public static bool IsOfficialPay()
        {
            getInstance().initSDKMgrXML();
            if (bAppStore)
            {
                return false;
            }
            return true;
        }
    }
}


