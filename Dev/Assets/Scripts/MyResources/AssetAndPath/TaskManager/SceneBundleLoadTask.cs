/***************************************************************


 *
 *
 * Filename:  	SceneLoadTask.cs	
 * Summary: 	网络场景加载任务
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
    public class SceneBundleLoadTask : TaskBase
    {
        private bool _bCanShow = false;//加载完成，且外界要求显示时才能显示

        private string _sSceneName = "";
        private string _strHttp = "";
        private int _iVersion = 0;
        private bool _bAdditive = false;
        private bool _bDownLoading = false;//下载中
        private bool _bCreating = false;//创建中
        private bool _bCompelete = false;
        private WWW _downloadTask;
        private AsyncOperation _asyncOptOfCreating;
        /// <summary>
        /// 创建场景加载任务
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public SceneBundleLoadTask(string strSceneName, string strHttp, int version, bool bAdditive, bool isAutoStart = true) :
            base(isAutoStart)
        {
            _taskName = strSceneName;
            _sSceneName = strSceneName;
            _strHttp = strHttp;
            _iVersion = version;
            _bAdditive = bAdditive;
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

            _bDownLoading = true;
            _downloadTask = WWW.LoadFromCacheOrDownload(_strHttp, _iVersion);
            yield return _downloadTask;
            _bDownLoading = false;

            _bCreating = true;
            if (_bAdditive)
            {
                _asyncOptOfCreating = SceneManager.LoadSceneAsync(_sSceneName, LoadSceneMode.Additive);
            }
            else
            {
                _asyncOptOfCreating = SceneManager.LoadSceneAsync(_sSceneName);
            }
            _asyncOptOfCreating.allowSceneActivation = _bCanShow;//false时：会卡在0.9f，true时：会继续
            yield return _asyncOptOfCreating;

            if (!_bAdditive)
                UtilTools.RemoveAllWinExpect();
                //UIManager.RemoveAllWinExpect(new string[] { UIName.WAITING, UIName.LOADING_WIN, UIName.MESSAGE_DIALOG, UIName.MAIN_CITY, UIName.MAIN_CITY_RIGHT, UIName.MAIN_CITY_TOP });
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

            if (_bDownLoading)
                return _downloadTask.progress*0.5f;

            if (_bCreating)
                return 0.5f + _asyncOptOfCreating.progress * 0.5f;

            return 0f;
        }

        #endregion
    }
}