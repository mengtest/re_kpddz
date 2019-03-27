using UnityEngine;
using System.Collections;
using version;
using Scene;
using sdk;
using object_c;
using UI.Controller;
using asset;
using network.protobuf;
using EventManager;
using MyExtensionMethod;
using Utils;

public class VersionUpdate : AsyncHttpResponseListner
{
    Transform tfBoxLoading;
    Transform tfBoxChecking;
    UILabel loadingTip;
    UILabel checkingTip;
    GameObject serverTip;

    Transform tfSlider;
    Transform tfOKButton;
    UILabel OKButtonText;
    UISlider sliderProgress;

    //健康游戏忠告
    Transform _healthTips = null;

    private bool _needCheckVersion = false;//是否开始版本检测的标志
    private VersionData _versionData = null;
    private bool _isDownloading = false;
    private float _totalSize = 0f;
    private VersionData.VersionType _eCurType = VersionData.VersionType.Checking;
    private VersionData.VersionType _eLastType = VersionData.VersionType.Checking;
    private bool _bLoadSDKManagerXML = false;

    private UILabel versionShowLb;
    private GameObject loadingBox;
    private UISlider loadingBoxSilder;
    private UILabel _loadingBoxTip;
    private GameObject _loginContainer;
    private GameObject _btnTouris;
    private GameObject _btnPhone;
    private UITexture _BeiJing;
    private bool _BeiJingBack = true;

    //下载安装包相关变量
    private float loadedPackageSize = 0;
    private float totalPackageSize = 1;
    private bool _bLoadPackageComplete = false;
    private bool _bLoadPackageFailed = false;
    private string _strLoadPackageFailed_code = "";
    private string _strbLoadPackageFailed_msg = "";
    bool _isSelect = true;
    GameObject _selectBox;
    GameObject _gouObj;
    GameObject _btnAgree;

    #region //下载安装包抽象类函数

    public override void onStartAbstract()
    {
        
    }
    public override void onProgressAbstract(long currentSize, long totalSize)
    {
        loadedPackageSize = (float) currentSize / 1024f;
        loadedPackageSize = loadedPackageSize / 1024f;
        totalPackageSize = (float)totalSize / 1024f;
        totalPackageSize = totalPackageSize / 1024f;
    }
    public override void onSuccessAbstract(string statusCode, string filePath, string msg)
    {
        _bLoadPackageComplete = true;
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
        {
            tools.startInstallAPK(filePath);
        }
    }

//     IEnumerator ToInstallApk(string filePath)
//     {
//         yield return new WaitForSeconds(5f);
//         JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
//         if (tools != null)
//         {
//             tools.startInstallAPK(filePath);
//         }
//     }

