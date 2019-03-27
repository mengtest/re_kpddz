/***************************************************************


 *
 *
 * Filename:  	StartUpScene.cs
 * Summary: 	开始场景控制脚本
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/21 2:26
 ***************************************************************/

#region using

using asset;
using customerPath;
using network.protobuf;
using object_c;
using sdk;
using System.Collections;
using sound;
using UI.Controller;
using UnityEngine;
using Utils;
using version;
using System.Collections.Generic;

#endregion using

namespace Scene
{
    //开始场景控制脚本
    public class StartUpScene : BaseScene
    {
        private bool _bStartUp = false;
        private int _startUpStep = 0;
        private ApplicationMgr _applicationMgr;
        public static Transform _uiCamera;
        public static Transform _sceneCamera;
        public static Transform _fishCamera;
        public static string _bindStrng = "";
        //加载
        private void Awake()
        {
            bCanClick = true;
            Caching.CleanCache ();
            FollowToSceneCamera follower = null;
            GameObject pListener = GameObject.Find("Listener");//声音接收者
            if (pListener == null) {
                pListener = new GameObject("Listener");
                pListener.AddComponent<AudioListener>();
                follower = pListener.AddComponent<FollowToSceneCamera>();
                DontDestroyOnLoad(pListener);
            } else {
                follower = pListener.GetComponent<FollowToSceneCamera>();
            }

            GameObject pSingletonObj = GameObject.Find("Singleton");
            if (pSingletonObj == null) {
                pSingletonObj = new GameObject("Singleton");
                pSingletonObj.transform.position = default(Vector3);
            }
            DontDestroyOnLoad(pSingletonObj);

            if (SDKManager.isAppStoreVersion())
            {
                if (pSingletonObj.GetComponent<ObjectCCallback>() == null)
                {
                    pSingletonObj.AddComponent<ObjectCCallback>();
                    Utils.LogSys.Log("XXXXXXXXXXXXXXXX--Add ObjectCCallback--XXXXXXXXXXXXXXXX");
                }
            }

            DestroyUnUsedUIRoot();

            GameObject camObj1 = GameObject.Find("Scene/Cameras/SceneCamera");
            if (camObj1 && !camObj1.GetComponent<CameraAjustor>()) {
                camObj1.AddComponent<CameraAjustor>();
                follower.camera_tf = camObj1.transform;
            }

            GameObject camObj2 = GameObject.Find("UIRoot/UICamera");
            if (camObj2 && !camObj2.GetComponent<CameraAjustor>()) {
                camObj2.SetActive(true);
                camObj2.AddComponent<CameraAjustor>();
            }
            GameSceneManager.uiCameraObj = camObj2;
            GameObject versionUpdate = GameObject.Find("VersionUpdate");
            //versionUpdate.transform.parent = GameObject.Find("UIRoot").transform;
            //DontDestroyOnLoad(versionUpdate.gameObject);
            CheckGameHead();
            versionUpdate.gameObject.SetActive(true);
            //uiBattlePauseController.GetPreState();//获取声音设置

            GameSceneManager.getInstance().SceneMono = this;
            bCanClick = true;
        }

        private void CheckGameHead()
        {
            GameObject uiroot = GameObject.Find("Scene");
            if (uiroot != null){
                var gameHeadLoader = uiroot.GetComponent<GameHeadLoader>();
                if (gameHeadLoader == null){
                    uiroot.AddComponent<GameHeadLoader>();
                }
//                var gameHeadUp = uiroot.GetComponent<GameHeadHttpUp>();
//                if (gameHeadUp == null){
//                    uiroot.AddComponent<GameHeadHttpUp>();
//                }
            }
        }

        public override void Start()
        {
            SDKManager.getInstance().sendNewbieguideDeviceUniqueIdentifier("1");

            //SDK初始化
            SDKManager.getInstance().initToolSDK();
            SDKManager.getInstance().initCommonSDK();

            _fishCamera = transform.Find("Cameras/FishCamera");
            _sceneCamera = transform.Find("Cameras/SceneCamera");
            _uiCamera = GameObject.Find("UIRoot/UICamera").transform;
            UtilTools.SetBgm("Sound/BGM/login");
        }

        //留一个，多其余删除（从主城退出时会有2个）
        private void DestroyUnUsedUIRoot()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("uiroot");
            int count = objs.Length;
            if (count >= 1)
                DontDestroyOnLoad(objs[0]);
            if (count >= 2) {
                for (int i = 1; i < count; i++) {
                    Destroy(objs[i]);
                }
            }
        }


