/***************************************************************


 *
 *
 * Filename:  	SceneLoadTask.cs	
 * Summary: 	场景加载任务
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
using Scene;
using UnityEngine.SceneManagement;
#endregion

namespace task
{
    public class SceneLoadTask : TaskBase
    {
        private bool _bCanShow = false;//外界要求显示
        private bool _bLoadOver = false;//加载完成

        private string _sSceneName = "";
        private bool _bIsAsync = false;
        private bool _bAdditive = false;
        private bool _bLoading = false;
        private bool _bCompelete = false;
        private AsyncOperation _asyncOptOfLoading;
        /// <summary>
        /// 创建场景加载任务
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public SceneLoadTask(string strSceneName, bool bIsAsync, bool bAdditive, bool isAutoStart = true) :
            base(isAutoStart)
        {
            _taskName = strSceneName;
            _sSceneName = strSceneName;
            _bIsAsync = bIsAsync;
            _bAdditive = bAdditive;
        }

        /// <summary>
        /// 默认加载进度会卡在0.9f,调用该函数后才能结束
        /// </summary>
        public override void ToShowScene()
        {
            _bCanShow = true;
            if (_bLoadOver)
            {
                GameSceneManager.getInstance().ActiveLoadingScene();
            }
        }

        private void LoadSceneOver()
        {
            _bLoadOver = true;
            if (!_bCanShow)
            {
                GameSceneManager.getInstance().EnActiveLoadingScene();
            }
            else
            {
                GameSceneManager.getInstance().ActiveLoadingScene();
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
            _bLoading = true;
            if (_bIsAsync)
            {
                if (_bAdditive)
                {
                    _asyncOptOfLoading = SceneManager.LoadSceneAsync(_sSceneName, LoadSceneMode.Additive);
                }
                else
                {
                    _asyncOptOfLoading = SceneManager.LoadSceneAsync(_sSceneName);
                }
                while (!_asyncOptOfLoading.isDone )
                {
                    yield return null;
                }
                if (_bAdditive)
                    LoadSceneOver();
                while ( _bAdditive && !_bCanShow)
                {
                    yield return null;
                }
                //UIManager.RemoveAllWinExpect(UIName.LOADING_WIN);
                if (!_bAdditive)
                    UtilTools.RemoveAllWinExpect();
            }
            else
            {
                if (_bAdditive)
                {
                    SceneManager.LoadScene(_sSceneName, LoadSceneMode.Additive);
                }
                else
                {
                    SceneManager.LoadScene(_sSceneName);
                    UtilTools.RemoveAllWinExpect();
                }
                //UIManager.RemoveAllWinExpect(UIName.LOADING_WIN);
                _bCompelete = true;
                yield return null;
            }
            _bLoading = false;
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

            if (_bLoading)
                return _asyncOptOfLoading.progress;

            return 0f;
        }

        #endregion
    }
}
