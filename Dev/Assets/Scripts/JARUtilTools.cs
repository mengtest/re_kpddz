using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using sdk;
using task;
using Utils;
using object_c;
using System.Collections.Generic;

public class JARUtilTools : MonoBehaviour
{

    public static string Channel_Name = "";
    public static string Channel_QID = "";
    public static string Channel_SQID = "";
    public static string Channel_XmlFile = "";
    public static string Channel_ApkName = "";
    //页面类型
    public enum EWebViewType
    {
        none,
        normal,
        alipay,
    }

    private string messsage = "log area";
    string path_moveto;
    string path_save;
    string file_name;

    // 视频播放回调
    Dictionary<string, UtilTools.onPlayVideoComplete> _videosCallback = new Dictionary<string, UtilTools.onPlayVideoComplete>();

    //当前页面打开类型
    EWebViewType _eCurrentWebType = EWebViewType.none;

#if UNITY_ANDROID

    //安卓系统通用操作接口
	private AndroidJavaObject _javaSysUtilsOptObj = null;
    //音频
    private AndroidJavaObject _javaGameAudioManagerObj = null;
    //硬件信息接口
    private AndroidJavaObject _javaSysHardwareInfoObj = null;
        // 瓦力
    private AndroidJavaObject _javaWallePlugin = null;
    private AndroidJavaObject _javaMainActivity = null;
    
    void Awake()
    {
        initUtilTools();
    }

    // 初始化
    void Start () {
        //initUtilTools();
    
        GetDeviceUUID();
	}

    //初始化JAVA对象
    private bool initUtilTools()
	{
		if (_javaSysUtilsOptObj == null)
		{
			try
			{
                _javaSysUtilsOptObj = new AndroidJavaObject("com.iwodong.unityplugin.SystemUtilsOperation");
                _javaSysHardwareInfoObj = new AndroidJavaObject("com.iwodong.unityplugin.HardwareInfo");
                AndroidJavaClass ac = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                if (ac != null)
                {
                    _javaMainActivity = ac.GetStatic<AndroidJavaObject>("currentActivity");
                }
                if (_javaMainActivity != null)
                {
                    _javaWallePlugin = new AndroidJavaObject("com.meituan.android.walle.WalleChannelReader");
                    if (_javaWallePlugin != null)
                    {
                        Channel_Name = _javaWallePlugin.CallStatic<string>("getChannel", _javaMainActivity, "version_poker_android_official");
                        Channel_QID = _javaWallePlugin.CallStatic<string>("get", _javaMainActivity, "qid");
                        Channel_SQID = _javaWallePlugin.CallStatic<string>("get", _javaMainActivity, "sqid");
                        Channel_XmlFile = _javaWallePlugin.CallStatic<string>("get", _javaMainActivity, "xmlfile");
                        Channel_ApkName = _javaWallePlugin.CallStatic<string>("get", _javaMainActivity, "apkname");
                        Debug.Log("=======>getChannel:  " + Channel_Name);

                    }
                    else
                    {
                        Debug.Log("_javaWallePlugin is null  !!!!!!!!!!!!!");
                    }
                }
            }
			catch
			{
                messsage = "Init AndroidNotificator UtilTools Fail";
				return false;
			}
		}

		if (_javaSysUtilsOptObj == null)
		{
            messsage = "AndroidNotificator UtilTools Not Found.";
			return false;
		}

        _javaSysUtilsOptObj.CallStatic("init");
        _javaSysUtilsOptObj.CallStatic("initAsyncHttpClient", "");  //初始化异步http

        //开启推送服务器
        //startLocalNotification();

        return true;
	}