    public override void onFailureAbstract(string statusCode, string msg)
    {
        _bLoadPackageFailed = true;
        _strLoadPackageFailed_code = statusCode;
        _strbLoadPackageFailed_msg = msg;
    }
    public override void onFinishAbstract()
    {

    }
    public override void onRetryAbstract()
    {

    }
    #endregion
    private void OnClickBtnAgreeWin(GameObject go){

        UIManager.CreateLuaWin("UserAgreeWin");
    }
    private void OnClickSelectBox(GameObject go){
        _isSelect = !_isSelect;
        _gouObj.SetActive(_isSelect);
    }        
        
    
    // 初始化
	void Awake () {
        
        tfBoxChecking = transform.Find("Container/BoxChecking");
        checkingTip = tfBoxChecking.Find("Tip").GetComponent<UILabel>();
        checkingTip.text = "";// GameText.GetStr("versionCheck");

        _healthTips = transform.Find("Background");
        tfBoxLoading = transform.Find("Container/BoxLoading");
        loadingTip = tfBoxLoading.Find("Tip").GetComponent<UILabel>();
        tfSlider = tfBoxLoading.Find("slider");
        sliderProgress = tfSlider.GetComponent<UISlider>();
        
        tfOKButton = tfBoxLoading.Find("OKButton");
        OKButtonText = tfOKButton.Find("Text").GetComponent<UILabel>();
        OKButtonText.text = "确定";//GameText.GetStr("update");
        UIEventListener.Get(tfOKButton.gameObject).onClick = OnClickOKButton;
	    versionShowLb = transform.Find<UILabel>("versionShowLb");
	    _BeiJing = transform.Find<UITexture>("BeiJing");

        versionShowLb.text = string.Format("v{0}.{1}", ClientDefine.LOCAL_PROGRAM_VERSION, VersionData.GetLocalVersion());

	    loadingBox = transform.Find<GameObject>("Loading/BoxLoading");
	    loadingBoxSilder = loadingBox.transform.Find<UISlider>("slider");
	    _loadingBoxTip = loadingBox.transform.Find<UILabel>("Tip");

        serverTip = transform.Find("Container/ServerListTip").gameObject;
        if (GameDataMgr.LOGIN_DATA == null)
        {
            serverTip.SetActive(true);
        }

	    _loginContainer = transform.Find<GameObject>("Loading/loginContainer");
	    _btnTouris = transform.Find<GameObject>("Loading/loginContainer/btnTouris");
	    _btnPhone = transform.Find<GameObject>("Loading/loginContainer/btnPhone");
        _selectBox = transform.Find<GameObject>("Loading/loginContainer/gou");
        _gouObj = transform.Find<GameObject>("Loading/loginContainer/selectBox"); 
        _btnAgree = transform.Find<GameObject>("Loading/loginContainer/btnAgree");

        GameObject btnTour = transform.Find<GameObject>("Loading/loginContainer/btnTouris2");
        if (VersionData.IsReviewingVersion())
        {
            btnTour.gameObject.SetActive(true);
            _btnTouris.gameObject.SetActive(false);
        }
        else
        {
            btnTour.gameObject.SetActive(false);
            _btnTouris.gameObject.SetActive(true);
        }
        UIEventListener.Get(_selectBox).onClick = OnClickSelectBox;
        UIEventListener.Get(_btnAgree).onClick = OnClickBtnAgreeWin;
	    UIEventListener.Get(_btnPhone).onClick = OnPhoneLoginHandler;
	    UIEventListener.Get(_btnTouris).onClick = OnTourisLoginHandler;
        UIEventListener.Get(btnTour).onClick = OnTourisLoginHandler;
        
//	    EventSystem.RegisterEvent(EventID.SHOW_LOGIN_BTN,ShowLoginBtn);
	    var ctrl = UIManager.GetControler<LoginInputController>();
	    ctrl.SetLoginShow(ShowLoginBtn);
	}

    private void OnEnable()
    {
        if (_BeiJing.mainTexture == null && _BeiJingBack == false){
            UtilTools.loadTexture(_BeiJing, "Levels/startupscene/loading.png", true);
//            Utils.LogSys.Log("[VersionUpdate.OnEnable]:+");
//            _BeiJing.mainTexture = AssetManager.getInstance().loadTexture("Levels/startupscene");
            _BeiJingBack = true;
        }
    }

    private void OnDisable()
    {
        if (_BeiJing.mainTexture !=null){
            _BeiJingBack = false;
            _BeiJing.mainTexture = null;
        }
    }

    //设置提示文字
    public void SetServerListTip(string sTip,bool isShowProgress = false,float process=0f)
    {
        serverTip = transform.Find("Container/ServerListTip").gameObject;
        _loadingBoxTip.text = sTip;
        loadingBoxSilder.gameObject.SetActive(isShowProgress);
        loadingBoxSilder.value = process;
        if (isShowProgress && Mathf.FloorToInt(process * 100) == 100){
            loadingBoxSilder.gameObject.SetActive(false);
        }
        bool isEmptyShow = string.IsNullOrEmpty(sTip);

        if (loadingBox.activeSelf == isEmptyShow){
            loadingBox.SetActive(!isEmptyShow);
        }
    }


    void Start()
    {
        _loginContainer.SetActive(false);
        LoadSDKManagerXml();
    }

    //第一步下载SDKManager.xml
    void LoadSDKManagerXml()
    {
        SDKManager.getInstance().loadCallback = LoadSDKManagerXmlCallback;
        if (!SDKManager.needWaitForSDKInitComplete_ToCheckVersion())//如果是Quick平台要等SDK初始化后才能开始
        {
            SDKManager.getInstance().LoadUrlXml();
        }
        if (_healthTips != null)
            _healthTips.gameObject.SetActive(VersionData.HealthTips);

        StartCoroutine("hideHeathTips");

//        if (ApplicationMgr._bInitOK){
       // versionShowLb.text = string.Format("版本号:{0}", VersionData._strShowVersion);
//        }
    }

    void LoadSDKManagerXmlCallback(bool result, int errorCode)
    {
        if (result)
        {
            _bLoadSDKManagerXML = true;
            StartCheckVersion();
        }
        else
        {
            tfBoxChecking.gameObject.SetActive(false);
            tfBoxLoading.gameObject.SetActive(true);
            loadingTip.text = string.Format("检测版本中断，请检查网络后继续(错误码:{0})。", errorCode);
            tfOKButton.gameObject.SetActive(true);
            tfSlider.gameObject.SetActive(false);
            OKButtonText.text = "确定";
        }
    }

