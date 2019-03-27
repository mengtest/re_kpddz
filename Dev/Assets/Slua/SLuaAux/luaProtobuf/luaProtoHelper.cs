/***************************************************************


 *
 *
 * Filename:  	luaProtoHelper.cs	
 * Summary: 	帮助函数
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/24 16:58
 ***************************************************************/


using UnityEngine;
using System.Collections;
using ProtoBuf;
using SLua;
using LuaInterface;

namespace sluaAux.proto
{

    public class luaProtoHelper
    {
        /// <summary>
        /// 初始化luaProto模块
        /// </summary>
        internal static void initLuaProto()
        {
            valueFactroyManager.initialize();
            luaProtoCfgParser.init();
        }

        /// <summary>
        /// 调整key值, int和string类型为合法的。
        /// lua中的number数据为double，需要转为int
        /// </summary>
        internal static object regualarKey(object key)
        {
            if (key.GetType() == typeof(double))
            {
                double n = (double)key;
                return (int)n;
            }
            else if (key.GetType() == typeof(string))
                return key;

            return null;
        }

        /// <summary>
        /// 获取wireType 
        /// </summary>
        internal static WireType getWireType(ProtoTypeCode typeCode)
        {
            WireType wireType = WireType.None;
            switch (typeCode)
            {
                case ProtoTypeCode.Int32:
                case ProtoTypeCode.UInt32:
                case ProtoTypeCode.Int64:
                case ProtoTypeCode.UInt64:
                    wireType = WireType.Variant;
                    break;
                case ProtoTypeCode.String:
                    wireType = WireType.String;
                    break;
                case ProtoTypeCode.Single:
                    wireType = WireType.Fixed32;
                    break;
                case ProtoTypeCode.Double:
                    wireType = WireType.Fixed64;
                    break;
                case ProtoTypeCode.Boolean:
                    wireType = WireType.Variant;
                    break;
                case ProtoTypeCode.Decimal:
                    wireType = WireType.String;
                    break;
                case ProtoTypeCode.ByteArray:
                    wireType = WireType.String;
                    break;
                case ProtoTypeCode.Char:
                    wireType = WireType.Variant;
                    break;
                case ProtoTypeCode.Type:
                    wireType = WireType.String;
                    break;
            }

            return wireType;
        }


        /// <summary>
        /// lua中的数据类型转换为C#中的具体数据类型
        /// </summary>
        internal static object luaType2CType(ProtoTypeCode typeCode, object luaValue)
        {
            if (luaValue == null)
                return null;
         
