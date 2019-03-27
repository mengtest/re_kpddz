/***************************************************************


 *
 *
 * Filename:  	SceneManager.cs	
 * Summary: 	场景管理：负责场景的管理，加载，卸载，切换等等
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/19 23:35
 ***************************************************************/


#region Using
using asset;
using EventManager;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using task;
using UI.Controller;
using UnityEngine;
using Utils;
using sound;
using battleBaseDefine;
using System.Collections;
using UnityEngine.SceneManagement;
#endregion

namespace Scene
{

    //场景名字
    public struct SceneConfig
    {
        public string sceneName;
        public int nType;
        public int nOpenType;
        public int battleScene;
        public string sHttp;
        public int nVersion;

    }

    public struct BackSceneData
    {
        public GameObject backScene;
        public BaseScene backSceneMono;
        public string backSceneName;
    }

    //场景名字
    public class SceneName
    {
        public const string s_StartupScene           = "startUpScene";//"dengruchangjing";//"startUpScene";
        public const string s_LoadingScene           = "loadingScene";
        public const string s_MainScene              = "mainScene";
        //public const string s_HuLaoGuan              = "hulaoguan";
        public const string s_TongQueLou             = "TongQueLou";
        public const string s_JiuGuang               = "JiuGuang";
        //public const string s_Arena                  = "jingjichang";
        public const string s_Copy                   = "fubenxuanzhe";//副本
        public const string s_BattleCopy_caodi1_1    = "zhandoufuben_caodi1-1";
        public const string s_BattleCopy_caodi1_2    = "zhandoufuben_caodi1-2";
        public const string s_BattleCopy_caodi1_3    = "zhandoufuben_caodi1-3";
        public const string s_BattleCopy_chengshi1_1 = "zhandoufuben_chengshi1-1";
        public const string s_BattleCopy_chengshi1_2 = "zhandoufuben_chengshi1-2";
        public const string s_BattleCopy_chengshi1_3 = "zhandoufuben_chengshi1-3";
        public const string s_BattleCopy_shamo_1     = "zhandoufuben_shamo-1";
        public const string s_BattleCopy_shamo_2     = "zhandoufuben_shamo-2";
        public const string s_BattleCopy_shamo_3     = "zhandoufuben_shamo-3";
        public const string s_Battle_tongquetai      = "zhandoufuben_tongquetai";
        public const string s_Battle_jingjichang     = "zhandoufuben_jingjichang";
        public const string s_Battle_test            = "zhandoufuben_test";
        public const string s_BattleCopy_hulaoguan   = "zhandoufuben_hulaoguan";
        public const string s_Battle_guangduzhizhan  = "zhandoufuben_guangduzhizhan";
        public const string s_chibizhizhanV2         = "chibizhizhanV2";
        public const string s_GuidBoss_JiGuanRen     = "zhandouditu_boss02";
        public const string s_GuidBoss_Normal        = "zhandouditu_boss01";
        public const string s_Guild                  = "gonghuichangjing";
        public const string s_ZhouMuZhiZhan          = "zhandoufuben_zhoumuzhizhan";
        public const string s_Yilingzhizhan          = "zhandoufuben_yilingzhizhan_new";
        public const string s_guanduzhizhan          = "zhandoufuben_yilingzhizhan_new";
        public const string s_Battle_changancheng    = "zhandoufuben_changancheng";
        public const string s_Battle_jingong         = "chibizhizhan";
        public const string s_Battle_xiangyang       = "zhandoufuben_ronyangcheng";



    }


    //场景管理类
    public class GameSceneManager : Singleton<GameSceneManager>
    {

        //场景代理类型
        public delegate void delegateScene(string name, SceneConfig config);

        //开始加载 && 加载完成 && 释放完成
        static public event delegateScene onLoadStart;
        static public event delegateScene onLoadComplete;//加载场景完成
        static public event delegateScene onInitComplete;//加载场景Mono和部件完成
        static public event delegateScene onReleaseComplete;

        private TaskBase _taskLoadScene;
        private Dictionary<string, SceneConfig> dicSceneCofig;// = new Dictionary<string, SceneConfig>();
        public static string changeToSence = "";
        public static string sCurSenceName = "";
        GameObject pCurScene;//当前场景对象
        GameObject pLoadingScene;//加载中场景对象
        private List<BackSceneData> _backSeneList = new List<BackSceneData>();//缓存后台场景