        // Update 每帧调用一次
        override public void Update()
        {
            if (!_bStartUp)
                return;

            base.Update();


            int ok_step = _applicationMgr.InitOkStep;
            if (ApplicationMgr._bInitOK || ok_step == -1)//未开始初始化，或已完成
            {
                UtilTools.SetServerListTip("");
            }
            else
            {
                if (ok_step == 1)
                {
                    float process = ConfigDataMgr.getInstance().GetConfigLoadProcess();
                    string tips = GameText.Format("initingLoadXml", Mathf.FloorToInt(process * 100).ToString());
                    UtilTools.SetServerListTip(tips,true,process);
                }
                else if (ok_step == 4)
                {
                    //UtilTools.SetServerListTip(GameText.GetStr("loadingServerList"));
                }
                else if (ok_step == 5)
                {
                    UtilTools.SetServerListTip(GameText.GetStr("loadingNotice"), false, 1f);
                }
            }

            if (ApplicationMgr._bInitOK)
            {
                if (_startUpStep == 1)
                {
                    StartUp_step2();
                }
                else if (_startUpStep == 2 && AssetManager.IsPreLoadComplete())
                {
                    StartUp_step3();
                }
                else if (_startUpStep == 3 && GameDataMgr.LOGIN_DATA.IsConnectGamerServer)
                {
                    /*MainCityController cont = UIManager.GetControler<MainCityController>();
                    if (cont.IsActive)
                    {
                        StartUp_step4();
                    }*/
                    StartUp_step4();
                }
                else if (_startUpStep == 4 && GameDataMgr.LOGIN_DATA.IsLoginGameServer)
                {
                    StartUp_step5();
                }
            }

        }

        void ________thirdPartyLogin()
        {
            
            SDKManager.getInstance().CommonSDKInterface.login();
        }

        void sendDeviceUniqueIdentifierNewBie()
        {
            SDKManager.getInstance().sendNewbieguideDeviceUniqueIdentifier("5");
        }


        /// <summary>
        /// 在版本更新完成后，才调用该函数，开启游戏初始化。
        /// step1:开始加载配置文件
        /// </summary>
        public void StartUp()
        {
            SDKManager.getInstance().sendNewbieguideDeviceUniqueIdentifier("2");
            _bStartUp = true;
            GameObject pMgrObj = GameObject.Find("ApplicationMgr");
            if (pMgrObj == null) {
                pMgrObj = new GameObject("ApplicationMgr");
                pMgrObj.transform.position = default(Vector3);
                _applicationMgr = pMgrObj.AddComponent<ApplicationMgr>();
            }
            else
                _applicationMgr = pMgrObj.GetComponent<ApplicationMgr>();
            DontDestroyOnLoad(pMgrObj);

            BaseScene pSceneMono = GameSceneManager.getInstance().SceneMono;
            bool isStartUp = pSceneMono is StartUpScene;
            if (pSceneMono == null || !isStartUp) {
                //SceneConfig config = GameSceneManager.getInstance().GetSceneConfig(SceneName.s_StartupScene);
                GameSceneManager.getInstance().sceneLoadCompleteAndInit(SceneName.s_StartupScene, new SceneConfig());
            }

            base.Start();
            _startUpStep = 1;

            UtilTools.SetServerListTip(""); 
        }

        //step2:开始加载常用资源
        void StartUp_step2()
        {
            _startUpStep = 2;
            AssetManager.PreLoadCommonResources();//预加载常用资源，常驻内存。

        }

        //step3:开始登录帐号服务器
        void StartUp_step3()
        {
            _startUpStep = 3;
            UIManager.CreateWin(UIName.WAITING);//只创建不显示
            //ChangeAccountController.ConnectToAllocationServer();
            LoginInputController.LoginAccountServer();
//            UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
        }
        
