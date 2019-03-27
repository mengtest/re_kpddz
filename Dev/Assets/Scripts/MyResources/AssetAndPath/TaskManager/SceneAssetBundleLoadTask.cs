/***************************************************************

 *
 *
 * Filename:  	SceneLoadTask.cs	
 * Summary: 	assetbundle场景加载任务管理器（本身并不加载东西）
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
    public class SceneAssetBundleLoadTask : TaskBase
    {
        private bool _bCanShow = false;//加载完成，且外界要求显示时才能显示

        private string _sSceneName = "";
        private string _strPath = "";
        private int _iVersion = 0;
        private bool _bAdditive = false;
        private bool _bDownLoading = false;//下载中
        private bool _bCreating = false;//创建中
        private bool _bCompelete = false;
        private AsyncOperation _asyncOptOfCreating;
        List<TaskBase> _listTaskItem = new List<TaskBase>();
        /// <summary>
        /// 完成列表
        /// </summary>
        List<string> _listAssetsPathComplete = new List<string>();
        SceneAssetBundleCreateTask _create_task;
        /// <summary>
        /// 依赖的资源列表
        /// </summary>
        List<string> _listAssetsPathDepend = new List<string>();
        /// <summary>
        /// 创建场景加载任务
        /// </summary>
        /// <param name="strAssetPath">资源相对于Resources的路径</param>
        public SceneAssetBundleLoadTask(string strSceneName, string strPath, int version, bool bAdditive, bool isAutoStart = true) :
            base(false)//SceneAssetBundleLoadTask是不参与下载的，所以用false
        {
            string exr_path = "resources/levels/" + strSceneName + "/lightmap_comp_light.exr";
            _taskName = "resources/levels/" + strSceneName + "/" + strSceneName + ".unity3d";
            _sSceneName = strSceneName;
            _strPath = strPath;
            _iVersion = version;
            _bAdditive = bAdditive;

            _bDownLoading = true;
//             AssetBundleLoadTaskItem load_task_exr = new AssetBundleLoadTaskItem(exr_path);
//             load_task_exr.EventFinished += LoadFinishedHandler;
//             _listTaskItem.Add(load_task_exr);
            if (string.IsNullOrEmpty(strPath))
            {
                AssetBundleLoadTaskItem load_task = new AssetBundleLoadTaskItem(_taskName);
                load_task.EventFinished += LoadFinishedHandler;
                _listTaskItem.Add(load_task);
            }
            else
            {
                SceneHttpLoadTask load_task = new SceneHttpLoadTask(strSceneName, strPath, version, isAutoStart);
                load_task.EventFinished += LoadFinishedHandler;
                _listTaskItem.Add(load_task);
            }
//             string[] objDepRec = AssetManager.getInstance().getAssetBundleDependencies(_taskName);
//             if (objDepRec != null)
//             {
//                 for (int i = 0; i < objDepRec.Length; i++)
//                 {
//                     AssetBundleLoadTaskItem load_task = new AssetBundleLoadTaskItem(objDepRec[i]);
//                     load_task.EventFinished += LoadFinishedHandler;
//                     _listTaskItem.Add(load_task);
//                 }
//             }

            CollectAllDependFiles(_taskName);
            _listAssetsPathDepend.Remove(_taskName);
            for (int i = 0; i < _listAssetsPathDepend.Count; i++)
            {
                AssetBundleLoadTaskItem load_task = new AssetBundleLoadTaskItem(_listAssetsPathDepend[i]);
                load_task.EventFinished += LoadFinishedHandler;
                _listTaskItem.Add(load_task);
            }
        }
        void CollectAllDependFiles(string strAssetPath)
        {
            string[] objDepRec = AssetManager.getInstance().getAssetBundleDependencies(strAssetPath);
            if (objDepRec != null)
            {
                for (int i = 0; i < objDepRec.Length; i++)
                {
                    if (objDepRec[i] != strAssetPath)
                        CollectAllDependFiles(objDepRec[i]);
                }
            }
            if (!_listAssetsPathDepend.Contains(strAssetPath))
                _listAssetsPathDepend.Add(strAssetPath);

        }
        public void LoadFinishedHandler(bool manual, TaskBase currentTask)
        {
            _listAssetsPathComplete.Add(currentTask._taskName);
            if (_listTaskItem.Count == _listAssetsPathComplete.Count)
            {
                _bDownLoading = false;
                _bCreating = true;
                _create_task = new SceneAssetBundleCreateTask(_sSceneName, _bAdditive);
                _create_task.EventFinished += CreateFinishedHandler;
                if (_bCanShow)
                    _create_task.ToShowScene();

                /*
                AssetBundle bundle = _listTaskItem[1].getTargetAssetbundle();//如果有lightmap.exr
                if (bundle != null)
                {
                    Object[] objects = bundle.LoadAllAssets<Object>();
                    List<LightmapData> lmdList = new List<LightmapData>();
                    foreach (Object obj in objects)
                    {
                        if (obj.GetType() == typeof(Texture2D))
                        {
                            if (obj.name.Contains("Lightmap-") && obj.name.Contains("_comp_light"))
                            {
                                LightmapData lmd = new LightmapData();
                                Texture2D tex = obj as Texture2D;
                                lmd.lightmapFar = tex;
                                lmdList.Add(lmd);
                            }
                        }
                    }
                   LightmapSettings.lightmaps = lmdList.ToArray();
                }
                 * */
            }
        }
        
        public void CreateFinishedHandler(bool manual, TaskBase currentTask)
        {
            finishExec();
            _bCreating = false;
            _bCompelete = true; 
            
            //刷新shader
            UnityEngine.SceneManagement.Scene newScene = SceneManager.GetSceneByName(currentTask._taskName);
            GameObject[] roots = newScene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++ )
            {
                UtilTools.UpdateShaders(roots[i]);
            }
        }
        /// <summary>
        /// 默认加载进度会卡在0.9f,调用该函数后才能结束
        /// </summary>
        public override void ToShowScene()
        {
            if (_create_task != null)
                _create_task.ToShowScene();
            _bCanShow = true;
        }
        #region TaskBase

        /// <summary>
        /// 任务功能执行
        /// </summary>
        /// <returns>枚举器</returns>