            switch (typeCode)
            {
                case ProtoTypeCode.Boolean:
                    return (bool)luaValue;
                case ProtoTypeCode.UInt16:
                    return (ushort)((double)luaValue);
                case ProtoTypeCode.Int16:
                    return (short)((double)luaValue);
                case ProtoTypeCode.Int32:
                    return (int)((double)luaValue);
                case ProtoTypeCode.UInt32:
                    return (uint)((double)luaValue);
                case ProtoTypeCode.Int64:
                    return (long)((double)luaValue);
                case ProtoTypeCode.UInt64:
                    return (ulong)((double)luaValue);
                case ProtoTypeCode.Single:
                    return (float)((double)luaValue);
                case ProtoTypeCode.Double:
                    return (double)luaValue;
                case ProtoTypeCode.String:
                    return (string)luaValue;
                case ProtoTypeCode.ByteArray:
                    var str = (string)luaValue;
                    byte[] bts = System.Text.Encoding.UTF8.GetBytes(str);
                    return bts;
                case ProtoTypeCode.Type:
                    return (LuaTable)luaValue;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 读取lua消息
        /// </summary>
        internal static luaMessage readLuaMessage(object idMsg, LuaTable luaMsgTbl)
        {
            if (luaMsgTbl == null)
                return null;

            luaMessage msg = (luaMessage)luaMessageCache.createTypeInstance(idMsg);
            if (msg == null)
                return null;

            //按照索引顺序读取lua表中对应字段的值
            int count = msg.fieldsCount();
            fieldDataInfo fieldData = null;
            for (int i=1; i<=count; i++)
            {
                fieldData = (fieldDataInfo)msg[i];
                if (fieldData == null) continue;

                //检查lua表中对应字段的值
                string name = fieldData.descriptor.name;
                if (luaMsgTbl[name] == null) continue;

                //创建值字段
                if (!fieldData.appendValue())
                    continue;

                fieldDescriptor des = fieldData.descriptor;
                if (des.isRepeated())
                {
                    LuaTable repeatedTbl = (LuaTable)luaMsgTbl[name];
                    for (int j=1; j<=repeatedTbl.length(); j++)
                    {
                        object subValue = luaType2CType(des.typeCode, repeatedTbl[j]);
                        if (subValue == null)
                            continue;

                        //嵌入类型
                        if (des.haveNestedType())
                        {
                            object subMsg = readLuaMessage(des.nestedTypeName, (LuaTable)subValue);
                            if (subMsg != null)
                                fieldData.Value.addChild(subMsg);
                        }
                        else
                        {
                            fieldData.Value.addChild(subValue);
                        }
                    }
                }
                else
                {
                    object value = luaType2CType(des.typeCode, luaMsgTbl[name]);
                    if (value == null)
                        continue;

                    //嵌入类型
                    if (des.haveNestedType())
                        fieldData.Value.valueL = readLuaMessage(des.nestedTypeName, (LuaTable)value);
                    else
                        fieldData.Value.valueL = value;
                }
            }

            return msg;
        }

        /// <summary>
        /// 压入一个lua值到LuaState中
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="state"></param>
        /// <param name="luaValue"></param>
        private static void pushLuaValue(ProtoTypeCode typeCode, LuaState state, object luaValue)
        {
            switch (typeCode)
            {
                case ProtoTypeCode.Boolean:
                    LuaDLL.lua_pushboolean(state.L, (bool)luaValue);
                    break;
                case ProtoTypeCode.Int16:
                case ProtoTypeCode.Int32:
                    LuaDLL.lua_pushinteger(state.L, (int)luaValue);
                    break;
                case ProtoTypeCode.UInt16:
                case ProtoTypeCode.UInt32:
                    LuaDLL.lua_pushinteger(state.L, (int)(uint)luaValue);
                    break;
                case ProtoTypeCode.Int64:
                    LuaDLL.lua_pushnumber(state.L, (long)luaValue);
                    break;
                case ProtoTypeCode.UInt64:
                    LuaDLL.lua_pushnumber(state.L, (ulong)luaValue);
                    break;
                case ProtoTypeCode.Single:
                    LuaDLL.lua_pushnumber(state.L, (float)luaValue);
                    break;
                case ProtoTypeCode.Double:
                    LuaDLL.lua_pushnumber(state.L, (double)luaValue);
                    break;
                case ProtoTypeCode.String:
                    LuaDLL.lua_pushstring(state.L, (string)luaValue);
                    break;
                case ProtoTypeCode.ByteArray:
                    var str = System.Text.Encoding.UTF8.GetString((byte[])luaValue);
                    LuaDLL.lua_pushstring(state.L, str);
                    break;
                default:
                    LuaDLL.lua_pushnil(state.L);
                    break;
            }
        }

        /// <summary>
        /// 根据message生成table
        /// </summary>
        internal static void createLuaTable(LuaState state, luaMessage msg)
        {
            //压入消息表
            LuaDLL.lua_newtable(state.L);
            fieldDataInfo field = null;
            for (int idx = 1; idx <= msg.fieldsCount(); idx++)
            {
                field = (fieldDataInfo)msg[idx];
                if (field.Value == null)
                    continue;

                string key = field.descriptor.name;
                ProtoTypeCode typeCode = field.descriptor.typeCode;
                if (field.descriptor.isRepeated())
                {
                    LuaDLL.lua_pushstring(state.L, key);
                    LuaDLL.lua_newtable(state.L);
                    for (int j=0; j<field.Value.count(); j++)
                    {
                        if (field.descriptor.haveNestedType())
                        {
                            createLuaTable(state, (luaMessage)field.Value[j]);
                        }
                        else
                        {
                            object luaValue = field.Value[j];
                            pushLuaValue(typeCode, state, luaValue);
                        }
                        LuaDLL.lua_rawseti(state.L, -2, j+1);
                    }
                    LuaDLL.lua_settable(state.L, -3);
                }
                else
                {
                    LuaDLL.lua_pushstring(state.L, key);
                    if (field.descriptor.haveNestedType())
                    {
                        createLuaTable(state, (luaMessage)field.Value.valueL);
                    }
                    else
                    {
                        object luaValue = field.Value.valueL;
                        pushLuaValue(typeCode, state, luaValue);
                    }
                    LuaDLL.lua_settable(state.L, -3);
                }
            }
        }
    }
}

