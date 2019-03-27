/***************************************************************


 *
 *
 * Filename:  	AssetLoadTask.cs	
 * Summary: 	Resources下的资源加载任务，使用异步加载
 *
 * Version:   	1.0.0
 * Author: 		XueMG
 * Date:   		2015/06/03 13:51
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using asset;
#endregion

namespace task
{
    /// <summary>
    /// 资源加载任务
    /// </summary>
    public class AssetLoadTask : TaskBase
    {
        /// <summary>
        /// 任务名
        /// </summary>
        string _strTaskName = "";

        /// <summary>
        /// 存储资源Assetbundle，每个资源对应单独的键值
        /// </summary>
        Dictionary<string, Object> _dictAssetBundles = new Dictionary<string, Object>();

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
        public string AssetTaskName
        {
            get { return _strTaskName; }
        }

        /// <summary>
        /// 创建资源加载任务
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public AssetLoadTask(string strAssetPath, string[] append = null, bool isAutoStart = true) :
            base(isAutoStart)
        {
            //Utils.LogSys.Log("to load:"+strAssetPath);
            _taskName = strAssetPath;
            _strTaskName = strAssetPath;
            if (append != null)
            {
                for (int i = 0; i < append.Length; i++)
                {
					if (!_listAssetsPathWaitingLoad.Contains(append[i].ToLower()))
						_listAssetsPathWaitingLoad.Add(append[i].ToLower());
                }
            }
			_listAssetsPathWaitingLoad.Add(_strTaskName.ToLower());

        }

        /// <summary>
        /// 获取目标资源Asset对象
        /// </summary>
        /// <returns>返回ASSET(Unity.Object)</returns>
        public Object getTargetAsset()
        {
//            Utils.LogSys.Log(_strTaskName);
						if (!_dictAssetBundles.ContainsKey(_strTaskName.ToLower()))
            {
								Object obj = AssetManager.getInstance().getAsset(_strTaskName.ToLower());
                if (obj != null) return obj;

                return null;
            }

						return _dictAssetBundles[_strTaskName.ToLower()];
        }

        /// <summary>
        /// 卸载无用的assetbundle
        /// </summary>
        /// <param name="bValue"></param>
        public override void unloadUnusedAssetbundle(bool bValue)
        {
            foreach (var kvp in _dictAssetBundles)
            {
                AssetManager.getInstance().minusAssetbundleRefCount(kvp.Key);
            }
        }

        #region TaskBase

        /// <summary>
        /// 任务功能执行
        /// </summary>
        /// <returns>枚举器</returns>
        public override IEnumerator taskExec()
        {
            yield return null;

            //加载依赖资源
            foreach (var strPath in _listAssetsPathWaitingLoad)
            {
                //已经加载则直接使用
                if (AssetManager.getInstance().isAssetbundleLoaded(strPath))
                {
                    Object ab = AssetManager.getInstance().getAsset(strPath);
                    if (ab != null)
                    {
                        _dictAssetBundles[strPath] = ab;
                        _listAssetsPathComplete.Add(strPath);
						AssetManager.getInstance().addAssetbundleRefCount(strPath);
                    }
                }
                else
                {
                    float now_time = Time.realtimeSinceStartup;
                    string strLatesVersionpath = AssetManager.getInstance().PathData.getLatestVersionPath(strPath);
                    System.Type assetType = AssetManager.getInstance().PathData.getAssetType(strPath);
                    ResourceRequest resRequest = null;
                    if (assetType != null)
                        resRequest = Resources.LoadAsync(strLatesVersionpath, assetType);
                    else
                        resRequest = Resources.LoadAsync(strLatesVersionpath);

                    while(!resRequest.isDone)
                    {
                        yield return null;
                    }
                    //Utils.LogSys.Log(string.Format("{0:0.00}", Time.realtimeSinceStartup - now_time) + " load :" + strPath);
                    //获取资源
                    Object prefab = resRequest.asset;
                    _dictAssetBundles[strPath] = prefab;
                    _listAssetsPathComplete.Add(strPath);
                    resRequest = null;
                    AssetManager.getInstance().addAssetBundle(strPath, prefab);
                }
            }
        }

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
    }
}


