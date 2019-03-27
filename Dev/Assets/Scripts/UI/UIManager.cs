/***************************************************************


 *
 *
 * Filename:  	UIManager.cs	
 * Summary: 	UI管理类：初始化所有UI的controller，提供统一的界面创建销毁接口
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/12 19:22
 ***************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Scene;
using EventManager;
using MyExtensionMethod;
using System;
using System.Linq;
using sluaAux;

namespace UI.Controller
{
   	[SLua.CustomLuaClass]
    public enum UILevel
    {
        BACKGROUND = 0,//depth：-1000~-1
        NORMAL = 1,//depth:0~99999
        HIGHT = 2,//depth:100000~199999
        TOP = 3,//depth:200000~
        TOP_HIGHT = 4,//depth:300000~不到最后时刻勿用，切记切记
        TOP_TOP = 5,//depth:400000~不到最后时刻勿用，切记切记
    }

    [SLua.CustomLuaClass]
    public class UIDepth
    {
        public const int BACKGROUND = -100;//-1000;
        public const int NORMAL = 0;
        public const int HIGHT = 100;//100000;
        public const int TOP = 200;//200000;
        public const int TOP_HIGHT = 300;//300000;不到最后时刻勿用，切记切记
        public const int TOP_TOP = 400;//400000;不到最后时刻勿用，切记切记
    }

    [SLua.CustomLuaClass]
    public class UIManager
    {
        public delegate void onLuaFuncEventHandle(GameObject go);

        // lua创建界面与删除界面的回调
        static Dictionary<string, onLuaFuncEventHandle> _luaOnCreateFuncDict = new Dictionary<string, onLuaFuncEventHandle>();
        static Dictionary<string, onLuaFuncEventHandle> _luaRenderResetFuncDict = new Dictionary<string, onLuaFuncEventHandle>();
        static Dictionary<string, onLuaFuncEventHandle> _luaOnDestroyFuncDict = new Dictionary<string, onLuaFuncEventHandle>();
        static Dictionary<string, onLuaFuncEventHandle> _luaFuncCallDict = new Dictionary<string, onLuaFuncEventHandle>();

        static readonly UIManager instance = new UIManager();//默认为private，不需要显式声明为private  
        public static UIManager Instance
        {
            get {
                return instance;
            }
        }

        private static int topDepth = 0;//记录当前最高UI的depth
        public static int TopDepth
        {
            get {
                return topDepth;
            }
        }

        private static List<string> showingWindows = new List<string>();//记录显示的窗口name
        //记录显示的窗口id,返回管理器分配的depth
        public static void RecordShowingWin(string uiName, UILevel eLev)
        {
            if (!showingWindows.Contains(uiName)) {
                showingWindows.Add(uiName);
            }

            AddCurDepth(eLev);
            ControllerBase controller = GetControler(uiName);
            if (controller._needSceneBlur) {
                AutoSceneBlur();
            }
        }

        public static void UnRecordShowingWin(string uiName, UILevel eLev)
        {
            if (showingWindows.Contains(uiName)) {
                showingWindows.Remove(uiName);
                CleanCurDepth(eLev);
            }

            ControllerBase controller = GetControler(uiName);
            controller.ClearShowingScrollView();
            if (controller._needSceneBlur) {
                AutoSceneBlur();
            }
        }

        //设置窗口要显示的层级
        public static void SetWinRenderQueue(GameObject winObject, UILevel eLev, int windDepth)
        {
            UIPanel uiPanel = winObject.GetComponent<UIPanel>();
            uiPanel.depth = windDepth;
            uiPanel.renderQueue = UIPanel.RenderQueue.StartAt;
            int dep = Mathf.Abs(windDepth) % 100;
            if (eLev == UILevel.TOP_TOP) {
                uiPanel.startingRenderQueue = 6000 + dep * 50;
                Vector3 point = winObject.transform.localPosition;
                winObject.transform.localPosition = new Vector3(point.x, point.y, -9.4f);
            }
            else if (eLev == UILevel.TOP_HIGHT)
            {
                uiPanel.startingRenderQueue = 5000 + dep * 50;
                Vector3 point = winObject.transform.localPosition;
                winObject.transform.localPosition = new Vector3(point.x, point.y, -9.2f);
            }
            else if (eLev == UILevel.TOP)
            {
                uiPanel.startingRenderQueue = 4000 + dep * 50;
                Vector3 point = winObject.transform.localPosition;
                winObject.transform.localPosition = new Vector3(point.x, point.y, -9f);
            }
            else if (eLev == UILevel.HIGHT)
            {
                uiPanel.startingRenderQueue = 3000 + dep * 50;
                Vector3 point = winObject.transform.localPosition;
                winObject.transform.localPosition = new Vector3(point.x, point.y, -5f);
            }
            else if (eLev == UILevel.NORMAL)
            {
                uiPanel.startingRenderQueue = 2000 + dep * 50;
                Vector3 point = winObject.transform.localPosition;
                winObject.transform.localPosition = new Vector3(point.x, point.y, 5f);
            }
            else if (eLev == UILevel.BACKGROUND)
            {
                dep = Mathf.Abs(windDepth + 1000) % 100;
                uiPanel.startingRenderQueue = 1000 + dep * 50;
                Vector3 point = winObject.transform.localPosition;
                winObject.transform.localPosition = new Vector3(point.x, point.y, 9f);
            }
        }

        public static void SetScrollViewRenderQueue(ControllerBase controller, GameObject ScrollViewObject, UILevel eLev, int windDepth)
        {
            UIPanel uiPanel = ScrollViewObject.GetComponent<UIPanel>();
            uiPanel.depth = windDepth + 1;
            uiPanel.renderQueue = UIPanel.RenderQueue.StartAt;
            int dep = Mathf.Abs(windDepth) % 100;
            if (eLev == UILevel.TOP_TOP)
            {
                uiPanel.startingRenderQueue = 6000 + dep * 50 + 40;
            }
            else if (eLev == UILevel.TOP_HIGHT)
            {
                uiPanel.startingRenderQueue = 5000 + dep * 50 + 40;
            }
            else if (eLev == UILevel.TOP) {
                uiPanel.startingRenderQueue = 4000 + dep * 50 + 40;
                //Vector3 point = ScrollViewObject.transform.localPosition;
                //ScrollViewObject.transform.localPosition = new Vector3(point.x, point.y, -5.1f);
            } else if (eLev == UILevel.HIGHT) {
                uiPanel.startingRenderQueue = 3000 + dep * 50 + 40;
                //Vector3 point = ScrollViewObject.transform.localPosition;
                //ScrollViewObject.transform.localPosition = new Vector3(point.x, point.y, -5.1f);
            } else if (eLev == UILevel.NORMAL) {
                uiPanel.startingRenderQueue = 2000 + dep * 50 + 40;
                //Vector3 point = ScrollViewObject.transform.localPosition;
                //ScrollViewObject.transform.localPosition = new Vector3(point.x, point.y, 4.9f);
            } else if (eLev == UILevel.BACKGROUND) {
                dep = Mathf.Abs(windDepth + 1000) % 100;
                uiPanel.startingRenderQueue = 1000 + dep * 50 + 40;
                //Vector3 point = ScrollViewObject.transform.localPosition;
                //ScrollViewObject.transform.localPosition = new Vector3(point.x, point.y, 4.9f);
            }
            controller.AddShowingScrollView(ScrollViewObject);
        }

        /// <summary>
        /// 重置所有界面的renderQueue(关闭界面时不管，打开一个界面时才重置)
        /// </summary>
        public static void autoResetAllRenderQueue()
        {
            curDepth = new Dictionary<UILevel, int>()
            {
                {UILevel.BACKGROUND, UIDepth.BACKGROUND},
                {UILevel.NORMAL, UIDepth.NORMAL},
                {UILevel.HIGHT, UIDepth.HIGHT},
                {UILevel.TOP, UIDepth.TOP},
                {UILevel.TOP_HIGHT, UIDepth.TOP_HIGHT},
                {UILevel.TOP_TOP, UIDepth.TOP_TOP},
            };
            foreach (string uiName in showingWindows) {
                ControllerBase contro = GetControler(uiName);
                int nDepth = AddCurDepth(contro.ELevel);
                if (contro.windDepth != nDepth)//depth发生改变时，才重置renderQueue
                {
                    contro.SetWinRenderQueue(nDepth);
                    contro.autoResetScrollViewRenderQueue();
                    contro.ChangeWindowRenderQueue();
                }
            }

        }

        public static void changeToTopInUILevel(string sWindowName)
        {
            bool bFind = false;
            for (int i = 0; i < showingWindows.Count; i++) {
                if (showingWindows[i] == sWindowName) {
                    showingWindows.Remove(sWindowName);
                    bFind = true;
                    break;
                }
            }
            if (bFind) {
                showingWindows.Add(sWindowName);
                autoResetAllRenderQueue();
            }
        }

        //记录当前层级的最高depth
        private static Dictionary<UILevel, int> curDepth = new Dictionary<UILevel, int>()
        {
            {UILevel.BACKGROUND, UIDepth.BACKGROUND},
            {UILevel.NORMAL, UIDepth.NORMAL},
            {UILevel.HIGHT, UIDepth.HIGHT},
            {UILevel.TOP, UIDepth.TOP},
            {UILevel.TOP_HIGHT, UIDepth.TOP_HIGHT},
            {UILevel.TOP_TOP, UIDepth.TOP_TOP},
        };

        //当某个层有界面显示时，depth就+1，保证后打开的界面在上显示
        public static int AddCurDepth(UILevel eLev)
        {
            if (eLev == UILevel.BACKGROUND || eLev == UILevel.NORMAL || eLev == UILevel.HIGHT || eLev == UILevel.TOP || eLev == UILevel.TOP_HIGHT || eLev == UILevel.TOP_HIGHT)
                curDepth[eLev] = curDepth[eLev] + 1;

            return curDepth[eLev] * 2;
        }

        //当某个层有界面显示时，depth就+1，保证后打开的界面在上显示
        public static int GetNextDepth(UILevel eLev)
        {
            return curDepth[eLev] * 2 + 2;//不是+1，因为要给scrollView留一个
        }

        //当某个层没有界面时，depth就重置
        public static void CleanCurDepth(UILevel eLev)
        {
            foreach (string uiName in showingWindows) {
                ControllerBase contro = GetControler(uiName);
                if (contro.ELevel == eLev) {
                    return;
                }
            }

            if (eLev == UILevel.BACKGROUND)
                curDepth[eLev] = UIDepth.BACKGROUND;
            else if (eLev == UILevel.NORMAL)
                curDepth[eLev] = UIDepth.NORMAL;
            else if (eLev == UILevel.HIGHT)
                curDepth[eLev] = UIDepth.HIGHT;
            else if (eLev == UILevel.TOP)
                curDepth[eLev] = UIDepth.TOP;
            else if (eLev == UILevel.TOP_HIGHT)
                curDepth[eLev] = UIDepth.TOP_HIGHT;
            else if (eLev == UILevel.TOP_TOP)
                curDepth[eLev] = UIDepth.TOP_TOP;
        }

        private static Dictionary<string, ControllerBase> _components;
        /// <summary>
        /// ctrl-mono 名称索引
        /// </summary>
        private static Dictionary<string, string> CtrlMonoDic;
        static UIManager()//加入静态构造函数，是为了控制类的初始化时机
        {
            InitAllControllers();

            //// 一定要放在 InitAllControllers 后,为 GetControler<T>() 接口获取 Controler 生成索引
            CtrlMonoDic = _components.ToDictionary(n => n.Value.ToString(), m => m.Key);
        }
        public static void ReInit()
        {
            InitAllControllers();
            //// 一定要放在 InitAllControllers 后,为 GetControler<T>() 接口获取 Controler 生成索引
            CtrlMonoDic = _components.ToDictionary(n => n.Value.ToString(), m => m.Key);
        }

        public static void InitAllControllers()
        {
            if (_components != null)
                _components.Clear();

            _components = new Dictionary<string, ControllerBase>();
            _components.Add(UIName.TIPS, new TipsController(UIName.TIPS));
            _components.Add(UIName.WAITING, new WaitingController(UIName.WAITING));
            _components.Add(UIName.MESSAGE_DIALOG, new MessageDialogController(UIName.MESSAGE_DIALOG));
            _components.Add(UIName.MESSAGE_WIN, new MessageWinController(UIName.MESSAGE_WIN));
            _components.Add(UIName.BACKGROUND_BLACK, new BackgroundBlackController(UIName.BACKGROUND_BLACK));
            _components.Add(UIName.PM_WIN, new PMController(UIName.PM_WIN));
            _components.Add(UIName.LOADING_WIN, new LoadingWinController(UIName.LOADING_WIN));
            _components.Add(UIName.FADE_IN_WIN, new FadeInWinController(UIName.FADE_IN_WIN));
            _components.Add(UIName.MESSAGE_DIALOG_USE_MONEY, new MessageDialogUseMoneyController(UIName.MESSAGE_DIALOG_USE_MONEY));
            _components.Add(UIName.SCREENSHOT_WIN, new ScreenshotController(UIName.SCREENSHOT_WIN));
            _components.Add(UIName.MESSAGE_WITH_ICON_WIN, new MessageWithIconController(UIName.MESSAGE_WITH_ICON_WIN));
            _components.Add(UIName.SYSTEM_TIPS_WIN, new SystemTipsWinController(UIName.SYSTEM_TIPS_WIN));
            //队列载入界面
            _components.Add(UIName.QUEUE_LOGIN_WIN,new QueueLoginController(UIName.QUEUE_LOGIN_WIN));
            _components.Add(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT, new MessageDialogWithTwoSelectController(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT));

            _components.Add(UIName.RELIFE_WIN, new RelifeController(UIName.RELIFE_WIN));
            _components.Add(UIName.BROADCAST_WIN, new AllServerBroadcastController(UIName.BROADCAST_WIN));
//            _components.Add(UIName.EXCHANGE_WIN, new ExchangeController(UIName.EXCHANGE_WIN));

            _components.Add(UIName.SOUND_WIN, new SoundController(UIName.SOUND_WIN));

            _components.Add(UIName.LOGIN_INPUT_WIN,new LoginInputController(UIName.LOGIN_INPUT_WIN));
            _components.Add(UIName.REGISTER_BINDING_WIN,new RegisterBindingController(UIName.REGISTER_BINDING_WIN));
            _components.Add(UIName.PI_CHANGE_PASSWORD,new PIChangePasswordController(UIName.PI_CHANGE_PASSWORD));
            _components.Add(UIName.SHOP_RECHARGE_OTHER_WIN,new ShopRechargeOtherController(UIName.SHOP_RECHARGE_OTHER_WIN));
            _components.Add(UIName.BARRAGE_WIN, new BarrageController(UIName.BARRAGE_WIN));

        }


        public static ControllerBase GetControler(string name)
        {
            if (_components == null) {
                Utils.LogSys.LogError("编辑器代码已修改，需重新运行游戏");
            }
            if (!_components.ContainsKey(name)) {             
                return null;
            }
            return _components[name];
        }

        /// <summary>
        /// 获取 Controler
        /// </summary>
        /// <typeparam name="T">要获取的值的键。</typeparam>
        /// <exception cref="T:System.ArgumentNullException"><typeparamref name="T"/> 对应的 Mono 不存在。</exception>
        /// <returns></returns>
        public static T GetControler<T>() where T : ControllerBase
        {
            if (_components == null) {
                Utils.LogSys.LogError("编辑器代码已修改，需重新运行游戏");
            }

            string name;
            var ctrl = typeof(T).FullName;
            if (CtrlMonoDic.TryGetValue(ctrl, out name)) {
                return (T)_components.GetValue(name);
            }

            throw new Exception("UIManager.GetControler Not Find" + ctrl);
        }

        public static bool CreateLuaWin(string name, bool byAction = false, EventMultiArgs actionArgs = null)
        {
            ControllerBase contro = GetControler(name);
            if (contro == null)
            {
                contro = new LuaWinController(name);
                _components.Add(name, contro);
            }
            return CreateWin(name, byAction, actionArgs);
        }

        public static bool CreateWin(string name, bool byAction = false, EventMultiArgs actionArgs = null)
        {
            ControllerBase contro = GetControler(name);
            if (contro == null) {
                Utils.LogSys.LogError("CreateWin Error: ‘" + name + "’cannot find controller");
                return false;
            }
            contro._createByActionArgs = actionArgs;
            autoResetAllRenderQueue();
            UILevel eLevel = contro.ELevel;
            int newWinDepth = GetNextDepth(eLevel);
            bool res = contro.CreateWin(newWinDepth, byAction);
            if (res)
                RecordShowingWin(name, eLevel);

#if UNITY_EDITOR
            Utils.LogSys.Log("CreateWin:" + name + ", depth=" + curDepth[eLevel].ToString());
#endif
            return res;
        }

        public static void DestroyWin(string name, bool byAction = false, EventMultiArgs actionArgs = null)
        {
            ControllerBase contro = GetControler(name);
            UILevel eLevel = contro.ELevel;
            contro._destroyByActionArgs = actionArgs;
            if (contro._needSceneHide || contro._needSceneBlur)
            {
                AutoSceneHide(name);
            }
            if (contro.DestroyWin(byAction)) {
                UnRecordShowingWin(name, eLevel);
                contro.UnloadUnusedResources();
                //AssetManager.getInstance().UnloadUnusedResourcesAssets();
            }
        }
        public static bool CreateWinByAction(string name, EventMultiArgs actionArgs = null)
        {
            return CreateWin(name, true, actionArgs);
        }
        public static void DestroyWinByAction(string name, EventMultiArgs actionArgs = null)
        {
            DestroyWin(name, true, actionArgs);
        }

        public static void RemoveAllWinExpect()
        {
            List<string> toRemoveWindows = new List<string>(showingWindows.ToArray());//记录显示的窗口id

            foreach (string uiName in toRemoveWindows) {
                DestroyWin(uiName);
            }
        }
        public static void RemoveAllWinExpect(string expectName)
        {
            List<string> toRemoveWindows = new List<string>(showingWindows.ToArray());//记录显示的窗口id

            foreach (string uiName in toRemoveWindows) {
                if (uiName != expectName) {
                    DestroyWin(uiName);
                }
            }
        }
        public static void RemoveAllWinExpect(string[] expectNames)
        {
            List<string> toRemoveWindows = new List<string>(showingWindows.ToArray());//记录显示的窗口id
            foreach (string uiName in toRemoveWindows) {
                bool isExpect = false;
                foreach (string name in expectNames) {
                    if (name == uiName) {
                        isExpect = true;
                        break;
                    }
                }
                if (!isExpect)
                    DestroyWin(uiName);
            }
        }

        //自动判断是否要开启场景模糊效果
        public static void AutoSceneBlur(string expect_name = "")
        {
            //打开镜头模糊

            BaseScene baseScene = GameSceneManager.getInstance().SceneMono;
            if (baseScene == null)
                return;
            int length = showingWindows.Count;
//             for (int i = 0; i < length; i++) {
// 
//                 if (expect_name != showingWindows[i])
//                 {
//                     ControllerBase contro = GetControler(showingWindows[i]);
//                     if (contro._needSceneHide)
//                     {
//                         return;
//                     }
//                 }
//             }
            for (int i = 0; i < length; i++)
            {
                if (expect_name != showingWindows[i])
                {
                    ControllerBase contro = GetControler(showingWindows[i]);
                    if (contro._needSceneBlur)
                    {
                        //打开镜头模糊
                        baseScene.BlurOpen();
                        UtilTools.SetFPS(FPSLevel.OnlyUI);
                        return;
                    }
                }
            }
            //关闭镜头模糊
            baseScene.BlurClose(); 
            UtilTools.SetFPS(FPSLevel.Normal);
            UtilTools.HideScreenshot();
        }

        //自动判断是否要隐藏场景镜头
        public static void AutoSceneHide(string expect_name = "")
        {
            BaseScene baseScene = GameSceneManager.getInstance().SceneMono;
            if (baseScene == null)
                return;
            int length = showingWindows.Count;
            for (int i = 0; i < length; i++)
            {
                if (expect_name != showingWindows[i])
                {
                    ControllerBase contro = GetControler(showingWindows[i]);
                    if (contro._needSceneHide)
                    {
                        GameSceneManager.sceneCameraObj.SetActive(false);
                        UtilTools.SetFPS(FPSLevel.OnlyUI);
                        return;
                    }
                }
            }
            for (int i = 0; i < length; i++)
            {
                if (expect_name != showingWindows[i])
                {
                    ControllerBase contro = GetControler(showingWindows[i]);
                    if (contro._needSceneBlur)
                    {
                        return;
                    }
                }
            }
            UtilTools.SetFPS(FPSLevel.Normal);
            GameSceneManager.sceneCameraObj.SetActive(true);
            UtilTools.HideScreenshot();
        }

        //窗口是否打开（包括正在打开中）
        public static bool IsWinShow(string sUIName)
        {
            var controller = GetControler(sUIName);
            return controller != null && controller.IsShow;
        }

        public static bool isWinLevelGuideUnLess(string sName)
        {
            bool result = false;
            switch (sName)
            {
                case "LevelGuide":
                case "DetialTipsWin":
                case "MessageWin":
                case "ChatWin":
                case "PlayerInfoWin":
                case "RechargeWin":
                case "GetAwardWin":
                case "FastBuyWin":
                    result = true;
                    break;
            }

            return result;
        }

        public static void RegisterLuaWinFunc(string winName, onLuaFuncEventHandle onCreate, onLuaFuncEventHandle onDestroy)
        {
            if (!_luaOnCreateFuncDict.ContainsKey(winName))
            {
                _luaOnCreateFuncDict.Add(winName, onCreate);
            }

            if (!_luaOnDestroyFuncDict.ContainsKey(winName)) 
            {
                _luaOnDestroyFuncDict.Add(winName, onDestroy);
            }   
        }

        public static void RegisterLuaWinRenderFunc(string winName, onLuaFuncEventHandle onRenderReset)
        {
            if (!_luaRenderResetFuncDict.ContainsKey(winName)){
                _luaRenderResetFuncDict.Add(winName,onRenderReset);
            }
        }

        public static void CallLuaWinRenderFunc(string winName, GameObject go)
        {
            if (_luaRenderResetFuncDict.ContainsKey(winName)){
                _luaRenderResetFuncDict[winName](go);
            }
        }

        public static void CallLuaWinOnCreateFunc(string winName, GameObject go)
        {
            if (_luaOnCreateFuncDict.ContainsKey(winName))
            {
                _luaOnCreateFuncDict[winName](go);
            }
        }

        public static void CallLuaWinOnDestoryFunc(string winName, GameObject go)
        {
            if (_luaOnDestroyFuncDict.ContainsKey(winName))
            {
                _luaOnDestroyFuncDict[winName](go);
            }
        }

        public static void RegisterLuaFuncCall(string eventName, onLuaFuncEventHandle funcCall)
        {
            if (!_luaFuncCallDict.ContainsKey(eventName))
            {
                _luaFuncCallDict.Add(eventName, funcCall);
            }
        }

        public static void CallLuaFuncCall(string eventName, GameObject go)
        {
            if (_luaFuncCallDict.ContainsKey(eventName))
            {
                _luaFuncCallDict[eventName](go);
            }
        }
    }
}
