/***************************************************************


 *
 *
 * Filename:  	luaProtoTest.cs	
 * Summary: 	luaproto相关的测试和应用举例
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/11/05 7:25
 ***************************************************************/


//#define _CS_PROTO_RW_WITH_NEST__       // c#带有嵌入消息的proto读写
//#define _CS_PROTO_RW_WITHOUT_NEST__    // c#带无嵌入消息的proto读写
#define _LUA_PROTO_W_WITH_NEST_        // LUA带有嵌入式消息写入
#define _LUA_PROTO_W_WITHOUT_NEST_     // LUA无嵌入式消息写入
#define _LUA_PROTO_R_WITH_NEST_        // LUA带有嵌入式消息读取
#define _LUA_PROTO_R_WITHOUT_NEST_     // LUA无嵌入式消息读取


using UnityEngine;
using System.Collections;
using System.IO;
using Utils;

namespace sluaAux.proto
{
    public class luaProtoTest
    {
        /// <summary>
        /// 测试函数
        /// </summary>
        internal static void test()
        {
#if _CS_PROTO_RW_WITH_NEST__
            csProtoMsgWithNestRW();
#endif

#if _CS_PROTO_RW_WITHOUT_NEST__
            csProtoMsgWithoutNestRW();
#endif

#if _LUA_PROTO_W_WITH_NEST_
            luaProtoMsgWithNestW();
#endif

#if _LUA_PROTO_W_WITHOUT_NEST_
            luaProtoMsgWithoutNestW();
#endif


#if _LUA_PROTO_R_WITH_NEST_
            luaProtoMsgWithNestR();
#endif

#if _LUA_PROTO_R_WITHOUT_NEST_
            luaProtoMsgWithoutNestR();
#endif
        }

        /// <summary>
        /// C#带有嵌入消息的写入和读取
        /// 
        /// 消息定义文件: Analys.proto
        /// 消息类型: lua_message
        /// </summary>
        static void csProtoMsgWithNestRW()
        {
            Msg.lua_message msg = new Msg.lua_message();
            msg.value_repeated.Add(new Msg.lua_nested()
            {
                value_int32 = 1,
                value_bool = true,
                value_bytes = System.Text.Encoding.UTF8.GetBytes("Hello"),
                value_string = "Oh",

            });

            msg.value_repeated.Add(new Msg.lua_nested()
            {
                value_int32 = 2,
                value_bool = true,
                value_bytes = System.Text.Encoding.UTF8.GetBytes("World"),
                value_string = "Yes",

            });

            using (MemoryStream stream = new MemoryStream())
            {
                //序列化
                ProtoBuf.Serializer.Serialize(stream, msg);

                //反序列化
                Msg.lua_message msgRev = null;
                using (var desializeStream = new MemoryStream(stream.ToArray(), 0, (int)stream.Length))
                {
                    msgRev = ProtoBuf.Serializer.Deserialize<Msg.lua_message>(desializeStream);
                }

                if (msgRev != null)
                    LogSys.Log("_C_SHARP_PROTO_RW_WITH_NEST__ success!");
            }
        }

        /// <summary>
        /// C#无嵌入消息的写入和读取
        /// 
        /// 消息定义文件: Analys.proto
        /// 消息类型: lua_message2
        /// </summary>
        static void csProtoMsgWithoutNestRW()
        {
            var msg = new Msg.lua_message2();

            msg.value_int32 = 1;
            msg.value_uint32 = 2;
            msg.value_int64 = 3;
            msg.value_uint64 = 4;
            msg.value_bool = true;
            msg.value_bytes = System.Text.Encoding.UTF8.GetBytes("I have a dream!");
            msg.value_float = 5.0f;
            msg.value_double = 6.0;
            msg.value_string = "All men are created equal!";

            using (MemoryStream stream = new MemoryStream())
            {
                //序列化
                ProtoBuf.Serializer.Serialize(stream, msg);

                //反序列化
                Msg.lua_message2 msgRev = null;
                using (var desializeStream = new MemoryStream(stream.ToArray(), 0, (int)stream.Length))
                {
                    msgRev = ProtoBuf.Serializer.Deserialize<Msg.lua_message2>(desializeStream);
                }

                if (msgRev != null)
                    LogSys.Log("_C_SHARP_PROTO_RW_WITHOUT_NEST__ success!");
            }
                
        }