        /// <summary>
        /// 登陆到账号服务器
        /// </summary>
        IEnumerator loginAccountServer()
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++连接分发服务器++++++++++++++++++");
            UtilTools.ShowWaitWin(WaitFlag.ConnectSocketFirst, 5f);
            ClientNetwork.Instance.Connect();//连接游戏服务器
//Post数据表
//         WWWForm dataForm = new WWWForm();
//         dataForm.AddField("username", inputName.value.Trim());
//         dataForm.AddField("password", inputPassword.value);
// 
//         WWW w = new WWW(SDKManager.AccountLoginURL, dataForm);
//         yield return w;
// 
//         if (w.error != null)
//         {
//             w = new WWW(SDKManager.AccountLoginURL, dataForm);
//             yield return w;
//         }
// 
//         if (w.error != null)
//         {
//             w = new WWW(SDKManager.AccountLoginURL, dataForm);
//             yield return w;
//         }
// 
//         UtilTools.HideWaitWin(WaitFlag.ChangeUserToAccountServer);
//         if (w.error == null)
//         {
//             if (w.isDone)
//             {
//                 bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text);
//                 if (bRlt)
//                 {
//                     GameDataMgr.LOGIN_DATA.userName = inputName.value.Trim();
//                     GameDataMgr.LOGIN_DATA.userPassword = inputPassword.value;
//                     GameDataMgr.LOGIN_DATA.AccountSrc = EAccountInfoSrc.eNewInput;
//                     GameDataMgr.LOGIN_DATA.IsLoginSuccess = true;
//                     //登录帐服成功后将玩家信息存入本地
//                     GameDataMgr.LOGIN_DATA.SetUserData();
//                     controller.ToLoginMainUI();
//                 }
//             }
//         }
//         else
//         {
//             //UtilTools.MessageDialog(w.error);
//             UtilTools.PlaySoundEffect("Sounds/UISound/error");
//             UtilTools.MessageDialog(GameText.GetStr("connect_account_server_failed"));
//        } 
        }

        //step4:socket连接完成
        void StartUp_step4()
        {
            LogSys.LogWarning("-----> _startUpStep = 4;");
            _startUpStep = 4;
//            ChangeAccountController.SendLoginMsg();
            //TODO 假设登陆成功
//            GameDataMgr.LOGIN_DATA.IsLoginGameServer = true;
        }

        //step5登录完成
        void StartUp_step5()
        {
            _startUpStep = 5;
            Transform versionUpdate = transform.Find("VersionUpdate");
            versionUpdate.gameObject.SetActive(false);
            /*if (PlayerPrefs.GetInt("PlayerMusic",50) == 50){
                UtilTools.SetBgm("Sounds/BGM/zhandou1BGM");
            }*/
        }

        //登录失败时显示登录界面
        public void LoginFailedAndShowLoginWin()
        {
            _startUpStep = 5;
            var versionUpdate = transform.Find("VersionUpdate").GetComponent<VersionUpdate>();
            versionUpdate.gameObject.SetActive(false);
//            UIManager.CreateWin(UIName.CHANGE_ACCOUNT_WIN);
            UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
        }


        public void ReturnToLogin()
        {
            _startUpStep = 4;
            var versionUpdate = transform.Find("VersionUpdate").GetComponent<VersionUpdate>();
            versionUpdate.gameObject.SetActive(true);
//            UIManager.CreateWin(UIName.CHANGE_ACCOUNT_WIN);
//            UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
            GameDataMgr.LOGIN_DATA.IsLoginGameServer = false;
            versionUpdate.ShowLoginBtn();

            var uiroot = GameObject.Find("UIRoot");
            if (uiroot == null) return;
            var loadinWin = uiroot.transform.Find("LoadingWin").gameObject;
            if (loadinWin != null){
                loadinWin.SetActive(true);
                Invoke("HideWaitWin", .5f);
            }
        }

        public void HideWaitWin()
        {
            var uiRoot = GameObject.Find("UIRoot");
            if (uiRoot == null) return;
            var loadinWin = uiRoot.transform.Find("LoadingWin").gameObject;
            if (loadinWin != null){
                loadinWin.SetActive(false);
                CancelInvoke("HideWaitWin");
            }
        }

        public void RestartVersionUpdate()
        {
            Transform versionUpdate = transform.Find("VersionUpdate");
            VersionUpdate comp = versionUpdate.GetComponent<VersionUpdate>();
            if (comp != null)
                Destroy(comp);

            versionUpdate.gameObject.SetActive(true);
            versionUpdate.gameObject.AddComponent<VersionUpdate>();
        }

        #region
        /// <summary>
        /// 开始登录帐号
        /// </summary>
        /// <param name="usrname"></param>
        /// <param name="password"></param>
        public void StartLogin(string usrname, string password)
        {
            StartCoroutine(loginAccountServer());
        }

        /// <summary>
        /// 手机登陆
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        public void PhoneLogin(string phoneNum, string password)
        {
            StartCoroutine(loginAccountServer_Phone(phoneNum, password));
        }

        /// <summary>
        /// 登陆到账号服务器
        /// </summary>
        IEnumerator loginAccountServer_Phone(string phoneNum, string password)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++连接分发服务器++++++++++++++++++");
            //Post数据表
            WWWForm dataForm = new WWWForm();
            dataForm.AddField("username", phoneNum);
            dataForm.AddField("password", password);
            dataForm.AddField("qid", SDKManager.Q_ID);
            dataForm.AddField("iid", StartUpScene._bindStrng);
            GameDataMgr.LOGIN_DATA.nowLoginAccount = phoneNum;
            GameDataMgr.LOGIN_DATA.nowLoginPassword = password;

            WWW w = new WWW(SDKManager.AccountLoginURL, dataForm);
            yield return w;

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AccountLoginURL, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AccountLoginURL, dataForm);
                yield return w;
            }

            //         UtilTools.HideWaitWin(WaitFlag.ChangeUserToAccountServer);
            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
                    bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text, true);
