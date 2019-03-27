/***************************************************************


 *
 *
 * Filename:  	TaskManager.cs	
 * Summary: 	任务管理器，管理协程任务
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/06/02 18:04
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;
#endregion


namespace task
{
    public class TaskManager : Singleton<TaskManager>
    {
        /// <summary>
        /// 同时进行的任务数量
        /// </summary>
        public static int MAX_CONCURRENCY_TASK = 5;

        /// <summary>
        /// 正在运行的任务列表
        /// </summary>
        List<TaskBase> _listRuningTasks = new List<TaskBase>();

        /// <summary>
        /// 等待开始的任务列表
        /// </summary>
        List<TaskBase> _listWaitingTasks = new List<TaskBase>();

        /// <summary>
        /// 暂停的任务列表
        /// </summary>
        List<TaskBase> _listPausedTasks = new List<TaskBase>();


        ///////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="task">任务对象</param>
        public void startTask(TaskBase task)
        {
            if (task == null)
            {
                return;
            }

            task.EventFinished += taskFinishHandler;

            if (_listRuningTasks.Count < MAX_CONCURRENCY_TASK)
            {
                _listRuningTasks.Add(task);
                StartCoroutine(task.coroutineWrapper());
            }
            else
            {
                _listWaitingTasks.Add(task);
            }
// 						for (int i=0; i<_listRuningTasks.Count; i++)
// 								Utils.LogSys.Log ("------------>Runing Task:"+_listRuningTasks[i]._taskName);
        }

        /// <summary>
        /// 任务停止执行回调处理
        /// </summary>
        /// <param name="bManual">是否手动停止</param>
        /// <param name="task">停止的任务</param>
        public void taskFinishHandler(bool bManual, TaskBase task)
        {
			//Utils.LogSys.Log("Task complete: " + task._taskName);
            if (_listRuningTasks.Contains(task))
            {
                _listRuningTasks.Remove(task);
            }

            if (_listWaitingTasks.Count > 0)
            {
                TaskBase t = _listWaitingTasks[0];
                _listWaitingTasks.RemoveAt(0);

                //添加到运行列表
                _listRuningTasks.Add(t);
                StartCoroutine(t.coroutineWrapper());
            }
        }

        public void OtherTaskLoadingAssetPath(out AssetBundleLoadTaskItem taskLoading, string assetBundlePath)
        {
            taskLoading = null;
            int count = _listRuningTasks.Count;
            bool bLoading = false;
            bool bWaiting = false;
            for (int i = 0; i < count; i++)
            {
                AssetBundleLoadTaskItem loadtask = _listRuningTasks[i] as AssetBundleLoadTaskItem;
                if (loadtask != null)
                {
                    if (loadtask._taskName == assetBundlePath)
                    {
                        //taskLoading = loadtask;
                        if (loadtask.IsLoadComplete())//优先取已加载完的
                        {
                            taskLoading = loadtask;
                            return;
                        }
                        else if (bLoading)
                        {
                            continue;
                        }
                        else if (!bLoading && loadtask.IsLoading())//其次取正在加载的
                        {
                            taskLoading = loadtask;
                            bLoading = true;
                        }
                        else if (bWaiting)
                        {
                            continue;
                        }
                        else if (!bWaiting && loadtask.IsWaiting())//最次取正在等待的
                        {
                            taskLoading = loadtask;
                            bWaiting = true;
                        }
                            
                    }
                }
            }
        }

        #region Monobehavior

        public void Start()
        {
        }

        //public void Update()
        //{
        //    //Utils.LogSys.Log(string.Format("_listRuningTasks: {0}", _listRuningTasks.Count));
        //    //Utils.LogSys.Log(string.Format("_listWaitingTasks: {0}", _listWaitingTasks.Count));
        //}

        #endregion //Monobehavior
    }
}