        /// <summary>
        /// LUA带有嵌入式消息写入
        /// 
        /// 测试流程： 
        ///  1、从lua中读取消息数据表（table）
        ///  2、用lua的写入方式写入流中
        ///  3、用原有的解析方式从流中读取数据，生成对应C#消息类型实例
        /// 
        /// 消息定义文件：
        ///  1、lua部分: 
        ///     文件: protoMsgDef.lua.txt
        ///     定义: idMsg=900000, table="lua_message", nestedTable="lua_nested"
        ///     数据: table="____protoMsg900000"
        ///  
        ///  2、C#部分
        ///     文件: Analys.proto
        ///     定义: message=lua_message, message=lua_nested
        /// 
        /// </summary>
        static void luaProtoMsgWithNestW()
        {
            var protoMsg = luaSvrManager.getInstance().luaSvr.luaState.getTable("____protoMsg900000");
            luaMessage msg = luaProtoHelper.readLuaMessage(900000, protoMsg);

            //打印消息
            string lsss = msg.logStr();
            LogSys.Log(lsss);

            using (MemoryStream stream = new MemoryStream())
            {
                //把lua表的数据写入流中
                var protoWriter = new luaProtoWriter(stream);
                protoWriter.writeLuaMessage(msg);
                protoWriter.close();

                MemoryStream desializeStream = null;
                Msg.lua_message msgRev = null;
                try
                {
                    desializeStream = new System.IO.MemoryStream(stream.ToArray(), 0, (int)stream.Length);
                    msgRev = ProtoBuf.Serializer.Deserialize<Msg.lua_message>(desializeStream);
                }
                finally
                {
                    if (desializeStream != null) desializeStream.Dispose();
                    desializeStream = null;
                }

                if (msgRev != null)
                    LogSys.Log("_LUA_PROTO_W_WITH_NEST_ success!");
            }
        }


        /// <summary>
        /// LUA无嵌入式消息写入
        /// 
        /// 测试流程： 
        ///  1、从lua中读取消息数据表（table）
        ///  2、用lua的写入方式写入流中
        ///  3、用原有的解析方式从流中读取数据，生成对应C#消息类型实例
        /// 
        /// 消息定义文件：
        ///  1、lua部分: 
        ///     文件: protoMsgDef.lua.txt
        ///     定义: idMsg=900001, table="lua_message2"
        ///     数据: table="____protoMsg900001"
        ///  
        ///  2、C#部分
        ///     文件: Analys.proto
        ///     定义: message=lua_message2
        /// 
        /// </summary>
        static void luaProtoMsgWithoutNestW()
        {
            var protoMsg = luaSvrManager.getInstance().luaSvr.luaState.getTable("____protoMsg900001");
            luaMessage msg = luaProtoHelper.readLuaMessage(900001, protoMsg);

            //打印消息
            string lsss = msg.logStr();
            LogSys.Log(lsss);

            using (MemoryStream stream = new MemoryStream())
            {
                //把lua表的数据写入流中
                var protoWriter = new luaProtoWriter(stream);
                protoWriter.writeLuaMessage(msg);
                protoWriter.close();

                MemoryStream desializeStream = null;
                Msg.lua_message2 msgRev = null;
                try
                {
                    desializeStream = new System.IO.MemoryStream(stream.ToArray(), 0, (int)stream.Length);
                    msgRev = ProtoBuf.Serializer.Deserialize<Msg.lua_message2>(desializeStream);
                }
                finally
                {
                    if (desializeStream != null) desializeStream.Dispose();
                    desializeStream = null;
                }

                if (msgRev != null)
                    LogSys.Log("_LUA_PROTO_W_WITHOUT_NEST_ success!");
            }
        }


