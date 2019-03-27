/***************************************************************


 *
 *
 * Filename:  	WWWLoadTask.cs	
 * Summary: 	资源更新任务
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/07/07 18:03
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
    public class WWWLoadTask : TaskBase
    {
        /// <summary>
        /// 任务名
        /// </summary>
        public string _strTaskName = "";
        public string _strMD5 = "";

        private string _strHttp = "";
        private bool _bDownLoading = false;//下载中
        private bool _bCompelete = false;
        private WWW _downloadTask = null;
        /// <summary>
        /// 创建场景加载任务(strName为任务名，回调中要用到时传。)
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public WWWLoadTask(string strName, string strHttp, string strMD5="", bool isAutoStart = true) :
            base(isAutoStart)
        {
            _strTaskName = strName;
            _strMD5 = strMD5;
            _strHttp = strHttp;
            _bCompelete = false;
            _bDownLoading = true;
        }

        #region TaskBase

        /// <summary>
        /// 任务功能执行
        /// </summary>
        /// <returns>枚举器</returns>
        public override IEnumerator taskExec()
        {
            yield return null;

            //加载资源
            _downloadTask = new WWW(_strHttp);
            while (!_downloadTask.isDone)
            {
                yield return null;
            }
            if (_downloadTask.text.Length == 0)
            {
                Utils.LogSys.Log("LoadFileFailed:" + _strHttp);
            }
            _bCompelete = true;
            _bDownLoading = false;
        }


        public override WWW GetWWW()
        {
            return _downloadTask;
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
                return _downloadTask.progress;


            return 0f;
        }

        #endregion
    }
}

