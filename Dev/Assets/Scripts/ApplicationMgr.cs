/***************************************************************

 *
 *
 * Filename:  	ApplicationMgr.cs	
 * Summary: 	游戏程序管理
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/19 20:22
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using network.protobuf;
using Scene;
using UI.Controller;
using Utils;
using Mono.Xml;
using System.Security;
using player;
using asset;
using effect;
using EventManager;
using PushNotification;
using sdk;
using sluaAux;
//using dataEyeStatistics;
#endregion

public class ApplicationMgr : MonoBehaviour
{
#region 测试开启引导 (Author: @XB.Wu)
    public static int _OpenGuideFlag = ClientDefine._OpenGuideFlag;
    public static int _ShowGuideSwitch = ClientDefine._ShowGuideSwitch;//0:隐藏    1:显示
    public static int _OpenVipPay = ClientDefine._OpenVipPay;//1：开启  2:不开启
    public static int _OpenNotice = ClientDefine._OpenNotice;//2不弹出
    public static bool _DoNotCheckVersion = ClientDefine._DoNotCheckVersion;//是否不检查更新
#endregion

    private bool _bIsMgrObjCreate = false;
    private bool isReGet = false;
    private int _initOKStep = -1;
    private float _lastLogTime = 0;
    private string noticeUrl = "";


    public static bool _bInitOK = false;
    public int InitOkStep
    {
        get { return _initOKStep; }
    }
	//////////////////////////////////////////////////////////

    int temp_task_count;
    void Awake()
    {
        if (_bIsMgrObjCreate)
        {
            return;
        }

        Utils.LogSys.Log("ApplicationMgr Awake");


        
#if UNITY_ANDROID
		InitData_Step1();
        /*
        int isCGPlayed = PlayerPrefs.GetInt("cg", 0);
        if (isCGPlayed == 0)
        {
            PlayerPrefs.SetInt("cg", 1);
            UtilTools.PlayeVideo("cg.mp4", 856, 480, 1, 1, 
                delegate() { InitData_Step1();} );
        }
        else
        {
            InitData_Step1();
        }*/
#else
        InitData_Step1();