    /*
    void Update()
    {
        if (_javaObj != null)
            messsage = _javaObj.CallStatic<string>("GetLOG");
    }

    void OnGUI()  
    {
        if (GUI.Button(new Rect(20, 20, 100, 100), "Save"))  
        {
            //SaveScreenshot();

            playVideo("threecountrycg.mov", 1136, 640, 1);
        }

        if (GUI.Button(new Rect(20, 250, 100, 100), "Copy"))
        {
            //_javaObj.CallStatic("Copy", "sssssssa");

            playVideo("newbieguide1.mp4", 1136, 640, 1);
        }

        if (GUI.Button(new Rect(20, 400, 100, 100), "Paste"))
        {
            //_javaObj.CallStatic<string>("Paste");

            playVideo("newbieguide3.mp4", 1136, 640, 1);
        }

        //GUI.TextField(new Rect(0, Screen.height - 100, Screen.width, 100), messsage);

    }

*/
    /// <summary>
    /// 开启推送服务器
    /// </summary>
    public void startLocalNotification()
    {
        string xmlPath = "Config/localNotification.json";

        AssetLoadTask task = new AssetLoadTask(xmlPath, null);
        task.EventFinished += new task.TaskBase.FinishedHandler(delegate (bool manual, TaskBase currentTask)
        {
            UnityEngine.Object assetObj = ((AssetLoadTask)currentTask).getTargetAsset();
            if (assetObj != null)
                _javaSysUtilsOptObj.Call("startPushNotificationService", assetObj.ToString());
            else
                LogSys.LogError("load config failed:" + xmlPath);
        });
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    //存储路径
    public void SaveScreenshot()
    {
        /*
        //step1判断SD卡容量
        Boolean hasSDCard = _javaObj.CallStatic<Boolean>("ExistSDCard");//是否插SD卡
        long freeSize = _javaObj.CallStatic<long>("GetSDFreeSize");//空闲容量，单位M
        if (!hasSDCard || freeSize < 1)
        {
            return;
        }

        //step2准备好文件名和目录
        System.DateTime now = new System.DateTime();
        now = System.DateTime.Now;
        file_name = string.Format("{0}-{1}-{2} {3}:{4}:{5}.png", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        path_save = Application.persistentDataPath; //"/storage/WoDong/SanGuo";//非SD卡保存目录
        path_moveto = _javaObj.CallStatic<string>("GetSDCardPath") + "/DCIM/WoDong/SanGuo";//改为SD卡目录
        messsage = path_save + "/" + file_name;
        if (!Directory.Exists(path_moveto))
        {
            Directory.CreateDirectory(path_moveto);
        }

        //step3截屏
        Application.CaptureScreenshot(file_name);

        //step4移到相册目录
        StartCoroutine(SaveToMyPhoto(path_save + "/" + file_name, path_moveto + "/" + file_name));
        */
}

    //将截图移到相册目录，并在相册中显示
    IEnumerator SaveToMyPhoto(string pathShot, string pathMoveTo, int nTimes=10)
    {
        yield return new WaitForSeconds(1f);
        //移到相册目录
        bool bSuccess = true;
        try
        {
            System.IO.File.Move(pathShot, pathMoveTo);//移截屏文件
        }
        catch (System.Exception e)
        {
            messsage = e.ToString();
            bSuccess = false;
        }
        if (bSuccess)
        {
            messsage = "move success";
            _javaSysUtilsOptObj.CallStatic("SaveScreenshot", pathMoveTo);//扫描该文件，在相册中显示
        }

        //如果失败再试10次
        if (nTimes > 0 && !bSuccess)
        {
            nTimes--;
            StartCoroutine(SaveToMyPhoto(pathShot, pathMoveTo, nTimes));
        }
    }

    void SaveScreenshotCallback()
    {
        _javaSysUtilsOptObj.CallStatic("SaveScreenshot", path_save, file_name);
    }

    //同步读取assetbundle文件
    //path:相对StreamingAssets目录
    public byte[] GetAssetBundleBytes(string path)
    {
        if (_javaSysUtilsOptObj != null)
        {
            //UtilTools.SetServerListTip("_javaObj: " + path);
            return _javaSysUtilsOptObj.CallStatic<byte[]>("GetAssetBundle", path);//空闲容量，单位M
        }
        else
        {
            //UtilTools.SetServerListTip("_javaObj is null");
            return File.ReadAllBytes(path);
        }

        return null;
    }


    /**********音频**********/
    /// <summary>
    /// 初始化音频对象
    /// </summary>
    /// <param name="strStorageDir">存放地址</param>
    public void initAudioManager()
    {
        if (_javaGameAudioManagerObj != null)
            return;

    
        string sPath = Application.persistentDataPath;
        sPath = sPath.Replace("\\", "/");
        sPath = string.Format("{0}/Audio",sPath);
        _javaGameAudioManagerObj = new AndroidJavaObject("com.iwodong.unityplugin.GameAudioManager");
        _javaGameAudioManagerObj.Call("init", sPath);
    }
    
    /// <summary>
    /// 清空语音文件
    /// </summary>
    public void clearAudio()
    {
        if (_javaGameAudioManagerObj == null)
            return;

        _javaGameAudioManagerObj.Call("clear");
    }
    
    /// <summary>
    /// 清删除指定语音文件
    /// </summary>
    public void deleteAudio(string strAudioName)
    {
        if (_javaGameAudioManagerObj == null)
            return;

        _javaGameAudioManagerObj.Call("deleteAudio",strAudioName);
    }
    
    /// <summary>
    /// 添加音频名称
    /// </summary>
    public void addAudioName(string strAudioName)
    {
        if (_javaGameAudioManagerObj == null)
            return;

        _javaGameAudioManagerObj.Call("addAudioName",strAudioName);
    }
    
    /// <summary>
    /// 开始录音
    /// </summary>
    public bool startRecord(string strAudioName)
    {
        if (_javaGameAudioManagerObj == null)
            return false;

        return _javaGameAudioManagerObj.Call<bool>("startRecord",strAudioName);
    }
    
    /// <summary>
    /// 停止录制
    /// </summary>
    public void stopRecord()
    {
        if (_javaGameAudioManagerObj == null)
            return;

        _javaGameAudioManagerObj.Call("stopRecord");
    }
    
    /// <summary>
    /// 播放音频
    /// </summary>
    public void playAudio(string strAudioName)
    {
        Utils.LogSys.Log("playAudio:" + strAudioName);
        if (_javaGameAudioManagerObj == null)
            return;
        addAudioName(strAudioName);
        _javaGameAudioManagerObj.Call("playAudio",strAudioName);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 复制到粘贴板
    /// </summary>
    /// <param name="strVal"></param>
    public void copyString2Clipboard(string strVal)
    {
        if (_javaSysUtilsOptObj != null)
        {
            _javaSysUtilsOptObj.CallStatic("copyString2Clipboard", strVal);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 从粘贴板获取字符串
    /// </summary>
    /// <returns></returns>
    public string getStringFromClipboard()
    {
        string strContent = "";
        if (_javaSysUtilsOptObj != null)
        {
            strContent = _javaSysUtilsOptObj.CallStatic<string>("getStringFromClipboard");
        }

        return strContent;
    }

    /// <summary>
    /// 获取CPU名字
    /// </summary>
    /// <returns></returns>
    public string getCPUName()
    {
        if (_javaSysHardwareInfoObj != null)
            return _javaSysHardwareInfoObj.CallStatic<string>("getCPUName");

        return "";
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取最大主频(MHz)
    /// </summary>
    /// <returns></returns>
    public int getCPUMaxFreqM()
    {
        int maxFreq = 0;
        if (_javaSysHardwareInfoObj != null)
            maxFreq = _javaSysHardwareInfoObj.CallStatic<int>("getCPUMaxFreqK") / 1024;

        return maxFreq;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取CPU核数
    /// </summary>
    /// <returns></returns>
    public int getCPUCoreNum()
    {
        int cpuCoreNum = 0;
        if (_javaSysHardwareInfoObj != null)
            cpuCoreNum = _javaSysHardwareInfoObj.CallStatic<int>("getCPUCoreNum");

        return cpuCoreNum;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取总RAM内存(KB)
    /// </summary>
    /// <returns></returns>
    public long getTotalMemoryKB()
    {
        long sysMem = 0;
        if (_javaSysHardwareInfoObj != null)
            sysMem = _javaSysHardwareInfoObj.CallStatic<long>("getTotalMemoryKB");

        return sysMem;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取可用RAM(KB)
    /// </summary>
    /// <returns></returns>
    public long getAvailableMemoryKB()
    {
        long sysAvailableMem = 0;
        if (_javaSysHardwareInfoObj != null)
            sysAvailableMem = _javaSysHardwareInfoObj.CallStatic<long>("getAvailableMemoryKB");

        return sysAvailableMem;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 播放视频
    /// </summary>
    /// <param name="videoName">视频名字</param>
    /// <param name="videoWidth">视频宽度</param>
    /// <param name="videoHeight">视频高度</param>
    /// <param name="screenWidth">屏幕宽度</param>
    /// <param name="screenHeight">屏幕高度</param>
    /// <param name="mode">视屏模式： 1宽高比缩放  2高宽比缩放</param>
    public void playVideo(string videoName, int videoWidth, int videoHeight, int clipCount, int mode, UtilTools.onPlayVideoComplete callback = null)
    {
        if (_javaSysUtilsOptObj != null)
        {
            _javaSysUtilsOptObj.CallStatic("startPlayVideo", videoName, clipCount, videoWidth, videoHeight, Screen.width, Screen.height, mode, gameObject.name);

            // 添加回调
            if (!_videosCallback.ContainsKey(videoName) && callback != null)
                _videosCallback.Add(videoName, callback);
        }
    }
    
    public string GetDeviceUUID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }

    /// <summary>
    /// 打开内置web页面
    /// </summary>
    /// <param name="url">URL地址</param>
    /// <param name="bToolbarHide">隐藏工具条</param>
    public void openWebView(string url, EWebViewType eType, bool bToolbarHide = false)
    {
        if (_javaSysUtilsOptObj != null)
        {
            _eCurrentWebType = eType;
            _javaSysUtilsOptObj.CallStatic("openWebView", url, bToolbarHide, gameObject.name);
        }
    }

    /// <summary>
    /// 打开第三方app
    /// </summary>
    /// <param name="packageName">目标包名</param>
    /// <param name="category">启动方式 目前支持两种启动方式：
	///     android.intent.category.DEFAULT
    ///     android.intent.category.LAUNCHER</param>
    /// <param name="extraInfo">额外传递到第三方app的信息，通过itent发送</param>
    /// <returns>是否启动成功</returns>
    public bool openPackage(string packageName, string category, string extraInfo)
    {
        if (_javaSysUtilsOptObj != null)
            return _javaSysUtilsOptObj.CallStatic<bool>("openPackage", packageName, category, extraInfo);

        return false;
    }

    /// <summary>
    /// 启动APK安装程序
    /// </summary>
    /// <param name="apkAbsolutePath">APK文件绝对路径</param>
    public void startInstallAPK(string apkAbsolutePath)
    {
        if (_javaSysUtilsOptObj != null)
            _javaSysUtilsOptObj.CallStatic("startInstallAPK", apkAbsolutePath);
    }

    /// <summary>
    /// 异步HTTP下载文件
    /// </summary>
    /// <param name="url">下载URL地址</param>
    /// <param name="relativeUrl">是否相对地址，是则取 baseUrl+url</param>
    /// <param name="destPath">目标文件路径（SDCard存在时有效）</param>
    /// <param name="destFileName">目标文件名</param>
    /// <param name="vibrator">下载完成震动时间，0不震动</param>
    /// <param name="gameObjectName">unity回调对象名称</param>
    public void asyncHttpDownloadFile(string url, bool relativeUrl, string destPath, string destFileName, int vibrator, string gameObjectName)
    {
        if (_javaSysUtilsOptObj != null)
            _javaSysUtilsOptObj.CallStatic("asyncDownloadFile", url, relativeUrl, destPath, destFileName, vibrator, gameObjectName);
    }

    /// <summary>
    /// 上传文件（字节）到服务器
    /// </summary>
    /// <param name="url">URL地址</param>
    /// <param name="relativeUrl">是否相对地址</param>
    /// <param name="destFileName">目标文件名</param>
    /// <param name="content">字节内容</param>
    /// <param name="gameObjectName">unity回调对象名称</param>
    public void asyncHttpUploadFile(string url, bool relativeUrl, string destFileName, byte[] content, string gameObjectName)
    {
        if (_javaSysUtilsOptObj != null)
            _javaSysUtilsOptObj.CallStatic("asyncUploadFile", url, relativeUrl, destFileName, content, gameObjectName);
    }

    /// <summary>
    /// 上传文件（流）到服务器
    /// </summary>
    /// <param name="url">URL地址</param>
    /// <param name="relativeUrl">是否相对地址</param>
    /// <param name="destFileName">目标文件名</param>
    /// <param name="fileAbsolutePath">文件绝对路径</param>
    /// <param name="gameObjectName">unity回调对象名称</param>
    public void asyncHttpUploadFile(string url, bool relativeUrl, string destFileName, string fileAbsolutePath, string gameObjectName)
    {
        if (_javaSysUtilsOptObj != null)
            _javaSysUtilsOptObj.CallStatic("asyncUploadFile", url, relativeUrl, destFileName, fileAbsolutePath, gameObjectName);
    }

    /// <summary>
    /// 从相册捡取图片
    /// </summary>
    /// <param name="compressFormat">压缩格式 (png, jpeg/jpg)</param>
    /// <param name="crop">是否裁剪</param>
    /// <param name="outputX">输出X（宽度）</param>
    /// <param name="outputY">输出Y（高度）</param>
    /// <param name="gameObjectName">unity回调对象名称</param>
    public void pickPhotoFromAlbum(string compressFormat, bool crop, int outputX, int outputY, string gameObjectName)
    {
        if (_javaSysUtilsOptObj != null)
            _javaSysUtilsOptObj.CallStatic("pickPhotoFromAlbum", compressFormat, crop, outputX, outputY, gameObjectName);
    }

    /// <summary>
    /// 从相册捡取图片
    /// </summary>
    /// <param name="compressFormat">压缩格式 (png, jpeg/jpg)</param>
    /// <param name="crop">是否裁剪</param>
    /// <param name="outputX">输出X（宽度）</param>
    /// <param name="outputY">输出Y（高度）</param>
    /// <param name="gameObjectName">unity回调对象名称</param>
    public void pickPhotoFromCamera(string compressFormat, bool crop, int outputX, int outputY, string gameObjectName)
    {
        if (_javaSysUtilsOptObj != null)
            _javaSysUtilsOptObj.CallStatic("pickPhotoFromCamera", compressFormat, crop, outputX, outputY, gameObjectName);
    }

    /// <summary>
    /// 获取“国际移动设备身份码”
    /// </summary>
    /// <returns></returns>
    public string GetIMEI()
    {
        if (_javaSysUtilsOptObj == null)
            return GetDeviceUUID();

        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
        string telSer = new AndroidJavaClass("android.content.Context").GetStatic<string>("TELEPHONY_SERVICE");
        AndroidJavaObject tm = currentActivity.Call<AndroidJavaObject>("getSystemService", telSer);
        string IMEI = tm.Call<string>("getDeviceId");
        Debug.Log("IMEI=" + IMEI);
        return IMEI;
    }
#elif UNITY_IOS
    
    void Start()
	{
		RuntimePlatform rp = Application.platform;
		if (rp != RuntimePlatform.OSXEditor) {
			ObjectCInterface.getUUID ();
			ObjectCInterface.getIDFA ();
			Utils.LogSys.Log ("XXXXXXXXXXXXXXXX--GetDeviceUUID--XXXXXXXXXXXXXXXX");
			_object_c_interface_.AsyncHttpClient.initAsyncClient("");
		}
    }


    public void SaveScreenshot()
    {
    }
    
	/// <summary>
	/// 异步HTTP下载文件
	/// </summary>
	/// <param name="url">下载URL地址</param>
	/// <param name="relativeUrl">是否相对地址，是则取 baseUrl+url</param>
	/// <param name="destPath">目标文件路径（SDCard存在时有效）</param>
	/// <param name="destFileName">目标文件名</param>
	/// <param name="vibrator">下载完成震动时间，0不震动</param>
	/// <param name="gameObjectName">unity回调对象名称</param>
	public void asyncHttpDownloadFile(string url, bool relativeUrl, string destPath, string destFileName, int vibrator, string gameObjectName)
	{
		_object_c_interface_.AsyncHttpClient.downloadFileDefault (url, relativeUrl, destPath, destFileName, vibrator, gameObjectName);
	}
	/// <summary>
	/// 上传文件（流）到服务器
	/// </summary>
	/// <param name="url">URL地址</param>
	/// <param name="relativeUrl">是否相对地址</param>
	/// <param name="destFileName">目标文件名</param>
	/// <param name="fileAbsolutePath">文件绝对路径</param>
	/// <param name="gameObjectName">unity回调对象名称</param>
	public void asyncHttpUploadFile(string url, bool relativeUrl, string destFileName, string fileAbsolutePath, string gameObjectName)
	{
		_object_c_interface_.AsyncHttpClient.uploadFile(url, relativeUrl, destFileName, fileAbsolutePath, gameObjectName);
	}

    //同步读取assetbundle文件
    //path:相对StreamingAssets目录
    public byte[] GetAssetBundleBytes(string path)
    {
        return File.ReadAllBytes(path);
    }
    
    /// <summary>
    /// 复制到粘贴板
    /// </summary>
    /// <param name="strVal"></param>
    public void copyString2Clipboard(string strVal)
    {
        ObjectCInterface.copyToPasteBoard(strVal);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 从粘贴板获取字符串
    /// </summary>
    /// <returns></returns>
    public string getStringFromClipboard()
    {
        return "";
    }

    
    public string GetDeviceUUID()
    {

		RuntimePlatform rp = Application.platform;
		if (rp == RuntimePlatform.OSXEditor) {
			return SystemInfo.deviceUniqueIdentifier;
		} else {
			return  ObjectCCallback._deviceUUID;
		}

    }
    
    public void startInstallAPK(string apkAbsolutePath)

    {
    }
    

	/// <summary>
	/// 从相册捡取图片
	/// </summary>
	/// <param name="compressFormat">压缩格式 (png, jpeg/jpg)</param>
	/// <param name="crop">是否裁剪</param>
	/// <param name="outputX">输出X（宽度）</param>
	/// <param name="outputY">输出Y（高度）</param>
	/// <param name="gameObjectName">unity回调对象名称</param>
	public void pickPhotoFromAlbum(string compressFormat, bool crop, int outputX, int outputY, string gameObjectName)
	{
		_object_c_interface_.SystemUtilsOperation.pickPictureFromAlbum (compressFormat, crop, outputX, outputY, gameObjectName);
	}

	/// <summary>
	/// 从相机拍照
	/// </summary>
	/// <param name="compressFormat">压缩格式 (png, jpeg/jpg)</param>
	/// <param name="crop">是否裁剪</param>
	/// <param name="outputX">输出X（宽度）</param>
	/// <param name="outputY">输出Y（高度）</param>
	/// <param name="gameObjectName">unity回调对象名称</param>
	public void pickPhotoFromCamera(string compressFormat, bool crop, int outputX, int outputY, string gameObjectName)
	{
		_object_c_interface_.SystemUtilsOperation.pickPictureFromAlbum (compressFormat, crop, outputX, outputY, gameObjectName);
	}


    /// <summary>
    /// 获取“国际移动设备身份码”
    /// </summary>
    /// <returns></returns>
    public string GetIMEI()
    {
        return GetDeviceUUID();
    }
     /**********音频**********/
    /// <summary>
    /// 初始化音频对象
    /// </summary>
    /// <param name="strStorageDir">存放地址</param>
    public void initAudioManager()
    {
        string sPath = Application.persistentDataPath;
        sPath = sPath.Replace("\\", "/");
        sPath = string.Format("{0}/Audio",sPath);

        _object_c_interface_.GameAudioManager.initAudio(sPath);
        _object_c_interface_.GameAudioManager.setMinimumTime(1f);
    }
    
    /// <summary>
    /// 清空语音文件
    /// </summary>
    public void clearAudio()
    {
        _object_c_interface_.GameAudioManager.clear();
    }
    
    /// <summary>
    /// 清删除指定语音文件
    /// </summary>
    public void deleteAudio(string strAudioName)
    {
        _object_c_interface_.GameAudioManager.deleteAudio(strAudioName);
    }
    
    /// <summary>
    /// 添加音频名称
    /// </summary>
    public void addAudioName(string strAudioName)
    {
		_object_c_interface_.GameAudioManager.addAudionName(strAudioName);
    }
    
    /// <summary>
    /// 开始录音
    /// </summary>
    public bool startRecord(string strAudioName)
    {
        _object_c_interface_.GameAudioManager.startRecord(strAudioName);
        return true;
    }
    
    /// <summary>
    /// 停止录制
    /// </summary>
    public void stopRecord()
    {
        _object_c_interface_.GameAudioManager.stopRecord();
    }
    
    /// <summary>
    /// 播放音频
    /// </summary>
    public void playAudio(string strAudioName)
	{
		addAudioName(strAudioName);
        _object_c_interface_.GameAudioManager.playAudio(strAudioName);
    }
    /**********音频 End**********/
#else

    public void SaveScreenshot()
    {
        
    }
    
    //同步读取assetbundle文件
    //path:相对StreamingAssets目录
    public byte[] GetAssetBundleBytes(string path)
    {
        return File.ReadAllBytes(path);
    }
    
    /// <summary>
    /// 复制到粘贴板
    /// </summary>
    /// <param name="strVal"></param>
    public void copyString2Clipboard(string strVal)
    {
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 从粘贴板获取字符串
    /// </summary>
    /// <returns></returns>
    public string getStringFromClipboard()
    {
        return "";
    }
    public string GetDeviceUUID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }
    
    public void startInstallAPK(string apkAbsolutePath)

    {
    }
    
    public void asyncHttpDownloadFile(string url, bool relativeUrl, string destPath, string destFileName, int vibrator, string gameObjectName)
    {
    }
    /// <summary>
    /// 获取“国际移动设备身份码”
    /// </summary>
    /// <returns></returns>
    public string GetIMEI()
    {
        return GetDeviceUUID();
    }

    public void pickPhotoFromAlbum(string compressFormat, bool crop, int outputX, int outputY, string gameObjectName)
    {
    }

    public void pickPhotoFromCamera(string compressFormat, bool crop, int outputX, int outputY, string gameObjectName)
    {
    }

    public void asyncHttpUploadFile(string url, bool v1, string destFileName, string fileAbsolutePath, string v2)
    {
    }
    /**********音频**********/
    /// <summary>
    /// 初始化音频对象
    /// </summary>
    /// <param name="strStorageDir">存放地址</param>
    public void initAudioManager()
    {
        string sPath = Application.persistentDataPath;
        sPath = sPath.Replace("\\", "/");
        sPath = string.Format("{0}/Audio", sPath);
    }

    /// <summary>
    /// 清空语音文件
    /// </summary>
    public void clearAudio()
    {
    }

    /// <summary>
    /// 清删除指定语音文件
    /// </summary>
    public void deleteAudio(string strAudioName)
    {
    }

    /// <summary>
    /// 添加音频名称
    /// </summary>
    public void addAudioName(string strAudioName)
    {
    }

    /// <summary>
    /// 开始录音
    /// </summary>
    public bool startRecord(string strAudioName)
    {
        return true;
    }

    /// <summary>
    /// 停止录制
    /// </summary>
    public void stopRecord()
    {
    }

#endif

    //视频销毁
    public void onPlayVideoActivityDestroyed(string videoName)
    {
        if (!_videosCallback.ContainsKey(videoName))
            return;

        // 执行回调函数
        _videosCallback[videoName]();
    }

   
}
