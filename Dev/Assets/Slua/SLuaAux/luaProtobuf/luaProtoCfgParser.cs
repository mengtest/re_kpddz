/***************************************************************


 *
 *
 * Filename:  	luaProtoCfgParser.cs	
 * Summary: 	配置解析器，解析lua配置的proto消息，生成可序列化的消息描述信息
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/25 5:35
 ***************************************************************/


using UnityEngine;
using System.Collections;
using SLua;
using ProtoBuf;
using Utils;

namespace sluaAux.proto
{
    /// <summary>
    /// 配置解析器
    /// </summary>
    public class luaProtoCfgParser
    {
        /// <summary>
        /// 初始化消息定义
        /// </summary>
        static internal void init()
        {
            LuaTable protoCfgTbl = luaSvrManager.getInstance().LuaProtoDefTbl;
            if (protoCfgTbl == null)
                return;

            object key = 0;
            foreach (LuaTable.TablePair kvp in protoCfgTbl)
            {
                key = luaProtoHelper.regualarKey(kvp.key);
                if (key == null || kvp.value.GetType() != typeof(LuaTable))
                    continue;

                //LogSys.Log("***************************** << " + kvp.key.ToString() + " >> ***********************************");

                //创建消息实体
                var luaMsg = new luaMessage(key);

                //读取消息定义
                var msgTbl = (LuaTable)kvp.value;
                foreach (LuaTable.TablePair field in msgTbl)
                {
                    var fieldInfo = (LuaTable)field.value;
                    if (fieldInfo == null)
                        continue;

                    int idx = (int)luaProtoHelper.regualarKey(field.key);
                    EFieldModifier modifier = (EFieldModifier)luaProtoHelper.regualarKey(fieldInfo[1]);

                    //类型信息
                    ProtoTypeCode typeCode = ProtoTypeCode.Empty;
                    string nestedTypeName = null;
                    if (fieldInfo[2].GetType() == typeof(double))
                        typeCode = (ProtoTypeCode)luaProtoHelper.regualarKey(fieldInfo[2]);
                    else
                    {
                        typeCode = ProtoTypeCode.Type;
                        nestedTypeName = (string)fieldInfo[2];
                    }

                    string name = (string)fieldInfo[3];

                    //创建field
                    var descriptor = fieldDescriptor.createDescriptor(modifier,typeCode, name, nestedTypeName);
                    if (descriptor != null)
                    {
                        var info = fieldDataInfo.createFieldData(idx, descriptor);
                        if (info != null)
                            luaMsg.addField(idx, info);
                    }

                    //调试信息
                    string fieldLog = string.Format("idx={0}, modifier={1}, typeCode={2}[{3}], name={4}",
                        idx, modifier, typeCode, nestedTypeName, name);
                    //LogSys.Log(fieldLog);
                }

                //加入消息缓存
                luaMessageCache.add(key, luaMsg);
            }
        }
    }
}




