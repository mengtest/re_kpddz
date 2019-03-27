/***************************************************************


 *
 *
 * Filename:  	luaMessageCache.cs	
 * Summary: 	消息缓存
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/24 16:58
 ***************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace sluaAux.proto
{
    public class luaMessageCache : MonoBehaviour
    {
        //消息缓存
        static Hashtable _cache = new Hashtable(64);

        //遍历辅助数组
        static int[] _auxiliary = new int[128];

        ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 添加消息缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        internal static void add(object key, luaMessage msg)
        {
            if (_cache.Contains(key))
                return;

            _cache.Add(key, msg);
        }

        /// <summary>
        /// 获取消息缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static luaMessage get(object key)
        {
            return (luaMessage)_cache[key];
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object createTypeInstance(object param)
        {
            luaMessage other = luaMessageCache.get(param);
            if (other == null)
                return null;

            var newMsg = new luaMessage(param);
            var otherFieldsInfo = (Dictionary<int, fieldDataInfo>)other.valueL;

            otherFieldsInfo.Keys.CopyTo(_auxiliary, 0);
            int count = otherFieldsInfo.Count;
            int key = 0;
            fieldDataInfo info = null;
            for (int i=0; i<count; i++)
            {
                //创建field
                key = _auxiliary[i];
                info = otherFieldsInfo[key];
                var descriptor = info.descriptor; // fieldDescriptor.copyCreateDescriptor(info.descriptor);
                if (descriptor != null)
                {
                    var fieldData = fieldDataInfo.createFieldData(key, descriptor);
                    if (fieldData != null)
                        newMsg.addField(key, fieldData);
                }
            }

            return newMsg;
        }
    }
}