//                    bool bRlt = false;
                    if (bRlt)
                    {
                        GameDataMgr.LOGIN_DATA.SavePhoneLoginInfo();
                        LoginInputController.ConnectToServer();
                    }
                    else
                    {
                        var versionUpdate = transform.Find("VersionUpdate").GetComponent<VersionUpdate>();
                        versionUpdate.ShowLoginBtn();
                        UtilTools.HideWaitWin(WaitFlag.LoginWin);
                        UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);//如果登录帐号服务器失败， 显示登录界面

                    }
                }
            }
            else
            {
                UtilTools.HideWaitWin(WaitFlag.LoginWin);
                UtilTools.ErrorMessageDialog(GameText.GetStr("connect_account_server_failed"));
                UtilTools.ReturnToLoginScene();
                UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);//如果登录帐号服务器失败， 显示登录界面
            }
        }

        #endregion
        #region//微信自动登录
        public void AutoLoginWeiXin(string md5Content)
        {
            StartCoroutine(AutologinAccountServer_WeiXin(md5Content));
        }
        IEnumerator AutologinAccountServer_WeiXin(string md5Content)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++自动登录微信帐号++++++++++++++++++");
            WWWForm dataForm = new WWWForm();
            dataForm.AddField("code", md5Content);
            dataForm.AddField("qid", SDKManager.Q_ID);
            dataForm.AddField("iid", StartUpScene._bindStrng);
            string devid = "";
            if (sdk.SDKManager.isAppStoreVersion())
            {
                devid = object_c.ObjectCCallback._IDFA;
            }
            else
            {
                JARUtilTools tools = Scene.GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
                if (tools != null)
                {
                    devid = tools.GetIMEI(); //"fdsfd2";//
                    if (string.IsNullOrEmpty(devid))
                        devid = GameDataMgr.LOGIN_DATA.GetFastLoginUUID();
                }

            }
            dataForm.AddField("devid", devid);

            WWW w = new WWW(SDKManager.AutoLoginUrl, dataForm);
            yield return w;
            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AutoLoginUrl, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AutoLoginUrl, dataForm);
                yield return w;
            }

            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
                    bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text, true, true, true);
                    if (bRlt)
                    {
                        UtilTools.HideWaitWin(WaitFlag.LoginWin);
                        //GameDataMgr.LOGIN_DATA.SavePhoneLoginInfo();
                        LoginInputController.ConnectToServer();
                    }
                    else
                    {
                        var versionUpdate = transform.Find("VersionUpdate").GetComponent<VersionUpdate>();
                        versionUpdate.ShowLoginBtn();
                        UtilTools.HideWaitWin(WaitFlag.LoginWin);
                        UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);//如果登录帐号服务器失败， 显示登录界面
                    }
                }
            }
            else
            {
                var versionUpdate = transform.Find("VersionUpdate").GetComponent<VersionUpdate>();
                versionUpdate.ShowLoginBtn();
                UtilTools.HideWaitWin(WaitFlag.LoginWin);
                UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);//如果登录帐号服务器失败， 显示登录界面
            }
        }
        #endregion
        # region//手机注册
        /// <summary>
        /// 手机登录
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <param name="code">验证码</param>
        public void PhoneRegister(string phoneNum, string password, string code)
        {
            StartCoroutine(RegisterAccount_Phone(phoneNum, password, code));
        }

        /// <summary>
        /// 手机注册账号
        /// </summary>
        IEnumerator RegisterAccount_Phone(string phoneNum, string password, string code)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++连接分发服务器++++++++++++++++++");
            //Post数据表
            string devid = "";
            JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
            if (tools != null)
            {
                devid = tools.GetIMEI(); //"fdsfd2";//
            }

            if (string.IsNullOrEmpty(devid)){
                devid = GameDataMgr.LOGIN_DATA.GetFastLoginUUID();
            }

            WWWForm dataForm = new WWWForm();
            dataForm.AddField("username", phoneNum);
            dataForm.AddField("password", password);
            dataForm.AddField("code", code);

            dataForm.AddField("qid", SDKManager.Q_ID);
            dataForm.AddField("devid", devid);
            dataForm.AddField("simid", "");
            dataForm.AddField("logintype", ClientDefine.PLAT_FORM_TYPE);
            dataForm.AddField("iid", StartUpScene._bindStrng);
            WWW w = new WWW(SDKManager.AccountRegistURL, dataForm);
            yield return w;

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AccountRegistURL, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AccountRegistURL, dataForm);
                yield return w;
            }

            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
                    bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text, true);
                    if (bRlt)
                    {
                        LoginInputController.ConnectToServer();
                        GameDataMgr.LOGIN_DATA.nowLoginAccount = phoneNum;
                        GameDataMgr.LOGIN_DATA.nowLoginPassword = password;
                        GameDataMgr.LOGIN_DATA.SavePhoneLoginInfo();
                        UIManager.GetControler<RegisterBindingController>().cooldownEndTime = 0;
                        LogSys.LogWarning("手机注册成功！！！！");
                    }
                    else
                    {
                        LogSys.LogWarning("登录帐号服务器失败");

                        UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);//如果登录帐号服务器失败， 显示登录界面
                    }
                }
            }
            else
            {
                UtilTools.ErrorMessageDialog(GameText.GetStr("connect_account_server_failed"));
                UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);//如果登录帐号服务器失败， 显示登录界面
            }
            UtilTools.HideWaitWin(WaitFlag.RegisterAccount);
        }
        #endregion


        #region//手机绑定帐号
        /// <summary>
        /// 手机登录
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <param name="code">验证码</param>
        public void PhoneBind(string phoneNum, string password, string code)
        {
            StartCoroutine(BindAccount_Phone(phoneNum, password, code));
        }

        /// <summary>
        /// 手机绑定帐号
        /// </summary>
        IEnumerator BindAccount_Phone(string phoneNum, string password, string code)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++连接分发服务器++++++++++++++++++");
            //Post数据表
            WWWForm dataForm = new WWWForm();
            string str = GameDataMgr.LOGIN_DATA.GetOpenId();// +"," + GameDataMgr.LOGIN_DATA.GetFastLoginKey();
            dataForm.AddField("str", str);
            dataForm.AddField("username", phoneNum);
            dataForm.AddField("password", password);
            dataForm.AddField("code", code);
            dataForm.AddField("chaid", GameDataMgr.PLAYER_DATA.Account);

