/***************************************************************


 *
 *
 * Filename:  	AssetManager.cs	
 * Summary: 	游戏资源管理，以asset为管理单位
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/21 16:10
 ***************************************************************/

#region Using
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Utils;
using customerPath;
using task;
using Scene;
#endregion


namespace asset
{
    /// <summary>
    /// 资源管理单例类
    /// </summary>
    [SLua.CustomSingletonLuaClass]
    public class AssetManager : Singleton<AssetManager>
    {
#if UNITY_EDITOR
		private static bool _isFirstUseStreamingAssets = true;//true为优先使用StreamingAssets下资源，false为优先使用Resources下资源
#else
        private static bool _isFirstUseStreamingAssets = true;//true为优先使用StreamingAssets下资源，false为优先使用Resources下资源
#endif
        private static List<string> _lisPreLoadComplete = new List<string>();
        private static string[] _preLoadResources = new string[]{
            UIPrefabPath.ATLAS_COMMON,
            UIPrefabPath.ATLAS_HUNDRED,
            UIPrefabPath.ATLAS_POKER_COM,
            //UIPrefabPath.ATLAS_ICON_MASK,
            //UIPrefabPath.ATLAS_BATTLE,
            //UIPrefabPath.EFFECT_PAO_TAI_1,
            //UIPrefabPath.EFFECT_PAO_TAI_2,
            //UIPrefabPath.EFFECT_PAO_TAI_3,
            //UIPrefabPath.EFFECT_PAO_TAI_4,
            //UIPrefabPath.EFFECT_PAO_TAI_5,
            //UIPrefabPath.EFFECT_PAO_TAI_6,
            //UIPrefabPath.EFFECT_PAO_TAI_7,
            //UIPrefabPath.EFFECT_PAO_TAI_8,
            
//             UIPrefabPath.ATLAS_COMMON
//             ,UIPrefabPath.ATLAS_COMMON_BACK
//             ,UIPrefabPath.ATLAS_COMMON_BACKGROUND
//             ,UIPrefabPath.ATLAS_ICON_EQUIP
//             ,UIPrefabPath.ATLAS_ICON_HERO
//             ,UIPrefabPath.ATLAS_ICON_CONSUME
//             ,UIPrefabPath.ATLAS_ICON_REEL
//             ,UIPrefabPath.ATLAS_ICON_HERO_RANK
        };
        public bool IsFirstUseStreamingAssets
        {
            get { return _isFirstUseStreamingAssets; }
        }
        /// <summary>
        /// 实例化成功回调
        /// </summary>
        /// <param name="obj"></param>
        public delegate void InitiateHandler(GameObject obj);

        /// <summary>
        /// 资源依赖关系数据
        /// </summary>
        ResDependenciesHolder _objDependenciesCfg = null;

        /// <summary>
        /// 路径数据
        /// </summary>
        IPath _objPathData = null;

        /// <summary>
        /// 所有assetbundle的依赖关系
        /// </summary>
        AssetBundleManifest _mainfest;
        /// <summary>
        /// 存储资源Assetbundle，每个资源对应单独的键值
        /// </summary>
        Dictionary<string, Object> _dictAssetBundles = new Dictionary<string, Object>();
        Dictionary<string, int> _dictAssetbundlesRefCount = new Dictionary<string, int>();

        /// <summary>
        /// 获取路径数据
        /// </summary>
        public IPath PathData
        {
            get
            {
                return _objPathData;
            }
        }


        //是否开启回收
//         bool _bOpenClear = false;
// 
//         
//         public bool ClearUnusedAsset
//         {
//             get { return _bOpenClear; }
//             set { _bOpenClear = value; }
//         }

        bool _bIniting = false;

