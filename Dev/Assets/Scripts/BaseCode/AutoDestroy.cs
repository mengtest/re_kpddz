/***************************************************************

 *
 *
 * Filename:  	AutoDestroy.cs	
 * Summary: 	对象自动销毁控制
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/08/31 0:14
 ***************************************************************/


#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion


namespace Utils
{
    /// <summary>
    /// 对象自动销毁控制
    /// </summary>
    public class AutoDestroy : MonoBehaviour
    {
        //自动销毁类型
        public enum EType
        {
            eFixedTime,  //固定时间
            eCondition,  //满足条件   
            eForever,    //不销毁
        }

        //时间事件
        class TimeEvent
        {
            public float _fTime;
            public TimeEventDelegate _handle;
            public bool _bValid;
            public int _nIdx;
           
            public TimeEvent(float fTime, TimeEventDelegate handle)
            {
                _fTime = fTime;
                _handle = handle;
                _bValid = true;
            }

            //执行
            public void execWapper()
            {
                if (_handle != null) _handle(_nIdx, _fTime);
                _bValid = false;
            }
        }


        //销毁条件
        public delegate bool DestroyConditionDelegate();
        //销毁回调
        public delegate void onDestroyDelegate();
        //时间事件
        public delegate void TimeEventDelegate(int idEvent, float fTime);

        ////////////////////////////////////////////////////////////

        /// <summary>
        /// 自动销毁类型
        /// </summary>
        public EType AutoDestroyType = EType.eFixedTime;

        /// <summary>
        /// 持续时间(秒)
        /// </summary>
        //[HideInInspector]
        public float Duration = 5.0f;

        /// <summary>
        /// 销毁条件
        /// </summary>
        [HideInInspector]
        public DestroyConditionDelegate DestroyCondition = null;

        /// <summary>
        /// 销毁事件
        /// </summary>
        [HideInInspector]
        public event onDestroyDelegate onDestroyEvent = null;

        //时间事件
        List<TimeEvent> _listTimeEvent = new List<TimeEvent>();

        //开始时间
        float _fStartTime = 0.0f;

        ////////////////////////////////////////////////////////////

        //自动销毁
        void autoDestroy()
        {
            if (onDestroyEvent != null)
                onDestroyEvent();

            GameObject.Destroy(gameObject);
        }

        /// <summary>
        /// 注册时间事件
        /// </summary>
        public int registerTimeEvent(float fTime, TimeEventDelegate deg)
        {
            var value = new TimeEvent(fTime, deg);
            value._nIdx = _listTimeEvent.Count;
            _listTimeEvent.Add(value);

            return value._nIdx;
        }

        ////////////////////////////////////////////////////////////

        #region MONO

        // 初始化
        void Start()
        {
            _fStartTime = Time.time;
            if (AutoDestroyType == EType.eFixedTime)
                Invoke("autoDestroy", Duration);
        }

        void Update()
        {
            //TODO: 优化 @WP.Chu
            //时间事件处理
            float fDelta = Time.time - _fStartTime;
            for (int i = _listTimeEvent.Count - 1; i >= 0; i--)
            {
                TimeEvent value = _listTimeEvent[i];
                if (value._bValid && fDelta >= value._fTime)
                {
                    value.execWapper();
                    _listTimeEvent.RemoveAt(i);
                }
            }

            if (AutoDestroyType == EType.eCondition)
            {
                if (DestroyCondition == null || DestroyCondition())
                {
                    autoDestroy();
                }
            }
        }

        #endregion //MONO
    }
}