    //第二步下载version.xml
    void StartCheckVersion()
    {
        if (ApplicationMgr._DoNotCheckVersion)
        {
            Invoke("DonotCheckVersion", 1f);
        }
        else
        {
            Init();
        }
    }

    //跳过版本检测
    void DonotCheckVersion()
    {
        UpdateUIByState(VersionData.VersionType.None);
    }

    IEnumerator hideHeathTips()
    {
		yield return new WaitForSeconds (2.0f);
        if (_healthTips != null && _healthTips.gameObject.activeSelf)
        {
            VersionData.HealthTips = false;
            UIWidget mWidget = _healthTips.GetComponent<UIWidget>();
            if (mWidget != null)
            {
                TweenAlpha.Begin(_healthTips.gameObject, 0.3f, 0f);
            }
            else
            {
                _healthTips.gameObject.SetActive(false);
            }
        }
    }
	// Update 每帧调用一次
	void Update () {
        if (ApplicationMgr._DoNotCheckVersion)
            return;
        
        if (!_needCheckVersion || _versionData == null)
            return;

        VersionData.VersionType eType = _versionData.VersionTypeToUpdate;
        if (eType != _eCurType)//状态发生变化时刷新
        {
            _eCurType = eType;
            UpdateUIByState(eType);
            _eLastType = eType;
        }
        else if (eType == VersionData.VersionType.Inner && _isDownloading)//正在下载内更资源时刷新
        {
            UpdateUIByState(eType);
        }
        else if (eType == VersionData.VersionType.Program && _isDownloading)//正在下载大版本安装包
        {
            UpdateUIByState(eType);
        }
    }