        List<string> _listRegularlyClear = new List<string>();
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private string GetNameFromPath(string path)
        {
            string[] names = path.Split(new char[] { '/' });
            if (names.Length > 0)
            {
                return names[names.Length - 1];
            }

            return path;
        }
        /// <summary>
        /// 初始化Asset模块
        /// </summary>
        public void intialize()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            if (_objPathData == null)
                _objPathData = new StandalonePath();
#elif UNITY_IPHONE
            //_isFirstUseStreamingAssets = true;
            if (_objPathData == null)
                _objPathData = new iOSPath();
#elif UNITY_ANDROID
            //_isFirstUseStreamingAssets = true;
            if (_objPathData == null)
                _objPathData = new AndroidPath();
#endif
            if (_mainfest == null && !_bIniting)
            {
                _bIniting = true;
                //加载StreamingAssets下资源
               // if (_isFirstUseStreamingAssets)
               // {
                    AssetBundleLoadTask task = new AssetBundleLoadTask(IPath.getPlatformName());
                    task.EventFinished += (manual, currentTask) =>
                    {
                        AssetBundle assetObj = ((AssetBundleLoadTask)currentTask).getTargetAssetbundle();
                        if (assetObj != null) //assetObj为窗口的prefab
                        {
                            _mainfest = (AssetBundleManifest)assetObj.LoadAsset("AssetBundleManifest");
                            _bIniting = false;
                            removeAssetbundle(currentTask._taskName);
                        }

                    };
                //}
						
            }

//             GameObject ui_root = GameObject.Find("UIRoot");
//             LoadManifest loader = ui_root.AddComponent<LoadManifest>();
//             loader.assetBundlePath = "file:///" + IPath.streamingAssetsPathPlatform() + "/" + IPath.getPlatformName();// +".manifest";
//             loader.completeCallback = manifestLoadComplete;

        }

        public bool IsInitComplete()
        {
            if (_mainfest == null)
                return false;

            return true;
        }