#endif

        //绑定初始场景控制脚本
        //SceneManager.getInstance().sceneLoadCompleteAndInit(SceneName.s_StartupScene, 1);

        _bIsMgrObjCreate = true;
    }
    
    
    void Start()
	{

        temp_task_count = task.TaskManager.MAX_CONCURRENCY_TASK;
        //关闭屏幕LOG
        LogSys._bEnableScreenLog = false;

#if UNITY_EDITOR
        LogSys._bEnableLog = true;
#else
		LogSys._bEnableLog = false;
#endif
    }

    void Update()
    {
        if (_initOKStep == 0)
        {
            //step1:先加载依赖关系
            if (AssetManager.getInstance().IsInitComplete())
            {
                Utils.LogSys.Log("ApplicationMgr DataInit Step1 OK----->" + (Time.realtimeSinceStartup - _lastLogTime).ToString());
                _lastLogTime = Time.realtimeSinceStartup;
                _initOKStep = 1;
                InitData_Step2();
            }
        }
        else if (_initOKStep == 1)
        {
            //step2:加载配置文件
            if (ConfigDataMgr.getInstance().IsAllConfigLoaded())
            {
                Utils.LogSys.Log("ApplicationMgr DataInit Step2 OK----->" + (Time.realtimeSinceStartup - _lastLogTime).ToString());
                _lastLogTime = Time.realtimeSinceStartup;
                _initOKStep = 2;
                task.TaskManager.MAX_CONCURRENCY_TASK = temp_task_count;
                InitData_Step3();

                SDKManager.getInstance().sendNewbieguideDeviceUniqueIdentifier("3");
            }
        }
        else if (_initOKStep == 5)
        {
            if (luaSvrManager.getInstance().IsLoaded == true)
            {
                InitData_Complete();
                _initOKStep = 6;
            }
        }
        else if (_initOKStep > 5)
        {
            if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
            {
                SDKManager.getInstance().CommonSDKInterface.exit();
            }
        }
    }

    /// <summary>
    /// 显示退出窗口
    /// </summary>
    public static void showExitDialog()
    {
        UtilTools.applicationExitDialog("确定退出游戏？", "614d46", "Center", makeSureExit, makeSureCancel);
    }

    public static void makeSureExit()
    {
        exit();
    }

    public static void makeSureCancel()
    {
       
    }

    void OnDestroy()
    {
        ClientNetwork.ResetAllData();
    }

    //初始化第1步
    void InitData_Step1()
    {
        _initOKStep = 0;
        _lastLogTime = Time.realtimeSinceStartup;

        //资源依赖配置
        AssetManager.getInstance().intialize();
        //文字
        GameText.Instance.InitData();
    }
    //初始化第2步
    void InitData_Step2()
    {
        task.TaskManager.MAX_CONCURRENCY_TASK = 2;
        //配置文件管理类初始化
        ConfigDataMgr.getInstance().initialize();
    }
    //初始化第3步
    void InitData_Step3()
    {
        //配置文件管理类初始化
        //GameSceneManager.getInstance().initialize();
        /*
#if UNITY_EDITOR
#elif UNITY_ANDROID
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
            tools.startLocalNotification();
#elif UNITY_IOS
        LocalNotificationMgr.getInstance().initialize();
#endif
         * */
        //PathXMLMgr.getInstance().init();
        FishPathDataMgr.getInstance().initialize();
        Utils.LogSys.Log("ApplicationMgr DataInit Step3 OK----->" + (Time.realtimeSinceStartup - _lastLogTime).ToString());
        _lastLogTime = Time.realtimeSinceStartup;
        _initOKStep = 3;
        InitData_Step4();
    }

    //初始化第4步
    void InitData_Step4()
    {
        initializeMode();
        initializeUnity();
        initializeGame();
        Utils.LogSys.Log("ApplicationMgr DataInit Step4 OK----->" + (Time.realtimeSinceStartup - _lastLogTime).ToString());
        _lastLogTime = Time.realtimeSinceStartup;
		_initOKStep = 4;
        InitData_Step5();
    }
    //初始化第5步
    void InitData_Step5()
    {
        //脚本模块初始化
        if (ClientDefine.LUA_MODULE)
            if (luaSvrManager.getInstance().IsLoaded == false)
                luaSvrManager.getInstance().initialize();
        //StartCoroutine(GetServerList());
        //InitData_Step6();
        _initOKStep = 5;
    }

    void InitData_Step6()
    {
        //SDK初始化
        SDKManager.getInstance().init();
        noticeUrl = sdk.SDKManager.NoticeUrl + "?p=" + UtilTools.GetClientTime();
        StartCoroutine(GetNoticeXml());
//        ServerSimulation.getInstance().Init();
    }


    void InitData_Complete()
    {
        _bInitOK = true;
        Utils.LogSys.Log("ApplicationMgr DataInit Step All OK----->" + (Time.realtimeSinceStartup - _lastLogTime).ToString());
        _lastLogTime = Time.realtimeSinceStartup;
        UtilTools.clearAllAudio();
         if (ClientNetwork._bLoginOut)
         {
//             UIManager.ReInit();
         }
//        NoticeController.openNotice();
    }


    IEnumerator GetServerList()
    {
        //判断网址是否为空（Trim：移除所有前导空白字符和尾部空白字符）
        string serverlistUrl = sdk.SDKManager.ServerListUrl + "?p=" + UtilTools.GetClientTime();


        if (serverlistUrl == null || "".Equals(serverlistUrl.Trim()))
        {
            Utils.LogSys.Log("can not find server");
            yield break;
        }

        //下载ServerList.xml
        WWW www = new WWW(serverlistUrl);
        yield return www;

        if (www.error != null)
        {
            //下载ServerList.xml
            www = new WWW(serverlistUrl);
            yield return www;
        } 

        if (www.error != null)
        {
            //下载ServerList.xml
            www = new WWW(serverlistUrl);
            yield return www;
        }

        if (www.error != null) {
            Utils.LogSys.LogError(www.error);
            if (!isReGet) {
                isReGet = true;
                StartCoroutine(GetServerList());
            }

            UtilTools.MessageDialog(GameText.GetStr("loadingServerFail"), okCallbackFunc: ReLoadingServerList);
        } else {
            ParseXml(www.text);
        }
        www.Dispose();
        _initOKStep = 5;
        Utils.LogSys.Log("ApplicationMgr DataInit Step5 OK----->" + (Time.realtimeSinceStartup - _lastLogTime).ToString());
        _lastLogTime = Time.realtimeSinceStartup;


        InitData_Step6();
    }

    IEnumerator GetNoticeXml()
    {
        yield return new WaitForSeconds(1f);
        if (noticeUrl == null || "".Equals(noticeUrl.Trim()))
        {
            Utils.LogSys.Log("can not find notice");
            yield break;
        }

        //下载Announcement.xml
        WWW www = new WWW(noticeUrl);
        yield return www;

        if (www.error != null)
        {
            //下载Announcement.xml
            www = new WWW(noticeUrl);
            yield return www;
        }

        if (www.error != null)
        {
            //下载Announcement.xml
            www = new WWW(noticeUrl);
            yield return www;
        }

        if (www.error != null)
        {
            Utils.LogSys.LogError(www.error);
            UtilTools.MessageDialog(GameText.GetStr("loadingNoticeFailed"), okCallbackFunc: ReLoadingNotice);
        }
        else
        {
            ParseNoticeXML(www.text);
        }
        www.Dispose();
        _initOKStep = 6;
        Utils.LogSys.Log("ApplicationMgr DataInit Step6 OK----->" + (Time.realtimeSinceStartup - _lastLogTime).ToString());
        _lastLogTime = Time.realtimeSinceStartup;
        InitData_Complete();
    }

    private void ReLoadingServerList()
    {
        Invoke("StartLoadingList", 2.0f);
    }

    private void ReLoadingNotice()
    {
        Invoke("GetNoticeXml", 2.0f);
    }

    private void StartLoadingList()
    {
        StartCoroutine(GetServerList());
    }

    public void ParseXml(string xml)
    {
        SecurityParser sp = new SecurityParser();
        sp.LoadXml(xml);

        SecurityElement se = sp.ToXml();

        foreach (SecurityElement child in se.Children)
        {
            foreach (SecurityElement cc in child.Children)
            {
                if (cc.Tag == "Server")
                {
                    string id = cc.Attribute("Id");
                    int newTag = int.Parse(cc.Attribute("New"));
                    int state = int.Parse(cc.Attribute("State"));
                    string srvName = cc.Attribute("Name");
                    int httpPort = int.Parse(cc.Attribute("HttpPort"));
                    int port = int.Parse(cc.Attribute("Port"));
                    string ip = cc.Attribute("Ip");
                    GameDataMgr.LOGIN_DATA.AddServerData(id, newTag, state, srvName, httpPort, port, ip);
                }
            }
        }
    }

    public void ParseNoticeXML(string xml)
    {
        SecurityParser sp = new SecurityParser();
        try
        {
            sp.LoadXml(xml);
            SecurityElement se = sp.ToXml();
            foreach (SecurityElement child in se.Children)
            {
                if (child.Tag == "Content")
                {
                    string contentNotice = child.Attribute("text");
                    string contentSwitch = child.Attribute("switch");
                    /*NoticeController nCtrl = UIManager.GetControler<NoticeController>();
                    if (nCtrl != null)
                    {
                        nCtrl.setNoticeStr(contentNotice);
                        if (string.IsNullOrEmpty(contentSwitch) || contentSwitch == "0")
                        {
                            NoticeController.bSwitch = false;
                        }
                        else
                        {
                            NoticeController.bSwitch = true;
                        }
                    }*/
                    break;
                }
            }
        }
        catch (System.Exception ex)
        {

            Utils.LogSys.LogError(ex.Message);
        }

    }


