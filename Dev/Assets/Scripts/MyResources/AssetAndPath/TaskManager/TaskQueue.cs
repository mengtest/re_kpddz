/***************************************************************


 *
 *
 * Filename:  	TaskQueue.cs	
 * Summary: 	任务队列
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/11/25 4:35
 ***************************************************************/


using System.Collections;
using System.Collections.Generic;

namespace task
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public class TaskQueue : TaskBase
    {
        //队列
        List<TaskBase> _queue = new List<TaskBase>();

        //任务进度
        float _fProgress = 0.0f;

        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskQueue(TaskBase[] tasks, bool bAutoStart = true)
            :base(bAutoStart)
        {
            foreach (var t in tasks) _queue.Add(t);
        }

        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 兼容旧的单一task
        /// </summary>
        public TaskQueue(TaskBase tb, bool bAutoStart = true)
           : base(bAutoStart)
        {
            _queue.Add(tb);
        }

        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 执行的任务
        /// </summary>
        public override IEnumerator taskExec()
        {
            //执行任务
            for (int i = 0; i < _queue.Count; i++)
            {
                yield return _queue[i].taskExec();

                //执行单个任务的结束回调
                _queue[i].finishExec();

                _fProgress += 1.0f;
            }
        }

        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取任务进度
        /// </summary>
        /// <returns></returns>
        public override float getProgress()
        {
            if (_queue.Count > 0)
                return _fProgress / _queue.Count;

            return 1.0f;
        }
    }
}