        public static void PreLoadCommonResources()
        {
			Utils.LogSys.Log("preload asset:-------------------");
            if (IsPreLoadComplete())
            {
                return;
            }
            _lisPreLoadComplete.Clear();
            for (int i = 0; i < _preLoadResources.Length; i++)
            {
                AssetManager.getInstance().loadAssetAsync(_preLoadResources[i], FinishedPreLoadCallback);//多加载一次，使之常驻内存
            }
        }
        public static void FinishedPreLoadCallback(bool manual, TaskBase currentTask)
        {
            _lisPreLoadComplete.Add(currentTask._taskName);
        }
        public static bool IsPreLoadComplete()
        {
            if (_preLoadResources.Length == _lisPreLoadComplete.Count)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取asset的依赖项
        /// </summary>
        /// <param name="strAssetPath">asset在Resources下的相对路径</param>
        /// <returns>返回资源依赖对象</returns>
        public AssetDepRecord getAssetDependencies(string strAssetPath)
        {
            if (_objDependenciesCfg != null)
            {
                return _objDependenciesCfg.getAssetDependencies(strAssetPath);
            }

            return null;
        }

        public string[] getAssetBundleDependencies(string strAssetPath)
        {
            if (_mainfest != null)
            {
                return _mainfest.GetAllDependencies(strAssetPath);
            }
            return new string[] { };
        }

        /// <summary>
        /// 获取材质的shader名字
        /// </summary>
        /// <param name="strMat"></param>
        /// <returns></returns>
        public string getMatsShaderName(string strMat)
        {
            if (_objDependenciesCfg != null)
            {
                return _objDependenciesCfg.getMatsShaderName(strMat);
            }

            return "Diffuse";
        }
        
        /// <summary>
        /// 同步加载指定类型的asset
        /// needCheckPath: true时会根据_isFirstUseStreamingAssets判断是否要转成assetbundle下的目录
        /// </summary>
        public T loadAsset<T>(string strPath, bool needCheckPath = true) where T : Object
        {
            string asset_name = GetNameFromPath(strPath);
            strPath = strPath.ToLower();
            strPath = strPath.Replace("\\", "/");
            if (AssetManager.getInstance().IsStreamingAssets(strPath))
            {
                if (needCheckPath)
                    strPath = UtilTools.PathCheck(strPath);
            }
            Object obj = null;
            if (_dictAssetBundles.ContainsKey(strPath))//判断缓存中是否有
            {
                AssetBundle bundle = _dictAssetBundles[strPath] as AssetBundle;
                if (bundle != null)
                {
                    obj = bundle.LoadAsset<Object>(asset_name);
                    //引用计数+1
                    //_dictAssetbundlesRefCount[strPath]++;
                }
                else
                {
                    obj = _dictAssetBundles[strPath];
                    //引用计数+1
                    //_dictAssetbundlesRefCount[strPath]++;
                }
            }
            else//没有再去同步加载
            {

                Utils.LogSys.Log("AssetManager Get Path Data 2");
                string strLatesVersionpath = _objPathData.getLatestVersionPath(strPath);
                EAssetPathType eType = _objPathData.getAssetPathType(strPath);
                LogSys.Log("[AssetManager.LoadAesst]:+"+strLatesVersionpath+"  eType = "+eType);
                if (eType == EAssetPathType.ePersistent)
                {
                    if (File.Exists(strLatesVersionpath))
                    {
                        string[] objDepRec = AssetManager.getInstance().getAssetBundleDependencies(strPath);
                        for (int i = 0; i < objDepRec.Length; i++)
                        {
                            loadAsset<Object>(objDepRec[i], false);//依赖的资源不需要再检测路径,否则会出导致路径错误
                        }

                        AssetBundle assetbundle = AssetBundle.LoadFromFile(strLatesVersionpath);
                        if (assetbundle != null)
                        {
                            obj = assetbundle.LoadAsset<Object>(asset_name);
                            _dictAssetBundles[strPath] = assetbundle;
                            //引用计数初始化
                            if (!_dictAssetbundlesRefCount.ContainsKey(strPath))
                                _dictAssetbundlesRefCount[strPath] = 0;
                        }

//                         byte[] bytes = File.ReadAllBytes(strLatesVersionpath);
//                         if (bytes != null && bytes.Length != 0)
//                         {
//                             AssetBundle assetbundle = AssetBundle.LoadFromMemory(bytes);
//                             if (assetbundle != null)
//                             {
//                                 obj = assetbundle.LoadAsset<Object>(asset_name);
//                                 _dictAssetBundles[strPath] = assetbundle;
//                                 //引用计数初始化
//                                 if (!_dictAssetbundlesRefCount.ContainsKey(strPath))
//                                     _dictAssetbundlesRefCount[strPath] = 0;
//                             }
//                         }
                    }
                }
                else if (eType == EAssetPathType.eStreamingAssets)
                {
                    JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
                    if (tools != null)
                    {
                        string[] objDepRec = AssetManager.getInstance().getAssetBundleDependencies(strPath);
                        for (int i=0; i< objDepRec.Length; i++)
                        {
                            loadAsset<Object>(objDepRec[i], false);//依赖的资源不需要再检测路径,否则会出导致路径错误
                        }
                        string path = IPath.getPlatformName() + "/" + strPath;
                        string stream_asset_path = IPath.getPlatformName() + "/" + strPath;
#if UNITY_EDITOR
                        stream_asset_path = strLatesVersionpath;
#endif

                        AssetBundle assetbundle = AssetBundle.LoadFromFile(strLatesVersionpath);
                        if (assetbundle != null)
                        {
                            obj = assetbundle.LoadAsset<Object>(asset_name);
                            _dictAssetBundles[strPath] = assetbundle;
                            //引用计数初始化
                            if (!_dictAssetbundlesRefCount.ContainsKey(strPath))
                                _dictAssetbundlesRefCount[strPath] = 0;
                        }
//                         byte[] bytes = tools.GetAssetBundleBytes(stream_asset_path);
//                         if (bytes != null && bytes.Length != 0)
//                         {
//                             AssetBundle assetbundle = AssetBundle.LoadFromMemory(bytes);
//                             if (assetbundle != null)
//                             {
//                                 obj = assetbundle.LoadAsset<Object>(asset_name);
//                                 _dictAssetBundles[strPath] = assetbundle;
//                                 //引用计数初始化
//                                 if (!_dictAssetbundlesRefCount.ContainsKey(strPath))
//                                     _dictAssetbundlesRefCount[strPath] = 0;
//                             }
//                         }
                    }
                }
                else// (eType == EAssetPathType.eResources || (eType == EAssetPathType.eNone))
                {
                    obj = Resources.Load(strLatesVersionpath);
                    //addAssetBundle(strPath, obj);
                }
            }

            return (T)obj;
        }

        /// <summary>
        /// 同步加载资源
        /// needCheckPath: true时会根据_isFirstUseStreamingAssets判断是否要转成assetbundle下的目录
        /// </summary>
        public Object loadAsset(string strPath, bool needCheckPath = true)
        {
            return loadAsset<Object>(strPath, needCheckPath);
        }

        /// <summary>
        /// 同步加载assetbundle下的prefab资源
        /// needCheckPath: true时会根据_isFirstUseStreamingAssets判断是否要转成assetbundle下的目录
        /// </summary>
        public GameObject loadPrefab(string strPath, bool needCheckPath = true)
        {
            strPath = UtilTools.PrefabPathCheck(strPath);
            GameObject go = loadAsset<GameObject>(strPath, needCheckPath);
            if (go == null)
            {
                string asset_name = GetNameFromPath(strPath);
                go = new GameObject(asset_name);
            }
            return go;
        }
        /// <summary>
        /// 同步加载assetbundle下的Texure资源
        /// needCheckPath: true时会根据_isFirstUseStreamingAssets判断是否要转成assetbundle下的目录
        /// </summary>
        public Texture loadTexture(string strPath, bool needCheckPath = true)
        {
            strPath = UtilTools.PngPathCheck(strPath);
            Texture go = loadAsset<Texture>(strPath, needCheckPath);
            if (go == null)
            {
                string asset_name = GetNameFromPath(strPath);
                go = new Texture();
            }
            return go;
        }

        /// <summary>
        /// 同步加载XML
        /// </summary>
        public Object loadXML(string strPath)
        {
            Object xmlObj = null;
            strPath = strPath.Replace("\\", "/");

            Utils.LogSys.Log("AssetManager Get Path Data 3: " + strPath);
            string strLatesVersionpath = _objPathData.getLatestVersionPath(strPath);
            xmlObj = Resources.Load(strLatesVersionpath);
            if (xmlObj == null)
                Utils.LogSys.LogError("load xml file failed!! path is: " + strLatesVersionpath + "(" + strPath + ")");

            return xmlObj;
        }

        public bool IsStreamingAssets(string strAssetPath)
        {
            if (AssetManager.getInstance().IsFirstUseStreamingAssets)
            {
                string pre_str = "";
                if (strAssetPath.Length > 10)
                    pre_str = strAssetPath.Substring(0, 10);
                if (pre_str.Equals("Resources/") || pre_str.Equals("resources/"))
                {
                    Utils.LogSys.Log("AssetManager Get Path Data 4_1:" + strAssetPath);
                    EAssetPathType eType = _objPathData.getAssetPathType(strAssetPath);
                    if (eType == EAssetPathType.ePersistent || eType == EAssetPathType.eStreamingAssets)
                    {
                        return true;
                    }
                }
                else
                {
                    Utils.LogSys.Log("AssetManager Get Path Data 4:" + strAssetPath);
                    EAssetPathType eType = _objPathData.getAssetPathType("resources/" + strAssetPath);
                    if (eType == EAssetPathType.ePersistent || eType == EAssetPathType.eStreamingAssets)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 异步加载资源
        /// handlerFinish：结束回调
        /// </summary>
        public void loadAssetAsync(string strAssetPath, task.TaskBase.FinishedHandler handlerFinish = null)
        {
            if (!isAssetbundleLoaded(strAssetPath))
            {
                if (AssetManager.getInstance().IsStreamingAssets(strAssetPath))
                {
                    //异步创建StreamingAssets下资源
                    strAssetPath = UtilTools.PathCheck(strAssetPath);
                    AssetBundleLoadTask task = new AssetBundleLoadTask(strAssetPath, null);
                    if (handlerFinish != null)
                    {
                        task.EventFinished += handlerFinish;
                    }
                }
                else
                {
                    AssetLoadTask task = new AssetLoadTask(strAssetPath, null);
                    if (handlerFinish != null)
                    {
                        task.EventFinished += handlerFinish;
                    }
                }
            }
            else
            {
                if (AssetManager.getInstance().IsStreamingAssets(strAssetPath))
                {
                    strAssetPath = UtilTools.PathCheck(strAssetPath);
                }
                AssetManager.getInstance().addAssetbundleRefCount(strAssetPath);
                StartCoroutine("loadAssetAsyncCallback", handlerFinish);
            }
        }

        IEnumerator loadAssetAsyncCallback(object argsObj)
        {
            yield return new WaitForSeconds(0.1f);
            if (argsObj != null)
            {
                task.TaskBase.FinishedHandler handlerFinish = (task.TaskBase.FinishedHandler)argsObj;
                handlerFinish(true, null);
            }
        }

        /// <summary>
        /// 添加assetbundle对象
        /// </summary>
        public void addAssetBundle(string strPath, Object obj)
        {
            if (obj == null)
                return;

            if (isAssetbundleLoaded(strPath))
            {
                addAssetbundleRefCount(strPath);
            }
            else
            {
                //添加并引用计数初始化
                _dictAssetBundles[strPath] = obj;
                _dictAssetbundlesRefCount[strPath] = 1;
            }
        }

        /// <summary>
        /// 判断assetbundle是否已经加载
        /// </summary>
        public bool isAssetbundleLoaded(string strPath)
        {
            
            strPath = strPath.ToLower();
            strPath = strPath.Replace("\\", "/");
            if (_dictAssetBundles.ContainsKey(strPath))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取缓存中已经加载的asset(必需是之前加载好的）
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public Object getAsset(string strPathFile, bool needCheckPath = true)
        {
            string strPath = strPathFile.ToLower();
            strPath = strPath.Replace("\\", "/");
            if (AssetManager.getInstance().IsStreamingAssets(strPath))
            {
                if (needCheckPath)
                    strPath = UtilTools.PathCheck(strPath);
                if (_dictAssetBundles.ContainsKey(strPath))
                {
                    AssetBundle assetObj = _dictAssetBundles[strPath] as AssetBundle;
                    if (assetObj != null)//如果是assetBundle取asset
                    {
                        //                     Object target = assetObj.mainAsset;
                        //                     if (target != null)
                        //                         return target;
                        return assetObj.LoadAsset(GetNameFromPath(strPath));//
                    }
                    return _dictAssetBundles[strPath];//否则直接返回asset;
                }
            }
            return loadAsset(strPathFile, needCheckPath);
        }

		/// <summary>
		/// 获取缓存中已经加载的assetbundle
		/// </summary>
		public Object getAssetBundle(string strPath)
        {
            strPath = strPath.ToLower();
			strPath = strPath.Replace("\\", "/");
			if (_dictAssetBundles.ContainsKey(strPath))
			{
				AssetBundle assetObj = _dictAssetBundles[strPath] as AssetBundle;
				if (assetObj != null)//如果是assetBundle取asset
				{
					return assetObj;
				}
			}
			
			return null;
		}

        /// <summary>
        /// 增加assetbundle的引用计数
        /// </summary>
        public void addAssetbundleRefCount(string strPath, int nCount = 1)
        {
            strPath = strPath.Replace("\\", "/").ToLower();
            if (_dictAssetBundles.ContainsKey(strPath))
            {
                _dictAssetbundlesRefCount[strPath] += nCount;
            }
        }

        /// <summary>
        /// 减少assetbundle的引用计数
        /// </summary>
        public void minusAssetbundleRefCount(string strPath, int nCount = 1)
        {
            strPath = strPath.Replace("\\", "/").ToLower();
            if (_dictAssetBundles.ContainsKey(strPath))
            {
                int nRef = _dictAssetbundlesRefCount[strPath] - nCount;
                _dictAssetbundlesRefCount[strPath] = nRef;
                if (nRef <= 0)
                {
                    //removeAssetbundle(strPath);
                }
            }
        }

        /// <summary>
        /// 移除assetbundle
        /// </summary>
        public void removeAssetbundle(string strPath)
        {
            //if (!_bOpenClear) return;

            strPath = strPath.Replace("\\", "/");
            if (_dictAssetBundles.ContainsKey(strPath))
            {
                AssetBundle bundle = _dictAssetBundles[strPath] as AssetBundle;
                _dictAssetBundles.Remove(strPath);
                _dictAssetbundlesRefCount.Remove(strPath);
                if (bundle != null)
                {
                    bundle.Unload(false);
                }
                else
                {
                    //Resources.UnloadUnusedAssets();
                }
            }
        }

        /// <summary>
        /// 移除没用的Asset(Resources)
        /// </summary>
        public void UnloadUnusedResourcesAssets()
        {
            //if (!_bOpenClear) return;

            Resources.UnloadUnusedAssets();
        }


        /// <summary>
        /// 实例化1
        /// appendedAssets：指定额外的特殊依赖项
        /// </summary>
        public void Instantiate(string strAssetPath, InitiateHandler handlerFinish, string[] appendedAssets = null)
        {
            Instantiate(strAssetPath, Vector3.zero, Quaternion.identity, handlerFinish, appendedAssets);
        }

        /// <summary>
        /// 实例化2
        /// appendedAssets：指定额外的特殊依赖项
        /// </summary>
        public void Instantiate(string strAssetPath, Vector3 position, Quaternion rotation, InitiateHandler handlerFinish, string[] appendedAssets = null)
        {
            //string strLatesVersionpath = _objPathData.getLatestVersionPath(strPath);

            Utils.LogSys.Log("AssetManager Get Path Data 5:" + strAssetPath);
            EAssetPathType eType = _objPathData.getAssetPathType(strAssetPath);
            if (eType == EAssetPathType.eResources)//异步创建Resource下资源
            {
                AssetLoadTask task = new AssetLoadTask(strAssetPath, appendedAssets);
                task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
                {
                    Object assetObj = ((AssetLoadTask)currentTask).getTargetAsset();
                    if (assetObj != null)
                    {
                        GameObject obj = GameObject.Instantiate(assetObj, position, rotation) as GameObject;
                        ((AssetLoadTask)currentTask).unloadUnusedAssetbundle(false);

                        handlerFinish(obj);
                    }
                });
            }
            else
            {
                //异步创建StreamingAssets下资源
                AssetBundleLoadTask task = new AssetBundleLoadTask(strAssetPath, appendedAssets);
                task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
                {
                    Object assetObj = ((AssetBundleLoadTask)currentTask).getTargetAsset();
                    if (assetObj != null)
                    {
                        GameObject obj = GameObject.Instantiate(assetObj, position, rotation) as GameObject;
                        ((AssetBundleLoadTask)currentTask).unloadUnusedAssetbundle(false);

                        handlerFinish(obj);
                    }
                });
            }
        }


        /// <summary>
        /// 定时清理函数
        /// </summary>
        public void regularlyClearAssets()
        {
            //if (!_bOpenClear) return;

            Utils.LogSys.Log("【警告】一大波垃圾回收即将开始!!!!!");

            _listRegularlyClear.Clear();
            foreach (var kvp in _dictAssetbundlesRefCount)
            {
                if (kvp.Value <= 0)
                    _listRegularlyClear.Add(kvp.Key);
            }

            //删除
            foreach (var str in _listRegularlyClear)
                removeAssetbundle(str);

            _listRegularlyClear.Clear();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            Utils.LogSys.Log("【警告】垃圾回完成，体验卡卡的!!!!!");
        }

#if UNITY_ANDROID

        /// <summary>
        /// 检查可用内存并清理
        /// </summary>
        void checkAndroidAvailableRAMAndClear()
        {
            JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
            if (tools != null)
            {
                long sysAvailableMemMB = tools.getAvailableMemoryKB() / 1024;
                if (sysAvailableMemMB < 100)
                {
                    regularlyClearAssets();
                }
            }
        }

#endif

        #region MONOBEHAVIOUR

        void Awake()
        {
            //intialize();

#if UNITY_EDITOR
            //InvokeRepeating("regularlyClearAssets", 15, 6);//回收时体验卡卡的， 不做定时回收了， 改成定点回收
#elif UNITY_ANDROID
            InvokeRepeating("checkAndroidAvailableRAMAndClear", 15, 10);
#endif
        }

        #endregion //MONOBEHAVIOUR

        public void ClearAll()
        {
            _objPathData = null;
            _mainfest = null;
            //删除
            _listRegularlyClear.Clear();
            foreach (var kvp in _dictAssetbundlesRefCount)
            {
                _listRegularlyClear.Add(kvp.Key);
            }
            //删除
            foreach (var str in _listRegularlyClear)
                removeAssetbundle(str);
            _listRegularlyClear.Clear();

            _lisPreLoadComplete.Clear();
            _dictAssetBundles.Clear();
            Resources.UnloadUnusedAssets();
            _dictAssetbundlesRefCount.Clear();

        }

    }
         
}



