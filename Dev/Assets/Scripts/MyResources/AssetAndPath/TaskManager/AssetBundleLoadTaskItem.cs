/***************************************************************


 *
 *
 * Filename:  	AssetBundleLoadTaskItem.cs	
 * Summary: 	AssetBundle加载任务，负责加载单个assetbundle
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2016/02/15 13:51
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using asset;
using task;
using customerPath;
#endregion

namespace task
{
    /// <summary>
    /// 资源加载任务
    /// </summary>
    public class AssetBundleLoadTaskItem : TaskBase
    {
        /// <summary>
        /// 任务名
        /// </summary>
        //string _strTaskName = "";

        /// <summary>
        /// 存储资源Assetbundle或者asset，每个资源对应单独的键值
        /// </summary>
        Object _assetBundle = null;

        /// <summary>
        /// 是否正在加载
        /// </summary>
        bool _bLoading = false;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        bool _bLoaded = false;
        /// <summary>
        /// 是否正在加载
        /// </summary>
        bool _bWaiting = false;
        /// <summary>
        /// 依赖部件列表
        /// </summary>
        //List<AssetBundleLoadTaskItem> _listTaskItem = new List<AssetBundleLoadTaskItem>();
        WWW w = null;
        /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取任务名字
        /// </summary>
        public string AssetBundleTaskName
        {
            get { return _taskName; }
        }

        private string GetNameFromPath(string path)
        {
            string[] names = path.Split(new char[]{'/'});
            if (names.Length > 0)
            {
                return names[names.Length-1];
            }

            return path;
        }

        /// <summary>
        /// 创建资源加载任务
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public AssetBundleLoadTaskItem(string strAssetPath, bool isAutoStart = true) :
            base(isAutoStart)
        {
            _taskName = strAssetPath.ToLower();
        }

        /// <summary>
        /// 获取目标资源Asset对象
        /// </summary>
        /// <returns>返回ASSET(Unity.Object)</returns>
        public Object getTargetAsset()
        {
 //           return _dictAssets[_strTaskName];
//             string fileName = GetNameFromPath(_strTaskName);
//             return ((AssetBundle)_dictAssetBundles[_strTaskName]).LoadAsset(fileName);

            AssetBundle assetbundle = getTargetAssetbundle();
            if (assetbundle == null)
                return null;
            Object target = assetbundle.mainAsset;
            if (target != null)
                return target;
            string fileName = GetNameFromPath(_taskName);
            return assetbundle.LoadAsset(fileName);
        }

        /// <summary>
        /// 获取原始的assetbundle
        /// 需要使用者自己加载assetbundle中的资源
        /// </summary>
        public override AssetBundle getTargetAssetbundle()
        {
            if (_assetBundle == null)
            {
                Object obj = AssetManager.getInstance().getAssetBundle(_taskName);
                if (obj != null) return (AssetBundle)obj;

                return null;
            }

            return (AssetBundle)_assetBundle;
        }
    
        /// <summary>
        /// 卸载无用的assetbundle
        /// </summary>
        /// <param name="bValue"></param>
        public override void unloadUnusedAssetbundle(bool bValue)
        {
            AssetManager.getInstance().minusAssetbundleRefCount(_taskName);
        }

        #region TaskBase

        /// <summary>
        /// 任务功能执行
        /// </summary>
        /// <returns>枚举器</returns>
        public override IEnumerator taskExec()
        {
            yield return null;
            string strPath = _taskName;
            //将要加载的对象
            AssetBundle objWillLoad = null;
            AssetBundleLoadTaskItem otherLoadingTask;
            bool isLoaded = AssetManager.getInstance().isAssetbundleLoaded(strPath);
            if (!isLoaded)
            {
                TaskManager.getInstance().OtherTaskLoadingAssetPath(out otherLoadingTask, strPath);
                while (otherLoadingTask != null && otherLoadingTask != this)
                {
                    _bWaiting = true;
                    if (otherLoadingTask.IsLoadComplete())
                    {
                        otherLoadingTask = null;
                        isLoaded = true;
                    }
                    yield return null;
                }
            }
            _bWaiting = false;
            //已经加载则直接使用
            if (isLoaded)
            {
                Object ab = AssetManager.getInstance().getAssetBundle(strPath);
                if (ab != null)
                {
                    objWillLoad = (AssetBundle)ab;
                    _assetBundle = ab;
                    AssetManager.getInstance().addAssetbundleRefCount(strPath);
                }
                _bLoaded = true;
            }
            else
            {
                _bLoading = true;
                string strURL = AssetManager.getInstance().PathData.urlForWWW(strPath);
                EAssetPathType eType = AssetManager.getInstance().PathData.getAssetPathType(strPath);
                if (eType != EAssetPathType.eNone)
                {
                    if (strPath.Contains(".shader"))
                    {
                        Utils.LogSys.Log("---------->shader:" + strPath);
                    }
                    w = new WWW(strURL);
                    yield return w;
                    if (w.isDone)
                    {
                        objWillLoad = w.assetBundle;
                    }
                    if (objWillLoad != null)
                    {
                        _assetBundle = (Object)objWillLoad;
                        AssetManager.getInstance().addAssetBundle(strPath, (Object)objWillLoad);
                        if (strPath.IndexOf(".unity3d") > 0)
                        {
                            string[] names = objWillLoad.GetAllAssetNames();
                            for (int i = 0; i < names.Length; i++)
                            {
                                Utils.LogSys.Log("---------->asset:" + names[i]);
                            }
                        }
                    }
                }
                _bLoading = false;
                _bLoaded = true;

            }
        }

        /// <summary>
        /// 获取加载进度
        /// </summary>
        /// <returns>进度(0.0~1.0)</returns>
        public override float getProgress()
        {
            if (w == null)
            {
                return 0f;
            }
            return w.progress;
        }

        #endregion

        public bool IsLoadComplete()
        {
            return _bLoaded;
        }
        public bool IsLoading()
        {
            return _bLoading;
        }
        public bool IsWaiting()
        {
            return _bWaiting;
        }
    }
}