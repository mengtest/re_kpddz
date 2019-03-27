/***************************************************************


 *
 *
 * Filename:  	TaskBase.cs	
 * Summary: 	任务基类，提供任务的基本操作
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/06/02 4:03
 ***************************************************************/

using UnityEngine;
using System.Collections;

namespace task
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public class TaskBase
    {
        /// 协程终止订阅者代理。当且仅当显示调用stop函数终止协程时
        /// manual才为true
        public delegate void FinishedHandler(bool manual, TaskBase currentTask);

        /// 终止事件，当协程结束时触发
        public event FinishedHandler EventFinished = null;

        public string _taskName = "";
        public IEnumerator e = null;

        /// <summary>
        /// 任务状态
        /// </summary>
        bool _bRunning = false;
        bool _bPaused = false;
        bool _bStoped = false;
        
        public TaskBase(bool bAutoStart = true)
        {
            if (bAutoStart)
            {
                start();
            }
        }

        /// <summary>
        /// 获取暂停状态（只读）
        /// </summary>
        public bool Paused
        {
            get { return _bPaused; }
        }

        /// <summary>
        /// 获取运行状态（只读）
        /// </summary>
        public bool Running
        {
            get { return _bRunning; }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void start()
        {
            _bRunning = true;
            TaskManager.getInstance().startTask(this);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void pause()
        {
            _bPaused = true;
        }

        /// <summary>
        /// 取消暂停
        /// </summary>
        public void unPause()
        {
            _bPaused = false;
        }

        /// <summary>
        ///  停止
        /// </summary>
        public void stop()
        {
            _bStoped = true;
            _bRunning = false;
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void finishExec()
        {
            // 目标协程执行结束
            FinishedHandler handler = EventFinished;
            if (handler != null)
            {
                handler(_bStoped, this);
            }
        }


        /// <summary>
        /// 协程调度封装
        /// </summary>
        public IEnumerator coroutineWrapper()
        {
            yield return null;
            e = this.taskExec();
            while(_bRunning)
            {
                if (_bPaused)
                {
                    yield return null;
                }
                else
                {
                    if (e!= null && e.MoveNext())
                    {
                        yield return e.Current;
                    }
                    else
                    {
                        _bRunning = false;
                    }
                }
            }

            // 目标协程执行结束
            FinishedHandler handler = EventFinished;
            if (handler != null)
            {
                handler(_bStoped, this);
            }
        }

        /// <summary>
        /// 执行的任务
        /// </summary>
        public virtual IEnumerator taskExec()
        {
            yield return null;
        }

        /// <summary>
        /// 获取任务进度
        /// </summary>
        /// <returns></returns>
        public virtual float getProgress()
        {
            return .0f;
        }

        public bool IsInCach(string url)
        {
            return Caching.IsVersionCached(url, 0);
        }
        public virtual AssetBundle getTargetAssetbundle() { return null; }
        public virtual void unloadUnusedAssetbundle(bool bValue) { }
        public virtual void ToShowScene() { }
        public virtual WWW GetWWW() { return null; }
    }

}