        /// <summary>
        /// LUA带有嵌入式消息读取
        /// 
        /// 测试流程： 
        ///  1、C#中生成消息实例，并给消息字段赋值
        ///  2、用C#的写入方式写入流中
        ///  3、用lua的解析方式方式从流中读取数据，生成对应lua消息类型实例
        /// 
        /// 消息定义文件：
        ///  1、lua部分: 
        ///     文件: protoMsgDef.lua.txt
        ///     定义: idMsg=900000, table="lua_message", nestedTable="lua_nested"
        ///  
        ///  2、C#部分
        ///     文件: Analys.proto
        ///     定义: message=lua_message, message=lua_nested
        /// 
        /// </summary>
        static void luaProtoMsgWithNestR()
        {
            Msg.lua_message msg = new Msg.lua_message();
            msg.value_repeated.Add(new Msg.lua_nested()
            {
                value_int32 = 1,
                value_bool = true,
                value_bytes = System.Text.Encoding.UTF8.GetBytes("Hello"),
                value_string = "Oh",

            });

            msg.value_repeated.Add(new Msg.lua_nested()
            {
                value_int32 = 2,
                value_bool = true,
                value_bytes = System.Text.Encoding.UTF8.GetBytes("World"),
                value_string = "Yes",

            });

            using (MemoryStream stream = new MemoryStream())
            {
                //序列化
                ProtoBuf.Serializer.Serialize(stream, msg);

                //反序列化
                luaMessage msgRev = null;
                using (var desializeStream = new MemoryStream(stream.ToArray(), 0, (int)stream.Length))
                {
                    using (luaProtoReader reader = luaProtoReader.createLuaProtoReader(desializeStream))
                    {
                        if (reader != null)
                            msgRev = reader.readLuaMessage(900000);
                    }

                    //消息发送测试
                    luaProtobuf.getInstance().receiveMsg(900000, desializeStream);
                }

                //打印结果
                string lsss = msgRev.logStr();
                LogSys.Log(lsss);

                if (msgRev != null)
                    LogSys.Log("_LUA_PROTO_R_WITH_NEST_ success!");
            }
        }

        /// <summary>
        /// LUA无嵌入式消息读取
        /// 
        /// 测试流程： 
        ///  1、从lua中读取消息数据表（table）
        ///  2、用C#的写入方式写入流中
        ///  3、用lua的解析方式方式从流中读取数据，生成对应lua消息类型实例
        /// 
        /// 消息定义文件：
        ///  1、lua部分: 
        ///     文件: protoMsgDef.lua.txt
        ///     定义: idMsg=900001, table="lua_message2"
        ///  
        ///  2、C#部分
        ///     文件: Analys.proto
        ///     定义: message=lua_message2
        /// 
        /// </summary>
        static void luaProtoMsgWithoutNestR()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                var msg2 = new Msg.lua_message2();
                msg2.value_int32 = 1;
                msg2.value_uint32 = 2;
                msg2.value_int64 = 3;
                msg2.value_uint64 = 4;
                msg2.value_bool = true;
                msg2.value_bytes = System.Text.Encoding.UTF8.GetBytes("I have a dream!");
                msg2.value_float = 5.0f;
                msg2.value_double = 6.0;
                msg2.value_string = "All men are created equal!";
                ProtoBuf.Serializer.Serialize(mem, msg2);

                MemoryStream desializeStream = null;
                luaMessage msgRev = null;
                try
                {
                    desializeStream = new System.IO.MemoryStream(mem.ToArray(), 0, (int)mem.Length);
                    using (luaProtoReader reader = luaProtoReader.createLuaProtoReader(desializeStream))
                    {
                        if (reader != null)
                            msgRev = reader.readLuaMessage(900001);
                    }
                }
                finally
                {
                    if (desializeStream != null)
                        desializeStream.Dispose();

                    desializeStream = null;

                    //打印结果
                    string lsss = msgRev.logStr();
                    LogSys.Log(lsss);

                    if (msgRev != null)
                        LogSys.Log("_LUA_PROTO_R_WITHOUT_NEST_ success!");
                }
            }
        }
    } 
}

 


