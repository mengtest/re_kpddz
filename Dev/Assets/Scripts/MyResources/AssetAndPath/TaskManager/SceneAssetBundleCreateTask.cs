/***************************************************************


 *
 *
 * Filename:  	SceneLoadTask.cs	
 * Summary: 	场景异步创建任务
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
using UnityEngine.SceneManagement;
#endregion

namespace task
{
    public class SceneAssetBundleCreateTask : TaskBase
    {
        private bool _bCanShow = false;//加载完成，且外界要求显示时才能显示

        private string _sSceneName = "";
        private bool _bAdditive = false;
        private bool _bCreating = false;//创建中
        private bool _bCompelete = false;
        private WWW _downloadTask;
        private AsyncOperation _asyncOptOfCreating;
        /// <summary>
        /// 完成列表
        /// </summary>
        List<string> _listAssetsPathComplete = new List<string>();
        /// <summary>
        /// 创建场景加载任务
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public SceneAssetBundleCreateTask(string strSceneName, bool bAdditive, bool isAutoStart = true) :
            base(isAutoStart)
        {
            _sSceneName = strSceneName;
            _bAdditive = bAdditive;
            _taskName = strSceneName;
        }

        /// <summary>
        /// 默认加载进度会卡在0.9f,调用该函数后才能结束
        /// </summary>
        public override void ToShowScene()
        {
            _bCanShow = true;
        }
        #region TaskBase

        /// <summary>
        /// 任务功能执行
        /// </summary>
        /// <returns>枚举器</returns>
        public override IEnumerator taskExec()
        {
            yield return null;

            float lastRealTime = Time.realtimeSinceStartup;
            if (_bAdditive)
            {
                _asyncOptOfCreating = SceneManager.LoadSceneAsync(_sSceneName,LoadSceneMode.Additive);
            }
            else
            {
                _asyncOptOfCreating = SceneManager.LoadSceneAsync(_sSceneName);
            }
            Utils.LogSys.Log("create scene cast: " + (Time.realtimeSinceStartup - lastRealTime).ToString());
            if (_asyncOptOfCreating != null)
                _asyncOptOfCreating.allowSceneActivation = _bCanShow;//false时：会卡在0.9f，true时：会继续
            yield return _asyncOptOfCreating;

            if (!_bAdditive)
                UtilTools.RemoveAllWinExpect();
            _bCreating = false;
            _bCompelete = true;
        }


        /// <summary>
        /// 获取加载进度
        /// </summary>
        /// <returns>进度(0.0~1.0)</returns>
        public override float getProgress()
        {
            if (_bCompelete)
                return 1f;

            if (_bCreating)
                return _asyncOptOfCreating.progress;

            return 0f;
        }

        #endregion
    }
}