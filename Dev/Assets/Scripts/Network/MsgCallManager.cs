/***************************************************************


 *
 *
 * Filename:  	MsgCalleManager.cs	
 * Summary: 	消息分发管理
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/18 14:22
 ***************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using network;
using network.protobuf;
using System.Threading;
using EventManager;
using System.IO;

namespace network.protobuf
{

    /// <summary>
    /// 网络线程接收的消息包,需要手动释放内存资源
    /// </summary>
    public class NetThreadMsgPackage : IDisposable
    {
        public ProtoID id = 0;
        public MemoryStream stream = null;

        // 是否被释放
        bool _disposed = false;

        public NetThreadMsgPackage(ProtoID id, MemoryStream stream)
        {
            this.id = id;
            this.stream = stream;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // dispose managed object
                if (disposing)
                {
                    id = 0;
                }

                // dispose unmanaged objects
                if (stream != null)
                    stream.Dispose();

                stream = null;
                _disposed = true;
            }
        }

        // 实现IDispose接口
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~NetThreadMsgPackage()
        {
            Dispose(false);
        }

    }

    public class LuaStreamCache
    {
        public ProtoID id = 0;
        public MemoryStream stream = null;
        private static List<LuaStreamCache> _luaStreamCaches = new List<LuaStreamCache>();

        public LuaStreamCache(ProtoID id, MemoryStream stream)
        {
            this.id = id;
            this.stream = new MemoryStream(stream.ToArray());
        }

        public void Release()
        {
            stream.Dispose();
            stream = null;
        }

        public static void PushStreamCache(ProtoID pid, MemoryStream stream)
        {
            LuaStreamCache cache = new LuaStreamCache(pid, stream);
            PushStreamCache(cache);
        }

        public static void PushStreamCache(LuaStreamCache cache)
        {
            _luaStreamCaches.Add(cache);
        }

        public static void ProcessCaches()
        {
            if (_luaStreamCaches.Count > 0)
            {
                for (var index = 0; index < _luaStreamCaches.Count; index++)
                {
                    LuaStreamCache cache = _luaStreamCaches[index];
                    ProtoID pID = cache.id;
                    MemoryStream mem = cache.stream;
                    //Debug.LogWarning("发送缓存LUA消息：" + pID);
                    sluaAux.luaProtobuf.getInstance().receiveMsg((int)pID, mem);
                    cache.Release();
                }
                _luaStreamCaches.Clear();
            }
        }

        public static void ClearAll()
        {
            for (var index = 0; index < _luaStreamCaches.Count; index++)
            {
                LuaStreamCache cache = _luaStreamCaches[index];
                cache.Release();
            }
            _luaStreamCaches.Clear();
        }
    }

	public class MsgCallManager {

		private MsgCallManager()
		{
		}
		/// <summary>
		/// 存放消息的回调
		/// </summary>
		protected static Dictionary<ProtoID, DelegateType.ProtoCallback> idToCallback = new Dictionary<ProtoID, DelegateType.ProtoCallback>();

		/// <summary>
		/// 添加消息响应处理
		/// </summary>
		/// <param name="protoType">消息名</param>
		/// <param name="e">响应函数</param>
		public static void AddCallback(ProtoID protoType, DelegateType.ProtoCallback e)
		{
			if (idToCallback.ContainsKey(protoType))
			{
				idToCallback[protoType] += e;
			}
			else
			{
				idToCallback[protoType] = e;
			}
		}

		/// <summary>
		/// 移除消息响应处理
		/// </summary>
		/// <param name="protoType">消息名</param>
		/// <param name="e">响应函数</param>
		static public void RemoveCallback(ProtoID protoType, DelegateType.ProtoCallback e)
		{
			if (idToCallback.ContainsKey(protoType))
				idToCallback[protoType] -= e;
		}

        static public void RemoveAllCallback()
        {
            idToCallback.Clear();
        }

		//执行
		static public void RunCallback(ProtoID protoType, object proto)
		{
			try
			{
				if (idToCallback.ContainsKey(protoType))
					if (idToCallback[protoType] != null)
						idToCallback[protoType](proto);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		/// <summary>
		/// 存储要处理的消息
		/// </summary>
        private static Queue<NetThreadMsgPackage> _waitingForRun = new Queue<NetThreadMsgPackage>();

		//添加要处理的消息
        public static void addMsgObj(ProtoID protoType, MemoryStream proto)
		{
            ////若没有响应函数则不保存 分为：C#部分和Lua部分
            bool luaListener = sluaAux.luaProtobuf.getInstance().hasLuaListener((int)protoType);
            if (!idToCallback.ContainsKey(protoType) && !luaListener)
                return;

            var obj = new NetThreadMsgPackage(protoType, proto);
			try
			{
				Monitor.Enter(_waitingForRun);

				_waitingForRun.Enqueue(obj);
			
				Monitor.Pulse(_waitingForRun);
			}
			finally
			{
				Monitor.Exit(_waitingForRun);
			}
		}

		/// <summary>
		/// 执行count条存储的消息
		/// </summary>
		/// <param name="count">取出n条执行（默认取出全部）</param>
		public static void Run(int count = 0)
		{
			if (_waitingForRun.Count > 0)
			{
				try
				{
					Monitor.Enter(_waitingForRun);
					//Utils.LogSys.Log("Start to Call");
				
					if (count == 0 || count > _waitingForRun.Count)
						count = _waitingForRun.Count;
					
					while (count-- > 0)
					{
                        //Utils.LogSys.LogError("Duel " + _waitingForRun[0]._protoType);
                        NetThreadMsgPackage msgPkg = _waitingForRun.Dequeue();

                        //C#消息处理
                        if (idToCallback.ContainsKey(msgPkg.id))
                        {
                            object protoMsg = ProtoSerializer.ParseFrom(msgPkg.id, msgPkg.stream);
                            MsgObject msgObj = new MsgObject(msgPkg.id, protoMsg);
                            RunCallback(msgObj._protoType, msgObj._proto);
                        }
                        else
                        {
                            //Debug.LogWarning("处理LUA消息：" + msgPkg.id);
                        }

                        msgPkg.stream.Seek(0, SeekOrigin.Begin);

                        //Lua消息处理
                        if (sluaAux.luaSvrManager.getInstance().IsLoaded)
                        {
                            Utils.LogSys.Log("处理LUA消息：" + msgPkg.id);
                            sluaAux.luaProtobuf.getInstance().receiveMsg((int)msgPkg.id, msgPkg.stream);
                        }
                        else
                        {
                            LuaStreamCache.PushStreamCache(msgPkg.id, msgPkg.stream);
                        }
                        
                       
                        //释放（对应于接收消息线程创建的memorystream）
                        msgPkg.Dispose();

                        //_waitingForRun[0].RunCallback();
					}
					Monitor.Pulse(_waitingForRun);
				}
				finally
				{
					Monitor.Exit(_waitingForRun);
				}
            }
            else
            {
                ClientNetwork.Instance.CheckIsLostConnect();
            }
		}
		
		//清空
		static public void Dispose()
		{
			//idToCallback.Clear();
			try
			{
				Monitor.Enter(_waitingForRun);
				//Utils.LogSys.Log("Start to Call");
				_waitingForRun.Clear();
				Monitor.Pulse(_waitingForRun);
			}
			finally
			{
				Monitor.Exit(_waitingForRun);
			}
            LuaStreamCache.ClearAll();
		}

	}
}