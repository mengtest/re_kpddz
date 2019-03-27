/***************************************************************


 *
 *
 * Filename:  	AssetBundleLoadTask.cs	
 * Summary: 	AssetBundle加载任务，负责加载指定的资源及其依赖资源
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/06/03 13:51
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
    public class AssetBundleLoadTask : TaskBase
    {
        /// <summary>
        /// 任务名
        /// </summary>
        string _strTaskName = "";

        /// <summary>
        /// 存储资源Assetbundle或者asset，每个资源对应单独的键值
        /// </summary>
        Dictionary<string, Object> _dictAssetBundles = new Dictionary<string, Object>();
        List<AssetBundleLoadTaskItem> _listTaskItem = new List<AssetBundleLoadTaskItem>();

        /// <summary>
        /// 等待加载的资源列表
        /// </summary>
        List<string> _listAssetsPathWaitingLoad = new List<string>();

        /// <summary>
        /// 完成列表
        /// </summary>
        List<string> _listAssetsPathComplete = new List<string>();


        /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取任务名字
        /// </summary>
        public string AssetBundleTaskName
        {
            get { return _strTaskName; }
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
        public AssetBundleLoadTask(string strAssetPath, string[] append = null, bool isAutoStart = true) :
            base(false)//AssetBundleLoadTask是不参与下载的，所以用false
        {
            _taskName = strAssetPath;

            strAssetPath = strAssetPath.ToLower();
            _strTaskName = strAssetPath;// GetNameFromPath(strAssetPath);
//             string[] objDepRec = AssetManager.getInstance().getAssetBundleDependencies(strAssetPath);
//             if (objDepRec != null)
//             {
//                 for (int i = 0; i < objDepRec.Length; i++ )
//                 {
//                     if (objDepRec[i] != strAssetPath)
//                         _listAssetsPathWaitingLoad.Add(objDepRec[i]);
//                 }
// 
//                 if (append != null)
//                 {
//                     for (int i = 0; i < append.Length; i++)
//                     {
//                         if (!_listAssetsPathWaitingLoad.Contains(append[i].ToLower()))
//                             _listAssetsPathWaitingLoad.Add(append[i].ToLower());
//                     }
//                }
//                 _listAssetsPathWaitingLoad.Add(strAssetPath);
//             }
//             else
//             {
//                 _listAssetsPathWaitingLoad.Add(strAssetPath);
//             }
            CollectAllDependFiles(strAssetPath, append);

            for (int i = 0; i < _listAssetsPathWaitingLoad.Count; i++)
            {
                AssetBundleLoadTaskItem loadtask = new AssetBundleLoadTaskItem(_listAssetsPathWaitingLoad[i], isAutoStart);
                loadtask.EventFinished += FinishedHandlerCallback;
                _listTaskItem.Add( loadtask);
            }
        }

        void CollectAllDependFiles(string strAssetPath, string[] append)
        {
            string[] objDepRec = AssetManager.getInstance().getAssetBundleDependencies(strAssetPath);
            if (objDepRec != null)
            {
                for (int i = 0; i < objDepRec.Length; i++)
                {
                    if (objDepRec[i] != strAssetPath)
                        CollectAllDependFiles(objDepRec[i], null);
                }

                if (append != null)
                {
                    for (int i = 0; i < append.Length; i++)
                    {
                        if (append[i].ToLower() != strAssetPath)
                            CollectAllDependFiles(append[i].ToLower(), null);
                    }
                }
            }
            if (!_listAssetsPathWaitingLoad.Contains(strAssetPath))
                _listAssetsPathWaitingLoad.Add(strAssetPath);

        }

        public void FinishedHandlerCallback(bool manual, TaskBase currentTask)
        {
            _listAssetsPathComplete.Add(currentTask._taskName);
            if (_listTaskItem.Count == _listAssetsPathComplete.Count)
            {
                stop();
                finishExec();
            }
        }
        /// <summary>
        /// 获取目标资源Asset对象
        /// </summary>
        /// <returns>返回ASSET(Unity.Object)</returns>
        public Object getTargetAsset()
        {
            AssetBundle assetbundle = getTargetAssetbundle();
            if (assetbundle == null)
                return null;
//             Object target = assetbundle.mainAsset;
//             if (target != null)
//                 return target;
            string fileName = GetNameFromPath(_strTaskName);
            Utils.LogSys.Log("getTargetAsset:>>>>>>>>>>>   " + fileName);
            return assetbundle.LoadAsset(fileName);
        }

        /// <summary>
        /// 获取原始的assetbundle
        /// 需要使用者自己加载assetbundle中的资源
        /// </summary>
        public override AssetBundle getTargetAssetbundle()
        {
            //先从任务列表中找
            for (int i = 0; i < _listTaskItem.Count; i++)
            {
                if (_listTaskItem[i]._taskName == _strTaskName)
                {
                    return _listTaskItem[i].getTargetAssetbundle();
                }
            }
            //再从缓存列表中找
            Object obj = AssetManager.getInstance().getAssetBundle(_strTaskName);
            if (obj != null)
                return (AssetBundle)obj;

            return null;
        }
    
        /// <summary>
        /// 卸载无用的assetbundle
        /// </summary>
        /// <param name="bValue"></param>
        public override void unloadUnusedAssetbundle(bool bValue)
        {
            foreach (var item in _listTaskItem)
            {
                item.unloadUnusedAssetbundle(true);
            }

            _listTaskItem.Clear();
        }

        #region TaskBase

        /// <summary>
        /// 任务功能执行
        /// </summary>
        /// <returns>枚举器</returns>
//         public override IEnumerator taskExec()
//         {
// 
//             yield return null;
//             //执行任务
//             for (int i = 0; i < _listTaskItem.Count; i++)
//             {
//                 yield return _listTaskItem[i].e;
//                 //执行单个任务的结束回调
//                 //_listTaskItem[i].finishExec();
// 
//                 _listAssetsPathComplete.Add(_listTaskItem[i]._taskName);
//             }
//         }
        
        /// <summary>
        /// 获取加载进度
        /// </summary>
        /// <returns>进度(0.0~1.0)</returns>
        public override float getProgress()
        {
            float fCompleteCount = _listAssetsPathComplete.Count;
            float fTotalCount = _listAssetsPathWaitingLoad.Count;
            
            return fCompleteCount / fTotalCount;
        }

        #endregion

        public bool IsCurLoadingAssetPath(ref TaskBase except_task, string sAssetPath)
        {
            if (sAssetPath.Length == 0)
                return false;
            //先从任务列表中找
            for (int i = 0; i < _listTaskItem.Count; i++)
            {
                if (except_task != _listTaskItem[i] && _listTaskItem[i]._taskName == sAssetPath && !_listTaskItem[i].IsLoadComplete())
                {
                    return _listTaskItem[i].IsLoading();
                }
            }

            return false;
        }
        public bool GetLoadingAssetPathTask(out AssetBundleLoadTaskItem taskLoading, string sAssetPath)
        {
            if (sAssetPath.Length == 0)
            {
                taskLoading = null;
                return false;
            }
            //先从任务列表中找
            for (int i = 0; i < _listTaskItem.Count; i++)
            {
                if (_listTaskItem[i]._taskName == sAssetPath)
                {
                    taskLoading = _listTaskItem[i] as AssetBundleLoadTaskItem;
                    return true;
                }
            }
            taskLoading = null;
            return false;
        }
    }
}