        public static GameObject uiCameraObj;
        public static GameObject sceneCameraObj;

        string[] strMainCityBGM = new string[2] { "Sounds/BGM/bgm_city_loop", "Sounds/BGM/bgm_city_loop2" };

        private BaseScene _sceneMono = null;
        public BaseScene SceneMono
        {
            get { return _sceneMono; }
            set { _sceneMono = value; }
        }
        public GameObject CurSceneObject
        {
            set { pCurScene = value; }
            get
            {
                if (pCurScene == null)
                    return GameObject.Find("Scene");
                return pCurScene;
            }
        }
        public Transform CurSceneTransform
        {
            get
            {
                if (pCurScene == null)
                    return GameObject.Find("Scene").transform;
                return pCurScene.transform;
            }
        }
        public float LoadProcess
        {
            get
            {
                if (_taskLoadScene != null)
                    return _taskLoadScene.getProgress();
                return 0f;
            }
        }

        //////////////////////////////////////////////////////////////////////////
        void Start()
        {
            onLoadComplete += sceneLoadCompleteAndInit;
            sceneCameraObj = CurSceneObject.transform.FindChild(("Cameras/SceneCamera")).gameObject;
        }

        void OnDisable()
        {
        }


        //////////////////////////////////////////////////////////////////////////


        public GameSceneManager() { }


        /// <summary>
        /// 初始化场景管理类
        /// </summary>
//         public void initialize()
//         {
//             dicSceneCofig = new Dictionary<string, SceneConfig>();
//             try
//             {
//                 XDocument doc = null;
//                 UnityEngine.Object assets = AssetManager.getInstance().loadAsset("Config/ScenceConfig.xml");//同步加载XML
//                 if (assets != null)
//                     doc = XDocument.Parse(assets.ToString());
//                 if (doc == null)
//                     return;
//                 foreach (XElement item in doc.Root.Descendants("Sence")) //得到每一个Sence节点
//                 {
//                     string strName = item.Attribute("name").Value;
//                     SceneConfig config = new SceneConfig();
//                     if (item.Attribute("type") != null)
//                         int.TryParse(item.Attribute("type").Value, out config.nType);
//                     if (item.Attribute("openType") != null)
//                         int.TryParse(item.Attribute("openType").Value, out config.nOpenType);
//                     if (item.Attribute("battleScene") != null)
//                         int.TryParse(item.Attribute("battleScene").Value, out config.battleScene);
//                     if (item.Attribute("version") != null)
//                         int.TryParse(item.Attribute("version").Value, out config.nVersion);
//                     if (item.Attribute("http") != null)
//                         config.sHttp = item.Attribute("http").Value;
//                     dicSceneCofig.Add(strName, config);
//                 }
//             }
//             catch (Exception e)
//             {
//                 Utils.LogSys.LogError(e.Message);
//             }
// 
//         }

        private void ClearCurScene()
        {
            if (pCurScene == null)
                return;

            DestroyImmediate(pCurScene);

            pCurScene = new GameObject("Scene");
            pCurScene.transform.position = Vector3.zero;
            ClearBackSceneList();
        }

        private void ClearBackSceneList()
        {
            for (int i = 0; i < _backSeneList.Count; i++)
            {
                Destroy(_backSeneList[i].backScene);
            }
            _backSeneList.Clear();
        }

        /// <summary>
        /// insertEnd: 为false时, 加到后台列表的最前, 切回后台场景时会优先显示该场景.  为true时反之
        /// needNewScene: true时会新建一个Scene结点, 防止新场景还没加载好, 其他地方取不到Scene报错.
        /// </summary>
        /// <param name="insertEnd"></param>
        private void CurSceneInToBack(bool insertEnd = false, bool needNewScene = true)
        {
            if (pCurScene == null)
                return;

            BackSceneData pBackSceneData = new BackSceneData();
            pBackSceneData.backScene = pCurScene;
            pBackSceneData.backSceneName = sCurSenceName;
            pBackSceneData.backSceneMono = _sceneMono;
            if (insertEnd)
            {
                _backSeneList.Add(pBackSceneData);
            }
            else
            {
                _backSeneList.Insert(0, pBackSceneData);
            }
            pCurScene.transform.name = "BackScene";
            pCurScene.SetActive(false);
            if (needNewScene)
            {
                pCurScene = new GameObject("Scene");
                pCurScene.transform.position = Vector3.zero;
            }

        }