//         public override IEnumerator taskExec()
//         {
//             yield return null;
// 
//             _bDownLoading = true;
//             int taskCount = _listTaskItem.Count;
//             float lastRealTime = Time.realtimeSinceStartup; 
//             //执行任务
//             for (int i = 0; i < taskCount; i++)
//             {
//                 yield return _listTaskItem[i].taskExec();
//                 //执行单个任务的结束回调
//                 _listTaskItem[i].finishExec();
//             }
//             Utils.LogSys.Log("load scene units cast: " + (Time.realtimeSinceStartup - lastRealTime).ToString());
//             lastRealTime = Time.realtimeSinceStartup; 
//             _downloadTask = new WWW(_strPath);
//             yield return _downloadTask;
//             _bDownLoading = false;
//             Utils.LogSys.Log("load scene cast: " + (Time.realtimeSinceStartup - lastRealTime).ToString());
//             lastRealTime = Time.realtimeSinceStartup;
//             _bCreating = true;
//             if (_bAdditive)
//             {
//                 _asyncOptOfCreating = SceneManager.LoadSceneAsync(_sSceneName,LoadSceneMode.Additive);
//             }
//             else
//             {
//                 _asyncOptOfCreating = SceneManager.LoadSceneAsync(_sSceneName);
//             }
//             Utils.LogSys.Log("create scene cast: " + (Time.realtimeSinceStartup - lastRealTime).ToString());
//             _asyncOptOfCreating.allowSceneActivation = _bCanShow;//false时：会卡在0.9f，true时：会继续
//             yield return _asyncOptOfCreating;
// 
//             if (!_bAdditive)
//                 UtilTools.RemoveAllWinExpect();
//             _bCreating = false;
//             _bCompelete = true;
//         }


        /// <summary>
        /// 获取加载进度
        /// </summary>
        /// <returns>进度(0.0~1.0)</returns>
        public override float getProgress()
        {
            if (_bCompelete)
                return 1f;

            if (_bDownLoading)
                return 0.5f;

            if (_bCreating)
                return 0.5f + _create_task.getProgress() * 0.5f;

            return 0f;
        }

        #endregion
    }
}