    private void UpdateUIByState(VersionData.VersionType eType)
    {
        switch (eType)
        {
            case VersionData.VersionType.Checking://正在检测
                tfBoxChecking.gameObject.SetActive(true);
                tfBoxLoading.gameObject.SetActive(false);
                break;
            case VersionData.VersionType.None://无版本要更新，或已更新完成
                tfBoxChecking.gameObject.SetActive(false);
                tfBoxLoading.gameObject.SetActive(false);
                if (_eLastType == VersionData.VersionType.Inner)//更新完成
                {
                    tfBoxLoading.gameObject.SetActive(true);
                    loadingTip.text = "资源更新完成";//GameText.GetStr("updateFinish");
                    sliderProgress.value = 1f;

                    if (ClientNetwork._bLoginOut)
                    {
                        //UIManager.DestroyWin(UIName.MAIN_CITY_WIN);
                        //UIManager.DestroyWin(UIName.MAIN_CITY_CENTER_WIN);
                        ApplicationMgr.KillSelf();//销毁,让它重新加载配置文件
                        AssetManager.getInstance().ClearAll();
                        ConfigDataMgr.getInstance().ClearAll();
                        GameDataMgr.ClearAll();
                        MsgCallManager.RemoveAllCallback();
                        MsgCallManager.Dispose();
                        EventSystem.Dispose();
                    }
                    Invoke("OnUpdateVersionComplete", 0.5f);
                }
                else
                {
                    //tfJuHuaLoading.gameObject.SetActive(true);
                    //if (ClientNetwork._bLoginOut)//如果是註銷出來的
                    //{
                    //    UIManager.CreateWin(UIName.LOGIN_WIN);
                    //    Invoke("DelayToHideVersionUpdate", 0.5f);
                    //}
                    //else//如果第一次登錄進來
                    //{
                    //    GameObject sceneObj = GameObject.Find("Scene");
                    //    if (sceneObj)
                    //    {
                    //        StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
                    //        if (startUpMono)
                    //            startUpMono.StartUp();
                    //    }
                    //}
                    sliderProgress.value = 0f;
                    GameObject sceneObj = GameObject.Find("Scene");
                    if (sceneObj)
                    {
                        StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
                        if (startUpMono)
                            startUpMono.StartUp();
                    }
                }
                break;
            case VersionData.VersionType.Inner://内更资源
                //tfJuHuaLoading.gameObject.SetActive(false);
                tfBoxChecking.gameObject.SetActive(false);
                tfBoxLoading.gameObject.SetActive(true);
                if (_isDownloading == false)//提示框
                {
                    if (UtilTools.GetCurrentNetworkType() == 1)//wifi
                    {
                        tfBoxChecking.gameObject.SetActive(false);
                        tfBoxLoading.gameObject.SetActive(true);
                        tfOKButton.gameObject.SetActive(true);
                        tfSlider.gameObject.SetActive(false);
                        OnClickOKButton(null);
                    }
                    else
                    {
                        tfBoxChecking.gameObject.SetActive(false);
                        tfBoxLoading.gameObject.SetActive(true);
                        loadingTip.text = string.Format("当前非WIFI网络环境，是否继续下载更新包({0})？", GetVersionDataSize());//"有新版本发布，快去下载吧";//GameText.GetStr("checkTip");
                        tfOKButton.gameObject.SetActive(true);
                        tfSlider.gameObject.SetActive(false);
                        OKButtonText.text = "确定";
                    }

                }
                else//刷进度
                {
                    loadingTip.text = string.Format("下载资源包(编号{0}):  {1}/{2}", GetDownloadingVersion(), GetLoadedSize(), GetVersionDataSize());
                    sliderProgress.value = _versionData.GetUpdatePercent();
                    tfOKButton.gameObject.SetActive(false);
                    tfSlider.gameObject.SetActive(true);
                }
                OKButtonText.text = "更新";//GameText.GetStr("update");
                break;
            case VersionData.VersionType.Program://更新程序版本

                //tfJuHuaLoading.gameObject.SetActive(false);
                if (_isDownloading == false)//提示框
                {
                    if (!string.IsNullOrEmpty(SDKManager.PackageDownloadWeb))//跳网页
                    {
                        tfBoxChecking.gameObject.SetActive(false);
                        tfBoxLoading.gameObject.SetActive(true);
                        loadingTip.text = "有新版本发布，快去下载吧";
                        tfOKButton.gameObject.SetActive(true);
                        tfSlider.gameObject.SetActive(false);
                        OKButtonText.text = "更新";
                    }
                    else if (!string.IsNullOrEmpty(SDKManager.PackageDownloadUrl))//内部下载
                    {
                        if (UtilTools.GetCurrentNetworkType() == 1)
                        {
                            tfBoxChecking.gameObject.SetActive(false);
                            tfBoxLoading.gameObject.SetActive(true);
                            tfOKButton.gameObject.SetActive(true);
                            tfSlider.gameObject.SetActive(false);
                            OnClickOKButton(null);
                        }
                        else
                        {
                            tfBoxChecking.gameObject.SetActive(false);
                            tfBoxLoading.gameObject.SetActive(true);
                            loadingTip.text = string.Format("当前非WIFI网络环境，是否继续下载版本包({0}M)？", VersionData._strPackageSize);//"有新版本发布，快去下载吧";//GameText.GetStr("checkTip");
                            tfOKButton.gameObject.SetActive(true);
                            tfSlider.gameObject.SetActive(false);
                            OKButtonText.text = "确定";
                        }

                    }
                    else//如果没指定更新地址, 就不强制更新.
                    {
                        VersionData.ShowTipWhenInToGame = true;
                        UpdateUIByState(VersionData.VersionType.None);//直接跳过, 在点"进入游戏"时,提示有新版本,请前往更新.
                        return;
                    }
                }
                else//刷内部下载进度
                {
                    if (_bLoadPackageFailed)
                    {
                        _isDownloading = false;
                        tfBoxChecking.gameObject.SetActive(false);
                        tfBoxLoading.gameObject.SetActive(true);
                        loadingTip.text = string.Format("{0}(错误码：{1})", _strbLoadPackageFailed_msg, _strLoadPackageFailed_code);
                        tfOKButton.gameObject.SetActive(true);
                        tfSlider.gameObject.SetActive(false);
                        OKButtonText.text = "重新下载";
                        return;
                    }
                    else if (_bLoadPackageComplete)
                        loadingTip.text = string.Format("下载版本包完成，正在安装...");
                    else
                        loadingTip.text = string.Format("下载版本包({0}):  {1:F}M / {2:F}M", VersionData._toLoadProgramVersion, loadedPackageSize, totalPackageSize);
                    sliderProgress.value = loadedPackageSize/totalPackageSize;
                    tfOKButton.gameObject.SetActive(false);
                    tfSlider.gameObject.SetActive(true);
                }
                break;
            case VersionData.VersionType.Error://更新出错
                VersionData.ErrorCode errorCode = _versionData.ErrorCodeData;
                tfBoxChecking.gameObject.SetActive(false);
                tfBoxLoading.gameObject.SetActive(true);
                loadingTip.text = string.Format("检测版本中断，请检查网络后重试。(错误码:{0})", errorCode);
                tfOKButton.gameObject.SetActive(true);
                tfSlider.gameObject.SetActive(false);
                OKButtonText.text = "确定";
                break;
            default:
                break;
        }
    }

