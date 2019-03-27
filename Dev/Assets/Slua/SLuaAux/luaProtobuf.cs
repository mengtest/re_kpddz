/***************************************************************


 *
 *
 * Filename:  	luaProtobuf.cs	
 * Summary: 	lua使用Protobuf
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/18 18:09
 ***************************************************************/

using UnityEngine;
using System.Collections;
using SLua;
using System.Collections.Generic;
using LuaInterface;
using System;
using Utils;
using sluaAux.proto;

namespace sluaAux
{
    [CustomLuaClass]
    public class luaProtobuf
    {
        //lua中的消息回调函数
        Dictionary<int, string> _luaMsgCallbackMap = new Dictionary<int, string>();

        //单例
        volatile static luaProtobuf _instance = null;
        static readonly object lockHelper = new object();

        /////////////////////////////////////////////////////////////////////////////////

        //防止构造
        private luaProtobuf()
        { }

        /// <summary>
        /// 获取单例
        /// </summary>
        [StaticExport]
        public static luaProtobuf getInstance()
        {
            if (_instance == null)
            {
                lock(lockHelper)
                {
                    if (_instance == null)
                        _instance = new luaProtobuf();
                }
            }

            return _instance;
        }

        /// <summary>
        /// 注册消息回调函数
        /// </summary>
        public void registerMessageScriptHandler(int idMsg, string luaFuncName)
        {
            if (_luaMsgCallbackMap.ContainsKey(idMsg))
                return;

            _luaMsgCallbackMap.Add(idMsg, luaFuncName);
        }

        public void removeMessageHandler(int idMsg)
        {
            _luaMsgCallbackMap.Remove(idMsg);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="state"></param>
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int sendMessage(IntPtr state)
        {
            luaProtobuf self = (luaProtobuf)LuaObject.checkSelf(state);

            //消息ID
            int idMsg = LuaDLL.luaL_checkinteger(state, 2);

            //消息内容
            LuaTypes type = LuaDLL.lua_type(state,3);
            if (type != LuaTypes.LUA_TTABLE)
            {
                LogSys.LogError("LUA message error! idMsg = " + idMsg.ToString());
                return 0;
            }
            LuaTable msgTbl = (LuaTable)LuaObject.checkVar(state, 3);

            //生成消息
            luaMessage msg = luaProtoHelper.readLuaMessage(idMsg, msgTbl);

            //写入流并发出
            using (var stream = new System.IO.MemoryStream())
            {
                using (var protoWriter = new luaProtoWriter(stream))
                {
                    protoWriter.writeLuaMessage(msg);
                    protoWriter.close();
                }
                network.protobuf.ClientNetwork.Instance.SendLuaMsg(idMsg, stream);
            }

            LuaObject.pushValue(state, true);
            return 1;
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="idMsg"></param>
        /// <param name="stream"></param>
        [DoNotToLua]
        public void receiveMsg(int idMsg, System.IO.MemoryStream stream)
        {

            if (!_luaMsgCallbackMap.ContainsKey(idMsg))
                return;

            luaMessage msgRev = null;
            using (luaProtoReader reader = luaProtoReader.createLuaProtoReader(stream))
            {
                if (reader != null)
                    msgRev = reader.readLuaMessage(idMsg);
            }

            if (msgRev == null)
                return;

            //将消息回传给lua回调
            string luaCallbackFunc = "";
            if (_luaMsgCallbackMap.TryGetValue(idMsg, out luaCallbackFunc))
            {
                LuaState state = luaSvrManager.getInstance().luaSvr.luaState;
                LuaFunction luafunc = state.getFunction(luaCallbackFunc);
                if (luafunc == null)
                    return;

                int error = LuaObject.pushTry(state.L);  //错误处理
                LuaDLL.lua_pushinteger(state.L, idMsg);  //压入idMsg
                luaProtoHelper.createLuaTable(state, msgRev); //压入数据表
                luafunc.pcall(2, error);
                LuaDLL.lua_remove(state.L, error);
            }
        }

        [DoNotToLua]
        public bool hasLuaListener(int idMsg)
        {
            if (_luaMsgCallbackMap.ContainsKey(idMsg))
                return true;

            return false;
        }
    }
}