//            LogSys.LogWarning("chaid =         "+GameDataMgr.PLAYER_DATA.Account);

            WWW w = new WWW(SDKManager.AccountBindURL, dataForm);
            yield return w;

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AccountBindURL, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.AccountBindURL, dataForm);
                yield return w;
            }

            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
                    bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text, false);
                    if (bRlt)
                    {
                        GameDataMgr.LOGIN_DATA.nowLoginAccount = phoneNum;
                        GameDataMgr.LOGIN_DATA.nowLoginPassword = password;
                        GameDataMgr.LOGIN_DATA.SavePhoneLoginInfo();
                        UIManager.DestroyWin(UIName.REGISTER_BINDING_WIN);
                        UIManager.DestroyWinByAction(UIName.LOGIN_INPUT_WIN);
                        UtilTools.ShowMessage(GameText.GetStr("bind_account_success"), TextColor.GREEN);
                        UIManager.GetControler<RegisterBindingController>().cooldownEndTime = 0;
                        GameDataMgr.PLAYER_DATA.IsTouris = false;
//                        UIManager.GetControler<MainCityController>().UpdateBindBtn();
                    }
                }
            }
            else
            {
                UtilTools.ErrorMessageDialog(GameText.GetStr("connect_account_server_failed"));
            }
            UtilTools.HideWaitWin(WaitFlag.BindPhone);
        }
        #endregion
        public void ShareMutilPic(string userid)
        {
            StartCoroutine(ShareMutilPicToUtl(userid));
        }
        IEnumerator ShareMutilPicToUtl(string userid)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++连接分发服务器++++++++++++++++++");
            //Post数据表
            WWWForm dataForm = new WWWForm();
            string url = string.Format(SDKManager.SharePicUrl, userid);
            dataForm.AddField("chaid", userid);


            WWW w = new WWW(url, dataForm);
            yield return w;

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(url, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(url, dataForm);
                yield return w;
            }

            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
                    if (string.IsNullOrEmpty(w.text))
                        yield return w;
                    JSONObject arrStr = new JSONObject(w.text);
                    string[] strArr = new string[6];
                    GameDataMgr.PLAYER_DATA.ShareURL = arrStr[0].str;
                    GameDataMgr.PLAYER_DATA.SharePicUrl = arrStr[1].str;
                    for (int i = 0; i < 6; i++)
                    {
                        if (i + 2 < arrStr.Count)
                        {
                            strArr[i] = arrStr[i + 2].str;
                        }
                        else
                        {
                            strArr[i] = "";
                        }
                    }
                    GameDataMgr.PLAYER_DATA.ShareStr1 = strArr[0];
                    GameDataMgr.PLAYER_DATA.ShareStr2 = strArr[1];
                    GameDataMgr.PLAYER_DATA.ShareStr3 = strArr[2];
                    GameDataMgr.PLAYER_DATA.ShareStr4 = strArr[3];
                    GameDataMgr.PLAYER_DATA.ShareStr5 = strArr[4];
                    GameDataMgr.PLAYER_DATA.ShareStr6 = strArr[5];
                    BarcodeCam.getInstance().InitParam();
                    //SDKManager.getInstance().CommonSDKInterface.ShareMutilPic(flag, desc, arrStr[0].str, arrStr[1].str,arrStr[2].str);
                }
            }
            else
            {
                UtilTools.ErrorMessageDialog(GameText.GetStr("connect_account_server_failed"));
            }
        }

        # region//手机修改密码
        /// <summary>
        /// 手机修改密码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <param name="code">验证码</param>
        public void PhoneResetPassword(string phoneNum, string password, string code)
        {
            StartCoroutine(ResetPassword_Phone(phoneNum, password, code));
        }

        /// <summary>
        /// 手机修改密码
        /// </summary>
        IEnumerator ResetPassword_Phone(string phoneNum, string password, string code)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++连接分发服务器++++++++++++++++++");
            WWWForm dataForm = new WWWForm();
            dataForm.AddField("username", phoneNum);
            dataForm.AddField("password", password);
            dataForm.AddField("code", code);

            WWW w = new WWW(SDKManager.ResetPasswordURL, dataForm);
            yield return w;

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.ResetPasswordURL, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.ResetPasswordURL, dataForm);
                yield return w;
            }

            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
                    bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text, false);
                    if (bRlt)
                    {
                        GameDataMgr.LOGIN_DATA.nowLoginAccount = phoneNum;
                        GameDataMgr.LOGIN_DATA.nowLoginPassword = password;
                        GameDataMgr.LOGIN_DATA.SavePhoneLoginInfo();
                        UtilTools.ShowMessage(GameText.GetStr("change_password_succ"),TextColor.GREEN);
                        UIManager.GetControler<RegisterBindingController>().cooldownEndTime = 0;
                        UIManager.GetControler<PIChangePasswordController>().ResetPasswordSuccess();
                        if (UIManager.IsWinShow(UIName.LOGIN_INPUT_WIN)){
                            UIManager.GetControler<LoginInputController>().ResetPasswordSuccess();
                        }
                    }
                }
            }
            else
            {
                UtilTools.ErrorMessageDialog(GameText.GetStr("connect_account_server_failed"));
            }

            UtilTools.HideWaitWin(WaitFlag.ChangePassword);
        }
        #endregion


        //登录验证
        public void VerificationLoginSign(string userId, string sign)
        {
//             UIManager.GetControler<LoginController>().cooldownEndTime = UtilTools.GetClientTime() + 60;
//             return;
            StartCoroutine(VerificationLogin_Sign(userId, sign));
        }

        /// <summary>
        /// 请求手机验证码
        /// sType: 1001表示注册，1002表示找回密码, 1003绑定
        /// </summary>
        IEnumerator VerificationLogin_Sign(string userId, string sign)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++++++++++++++++++++");
            WWWForm dataForm = new WWWForm();
            dataForm.AddField("uid", userId);
            dataForm.AddField("sign", sign);//
            WWW w = new WWW(SDKManager.LoginSignUrl, dataForm);
            yield return w;

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.LoginSignUrl, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.LoginSignUrl, dataForm);
                yield return w;
            }

            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