    private void OnUpdateVersionComplete()
    {
        tfBoxLoading.gameObject.SetActive(false);

        GameObject sceneObj = GameObject.Find("Scene");
        if (sceneObj)
        {
            StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
            if (startUpMono)
                startUpMono.StartUp();
        }
    }

    private void DelayToHideVersionUpdate()
    {
        gameObject.SetActive(false);
    }

    public void Init()
    {
        if (VersionData._versionChecked)//已检测过则不再检测
            return;

        _versionData = new VersionData();
        tfBoxChecking.gameObject.SetActive(true);
        tfBoxLoading.gameObject.SetActive(false);
        _needCheckVersion = true;
    }

    private void OnClickOKButton(GameObject go)
    {
        if (!_bLoadSDKManagerXML)
        {
            //下载SDKManager.xml
            Start();
        }
        else
        {
            //下载version.xml
            VersionData.VersionType eType = _versionData.VersionTypeToUpdate;
            if (eType == VersionData.VersionType.Inner)
            {
                _versionData.StartUpdateVersion();
                _isDownloading = true;
            }
            else if (eType == VersionData.VersionType.Program)
            {
                if (SDKManager.isAppStoreVersion())
                {
                    ObjectCInterface.openURL(SDKManager.PackageDownloadWeb);
                }
                else if (!string.IsNullOrEmpty(SDKManager.PackageDownloadWeb))
                {
                    Application.OpenURL(SDKManager.PackageDownloadWeb);
                }
                else if (!string.IsNullOrEmpty(SDKManager.PackageDownloadUrl))
                {
                    _bLoadPackageFailed = false;
                    JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
                    if (tools != null)
                    {
                        string fileName = System.IO.Path.GetFileName(SDKManager.PackageDownloadUrl);
                        tools.asyncHttpDownloadFile(SDKManager.PackageDownloadUrl, false, "", fileName, 1000, transform.name);
                        _isDownloading = true;
                    }
                }
            }
            else if (eType == VersionData.VersionType.Error)
            {
                //重试，跳过确认。
                _versionData = new VersionData();
                tfBoxChecking.gameObject.SetActive(true);
                tfBoxLoading.gameObject.SetActive(false);
                _isDownloading = true;
                _versionData.StartUpdateVersion();
                _needCheckVersion = true;
            }
        }
    }

    private string GetLoadedSize()
    {
        if (_versionData == null || _totalSize == 0f)
            return "1k";

        float loadedPercenet = _versionData.GetUpdatePercent();
        float loadedSize = loadedPercenet * _totalSize;
        if (loadedSize < 100)
        {
            return string.Format("{0:0.00}K", loadedSize);
        }
        else
        {
            return string.Format("{0:0.00}M", loadedSize / 1000f);
        }
    }

    private string GetVersionDataSize()
    {
        if (_versionData == null)
            return "1k";

        _totalSize = _versionData.GetUpdateVersionSize();
        if (_totalSize < 100)
        {
            return string.Format("{0:0.00}K", _totalSize);
        }
        else
        {
            return string.Format("{0:0.00}M", _totalSize / 1000f);
        }
    }

    private string GetDownloadingVersion()
    {
        if (_versionData == null)
            return "????";

        return _versionData.GetDownloadingVersion();
    }

    public void ShowLoginBtn()
    {
        _loginContainer.SetActive(true);
    }
    /// <summary>
    /// 手机登陆
    /// </summary>
    /// <param name="go"></param>
    private void OnPhoneLoginHandler(GameObject go)
    {
//        UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
        var ctrl = UIManager.GetControler<LoginInputController>();
        ctrl.LoginPhone();
    }
    /// <summary>
    /// 游客登陆
    /// </summary>
    /// <param name="go"></param>
    private void OnTourisLoginHandler(GameObject go)
    {
        //UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
        if (ClientDefine.isWXLogin && !VersionData.IsReviewingVersion())
        {
            string md5Content = PlayerPrefs.GetString("accountServerLoginContent", "");
            if (!string.IsNullOrEmpty(md5Content))
            {
                UtilTools.ShowWaitWin(WaitFlag.LoginWin);
                LoginInputController.AccountServer_WXLogin(md5Content);
            }
            else
            {
                SDKManager.getInstance().CommonSDKInterface.login();
            }
        }
        else
        {
            var ctrl = UIManager.GetControler<LoginInputController>();
            StartCoroutine(ctrl.registerToAccountServerFast());
            //UtilTools.ShowWaitWin(WaitFlag.LoginWin, 5f);
            //快速登录
            //LoginController.AccountServer_QuickLogin();
        }
        
//        ctrl.LoginTouris();
    }

}
