/***************************************************************


 *
 *
 * Filename:  	SceneLoadTask.cs	
 * Summary: 	场景加载任务管理器（本身并不加载东西）
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/06/26 10:53
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using asset;
using task;
using UI.Controller;
#endregion

namespace task
{
    public class SceneHttpLoadTask : TaskBase
    {
        private string _sSceneName = "";
        private string _strHttp = "";
        private int _iVersion = 0;
        private bool _bCompelete = false;
        private WWW _downloadTask;
        /// <summary>
        /// 存储资源Assetbundle或者asset，每个资源对应单独的键值
        /// </summary>
        Object _assetBundle = null;
        /// <summary>
        /// 创建场景加载任务
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public SceneHttpLoadTask(string strSceneName, string strHttp, int version, bool isAutoStart = true) :
            base(isAutoStart)
        {
            _taskName = strHttp;
            _sSceneName = strSceneName;
            _strHttp = strHttp;
            _iVersion = version;
        }

        #region TaskBase

        /// <summary>
        /// 任务功能执行
        /// </summary>
        /// <returns>枚举器</returns>
        public override IEnumerator taskExec()
        {
            yield return null;

            _downloadTask = WWW.LoadFromCacheOrDownload(_strHttp, _iVersion);
            yield return _downloadTask;

            _bCompelete = true;
            _assetBundle = _downloadTask.assetBundle;
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
        /// 获取加载进度
        /// </summary>
        /// <returns>进度(0.0~1.0)</returns>
        public override float getProgress()
        {
            if (_bCompelete)
                return 1f;

            if (_downloadTask != null)
                return _downloadTask.progress;

            return 0f;
        }

        #endregion
    }
}