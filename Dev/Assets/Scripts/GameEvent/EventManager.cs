/***************************************************************


 *
 *
 * Filename:  	EventManager.cs	
 * Summary: 	事件管理
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/17 19:22
 ***************************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
using EventManager;
using System.Threading;

namespace EventManager
{

    [SLua.CustomLuaClass]
	public class EventSystem
	{
		private EventSystem()
		{
		}
        /// <summary>
        /// 事件缓存池
        /// </summary>
        static private Queue<EventMultiArgs> _eventCachePool = new Queue<EventMultiArgs>();


        /// <summary>
        /// 添加缓存的事件
        /// </summary>
        static private void SaveEvent(EventMultiArgs eventObj)
        {
            try
            {
                Monitor.Enter(_eventCachePool);
                _eventCachePool.Enqueue(eventObj);
                Monitor.Pulse(_eventCachePool);
            }
            finally
            {
                Monitor.Exit(_eventCachePool);
            }
            
        }

        /// <summary>
        /// 执行所有缓存事件
        /// </summary>
        static public void update()
        {
            try
            {
                Monitor.Enter(_eventCachePool);
                foreach (EventMultiArgs evntObj in _eventCachePool)
                {
                    RunEvent(evntObj);
                }
                _eventCachePool.Clear();
                Monitor.Pulse(_eventCachePool);
            }
            finally
            {
                Monitor.Exit(_eventCachePool);
            }
        }






		static protected Dictionary<uint, DelegateType.EventCallback> idEventCallback = new Dictionary<uint, DelegateType.EventCallback>();

        /// <summary>
        /// 添加事件响应处理
        /// </summary>
        /// <param name="id">消息名</param>
        /// <param name="de">响应函数</param>
        static public void RegisterEvent(uint id, DelegateType.EventCallback de)
		{
			if (idEventCallback.ContainsKey(id))
			{
				idEventCallback[id] += de;
			}
			else
			{
				idEventCallback[id] = de;
			}
		}

        /// <summary>
        /// 移除事件响应处理
        /// </summary>
        /// <param name="id">消息名</param>
        /// <param name="de">响应函数</param>
        static public void RemoveEvent(uint id, DelegateType.EventCallback de)
		{
			if (idEventCallback.ContainsKey(id))
				idEventCallback[id] -= de;
		}
		
		/// <summary>
		/// 执行事件（参数：bRunNow确保在主线程中调用时才能为true，进行立即执行，默认为false到下一帧执行，防止子线程中直接执行会报错）
		/// </summary>
		/// <param name="id"></param>
		/// <param name="args"></param>
		/// <param name="bRunNow"></param>
		static public void CallEvent(uint id, EventMultiArgs args, bool bRunNow = false)
		{
			try
			{
				if (idEventCallback.ContainsKey(id))
                    if (idEventCallback[id] != null)
                    {
                        if (args == null)
                            args = new EventMultiArgs();
                        args.AddArg("EVENT_ID", id);
                        if (bRunNow)
                            idEventCallback[id](args);
                        else
                            SaveEvent(args);
                    }
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

        static private void RunEvent(EventMultiArgs args)
        {
            uint id = args.GetArg<uint>("EVENT_ID");
            try
            {
                if (idEventCallback.ContainsKey(id))
                    if (idEventCallback[id] != null)
                    {
                        idEventCallback[id](args);
                    }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
		
		//清空
		static public void Dispose()
		{
			idEventCallback.Clear();
		}
	}



    [SLua.CustomLuaClass]	
	public class EventID
	{
		public static uint SOKECT_CONNECT_RESULT     = 1;//socket连接结果(bool result)
		public static uint GAME_RECONNECT_RESULT     = 2;//游戏重连结果(bool result, string reason)
		public static uint SERVER_MSG_DLSN_TEN_MUL   = 2;//服务端消息处理了n条。(n为10的倍数)
		public static uint SERVER_MSG_MISSING        = 3;//丢包
        public static uint LOGIN_COMPLETE            = 4;
        public static uint CLICK_SCENE_TARGET        = 5;//点到场景中的物体
        public static uint LOGINWIN_UPDATE           = 6;//
        public static uint PRESS_SCENE_TARGET        = 7;//按下场景中的物体
        public static uint PRESS_CANCEL_SCENE_TARGET = 8;//按下取消场景中的物体
        public static uint PRESS_CANCEL_PRESS = 9;//按下取消场景中的物体
        public static uint PRESS_MOVE_PRESS = 10;//拖动事件
        public static uint PRESS_REBOUND_PRESS = 11;//弹起事件


        public static uint BATTLEITEM_REFRESH = 12;//战斗道具数量刷新
	    public static uint SHOW_LOGIN_BTN = 103;
        public static uint NEWBIEGUIDE_FIRECOUNTCHANGE = 100;//新手引导发子弹事件
        public static uint NEWBIEGUIDE_KILLCOUNTCHANGE = 101;//新手引导殺魚事件
        public static uint NEWBIEGUIDE_FISHCOLLIDERCOUNTCHANGE = 102;//新手引导击中事件


	}
}