//                     bool rlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text, false);
//                     if (rlt)
//                     {
//                         UIManager.GetControler<RegisterBindingController>().cooldownEndTime = UtilTools.GetClientTime() + 60;
//                     }
//                     else
//                     {
//                         UIManager.GetControler<RegisterBindingController>().cooldownEndTime = UtilTools.GetClientTime();
//                     }
                }
            }
        }


        #region//请求手机验证码
        /// <summary>
        /// 请求手机验证码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="sType"> 1001表示注册，1002表示找回密码, 1003绑定</param>
        public void PhoneVerificationCode(string phoneNum, string sType,string sign)
        {
//             UIManager.GetControler<LoginController>().cooldownEndTime = UtilTools.GetClientTime() + 60;
//             return;
            StartCoroutine(VerificationCode_Phone(phoneNum, sType, sign));
        }

        /// <summary>
        /// 请求手机验证码
        /// sType: 1001表示注册，1002表示找回密码, 1003绑定
        /// </summary>
        IEnumerator VerificationCode_Phone(string phoneNum, string sType, string sign)
        {
            yield return null;
            Utils.LogSys.Log("+++++++++++++++连接分发服务器++++++++++++++++++");
            WWWForm dataForm = new WWWForm();
            dataForm.AddField("mobile", phoneNum);
            dataForm.AddField("t", sType);//
			if (sign.Equals ("IOS")) {
				dataForm.AddField("IOS", 1);//
			}else{
            	dataForm.AddField("sign", sign);//
			}
            WWW w = new WWW(SDKManager.VerificationCodeURL, dataForm);
            yield return w;

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.VerificationCodeURL, dataForm);
                yield return w;
            }

            if (!string.IsNullOrEmpty(w.error))
            {
                yield return new WaitForSeconds(1f);
                w = new WWW(SDKManager.VerificationCodeURL, dataForm);
                yield return w;
            }

            if (string.IsNullOrEmpty(w.error))
            {
                if (w.isDone)
                {
                    bool rlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(w.text, false);
                    if (rlt)
                    {
                        UIManager.GetControler<RegisterBindingController>().cooldownEndTime = UtilTools.GetClientTime() + 60;
                    }
                    else
                    {
                        UIManager.GetControler<RegisterBindingController>().cooldownEndTime = UtilTools.GetClientTime();
                    }
                }
            }
            else
            {
                UtilTools.ErrorMessageDialog(GameText.GetStr("connect_account_server_failed"));
                UIManager.GetControler<RegisterBindingController>().cooldownEndTime = UtilTools.GetClientTime();
            }
        }
        #endregion

        public void LoginFastTriuse()
        {
            var ctrl = UIManager.GetControler<LoginInputController>();
            StartCoroutine(ctrl.registerToAccountServerFast());
        }

        //appStore支付回调
        public void ApppStoreRechargeCallback(string productID, string sReceipt, string sTransaction)
        {
            int nCost = GameDataMgr.PLAYER_DATA.GetPriceByProductID(productID);
            int nKey = GameDataMgr.PLAYER_DATA.GetKeyByProductID(productID);
            StartCoroutine(ApppStoreRechargeAsync(productID, sReceipt, nCost, nKey, sTransaction));
        }

        public IEnumerator ApppStoreRechargeAsync(string productID, string sReceipt, int nCost, int nKey, string sTransaction)
         {

            yield return new WaitForEndOfFrame();
 
            string strInfoFormat = SDKManager.PlatformPayUrl;
			string sPlayerID = GameDataMgr.PLAYER_DATA.Account.ToString();
            int key = GameDataMgr.PLAYER_DATA.GetKeyByProductID(productID);
            WWWForm www_form = new WWWForm();
            www_form.AddField("d", "iOS");
            www_form.AddField("m", nCost.ToString());
            www_form.AddField("uname", GameDataMgr.PLAYER_DATA.UserName);
            www_form.AddField("server", GameDataMgr.LOGIN_DATA.serverId);
            www_form.AddField("id", sPlayerID);
            www_form.AddField("type", "1");//11微信支付   2支付宝支付   1表示apple支付
            www_form.AddField("saletype", key);
            www_form.AddField("ordersn", sTransaction);
            
            if (VersionData.IsReviewingVersion())
            {
                www_form.AddField("sandbox", "1");//"1"沙盒 “2”正式
            }
            else
            {
                www_form.AddField("sandbox", "2");//"1"沙盒 “2”正式
            }
            www_form.AddField("receipt", sReceipt);
            WWW www = new WWW(strInfoFormat, www_form);
            yield return www;
 
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                if (www.text.Equals("ok"))
                {
                    Utils.LogSys.Log("recharge ok");
                    ShopData.IOSPay_DelOrderNum(sTransaction);
                }
            }
            else if (!string.IsNullOrEmpty(www.error))
            {
                UtilTools.MessageDialog(www.error);
            }
            UtilTools.HideWaitWin(WaitFlag.AppStorePay);
        }
         #region //漏单处理

         //appStore漏单处理
         public void ApppStoreRechargeLost()
         {
             StopCoroutine("ApppStoreRechargeLostAsync");
             if (ShopData._dicIOSPayOrderNum.Count == 0)
             {
                 return;
             }
             StartCoroutine("ApppStoreRechargeLostAsync");
         }

         public IEnumerator ApppStoreRechargeLostAsync()
         {
             if (ShopData._dicIOSPayOrderNum.Count == 0)
             {
                 yield break;
             }

             List<string> _copyOrders = new List<string>();
             foreach (KeyValuePair<string, string> item in ShopData._dicIOSPayOrderNum)
             {
                 _copyOrders.Add(item.Key);
             }
             List<string> _toRemove = new List<string>();
             for (int ind = 0; ind < _copyOrders.Count; ind++)
             {
                 string sTransaction = _copyOrders[ind];
                 if (ShopData._dicIOSPayOrderNum.ContainsKey(sTransaction))
                 {
                     string sReceipt = ShopData._dicIOSPayOrderNum[sTransaction];
                     string productID = ShopData._dicIOSPayProductID[sTransaction];
                     int nCost = GameDataMgr.PLAYER_DATA.GetPriceByProductID(productID);
                     int nKey = GameDataMgr.PLAYER_DATA.GetKeyByProductID(productID);
                     int key = GameDataMgr.PLAYER_DATA.GetKeyByProductID(productID);

                     string strInfoFormat = SDKManager.PlatformPayUrl;
                     string sPlayerID = GameDataMgr.PLAYER_DATA.UserId.ToString();
                     WWWForm www_form = new WWWForm();
                     www_form.AddField("d", "ios");
                     www_form.AddField("m", nCost.ToString());
                     www_form.AddField("uname", GameDataMgr.PLAYER_DATA.UserName);
                     www_form.AddField("server", GameDataMgr.LOGIN_DATA.serverId);
                     www_form.AddField("id", sPlayerID);
                     www_form.AddField("type", "1");//11微信支付   2支付宝支付   1表示apple支付
                     www_form.AddField("saletype", key);
                     www_form.AddField("ordersn", sTransaction);

                     if (VersionData.IsReviewingVersion())
                     {
                         www_form.AddField("sandbox", "1");//"1"沙盒 “2”正式
                     }
                     else
                     {
                         www_form.AddField("sandbox", "2");//"1"沙盒 “2”正式
                     }
                     www_form.AddField("receipt", sReceipt);
                     WWW www = new WWW(strInfoFormat, www_form);
                     yield return www;
                     Utils.LogSys.Log("ApppStoreRechargeLostAsync：　" + sTransaction);
                     if (www.isDone && string.IsNullOrEmpty(www.error))
                     {
                         if (!string.IsNullOrEmpty(www.text))
                         {
                             Utils.LogSys.Log("recharge ok");
                             _toRemove.Add(sTransaction);
                         }
                         //ReYunUtils.Track_SetPayment(sReceipt, "normal", "RMB", (int)price);
                     }
                     else if (!string.IsNullOrEmpty(www.error))
                     {
                         //Utils.LogSys.Log("www.error：　" + www.error);
                         UtilTools.MessageDialog(www.error);
                     }
                 }
             }
             for (int i = 0; i < _toRemove.Count; i++)
             {
                 ShopData.IOSPay_DelOrderNum(_toRemove[i]);

             }
         }
         #endregion
		public void startIOSWxPay(int payTag, int nCost, int nKey, string sName, string sDesc)
		{
			if (version.VersionData.IsReviewingVersion())
			{
				return;
			}
			else
			{
				StartCoroutine (IOSWxPay(payTag,nCost,nKey,sName,sDesc));
			}
		}
		/// <summary>
		/// IOS微信支付接口
		/// </summary>
		/// <returns>The IOS official pay.</returns>
		/// <param name="tag">Tag.</param>
		public IEnumerator IOSWxPay(int payTag, int nCost, int nKey, string sName, string sDesc)
		{
			//UtilTools.ShowWaitWin(WaitFlag.AppStorePay);

			string extraInfo = "1";
			int payType = 0;
			if (payTag == 1) //微信支付
			{
				extraInfo = "2";
				payType = 10;
			}

			else if (payTag == 2) // 支付宝支付
			{
				extraInfo = "1";
				payType = 2;
			}

			string strUserID = GameDataMgr.PLAYER_DATA.Account;
			string strUrlInfo = SDKManager.PlatformPayUrlForAlipayAndWX;
			Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> WWW: " + strUrlInfo);
			WWWForm form = new WWWForm();
			form.AddField("d", "iOS");
			form.AddField("m", nCost.ToString());
			form.AddField("uname", GameDataMgr.PLAYER_DATA.UserName);
			form.AddField("server", GameDataMgr.LOGIN_DATA.serverId);
			form.AddField("id", strUserID);
			form.AddField("type", payType.ToString());
			form.AddField("saletype", nKey.ToString());

			//向后台请求订单号
			WWW www = new WWW(strUrlInfo, form);
			yield return www;

			Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> WWW: " + www.text);
			Debug.Log(">>>>>>>>>>>>>>>>>>>>>>> ERROR WWW: " + www.error);

			if (www.isDone && string.IsNullOrEmpty(www.error)){
				var orderInfo = new CommonOrderInfo();
				orderInfo.extrasParams = extraInfo;
				orderInfo.cpOrderID = www.text;
				orderInfo.goodsName = sName;
				orderInfo.amount = nCost;
				orderInfo.goodsDesc = sDesc;
				orderInfo.extrasParams = extraInfo;
				orderInfo.callbackUrl = SDKManager.PayCallbackUrl;

				var roleInfo = new CommonGameRoleInfo();
				roleInfo.gameRoleName = GameDataMgr.PLAYER_DATA.UserName;
				roleInfo.gameRoleID = GameDataMgr.PLAYER_DATA.Account.ToString();
				roleInfo.gameRoleLevel = GameDataMgr.PLAYER_DATA.Level.ToString();
				roleInfo.gameRoleBalance = GameDataMgr.PLAYER_DATA.Gold.ToString();
				roleInfo.vipLevel = GameDataMgr.PLAYER_DATA.VipLevel.ToString();
				SDKManager.getInstance().CommonSDKInterface.pay(orderInfo, roleInfo);
				//UtilTools.HideWaitFlag();
			}
		}
    }
}