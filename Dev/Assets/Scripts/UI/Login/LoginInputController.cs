/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	LoginInputController.cs
 * Summary: 	登陆界面
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/2/10 0010 下午 4:39
 ***************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.Text;
using EventManager;
using network;
using network.protobuf;
using sdk;
using Scene;
using UI.Controller;
using Utils;

public class LoginInputController : ControllerBase
{
    private LoginInputMono _mono;
    private Action _showLoginButton;
    public bool ChangeAccount = false;


    private string _playerLoginName = "";
    private string _playerLoginPassWard = "";
    public static StartUpScene startUpMono;

    private LoginData LoginGameData
    {
        get { return GameDataMgr.LOGIN_DATA; }
    }

    public LoginInputController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.HIGHT;
        prefabsPath = new string[] {UIPrefabPath.LOGIN_INPUT_WIN};
        MsgCallManager.AddCallback(ProtoID.SC_LOGIN_REPLY, OnLginReply); //登录回调
        EventSystem.RegisterEvent(EventID.SOKECT_CONNECT_RESULT, OnEventSocketConnectOK); //连接socket回调


        GameObject sceneObj = GameObject.Find("Scene");
        if (sceneObj){
            startUpMono = sceneObj.GetComponent<StartUpScene>();
        }
    }

    protected override void UICreateCallback()
    {
        _mono = winObject.AddComponent<LoginInputMono>();

        if (_showLoginButton != null){
            _showLoginButton();
            _mono.winBg.SetActive(false);
        }
        if (ChangeAccount){
            LoginPhone();
            ChangeAccount = false;
        }
    }

    protected override void UIDestroyCallback()
    {
        ChangeAccount = false;
    }

    public void ToBack(GameObject go)
    {
        UIManager.DestroyWin(sName);
    }


    public static void AccountServer_PhoneLogin(string account, string password)
    {
        if (startUpMono != null) startUpMono.PhoneLogin(account, password);
    }

    public void Login(string name, string passWord)
    {
        UIManager.CreateLuaWin(UIName.MAIN_CENTER_WIN);
//        UIManager.CreateLuaWin(UIName.MAIN_WIN);
        GameDataMgr.LOGIN_DATA.userName = name;
        GameDataMgr.LOGIN_DATA.userPassword = passWord;
        _playerLoginName = name;
        _playerLoginPassWard = passWord;

//        GameDataMgr.LOGIN_DATA.IsChangeAccountLogin = false;
        string userPwd = GameDataMgr.LOGIN_DATA.userPassword;
        EngineManager engine = EngineManager.GetInstance();
        string _apn = engine.GetAPNType();
        string _mac = engine.GetMac();
        string _model = engine.GetModel();
        int _channel = engine.GetChannel();
        int _subChanner = engine.GetSubChannel();
        int _sdkId = engine.GetSDKId();
        //        RequestServerInfo();
        Login(name, userPwd, _apn, _mac, _model, _channel, _subChanner, _sdkId);
//        UIManager.DestroyWin(sName);
    }


    public static void ConnectToServer()
    {
        Utils.LogSys.Log("+++++++++++++++连接Socket++++++++++++++++++");
        UtilTools.ShowWaitWin(WaitFlag.ConnectSocketFirst, 5f);
        ClientNetwork.Instance.Connect(); //连接游戏服务器
    }


    //连接socket回调
    public void OnEventSocketConnectOK(EventMultiArgs args)
    {
        bool result = args.GetArg<bool>("result", true);
//        bool bReconnect = args.GetArg<bool>("reconnecting", false);
        bool bFirstWaitShow = false;
        if (result){
//            UtilTools.ShowMessage("Socket登陆成功！！！！");
            UtilTools.HideWaitWin(WaitFlag.ConnectSocketFirst);
            UtilTools.HideWaitWin(WaitFlag.ConnectSocketSecond);
            if (!GameDataMgr.LOGIN_DATA.IsGetGameServerIP){
                GameDataMgr.LOGIN_DATA.IsConnectGamerServer = true;
                //是否为游客
                if (GameDataMgr.PLAYER_DATA.IsTouris){
                    _playerLoginName = GameDataMgr.LOGIN_DATA.lastLoginDeviceUUID;
                    _playerLoginPassWard = "";
//                    Login(GameDataMgr.LOGIN_DATA.lastLoginDeviceUUID,"");
                }
                else{
                    _playerLoginName = GameDataMgr.LOGIN_DATA.lastLoginAccount;
                    _playerLoginPassWard = GameDataMgr.LOGIN_DATA.lastLoginPassword;
                }
                Login(_playerLoginName, _playerLoginPassWard);
            }
            else{
                //连接游戏服务器成功
                Utils.LogSys.Log("+++++++++++++++连接游戏服务器成功++++++++++++++++++");
                GameDataMgr.LOGIN_DATA.IsConnectGamerServer = true;

                /*if (GameDataMgr.LOGIN_DATA.IsChangeAccountLogin)
                {
                    //切换帐号的登录流程
//                    UIManager.GetControler<ChangeAccountController>().ChangeAccount();
                }*/
            }
        }
        else{
            if (!GameDataMgr.LOGIN_DATA.IsGetGameServerIP){
                Utils.LogSys.LogError("ConectSocket Failed: Select Server");
            }
            else{
                Utils.LogSys.LogError("ConectSocket Failed: Game Server");
            }
            return; //socket连接超时不处理, 统一由WaitingController控制
        }
    }


    /// <summary>
    /// 登陆
    /// </summary>
    public void LoginAgain()
    {
        string userName = GameDataMgr.LOGIN_DATA.lastLoginAccount;
        string userPwd = GameDataMgr.LOGIN_DATA.lastLoginPassword;
        EngineManager engine = EngineManager.GetInstance();
        string _apn = engine.GetAPNType();
        string _mac = engine.GetMac();
        string _model = engine.GetModel();
        int _channel = engine.GetChannel();
        int _subChanner = engine.GetSubChannel();
        int _sdkId = engine.GetSDKId();
        UtilTools.ShowWaitWin(WaitFlag.LoginFirst);
        Login(userName, userPwd, _apn, _mac, _model, _channel, _subChanner, _sdkId);
    }

    public void Login(string user, string password, string apn, string mac, string model, int channel, int subChanner,
        int sdkId)
    {
        EngineManager engine = EngineManager.GetInstance();
        cs_login login = new cs_login();
        login.uid = user;
        login.password = string.IsNullOrEmpty(password) ? new byte[0] : TextUtils.GetBytes(password);
        //UtilTools.ShowMessage(LoginGameData.accountServerVerificationMD5());

        login.sz_param = LoginGameData.accountServerVerificationMD5();
		LogSys.LogError ("content=" + login.sz_param);
        login.version = engine.Message.GetAttribute("newVersions");
        login.network_type = apn;
        login.sys_type = (uint) engine.SysType;
        login.ios_idfa = "";
        login.ios_idfv = "";
        login.mac_address = mac;
        login.device_type = model;

        login.platform_flag = (uint) sdkId;
        login.chnid = (uint) channel;
        login.sub_chnid = (uint) subChanner;
//        UtilTools.ShowWaitWin(WaitFlag.LoginFirst);
        ClientNetwork.Instance.SendMsg(ProtoID.CS_LOGIN, login);
    }


    private void OnLginReply(object proto)
    {
//        LogSys.LogWarning("----->   lign ----- success ..."+proto);
        UtilTools.HideWaitWin(WaitFlag.LoginFirst);
        UtilTools.HideWaitFlag();
        if (proto == null){
            return;
        }

        UtilTools.OnUmSdkInit(sdk.SDKManager.AppKey, sdk.SDKManager.Q_ID.ToString());
        var msg = proto as sc_login_reply;
        if (msg.result == 1){
            
            SaveAllUserName(_playerLoginName);
            UIManager.DestroyWin(UIName.REGISTER_BINDING_WIN);
            UIManager.DestroyWin(UIName.LOGIN_INPUT_WIN);
//            GameDataMgr.LOGIN_DATA.IsLoginGameServer = true;
            UIManager.DestroyWin(UIName.LOADING_WIN);
            ClientNetwork.Instance.StartHeatBeast();
            if (GameDataMgr.LOGIN_DATA.IsRegister())
            {
                string loginContent = PlayerPrefs.GetString("accountServerLoginContent", "");
                if (string.IsNullOrEmpty(loginContent))
                {
                    loginContent = GameDataMgr.LOGIN_DATA.lastLoginAccount;
                }
                UtilTools.RegisterCount(loginContent);
            }
            var root = GameObject.Find("UIRoot");
            if (root != null){
                var loadingWin = root.transform.Find("LoadingWin").gameObject;
                if (loadingWin != null){
                    loadingWin.SetActive(true);
                }
            }
            ShopData.IOSPay_RetryLostOrder();
        }
        else{
            UtilTools.ShowMessage(msg.reason);
        }
    }

    public void SetLoginShow()
    {
    }


    public void UpdatePlayerInfo()
    {
        UIManager.CallLuaFuncCall("event_resource_update_from_csharp", GameObject.Find("UIRoot"));
    }

    public static void ClearLuaData()
    {
        UIManager.CallLuaFuncCall("event_clear_all_data_from_csharp", GameObject.Find("UIRoot"));
    }

    public void SetLoginShow(Action showLoginBtn)
    {
        _showLoginButton = showLoginBtn;
    }


    public void LoginTouris()
    {
        if (winObject == null) return;
//        _mono.winBg.SetActive(true);
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null){
            LogSys.LogWarning("LoginInputController:LoginTouris:    DeviceUUID==" + tools.GetDeviceUUID());
//            Login(tools.GetDeviceUUID(),"aaa");
            GameDataMgr.PLAYER_DATA.IsTouris = true;
        }
        else{
            _mono.winBg.SetActive(true);
        }
    }

    public void LoginPhone()
    {
        if (winObject == null) return;
        if (_mono.gameObject.activeSelf == false){
            _mono.gameObject.SetActive(true);
        }
        _mono.winBg.SetActive(true);
    }


    /// <summary>
    /// 保存登录过的所有玩家账号
    /// </summary>
    /// <param name="userName"></param>
    public void SaveAllUserName(string userName)
    {
        string allUserName = PlayerPrefs.GetString("allUser", "");
        JSONObject userList = new JSONObject(allUserName);

        if (string.IsNullOrEmpty(allUserName)){
            //服务器id
            JSONObject _userName = new JSONObject(JSONObject.Type.ARRAY);
            userList.AddField("userName", userName);
            _userName.Add(userName);
        }
        else{
            //游客不加入所有玩家账号列表中
            if (string.IsNullOrEmpty(_playerLoginPassWard) || _playerLoginPassWard == ""){
                return;
            }

            int haveSaveIndex = 0;
            bool isLogined = false;
            for (int i = 0; i < userList[0].Count; i++){
                if (userList[0].list[i].str == userName){
                    haveSaveIndex = i;
                    isLogined = true;
                }
            }
            if (isLogined){
                userList[0].list.RemoveAt(haveSaveIndex);
                userList[0].Add(userName);
            }
            else
//            if (!isLogined)
            {
                userList[0].Add(userName);
            }
        }

        PlayerPrefs.SetString("allUser", userList.ToString());
    }


    public void RemoveUser(string str)
    {
        string allUserName = PlayerPrefs.GetString("allUser", "");

        if (string.IsNullOrEmpty(allUserName)) return;
        JSONObject userList = new JSONObject(allUserName);
        int haveId = -1;
        for (int i = 0; i < userList[0].Count; i++){
            if (userList[0].list[i].str == str){
                haveId = i;
                break;
            }
        }
        if (haveId > -1){
            userList[0].list.RemoveAt(haveId);
        }

        PlayerPrefs.SetString("allUser", userList.ToString());
    }

    public string GetAllUserName()
    {
        return PlayerPrefs.GetString("allUser", "");
    }


    /*public JSONObject GetAllUserList()
    {

    }*/

    /// <summary>
    /// 快速登陆
    /// </summary>
    /// <returns></returns>
    public IEnumerator registerToAccountServerFast()
    {
        /*string _uuid = GameDataMgr.LOGIN_DATA.GetFastLoginUUID();
        WWWForm dataForm = new WWWForm();
        string _code = _uuid + "," + GameDataMgr.LOGIN_DATA.GetFastLoginKey();
        dataForm.AddField("type", "0");
        dataForm.AddField("code", _code);
        dataForm.AddField("t", "oauth");*/
        string devid = "";
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
        {
            devid = tools.GetIMEI(); //"fdsfd2";//
        }
        if (string.IsNullOrEmpty(devid)){
            devid = GameDataMgr.LOGIN_DATA.GetFastLoginUUID();
        }


        //Post数据表
        WWWForm dataForm = new WWWForm();
        dataForm.AddField("type", "0");
        string code = GameDataMgr.LOGIN_DATA.GetFastLoginUUID() + "," + GameDataMgr.LOGIN_DATA.GetFastLoginKey();
        dataForm.AddField("code", code);
        dataForm.AddField("t", "oauth");

        dataForm.AddField("qid", SDKManager.Q_ID);
        dataForm.AddField("devid", devid);
        dataForm.AddField("simid", "");
        dataForm.AddField("logintype", ClientDefine.PLAT_FORM_TYPE);
        WWW w = new WWW(sdk.SDKManager.FastLoginURL, dataForm);
        yield return w;
        if (string.IsNullOrEmpty(w.error)){
            if (w.isDone){
                Debug.LogError(w.text);
                bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text,true);
                LogSys.LogWarning("----->a= print fast word ======" + bRlt);
                if (bRlt){
//                    LogSys.LogWarning("-----> print fast word ======"+w.text);
                    GameDataMgr.LOGIN_DATA.SaveFastLoginInfo();
                    GameDataMgr.PLAYER_DATA.IsTouris = true;
                    if (!ClientNetwork.Instance.IsConnected()){
                        ClientNetwork.Instance.Connect();
                    }
                }
                else{
                    UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
                    var versionUpdate = startUpMono.transform.Find("VersionUpdate").GetComponent<VersionUpdate>();
                    versionUpdate.ShowLoginBtn();
                }
            }
        }
        else{
            UtilTools.HideWaitWin();
            UtilTools.MessageDialog(w.error, okCallbackFunc: ReturnToLogin);
        }
    }

    public void ReturnToLogin()
    {
        UtilTools.ReturnToLoginScene();
    }
    public static void UpdateHeadShow()
    {
        UIManager.CallLuaFuncCall("event_player_head_from_csharp", GameObject.Find("UIRoot"));
    }
    public static void AccountServer_WXLogin(string md5Content)
    {
        if (startUpMono != null)
            startUpMono.AutoLoginWeiXin(md5Content);
    }
    public static void ReConnectToServer()
    {
        var ctrl = UIManager.GetControler<LoginInputController>();
        string md5Content = PlayerPrefs.GetString("accountServerLoginContent", "");

        //打开登录界面
        if (!string.IsNullOrEmpty(md5Content) && ClientDefine.isWXLogin)
        {
            //微信自动登录
            UtilTools.ShowWaitWin(WaitFlag.LoginWin);
            AccountServer_WXLogin(md5Content);
        }
        else if (!ClientDefine.isWXLogin && !string.IsNullOrEmpty(GameDataMgr.LOGIN_DATA.lastLoginDeviceUUID))
        {
            Utils.LogSys.LogWarning("快速登录");
            //快速登录
            //            AccountServer_QuickLogin();
            startUpMono.LoginFastTriuse();
        }
        else if (!string.IsNullOrEmpty(GameDataMgr.LOGIN_DATA.lastLoginAccount) &&
                 !string.IsNullOrEmpty(GameDataMgr.LOGIN_DATA.lastLoginPassword))
        {
            Utils.LogSys.LogWarning("手机号自动登陆");
            //手机帐号自动登录
            AccountServer_PhoneLogin(GameDataMgr.LOGIN_DATA.lastLoginAccount, GameDataMgr.LOGIN_DATA.lastLoginPassword);

            ctrl.LoginPhone();
        }
        else
        {
            UtilTools.ReturnToLoginScene();
        }
    }
    public static void LoginAccountServer()
    {
//        UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
//        return;
        var ctrl = UIManager.GetControler<LoginInputController>();
        string md5Content = PlayerPrefs.GetString("accountServerLoginContent", "");

        //打开登录界面
        if (!string.IsNullOrEmpty(md5Content) && ClientDefine.isWXLogin)
        {
            //微信自动登录
            UtilTools.ShowWaitWin(WaitFlag.LoginWin);
            AccountServer_WXLogin(md5Content);
        }
        else if (!ClientDefine.isWXLogin && !string.IsNullOrEmpty(GameDataMgr.LOGIN_DATA.lastLoginDeviceUUID))
        {
            Utils.LogSys.LogWarning("快速登录");
            //快速登录
            //            AccountServer_QuickLogin();
            startUpMono.LoginFastTriuse();
        }
        else if (!string.IsNullOrEmpty(GameDataMgr.LOGIN_DATA.lastLoginAccount) &&
                 !string.IsNullOrEmpty(GameDataMgr.LOGIN_DATA.lastLoginPassword)){
            Utils.LogSys.LogWarning("手机号自动登陆");
            //手机帐号自动登录
            AccountServer_PhoneLogin(GameDataMgr.LOGIN_DATA.lastLoginAccount, GameDataMgr.LOGIN_DATA.lastLoginPassword);

            ctrl.LoginPhone();
        }
        else{
            var versionUpdate = startUpMono.transform.Find("VersionUpdate").GetComponent<VersionUpdate>();
            versionUpdate.ShowLoginBtn();
            //打开登录界面
            UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
        }
    }

    public void ResetPasswordSuccess()
    {
        if (_mono != null){
            _mono.ResetPasswordSucc();
        }
    }
}