        #region 对外接口
        //切回到后台场景
        public void ChageToBackScene(string sceneName = "")
        {
            if (pCurScene != null)
			{
				//DestroyImmediate(pCurScene);
				//pCurScene = null;
                //SceneManager.UnloadScene(sCurSenceName);
				//Resources.UnloadUnusedAssets();
                CurSceneInToBack(true, false);
            }
            if (_backSeneList.Count == 0)
                return;

            int backSceneIndex = 0;
            if (string.IsNullOrEmpty(sceneName))
            {
                backSceneIndex = 0;//默认第一个
            }
            else
            {
                bool bFound = false;
                for (int i = 0; i < _backSeneList.Count; i++)
                {
                    BackSceneData pBackData = _backSeneList[i];
                    if (pBackData.backSceneName == sceneName)
                    {
                        backSceneIndex = i;
                        bFound = true;
                        break;
                    }

                    if (!bFound)
                    {
                        return;
                    }
                }
            }

            BackSceneData pData = _backSeneList[backSceneIndex];
            pData.backScene.transform.name = "Scene";
            pData.backScene.SetActive(true);
            pCurScene = pData.backScene;
            sCurSenceName = pData.backSceneName;
            _sceneMono = pData.backSceneMono;
            sceneCameraObj = pCurScene.transform.FindChild(("Cameras/SceneCamera")).gameObject;
            _backSeneList.RemoveAt(backSceneIndex);
        }

        public bool IsInBackScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return false;
            }