#if UNITY_EDITOR
    void OnGUI()
    {

        //输出到屏幕
        LogSys.OnGUI();
    }
#endif

    //////////////////////////////////////////////////////////

    static ApplicationMgr()
    {
        
    }

    /// <summary>
    /// 初始化游戏模式
    /// </summary>
    public static void initializeMode()
    {
        //TODO: 游戏模式设置
    }

    /// <summary>
    /// 初始化Unity设置
    /// </summary>
    public static void initializeUnity()
    {

        UtilTools.SetFPS(FPSLevel.Normal);
        //屏幕不休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //TODO: 初始化UNITY的相关配置

#if UNITY_EDITOR
        QualitySettings.SetQualityLevel(2);
#elif UNITY_ANDROID
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        tools.initAudioManager();
        QualitySettings.SetQualityLevel(1);
        /*
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
        {
            int maxCPUFreq = tools.getCPUMaxFreqM();
            long sysMemMB = tools.getTotalMemoryKB() / 1024;

            if (maxCPUFreq <= 1500 || sysMemMB <= 1300)
                QualitySettings.SetQualityLevel(0);
            else if (maxCPUFreq > 1500 && maxCPUFreq <= 2000)
                QualitySettings.SetQualityLevel(1);
            else if (maxCPUFreq > 2000)
                QualitySettings.SetQualityLevel(2);
        }*/
#elif UNITY_IOS
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        tools.initAudioManager();
        QualitySettings.SetQualityLevel(1);
#endif

    }

    public static void initializeTagsAndLayers()
    {
        //TODO: 初始化tags和layers
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public static void initializeGame()
    {
        //UI数据
        GameDataMgr.InitData();
        
        //Player数据
        PlayerManager.getInstance().init();
        //Asset管理
        // AssetManager.getInstance().intialize();
        //光效初始化
        EffectManager.getInstance().initialize();

        //网络
        ClientNetwork.Instance.Init();

    }

    /// <summary> 
    /// 退出游戏
    /// </summary>
    public static void exit()
    {
        //DataEyeUtils.onKillProcessOrExit();
        Application.Quit();
    }

    public static void KillSelf()
    {
        GameObject pMgrObj = GameObject.Find("ApplicationMgr");
        if (pMgrObj != null)
        {
            DestroyImmediate(pMgrObj);
        }
        _bInitOK = false;
    }
}
