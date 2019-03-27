/***************************************************************


 *
 *
 * Filename:  	ControllerBase.cs	
 * Summary: 	controller基类：创建和销毁UI
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/12 19:22
 ***************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Scene;
using EventManager;
using asset;
using task;

namespace UI.Controller
{
    /// <summary>
    /// load UI resource, save UI base data.
    /// </summary>
    [SLua.CustomLuaClass]
    public class ControllerBase : IController {

		public class AtlasType
		{
			public const string HERO_HEAD = "heroHead";
			public const string EQUIP_ICON = "equipIcon";
			public const string SKILL_ICON = "skillIcon";
			public const string AWARD_ICON = "awardIcon";//奖励图集，包括很多

			public static List<string> heroHeadAtlas = new List<string>() { 
               // UIPrefabPath.ATLAS_ICON_HERO 
            };
			public static List<string> equipIconAtlas = new List<string>()  {
                //UIPrefabPath.ATLAS_ICON_EQUIP
            };
			public static List<string> skillIconAtlas = new List<string>() { 
                //UIPrefabPath.ATLAS_ICON_SKILL1,
                //UIPrefabPath.ATLAS_ICON_SKILL2
            };
			public static List<string> awardIconAtlas = new List<string>() { 
//                 UIPrefabPath.ATLAS_ICON_EQUIP,
// 				UIPrefabPath.ATLAS_ICON_HERO,
// 				UIPrefabPath.ATLAS_ICON_CONSUME,
// 				UIPrefabPath.ATLAS_ICON_REEL
            };
			public static List<string> allIconAtlas = new List<string>{ //所有图标集的名字
														"SkillAtlas"                        
														,"SkillAtlas2"           //图集 技能
														,"ObjAtlas"              //图集 装备
														,"HeroAtlas"             //图集 英雄
														,"PropAtlas"             //图集 消耗
														,"HeroItemAtlas"         //图集 将器
														,"HeroSpecialEquipAtlas" //图集 专属
			};
		}

		// window prefabs path.
		protected UILevel eLev = UILevel.NORMAL;
		protected string sName;
        protected string[] prefabsPath;
        protected string[] uitexturesPath;//界面一打开就要显示的uitexure(且prefab里没有进行引用的)
		protected Vector3 _vector3 = new Vector3(0, 0, 0);
		// mono
		//protected TaskBase _loadResTask;
        protected List<TaskBase> _loadResTaskList = new List<TaskBase>();
        protected List<string> _loadCompleteList = new List<string>();
		protected UIEventCachePool eventCachePool = new UIEventCachePool();
		protected string _resourceSign;
		protected bool _isLoading = false;
		protected bool _isShow = false;
        public GameObject winObject;
        public int windDepth = 1;
		public bool _needSceneBlur = false;//打开该界面时，场景镜头是否要模糊效果
		public bool _needSceneHide = false;//打开该界面时，场景镜头是否要隐藏
        public bool _bAddToScene = false;//加到场景中

        private List<GameObject> showingScrollView = new List<GameObject>();//记录该界面的scrollView,管理层级时用到
		private bool isCreateByAction = false;
		public EventMultiArgs _createByActionArgs = null;
		public EventMultiArgs _destroyByActionArgs = null;
        /// <summary>
        /// 是否关闭了界面
        /// </summary>
        public bool IsClose { get; private set; }

        public void AddShowingScrollView(GameObject scrollView)
		{

			for (int i = 0; i < showingScrollView.Count; i++)
			{
				if (scrollView == showingScrollView[i])
					return;//防止重复加
			}
			showingScrollView.Add(scrollView);
		}

		public void ClearShowingScrollView()
		{
			showingScrollView.Clear();
		}
		public void autoResetScrollViewRenderQueue()
		{
			for (int i = 0; i < showingScrollView.Count; i++ )
			{
				SetScrollViewRenderQueue(showingScrollView[i]);
			}
		}

		/// <summary>
		/// 是调用CreateWin之后就为true,因为异步加载的原因，可能界面还没加载完，还没显示，要过几帧才会显示
		/// </summary>
		public bool IsShow
		{
			get
			{
				return _isShow;
			}
		}

		/// <summary>
		/// 界面已真正显示出来
		/// </summary>
		public bool IsActive
		{
			get
			{
				if (winObject == null)
					return false;
				if (winObject.activeInHierarchy == false)
					return false;

				return true;
			}
		}

        /// <summary>
        /// 获得界面名称
        /// </summary>
		public string getWinName()
        {
            return sName;
        }


		public ControllerBase()
		{
            _resourceSign = ExporterPlan.WindowControllerPath;
		}

		virtual protected void UILoadCallback() { }
		virtual protected void UICreateCallback(){}
        virtual protected void UIDestroyCallback() { }
        virtual public void ChangeWindowRenderQueue() { }

		public UILevel ELevel
		{
			get {
				return eLev;
			}
			set
			{
				eLev = value;
			}
		}

		//show win
		public bool CreateWin(int iDepth, bool byAction)
		{
		    IsClose = false;
			if (_isLoading || _isShow)
				return false;

			windDepth = iDepth;
			_isLoading = true;
			_isShow = true;
			isCreateByAction = byAction;
			ClearAllCacheEvent();
            _loadResTaskList.Clear();
            _loadCompleteList.Clear();
			string[] appendPrefabs = GetAppendAtlasPrefabs();
            if (appendPrefabs.Length > 0)
            {
                for (int i = 0; i < appendPrefabs.Length; i++)
                {
                    if (AssetManager.getInstance().IsStreamingAssets(appendPrefabs[i]))
                    {
                        //转为assetbundle路径
                        appendPrefabs[i] = UtilTools.PathCheck(appendPrefabs[i]);
                        //加载StreamingAssets下资源
                        AssetBundleLoadTask task = new AssetBundleLoadTask(appendPrefabs[i]);
                        _loadResTaskList.Add(task);
                        task.EventFinished += (manual, currentTask) =>
                        {
                            LoadAppendResourceComplete(currentTask._taskName);
                        };
                    }
                    else
                    {
                        //加载Resources下资源
                        AssetLoadTask task = new AssetLoadTask(appendPrefabs[i]);
                        _loadResTaskList.Add(task);
                        task.EventFinished += (manual, currentTask) =>
                        {
                            LoadAppendResourceComplete(currentTask._taskName);
                        };
                    }
                }
            }
            else
            {
                LoadAppendResourceComplete("");
            }
			UILoadCallback();
			return true;
		}

        public void LoadAppendResourceComplete(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                _loadCompleteList.Add(path);
            }
            if (_loadCompleteList.Count >= _loadResTaskList.Count)//依赖资源加载完后加载界面
            {
                if (AssetManager.getInstance().IsStreamingAssets(prefabsPath[0]))
                {
                    //转为assetbundle路径
                    prefabsPath[0] = UtilTools.PathCheck(prefabsPath[0]);
                    //加载StreamingAssets下资源
                    AssetBundleLoadTask task = new AssetBundleLoadTask(prefabsPath[0]);
                    _loadResTaskList.Add(task);
                    task.EventFinished += (manual, currentTask) =>
                    {
                        Object assetObj = ((AssetBundleLoadTask)currentTask).getTargetAsset();
                        if (assetObj != null) //assetObj为窗口的prefab
                        {
                            LoadResourceComplete(assetObj);
                            //AssetManager.getInstance().removeAssetbundle(prefabsPath[0]);
                        }
                    };
                }
                else
                {
                    //加载Resources下资源
                    AssetLoadTask task = new AssetLoadTask(prefabsPath[0]);
                    _loadResTaskList.Add(task);
                    task.EventFinished += (manual, currentTask) =>
                    {
                        Object assetObj = ((AssetLoadTask)currentTask).getTargetAsset();
                        if (assetObj != null) //assetObj为窗口的prefab
                        {
                            LoadResourceComplete(assetObj);
                            assetObj = null;
                            //AssetManager.getInstance().removeAssetbundle(prefabsPath[0]);
                        }
                    };
                }
            }
        }

		//hide win
		public bool DestroyWin(bool byAction)
		{
		    IsClose = true;
			if (!_isShow) 
				return false;

			//UtilTools.PlaySoundEffect("Sounds/UISound/click_01");
			if (byAction)
			{
				CallUIEvent(UIEventID.DESTROY_WIN_ACTION, _destroyByActionArgs);
				return false;
			}
			_isShow = false;
            if (winObject != null)
            {
                GameObject.Destroy(winObject);
            }

			winObject = null;
			UIDestroyCallback();
            UIManager.CallLuaFuncCall(sName + ":" + "UIDestroyCallback", winObject);

			UnRegisterAllUIEvent();
            if (_loadResTaskList != null && _loadResTaskList.Count > 0)
            {
                for (int i=0; i < _loadResTaskList.Count; i++)//依赖资源加载完后加载界面
                {
                    _loadResTaskList[i].unloadUnusedAssetbundle(false);
                }
                _loadResTaskList.Clear();
            }

			return true;
		}

	    /// <summary>
		/// 解释prefabsPath里的内容，获取依赖的prefabs
		/// </summary>
		/// <returns></returns>
		private string[] GetAppendAtlasPrefabs()
		{
			int k = 0;
			List<string> atlas_prefabs = new List<string>();
			for (int i = 1; i < prefabsPath.Length; i++)
			{
				switch (prefabsPath[i])
				{
					case AtlasType.HERO_HEAD:
						for (k = 0; k < AtlasType.heroHeadAtlas.Count; k++)
						{
							if (!atlas_prefabs.Contains(AtlasType.heroHeadAtlas[k]))
							{
								atlas_prefabs.Add(AtlasType.heroHeadAtlas[k]);
							}
						}
						break;
					case AtlasType.EQUIP_ICON:
						for (k = 0; k < AtlasType.equipIconAtlas.Count; k++)
						{
							if (!atlas_prefabs.Contains(AtlasType.equipIconAtlas[k]))
							{
								atlas_prefabs.Add(AtlasType.equipIconAtlas[k]);
							}
						}
						break;
					case AtlasType.SKILL_ICON:
						for (k = 0; k < AtlasType.skillIconAtlas.Count; k++)
						{
							if (!atlas_prefabs.Contains(AtlasType.skillIconAtlas[k]))
							{
								atlas_prefabs.Add(AtlasType.skillIconAtlas[k]);
							}
						}
						break;
					case AtlasType.AWARD_ICON:
						for (k = 0; k < AtlasType.awardIconAtlas.Count; k++)
						{
							if (!atlas_prefabs.Contains(AtlasType.awardIconAtlas[k]))
							{
								atlas_prefabs.Add(AtlasType.awardIconAtlas[k]);
							}
						}
						break;
					default:
						if (!atlas_prefabs.Contains(prefabsPath[i]))
						{
							atlas_prefabs.Add(prefabsPath[i]);
						}
						
						break;
				}
			}
            if (uitexturesPath != null && uitexturesPath.Length > 0)
            {
                for (k = 0; k < uitexturesPath.Length; k++)
                {
                    if (!atlas_prefabs.Contains(uitexturesPath[k]))
                    {
                        atlas_prefabs.Add(uitexturesPath[k]);
                    }
                }
            }
			return atlas_prefabs.ToArray();
		}

		//load prefab callback
		private void LoadResourceComplete(Object assetObj)
		{
			_isLoading = false;
	//		if (_waitType == 1) _waitWin.Dispose(_windowPath[0]);
			if(_isShow)
			{
				//winObject = (GameObject)GameObject.Instantiate(MyResourceManager.GetInstance().GetResource(prefabsPath[0]), _vector3, Quaternion.identity);
				winObject = GameObject.Instantiate(assetObj, Vector3.zero, Quaternion.identity) as GameObject;
                if (_bAddToScene)
                {
                    GameObject pUIRootObj = GameObject.Find("Scene/World");
                    Transform parent = pUIRootObj.transform;
                    if (parent)
                    {
                        winObject.transform.parent = parent;
                    }
                }
                else
                {
                    GameObject pUIRootObj = GameObject.Find("UIRoot");
                    if (pUIRootObj == null)
                    {
                        pUIRootObj = new GameObject("UIRoot");
                        pUIRootObj.transform.position = Vector3.zero;
                        Object.DontDestroyOnLoad(pUIRootObj);
                    }
                    Transform parent = pUIRootObj.transform;
                    if (parent)
                    {
                        winObject.transform.parent = parent;
                    }
                }
				//UIManager.SetWinRenderQueue(winObject, eLev, windDepth);//调整界面层级
				SetWinRenderQueue(windDepth);//调整界面层级

				//SelfAdaption();
				winObject.AddComponent<UIAdjustor>();
				UICreateCallback();
				if (isCreateByAction)
					CallUIEvent(UIEventID.CREATE_WIN_ACTION, _createByActionArgs);

                UIManager.CallLuaFuncCall(sName + ":" + "UICreateCallback", winObject);

				isCreateByAction = false;
				CallAllCacheEvent();
				if (_needSceneHide)
				{
					UIManager.AutoSceneHide();
				}
	//			if (this as MainControler == null && this as MainRoleControler == null && this as BattleChangeController == null && MyScene.GetInstance().NowBattleScene == null)
	//			{
	//				++WindowManager.GetInstance().CannotDrag;
	//			}
			}
		}

		//adapte win size
		private void SelfAdaption()
		{
	//		if (this as MainScene2DController != null) return;
			return;
			//if ((float)Screen.width / Screen.height < 1.5f)
			//{
			//	float rate = (float)Screen.height / Screen.width * 3 / 2;
			//	UIRoot uiRoot = winObject.GetComponent<UIRoot>();
			//	if (uiRoot != null)
			//		uiRoot.manualHeight = (int)(rate * uiRoot.manualHeight);
			//}
		}

		public void SetWinRenderQueue(int nDepth)
		{
			if (winObject == null)
				return;

			windDepth = nDepth;
			UIManager.SetWinRenderQueue(winObject, eLev, windDepth);
		}
		public void SetScrollViewRenderQueue(GameObject scrollViewObject)
		{
			if (scrollViewObject == null)
				return;
			
			UIManager.SetScrollViewRenderQueue(this, scrollViewObject, eLev, windDepth);
		}

        public int GetScrollViewRenderQueueOffset(GameObject scrollViewObject)
        {
            if (winObject == null)
                return 0;
            UIPanel tgPanel = winObject.GetComponent<UIPanel>();
            if (tgPanel == null) return 0;
            int scrollViewRenderQueue = tgPanel.startingRenderQueue;
            UIPanel uiPanel = scrollViewObject.GetComponent<UIPanel>();
            if (uiPanel != null) {
                scrollViewRenderQueue = uiPanel.startingRenderQueue;
            }
            return scrollViewRenderQueue - tgPanel.startingRenderQueue;
	    }
		public void SetUIPanelRenderQueue(GameObject targetUIPanel, int offset)
		{
			if (winObject == null)
				return;
			UIPanel tgPanel = targetUIPanel.GetComponent<UIPanel>();
			if ( tgPanel == null) return;
			UIPanel uiPanel = winObject.GetComponent<UIPanel>();
			if ( tgPanel == null) return;
			tgPanel.renderQueue = UIPanel.RenderQueue.StartAt;
			tgPanel.startingRenderQueue = uiPanel.startingRenderQueue + offset;

		}


		/// <summary>
		/// UI的Mono里必须用这个注册事件
		/// </summary>
		/// <param name="eventID">事件id</param>
		/// <param name="de">响应函数</param>
		public void RegisterUIEvent(short eventID, DelegateType.UIEventCallback de)
		{
			eventCachePool.RegisterUIEvent(eventID, de);
		}

		/// <summary>
		/// 清空注册的事件
		/// </summary>
		public void UnRegisterAllUIEvent()
		{
			eventCachePool.UnRegisterAllUIEvent();
		}

		/// <summary>
		/// 清空注册的事件
		/// </summary>
		public void UnRegisterUIEvent(short eventID, DelegateType.UIEventCallback de)
		{
			eventCachePool.UnRegisterUIEvent(eventID, de);
		}

		/// <summary>
		/// 执行UI事件
		/// </summary>
		/// <param name="eventID">事件id</param>
		/// <param name="de">响应函数</param>
		public void CallUIEvent(short eventID, EventMultiArgs args)
		{
			if (args == null)
				args = new EventMultiArgs();
			args.AddArg("UI_EVENT_ID", eventID);
			if (!winObject)
			{
				eventCachePool.SaveUIEvent(args);
			}
			else
			{
				eventCachePool.RunUIEvent(args);
			}
		}

		/// <summary>
		/// 执行所有缓存的事件
		/// </summary>
		public void CallAllCacheEvent()
		{
			eventCachePool.CallAllCacheEvent();
		}

		/// <summary>
		/// 清空所有缓存的事件
		/// </summary>
		public void ClearAllCacheEvent()
		{
			eventCachePool.ClearAllCacheEvent();
		}

		public void UnloadUnusedResources()
		{ 
			
		}
	}

}