            for (int i = 0; i < _backSeneList.Count; i++)
            {
                BackSceneData pBackData = _backSeneList[i];
                if (pBackData.backSceneName == sceneName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 隐藏加载中的场景
        /// </summary>
        public void EnActiveLoadingScene()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("scene");
            for (int i = 0; i < gos.Length; i++)
            {
                if (gos[i] != pCurScene)
                {
                    pLoadingScene = gos[i];
                    pLoadingScene.transform.name = "LoadingScene";
                    pLoadingScene.SetActive(false);
                    return;
                }
            }
        }

        /// <summary>
        /// 显示加载中的场景
        /// </summary>
        public void ActiveLoadingScene()
        {
            if (pLoadingScene == null)
                return;

            pLoadingScene.SetActive(true);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="strSceneName"></param>
        public void LoadSceneInBack(string strSceneName)
        {
            if (_taskLoadScene != null)
            {
                Utils.LogSys.LogWarning("A Scene is loading already!! can't to load a new scene!");
                return;
            }

            if (!dicSceneCofig.ContainsKey(strSceneName))
            {
                Utils.LogSys.LogWarning("ScenceConfig.xml not find scence: " + strSceneName);
                return;
            }

            if (sCurSenceName == strSceneName)
                return;

            SceneConfig config = dicSceneCofig[strSceneName];
            bool isAddtive = false;

            switch (config.nOpenType)
            {
                case 1:// 切换场景
                    break;
                case 2:// 附加场景
                    isAddtive = true;
                    break;
                case 3:// 大场景切换（先渐黑，然后切到加载场景，再附加新场景，后删除旧场景）
                    isAddtive = false;
                    break;
                default:
                    break;
            }

            if (config.nType == 1)//本地场景
            {
                _taskLoadScene = new SceneLoadTask(strSceneName, true, isAddtive);
            }
            else if (config.nType == 2)//http场景
            {
                _taskLoadScene = new SceneAssetBundleLoadTask(strSceneName, config.sHttp, config.nVersion, isAddtive);
            }
            else if (config.nType == 3)//assetbundle场景
            {
                _taskLoadScene = new SceneAssetBundleLoadTask(strSceneName, "", 0, isAddtive);
            }
        }

        public bool HasSceneConfig(string strSceneName)
        {
            if (!dicSceneCofig.ContainsKey(strSceneName))
                return false;

            return true;
        }
        public SceneConfig GetSceneConfig(string strSceneName)
        {
            if (!dicSceneCofig.ContainsKey(strSceneName))
                return new SceneConfig();

            return dicSceneCofig[strSceneName];
        }

        /// <summary>
        /// 跳转场景
        /// </summary>
        /// <param name="strSceneName"></param>
        public void ChangeToScene(string strSceneName)
        {
            if (IsInBackScene(strSceneName))
            {
                ChageToBackScene(strSceneName);
            }
            else
            {
                SceneConfig config = dicSceneCofig[strSceneName];
                if (config.nOpenType == 2)
                {
                    ChangeToSceneImpl(strSceneName);
                }
                else
                {
                    LoadingScene.TargetScene = strSceneName;
                    ChangeToSceneImpl(SceneName.s_LoadingScene);
                }
            }
        }

        /// <summary>
        /// 跳到场景
        /// </summary>
        /// <param name="strSceneName"></param>
        public void ChangeToSceneImpl(string strSceneName)
        {
            if (sCurSenceName == strSceneName) return;
            if (!dicSceneCofig.ContainsKey(strSceneName)) return;

            SceneConfig config = dicSceneCofig[strSceneName];
            if (config.nOpenType == 1)// 直接切换场景
            {
                ClearBackSceneList();

                if (_taskLoadScene == null)
                {
                    LoadSceneInBack(strSceneName);//加载
                    _taskLoadScene.ToShowScene();
                }

                //切换完成的回调
                _taskLoadScene.EventFinished += new TaskBase.FinishedHandler(delegate (bool manual, TaskBase currentTask)
                {
                    if (!manual)//非被中断
                    {
                        onLoadComplete(strSceneName, config);
                        OnAddMonoComplete(strSceneName, config);//再加载部件，后渐隐过渡界面
                    }
                });
                //_taskLoadScene.ToShowScene();
            }
            else if (config.nOpenType == 2)// 附加场景（表现：当前场景渐黑后，显示附加的场景）
            {
                if (_taskLoadScene == null)
                    LoadSceneInBack(strSceneName);//加载场景

                FadeInWinController controller = (FadeInWinController)UIManager.GetControler(UIName.FADE_IN_WIN);

                //渐黑后的回调
                DelegateType.FadeInBlackCallback callback = new DelegateType.FadeInBlackCallback(delegate(string sceneName, SceneConfig scene_config)
                {
                    _taskLoadScene.ToShowScene();
                    CurSceneInToBack();
                });

                //附加完成的回调
                _taskLoadScene.EventFinished += new TaskBase.FinishedHandler(delegate (bool manual, TaskBase currentTask)
                {
                    if (!manual)//非被中断
                    {
                        onLoadComplete(strSceneName, config);
                        OnAddMonoComplete(strSceneName, config);//再加载部件，后渐隐过渡界面
                    }
                });

                controller.FadeInBlack(callback);//渐显黑色界面后，触发回调加载场景
            }
            else if (config.nOpenType == 3)// 大场景切换（先渐黑，然后切到加载场景，再附加新场景，后删除加载场景）
            {

                if (_taskLoadScene != null && _taskLoadScene._taskName != strSceneName)//当前不是加载场景的要中断
                {
                    _taskLoadScene.stop();
                    _taskLoadScene = null;
                }

                LoadingWinController controller = (LoadingWinController)UIManager.GetControler(UIName.LOADING_WIN);

                //步骤1：先显示云过滤界面
                if (strSceneName == SceneName.s_LoadingScene)
                    controller.FadeInWin(strSceneName, config, OnDestroyOldScene);//渐显界面后，触发回调
                else
                    OnDestroyOldScene(strSceneName, config);
            }
        }

        //步骤2：删除旧场景
        private void OnDestroyOldScene(string strSceneName, SceneConfig config)
        {
            ClearCurScene();//删旧场景
            config.sceneName = strSceneName;
            //StartCoroutine("OnLoadingSceneBegin", config);
            OnLoadingSceneBegin(config);
        }

        //步骤2-2：加载新场景
        void OnLoadingSceneBegin(SceneConfig config)
        {
           // yield return new WaitForSeconds(0.5f);
            LoadSceneInBack(config.sceneName);//加载新场景
            _taskLoadScene.EventFinished += new TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                if (!manual)//非被中断
                {
                    onLoadComplete(config.sceneName, config);
                    OnAddMonoComplete(config.sceneName, config);
                }
            });
            _taskLoadScene.ToShowScene();
        }

        //步骤3：加载新场景的部件
        private void OnAddMonoComplete(string strSceneName, SceneConfig config)
        {
            if (_sceneMono != null)
            {
                _sceneMono.EventLoadUnitsCompete += new BaseScene.LoadUnitsComplete(delegate ()
                {
                    if (onInitComplete != null)
                        onInitComplete(strSceneName, config);
                    if (config.nOpenType == 2)
                    {
                        Invoke("OnFadeOutWin", 0.2f);
                    }
                    else if (config.nOpenType == 3)
                    {
                        if (strSceneName != SceneName.s_LoadingScene)
                            Invoke("OnFadeOutLoadingWin", 0.5f);
                    }
                });
                _sceneMono.LoadUnits();
            }
            else
            {
                if (onInitComplete != null)
                    onInitComplete(strSceneName, config);
                if (config.nOpenType == 2)
                {
                    Invoke("OnFadeOutWin", 0.2f);
                }
                else if (config.nOpenType == 3)
                {
                    Invoke("OnFadeOutLoadingWin", 0.5f);
                }
            }
        }

        //步骤4：渐隐加载界面
        public void OnFadeOutLoadingWin()
        {
            LoadingWinController controller = (LoadingWinController)UIManager.GetControler(UIName.LOADING_WIN);
            controller.FadeOutWin();
        }

        private void OnFadeOutWin()
        {
            FadeInWinController controller = (FadeInWinController)UIManager.GetControler(UIName.FADE_IN_WIN);
            controller.FadeOutBlack();
        }

        //场景加载完成并初始化
        public void sceneLoadCompleteAndInit(string strSceneName, SceneConfig config)
        {
            _sceneMono = null;
            if (pCurScene != null)
                Destroy(pCurScene);

            _taskLoadScene = null;
            GameObject curScene = GameObject.Find("Scene");
            if (pLoadingScene != null)
            {
                pCurScene = pLoadingScene;
                pCurScene.transform.name = "Scene";
                pLoadingScene = null;
            }
            else
            {
                pCurScene = curScene;
            }


            sCurSenceName = strSceneName;

            if (pCurScene == null)
                return;

            Transform tfWorld = pCurScene.transform.Find("World");
            if (tfWorld != null)
            {
                tfWorld.gameObject.SetActive(true);
            }
            Transform tfCameras = pCurScene.transform.Find("Cameras");
            if (tfCameras != null)
            {
                tfCameras.gameObject.SetActive(true);
            }

            Transform uiCamObj = pCurScene.transform.Find("Cameras/UICamera");
            if (uiCamObj)
            {
                uiCamObj.gameObject.SetActive(false);
            }


            FollowToSceneCamera follower = null;
            GameObject pListener = GameObject.Find("Listener");//声音接收者
            if (pListener == null)
            {
                pListener = new GameObject("Listener");
                pListener.AddComponent<AudioListener>();
                follower = pListener.AddComponent<FollowToSceneCamera>();
                DontDestroyOnLoad(pListener);
            }
            else
            {
                follower = pListener.GetComponent<FollowToSceneCamera>();
            }
            sceneCameraObj = pCurScene.transform.Find("Cameras/SceneCamera").gameObject;
            if (sceneCameraObj && !sceneCameraObj.GetComponent<CameraAjustor>())
            {
                sceneCameraObj.AddComponent<CameraAjustor>();
                follower.camera_tf = sceneCameraObj.transform;

                AudioListener lisener = sceneCameraObj.GetComponent<AudioListener>();
                GameObject.Destroy(lisener);
            }

            //音源
            if (config.battleScene == 1)
            {
                UtilTools.MultiTouchSwitch(true);
               // _sceneMono = pCurScene.AddComponent<BattleScene>() as BaseScene;
            }
            else
            {
                BGM csBGM = pCurScene.AddComponent<BGM>();
                UtilTools.MultiTouchSwitch(false);
                switch (strSceneName)
                {
                    case SceneName.s_StartupScene:
                        _sceneMono = curScene.GetComponent<StartUpScene>() as BaseScene;
                        csBGM.strAudioClipName = "Sounds/BGM/bgm_login_loop";
                        break;
                    case SceneName.s_LoadingScene:
                        _sceneMono = pCurScene.AddComponent<LoadingScene>();
                        break;
                    default:
                        _sceneMono = pCurScene.AddComponent<BaseScene>();
                        break;
                }

//                 if (strSceneName != SceneName.s_StartupScene && strSceneName != SceneName.s_LoadingScene)
//                 {
//                     AllServerBroadcastController controller_temp = (AllServerBroadcastController)UIManager.GetControler(UIName.ALL_SERVER_BROADCAST);
//                     if (controller_temp != null)
//                     {
//                         controller_temp.ContinuePlay();
//                     }
//                 }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////


        #endregion  //对外接口
    }

}

